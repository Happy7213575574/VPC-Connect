using System;
using ConnectApp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PortalApi.Tests
{
    [TestClass]
    public class PortalHelperTests
    {

        [TestMethod]
        public void QueryConstructedSafelyDespiteDoubleSlash()
        {
            var target = "https://portal.vpc.police.uk/Event/Details//c4ab761c-1efa-4923-b8e9-1aeb1d0ac62c";
            var uri = new Uri(target);
            var fullQuery = PortalHelper.PreparePortalAnalyticsUriQuery(uri, PortalHelper.AnalyticsAction.OpenNotification, new PortalHelperTests(), null);
            var expected = "https://portal.vpc.police.uk/Event/Details/c4ab761c-1efa-4923-b8e9-1aeb1d0ac62c?action=OpenNotification&source=org.vpc.connect&originator=PortalHelperTests";
            Assert.AreEqual(fullQuery.ToString(), expected);
        }

        [TestMethod]
        public void QueryConstructedSafelyDespiteTrailingSlash()
        {
            var target = "https://portal.vpc.police.uk/Event/Details//c4ab761c-1efa-4923-b8e9-1aeb1d0ac62c/";
            var uri = new Uri(target);
            var fullQuery = PortalHelper.PreparePortalAnalyticsUriQuery(uri, PortalHelper.AnalyticsAction.OpenNotification, new PortalHelperTests(), null);
            var expected = "https://portal.vpc.police.uk/Event/Details/c4ab761c-1efa-4923-b8e9-1aeb1d0ac62c/?action=OpenNotification&source=org.vpc.connect&originator=PortalHelperTests";
            Assert.AreEqual(fullQuery.ToString(), expected);
        }

        [TestMethod]
        public void QueryConstructedCorrectlyWithSensibleUrl()
        {
            var target = "https://portal.vpc.police.uk/Event/Details/c4ab761c-1efa-4923-b8e9-1aeb1d0ac62c";
            var uri = new Uri(target);
            var fullQuery = PortalHelper.PreparePortalAnalyticsUriQuery(uri, PortalHelper.AnalyticsAction.OpenNotification, new PortalHelperTests(), null);
            var expected = "https://portal.vpc.police.uk/Event/Details/c4ab761c-1efa-4923-b8e9-1aeb1d0ac62c?action=OpenNotification&source=org.vpc.connect&originator=PortalHelperTests";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Found:    " + fullQuery.ToString());
            Assert.AreEqual(fullQuery.ToString(), expected);
        }

        [TestMethod]
        public void NullInNullOut()
        {
            Uri uri = null;
            var fullQuery = PortalHelper.PreparePortalAnalyticsUriQuery(uri, PortalHelper.AnalyticsAction.OpenNotification, new PortalHelperTests(), null);
            Assert.IsNull(fullQuery);
        }

    }
}
