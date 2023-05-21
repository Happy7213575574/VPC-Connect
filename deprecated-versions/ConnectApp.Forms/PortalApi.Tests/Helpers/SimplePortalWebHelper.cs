using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PortalApi.Tests.Helpers
{
    public class SimplePortalWebHelper
    {
        public static async Task AssertWebPageAvailableAsync(string url, bool? expectSignOn)
        {
            Console.WriteLine("Testing url: " + url);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                Console.WriteLine($"Response code: {response.StatusCode} ({(int)response.StatusCode})");
                Assert.IsTrue(response.IsSuccessStatusCode, "Response code unsuccessful.");

                var content = await response.Content.ReadAsStringAsync();
                var pageNotFound = content.Contains("Page Not Found");
                Assert.IsFalse(pageNotFound, "Content indicates that requested page not found.");

                var redirectedToSignOn = content.Contains("Marshall Volunteer Portal Sign On");
                Console.WriteLine($"Sign On seen: {redirectedToSignOn}");

                if (expectSignOn != null)
                {
                    Assert.AreEqual(
                        expectSignOn.Value, redirectedToSignOn,
                        $"Expected to see sign on: {expectSignOn}, Sign on seen: {redirectedToSignOn}");
                }
            }
        }

    }
}
