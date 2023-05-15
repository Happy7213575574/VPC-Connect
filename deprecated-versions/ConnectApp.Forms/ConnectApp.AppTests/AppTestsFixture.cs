using System;
using System.Collections.Generic;
using ConnectApp.AppTests.Helper;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests
{
    [SetUpFixture]
    public class AppTestsFixture
    {
        private static Dictionary<Platform, IApp> apps;

        public enum AppVariant
        {
            Android,
            iOS,
            AndroidClean
        }

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            apps = new Dictionary<Platform, IApp>();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
        }

        public static void Reset()
        {
            apps.Clear();
        }

        public static IApp GetApp(Platform platform)
        {
            if (!apps.ContainsKey(platform))
            {
                apps.Add(platform, AppInitializer.StartApp(platform));
            }
            return apps[platform];
        }
    }
}
