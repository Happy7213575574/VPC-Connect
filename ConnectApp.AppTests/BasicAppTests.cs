using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ConnectApp.AppTests
{
    // TODO: new tests for the app

    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class BasicAppTests : AbstractAppTests
    {
        public BasicAppTests(Platform platform) : base(platform)
        {
        }

        protected override bool AlwaysFreshApp => false;

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("Home screen");
        }


    }
}
