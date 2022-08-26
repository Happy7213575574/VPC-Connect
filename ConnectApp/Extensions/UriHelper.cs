using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectApp.Pages;
using ConnectApp.Pages.Models;
using Xamarin.Essentials;

namespace ConnectApp.Extensions
{
    public class UriHelper<P, M>
        where P : BaseAppContentPage<M>
        where M : AbstractBaseModel
    {
        private P view;

        public UriHelper(P view)
        {
            this.view = view;
        }

        public async Task OpenLinkAsync(Uri uri)
        {
            await OpenLinkAsync(uri.ToString());
        }

        public async Task OpenLinkAsync(string link)
        {
            try
            {
                view.log.Info("Uri requested: " + link ?? "null", false);
                view.app.Analytics?.SendEvent("UriRequest", "Uri", link ?? "null");

                if (link.ToLower().StartsWith("mailto:"))
                {
                    var recipients = new List<string>();
                    recipients.Add(link.Substring("mailto:".Length));
                    await SendEmailAsync(recipients);
                }
                else if (link.ToLower().StartsWith("https://"))
                {
                    var uri = new Uri(link);
                    if (PortalHelper.UriPermitted(uri))
                    {
                        view.log.Info($"Visiting uri: {link}", false);
                        var ok = await PortalHelper.VisitUriAsync(uri);
                        if (!ok)
                        {
                            view.log.Warning($"Unable to visit: {link}", false);
                            await view.DisplayAlert("Unexpected error", "Unable to follow this link.", "OK");
                        }
                    }
                    else
                    {
                        view.log.Warning($"Unsafe domain, unable to visit: {link}", false);
                        await view.DisplayAlert("Unsafe domain", $"Domain not safelisted: {uri.Host}", "OK");
                    }
                }
                else if (link.ToLower().StartsWith("tel:"))
                {
                    Dial(link.Substring("tel:".Length));
                }
                else if (link.ToLower().StartsWith("page:"))
                {
                    var pageStr = link.Substring("page:".Length);
                    PageTypes page;
                    var ok = Enum.TryParse(pageStr, out page);
                    if (ok)
                    {
                       view.app.SwitchToPage(page);
                    }
                    else
                    {
                        await view.DisplayAlert("Coming soon...", "This page is under construction.", "OK");
                    }
                }
                else if (link.ToLower().StartsWith("http://"))
                {
                    await view.DisplayAlert("Insecure link", "Only https links are supported.", "OK");
                }
                else
                {
                    await view.DisplayAlert("Coming soon...", "This feature is under construction.", "OK");
                }
            }
            catch
            {
                await view.DisplayAlert("Unexpected error", "Please let us know what you were doing, to help improve the app.", "OK");
            }

        }

        public async Task<bool> SendEmailAsync(List<string> recipients = null, string subject = null, string body = null, List<string> cc = null, List<string> bcc = null)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    Cc = cc,
                    Bcc = bcc
                };
                await Email.ComposeAsync(message);
                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                view.log.Warning("Email not supported, emailing: " + string.Join(", ", recipients), false, ex);
                return false;
            }
            catch (Exception ex)
            {
                view.log.Warning("Error attempting to email: " + string.Join(", ", recipients), false, ex);
                return false;
            }
        }

        internal bool Dial(string number)
        {
            try
            {
                PhoneDialer.Open(number);
                return true;
            }
            catch (ArgumentNullException ex)
            {
                view.log.Warning("Number was null or white space: " + number, false, ex);
                return false;
            }
            catch (FeatureNotSupportedException ex)
            {
                view.log.Warning("Phone Dialer is not supported on this device, for: " + number, false, ex);
                return false;
            }
            catch (Exception ex)
            {
                view.log.Warning("Error attempting to dial: " + number, false, ex);
                return false;
            }
        }

    }
}
