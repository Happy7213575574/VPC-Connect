using System;
namespace ConnectApp.Communication
{
    public class PortalUris
    {

#if TESTPORTAL
        public static readonly string PortalApi_BaseUri = "https://vpc-app-endpoint.stimulize.co.uk/api/";
        public static readonly string PortalApi_AccessCode = SensitiveConstants.PortalApiAccessCode;
        public static readonly string PortalWeb_BaseUri = "https://vpc-app-portal.stimulize.co.uk";
        public static readonly string PortalWeb_LoginUri = "https://vpc-app-sso.stimulize.co.uk";
#else
        public static readonly string PortalApi_BaseUri = "https://endpoint.vpc.police.uk/api";
        public static readonly string PortalApi_AccessCode = SensitiveConstants.PortalApiAccessCode;
        public static readonly string PortalWeb_BaseUri = "https://portal.vpc.police.uk";
        public static readonly string PortalWeb_LoginUri = "https://sso.vpc.police.uk";
#endif
        public static readonly string PortalWeb_DeepLinkToEventUri = PortalWeb_BaseUri + "/Event/Details/";
        public static readonly string PortalWeb_DiaryUri = PortalWeb_BaseUri + "/EventDiary/Index";
        public static readonly string PortalWeb_CalendarUri = PortalWeb_BaseUri + "/Event/Calendar";
        public static readonly string PortalWeb_ConversationsUri = PortalWeb_BaseUri + "/User/MessageBoardList";
        public static readonly string PortalWeb_MessageBoardsUri = PortalWeb_BaseUri + "/MessageBoards/Index";
        public static readonly string PortalWeb_ResourcesUri = PortalWeb_BaseUri + "/ResourceLibrary/Resources";

        public static readonly string DeviceCheckEndpoint = "/DeviceCheck";
        public static readonly string UserTokenEndpoint = "/RegistrationToken";
        public static readonly string RegistrationEndpoint = "/Registration";
        public static readonly string DeregistrationEndpoint = "/SignOut";

        public static readonly string WebsiteUri = "https://vpc.police.uk";
        public static readonly string PrivacyPolicyUri = "https://vpc.police.uk/privacy-policy/";

        private static readonly int _1m = 1 * 60 * 1000; // milliseconds
        private static readonly int _5m = 5 * 60 * 1000; // milliseconds

        public static readonly int? OverrideTimeout = _1m; // ms
    }
}
