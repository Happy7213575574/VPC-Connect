using System;
using ConnectApp.Communication;

namespace PortalApi.Tests.Helpers
{
    public class AccountHelper
    {
        public static string Username => SensitiveConstants.Tests_Username;
        public static string Password => SensitiveConstants.Tests_Password;
        public static string FullName => SensitiveConstants.Tests_FullName;
        public static string SignedInUrl => SensitiveConstants.Tests_SignedInUrl;
    }
}
