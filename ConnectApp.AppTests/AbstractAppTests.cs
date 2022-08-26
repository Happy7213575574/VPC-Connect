using System;
using ConnectApp.AppTests.Helper;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests
{
    public abstract class AbstractAppTests
    {
        protected IApp app;
        protected Platform platform;
        protected AppHelper helper;

        protected bool firstTest;

        protected abstract bool AlwaysFreshApp { get; }

        public AbstractAppTests(Platform platform)
        {
            this.platform = platform;
            this.firstTest = true;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            if (firstTest)
            {
                AppTestsFixture.Reset();
                firstTest = false;
            }

            if (AlwaysFreshApp)
            {
                this.app = AppInitializer.StartApp(platform);
            }
            else
            {
                this.app = AppTestsFixture.GetApp(platform);
            }

            this.helper = new AppHelper(app, platform);
        }
    }
}
