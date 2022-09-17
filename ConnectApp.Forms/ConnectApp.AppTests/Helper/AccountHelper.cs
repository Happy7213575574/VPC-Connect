using System;
using ConnectApp.Communication;

namespace ConnectApp.AppTests.Helper
{
    public class AccountHelper
    {
        public static string Username => SensitiveConstants.Tests_Username;
        public static string Password => SensitiveConstants.Tests_Password;
        public static string FullName => SensitiveConstants.Tests_FullName;
    }
}
