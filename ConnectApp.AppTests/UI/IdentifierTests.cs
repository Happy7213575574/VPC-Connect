using System;
using ConnectApp.AppTests;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests.UI
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class IdentifierTests : AbstractAppTests
    {
        protected override bool AlwaysFreshApp => false;

        public IdentifierTests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void AppHasIdentifiers()
        {
            var identifiers = helper.GetAppIdentifiers();

            foreach (var key in identifiers.Keys)
            {
                Console.WriteLine($"Identifier - {key}: {identifiers[key]}");
            }

            Assert.True(identifiers.ContainsKey("environment"));
            Assert.True(identifiers.ContainsKey("api_uri"));
            Assert.True(identifiers.ContainsKey("signin_uri"));
            Assert.True(identifiers.ContainsKey("token"));
            Assert.True(identifiers.ContainsKey("device"));
            Assert.True(identifiers.ContainsKey("uuid"));
        }

    }
}
