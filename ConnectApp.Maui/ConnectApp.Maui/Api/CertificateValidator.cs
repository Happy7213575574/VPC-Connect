using System;
using System.Security.Cryptography.X509Certificates;

namespace ConnectApp.Maui.Api
{
    public class CertificateValidator
    {
        // inspired by: https://www.meziantou.net/custom-certificate-validation-in-dotnet.htm
        public static bool ValidateCertificateAgainstKnownRoots(X509Certificate certificate)
        {
            var validRootCertificates = SensitiveConstants.PortalApiRootCertificates
                .Select(Convert.FromBase64String)
                .Select(bytes => new X509Certificate2(bytes));

            var chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            chain.ChainPolicy.VerificationTime = DateTime.Now;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);

            // add known roots (sectigo, usertrust)
            foreach (var cert in validRootCertificates)
                chain.ChainPolicy.ExtraStore.Add(cert);

            var certificate2 = new X509Certificate2(certificate);

            // confirm the chain can build
            var chainOk = chain.Build(certificate2);

            int issues = 0;
            // confirm each certificate in the chain shows no issues
            foreach (var cert in chain.ChainElements)
            {
                issues += cert.ChainElementStatus.Count();
            }

            // confirm each valid root certificate is in the chain
            foreach (var cert in validRootCertificates)
            {
                if (!chain.ChainElements.Any(el => el.Certificate.Thumbprint == cert.Thumbprint))
                    issues++;
            }

            return chainOk && issues == 0;
        }
    }
}

