using ConnectApp.Maui.Api;
using ConnectApp.Maui.Api.DTO;
using ConnectApp.Maui.AppLog;
using ConnectApp.Maui.Data;
using ConnectApp.Maui.Data.Entities;
using ConnectApp.Maui.Devices;
using ConnectApp.Maui.Extensions;
using ConnectApp.Maui.Pages;
using Newtonsoft.Json;
using Plugin.Firebase.CloudMessaging;
using ConnectApp.Maui.Analytics;

namespace ConnectApp.Maui;

public partial class App : Application
{
    public static App Instance { get; private set; }

    public static Random Random { get; } = new Random();

    private object writeLock = new object();

    internal ConnectAppData Db { get; private set; }
    internal PushConfiguration Config { get; private set; }
    internal ConnectDevice Device { get; private set; }
    internal IPortalApi Api { get; private set; }
    internal AppLogger Log { get; private set; }

    private AppShell main;

    private RegistrationStates registrationState = RegistrationStates.NotKnown;
    public RegistrationStates RegistrationState
    {
        get { return registrationState; }
        private set
        {
            registrationState = value;
            Log.Debug("RegistrationState: " + value, false);
            Analytics?.SendEvent("RegistrationState", "State", value.ToString());
            OnAppRegistrationStateChange?.Invoke(registrationState);
        }
    }

    private string latestPushToken;
    public string LatestPushToken
    {
        get { return latestPushToken; }
        set
        {
            latestPushToken = value;
            OnPushTokenChange?.Invoke(value);
        }
    }

    private AppActivity lastActivity;
    public AppActivity LastActivity
    {
        get { return lastActivity; }
        private set
        {
            lastActivity = value;
            Log.Debug("LastAppActivity.State: " + value.State.ToString(), false);
            Analytics?.SendEvent("AppActivity",
                new Dictionary<string, string>()
                {
                        { "Event", value.State.ToString() },
                        { "IsSuccess", value.Result != null ? value.Result.IsSuccess.ToString() : "" }
                });
            if (lastActivity.Result != null)
            {
                Log.Info("LastAppActivity.Result.IsSuccess: " + value.Result.IsSuccess.ToString(), false);
                Log.Debug("LastAppActivity.Result.Code: " + value.Result.Code ?? "", false);

                if (!string.IsNullOrWhiteSpace(value.Result.StatusDescription))
                {
                    Log.Debug("LastAppActivity.Result.StatusDescription: " + value.Result.StatusDescription, false);
                }
                if (!string.IsNullOrWhiteSpace(value.Result.RawContent))
                {
                    Log.Verbose("LastAppActivity.Result.RawContent: " + value.Result.RawContent, true);
                }
                if (lastActivity.Result.Headers != null && lastActivity.Result.Headers.Count > 0)
                {
                    var headerStrings = value.Result.Headers?.Select(t => " - " + t.Item1 + ": " + t.Item2);
                    var headers = string.Join("\n", headerStrings);
                    Log.Verbose("LastAppActivity.Result.Headers:\n" + headers, true);
                }
                if (lastActivity.Result.ErrorMessage != null)
                    Log.Warning("LastAppActivity.Result.ErrorMessage: " + value.Result.ErrorMessage, false);
                if (lastActivity.Result.ErrorException != null)
                    Log.Verbose("LastAppActivity.Result.ErrorException: " + value.Result.ErrorException.ToString(), false);
            }
            OnAppActivity?.Invoke(value);
        }
    }

    public struct AppActivity
    {
        public AppEvents State { get; set; }
        public ServerResponse Result { get; set; }
    }

    public enum AppEvents
    {
        Initialising,
        PushTokenCheckInitiated,
        PushTokenCheckResult,
        GetUserTokenInitiated,
        GetUserTokenResult,
        SubmitRegistrationInitiated,
        SubmitRegistrationResult,
        SubmitDeregistrationInitiated,
        SubmitDeregistrationResult
    }

    public enum RegistrationStates
    {
        NotKnown,
        Registered,
        NotRegistered,
    }

    internal event Action<AppActivity> OnAppActivity;
    internal event Action<RegistrationStates> OnAppRegistrationStateChange;
    internal event Action<NotificationRecord> OnNotificationReceived;
    internal event Action<NotificationRecord> OnNotificationArchived;
    internal event Action<string> OnPushTokenChange;
    internal event Action OnNotificationsErased;

    public App()
    {
        Db = new ConnectAppData();
        Log = new AppLogger(Db).For(this);
        Log.Debug("Db and log ready.", false);
        Log.RegisterForExceptions();
        Log.Debug("Registered for first chance exceptions.", false);

        Log.Info("App creation.", false);
        Instance = this;

        // TODO: reenable analytics and crashlytics
        //try
        //{
        //    Log.Debug("Creating Analytics...", false);
        //    Analytics = DependencyService.Get<IAnalyticsReporter>();
        //}
        //catch (Exception e)
        //{
        //    Log.Warning("Could not create analytics.", false, e);
        //}

        //try
        //{
        //    Log.Debug("Creating Crashlytics...", false);
        //    Crashlytics = DependencyService.Get<ICrashlyticsReporter>();
        //    Crashlytics.Init();
        //    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        //    TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        //}
        //catch (Exception e)
        //{
        //    Log.Warning("Could not create crashlytics.", false, e);
        //}

        Log.Debug("Init component...", false);
        InitializeComponent();

        Log.Debug("Init logic...", false);
        LastActivity = new AppActivity() { State = AppEvents.Initialising };
        Device = new ConnectDevice();
        // Api = new PortalApiHttpClient(this);
        Api = new PortalApiResharper(this);
        Config = InitConfiguration();
        Log.Verbose("InitConfiguration with PushToken: " + Config.PushToken ?? "(null)", true);
        LatestPushToken = Config.PushToken;

        // Xamarin Essentials provides version tracking for the app
        VersionTracking.Track();

        // when there's a new token, we need to run our cycle of checks
        CrossFirebaseCloudMessaging.Current.TokenChanged += async (source, args) =>
        {
            Log.Info("Token changed", false);
            Log.Verbose("Token: " + args.Token ?? "(null)", true);
            Analytics?.SendEvent("PushEvent", "Event", "TokenChanged");

            // Debug here to catch token refresh events
            if (string.IsNullOrWhiteSpace(Config.PushToken) || !Config.PushToken.Equals(args.Token))
            {
                LatestPushToken = args.Token;
                await CheckTokenWithPortalAsync(args.Token);
            }
        };

        // Android + iOS: triggered when notification received while foregrounded
        CrossFirebaseCloudMessaging.Current.NotificationReceived += (source, args) =>
        {
            Log.Info("OnNotificationReceived", false);
            // TODO: reenable analytics and crashlytics
            //Analytics?.SendEvent("PushEvent", "Event", "NotificationReceived");
            var notification = StoreReceivedNotification(args.Notification.Data);
            OnNotificationReceived?.Invoke(notification);
        };

        // iOS only: triggered when notification opened
        CrossFirebaseCloudMessaging.Current.NotificationTapped += (source, args) =>
        {
            Log.Info("OnNotificationOpened", false);
            // TODO: reenable analytics and crashlytics
            // Analytics?.SendEvent("PushEvent", "Event", "NotificationOpened");
            var notification = StoreReceivedNotification(args.Notification.Data);
            OnNotificationReceived?.Invoke(notification);
        };


        Log.Debug("Initiating firebase cloud messaging check...", false);
        CrossFirebaseCloudMessaging.Current.CheckIfValidAsync().SafeFireAndForget(true);

        // initialise the UI
        main = new AppShell();
        MainPage = main;
    }

    #region Analytics

    public IAnalyticsReporter Analytics { get; private set; }
    public ICrashlyticsReporter Crashlytics { get; private set; }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        try
        {
            Log.Exception(new Exception(e.ExceptionObject.ToString()));
            Analytics?.SendExceptionObject(e.ExceptionObject);
            Crashlytics?.RecordExceptionObject(e.ExceptionObject);
        }
        catch
        {
            Log.Error("Unable to record CurrentDomain_UnhandledException: " + e.ExceptionObject.ToString(), false);
        }
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        try
        {
            Log.Exception(e.Exception);
            Analytics?.SendException(e.Exception);
            Crashlytics.RecordException(e.Exception);
        }
        catch
        {
            Log.Error("Unable to record TaskSchedulerOnUnobservedTaskException", false, e.Exception);
        }
    }

    #endregion

    #region Requests

    internal void RequestNotificationArchive(NotificationRecord record)
    {
        Log.Debug("RequestNotificationArchive", false);
        ArchiveNotification(record);
        OnNotificationArchived?.Invoke(record);
    }

    #endregion

    #region Pages

    internal async Task SwitchToPageAsync(PageTypes page)
    {
        Log.Verbose("UI switching to page: " + page.ToString(), false);
        await main.SwitchToPageAsync(page);
    }

    #endregion

    #region Storage

    private PushConfiguration InitConfiguration()
    {
        var config = Db.GetPushConfiguration();
        Log.Verbose(config == null ? "No push config found." : "Retrieved a push config record.", false);
        return config ?? new PushConfiguration();
    }

    private void StoreNewPushToken(string token)
    {
        // Debug/breakpoint here to capture the token while debugging.
        Log.Debug("Storing push token. " + (string.IsNullOrWhiteSpace(token) ? "(null)" : "(string)"), false);
        if (!string.IsNullOrWhiteSpace(token)) { Log.Verbose(token, true); }
        Config.PushToken = token;
        Db.SavePushConfiguration(Config);
    }

    private void StoreUserToken(string token)
    {
        Log.Debug("Storing user token. " + (string.IsNullOrWhiteSpace(token) ? "(null)" : "(string)"), false);
        if (!string.IsNullOrWhiteSpace(token)) { Log.Verbose(token, true); }
        Config.UserToken = token;
        Db.SavePushConfiguration(Config);
    }

    private void ArchiveNotification(NotificationRecord record)
    {
        lock (writeLock)
        {
            try
            {
                Log.Debug("Archiving notification with id: " + record.NotificationId, false);
                Db.Archive(record);
                Log.Info("Archived notification with id: " + record.NotificationId, false);
            }
            catch (Exception e)
            {
                Log.Error("Unable to archive notification with id: " + record.NotificationId, false, e);
            }

        }
    }

    public NotificationRecord RequestAddNotification(IDictionary<string, string> dictionary)
    {
        return StoreReceivedNotification(dictionary);
    }

    private NotificationRecord StoreReceivedNotification(IDictionary<string, string> dictionary)
    {
        lock (writeLock)
        {
            Log.Debug("Storing new notification.", false);
            Log.Verbose(JsonConvert.SerializeObject(dictionary, Formatting.Indented), true);
            try
            {
                var notification = new NotificationRecord();
                notification.Received = DateTime.Now;
                notification.ReceiveCount = 1;
                notification.AsJson = JsonConvert.SerializeObject(dictionary);
                notification.Title = NotificationHelper.GetTitle(dictionary);
                notification.Message = NotificationHelper.GetMessage(dictionary);
                notification.TargetUrl = NotificationHelper.GetTargetUrl(dictionary);

                // do the comparison with or without the TargetUrl
                // and then merge the TargetUrl to a highly similar notification
                var existingAlike = Db.GetNotificationsByJson(notification.AsJson).FirstOrDefault();

                if (existingAlike == null)
                {
                    Db.Insert(notification);
                    Log.Debug("Stored new notification.", false);
                    return notification;
                }
                else
                {
                    existingAlike.Received = notification.Received;
                    existingAlike.ReceiveCount++;
                    Db.Insert(existingAlike);
                    Log.Debug("Increased count on existing notification.", false);
                    return existingAlike;
                }
            }
            catch (Exception e)
            {
                Log.Error("Unable to store notification.", false, e);
                return null;
            }
        } // release lock
    }

    private void EraseUserToken()
    {
        Log.Debug("Erasing user secrets.", false);
        Config.UserToken = null;
        Db.SavePushConfiguration(Config);
    }

    internal void EraseOutstandingPPI()
    {
        EraseNotifications();
        // Logs do not contain PPI, but may be useful for debugging issues
        // EraseLogs();
    }

    private void EraseNotifications()
    {
        Log.Debug("Erasing notifications.", false);
        int deleted = Db.EraseNotificationRecords();
        Log.Debug($"Erased {deleted} notification records.", false);
    }

    private void EraseLogs()
    {
        Log.Debug("Erasing logs.", false);
        int deleted = Db.EraseLogRecords();
        Log.Debug($"Erased {deleted} log records.", false);
    }

    #endregion

    #region API

    internal async Task RecheckTokenAsync()
    {
        await CheckTokenWithPortalAsync(Config.PushToken);
    }

    private async Task CheckTokenWithPortalAsync(string token)
    {
        // don't process if there's no token
        if (string.IsNullOrWhiteSpace(token))
        {
            Log.Warning("Blank push token received.", false);
            return;
        }

        Log.Info("Checking token with portal.", false);
        Log.Verbose(token, true);
        if (Config.PushToken == null || !Config.PushToken.Equals(token)) { StoreNewPushToken(token); }
        RegistrationState = RegistrationStates.NotKnown;

        // do the check
        LastActivity = new AppActivity() { State = AppEvents.PushTokenCheckInitiated };
        var tokenCheckResponse = await SubmitPushTokenCheckSafelyAsync(Config.PushToken);

        LastActivity = new AppActivity() { State = AppEvents.PushTokenCheckResult, Result = tokenCheckResponse };
        if (tokenCheckResponse.IsSuccess)
        {
            RegistrationState = RegistrationStates.Registered;
        }
        else
        {
            // if not already registered, and we have a token, submit automatically
            if (!string.IsNullOrWhiteSpace(Config.UserToken))
            {
                await RegisterUserTokenAsync(Config.UserToken, userInitiated: false);
            }
            else
            {
                RegistrationState = RegistrationStates.NotRegistered;
            }
        }
    }

    private async Task<ServerResponse> SubmitPushTokenCheckSafelyAsync(string token)
    {
        try
        {
            Log.Debug("Submitting push token check...", false);
            var response = await Api.SubmitDeviceCheckAsync(token, ConnectDevice.UUID);
            return response;
        }
        catch (Exception e)
        {
            var response = ServerResponse.From(e);
            return response;
        }
    }

    internal async Task<ServerResponse> GetUserTokenAndRegisterAsync(string username, string password, bool userInitiated)
    {
        Log.Debug("Retrieving user token...", false);
        LastActivity = new AppActivity() { State = AppEvents.GetUserTokenInitiated };

        var response = await Api.GetUserTokenAsync(username, password);

        LastActivity = new AppActivity() { State = AppEvents.GetUserTokenResult, Result = response };
        if (response.IsSuccess)
        {
            if (userInitiated) { EraseOutstandingPPI(); }
            StoreUserToken(response.UserToken);
            return await RegisterUserTokenAsync(response.UserToken, userInitiated);
        }
        else
        {
            // failure
            if (userInitiated) { EraseOutstandingPPI(); }
            EraseUserToken();
            RegistrationState = RegistrationStates.NotRegistered;
            return response;
        }
    }

    internal async Task<ServerResponse> RegisterUserTokenAsync(string userToken, bool userInitiated)
    {
        Log.Debug("Submitting portal registration (user token)...", false);
        LastActivity = new AppActivity() { State = AppEvents.SubmitRegistrationInitiated };
        var response = await Api.SubmitUserTokenRegistrationAsync(
            userToken,
            Config.PushToken,
            ConnectDevice.UUID,
            ConnectDevice.DeviceDescription);

        LastActivity = new AppActivity() { State = AppEvents.SubmitRegistrationResult, Result = response };
        if (response.IsSuccess)
        {
            if (userInitiated) { EraseOutstandingPPI(); }
            RegistrationState = RegistrationStates.Registered;
        }
        else
        {
            RegistrationState = RegistrationStates.NotRegistered;
        }

        return response;
    }

    [Obsolete("This method of registration is deprecated.")]
    internal async Task<ServerResponse> RegisterUsernamePasswordAsync(string username, string password, bool userInitiated)
    {
        Log.Error("Portal registration deprecated method (username, password) no longer supported.", false);
        throw new NotImplementedException("This method of registration is deprecated.");

        //LastActivity = new AppActivity() { State = AppEvents.SubmitRegistrationInitiated };
        //var response = await Api.SubmitUsernamePasswordRegistrationAsync(
        //    username,
        //    password,
        //    Config.PushToken,
        //    ConnectDevice.UUID,
        //    ConnectDevice.DeviceDescription);

        //LastActivity = new AppActivity() { State = AppEvents.SubmitRegistrationResult, Result = response };
        //if (response.IsSuccess)
        //{
        //    if (userInitiated) { EraseOutstandingPPI(); }
        //    RegistrationState = RegistrationStates.Registered;
        //}
        //else
        //{
        //    RegistrationState = RegistrationStates.NotRegistered;
        //}

        //return response;
    }


    internal async Task<ServerResponse> DeregisterAsync()
    {
        Log.Debug("Submitting portal deregistration...", false);
        LastActivity = new AppActivity() { State = AppEvents.SubmitDeregistrationInitiated };
        var response = await Api.SubmitPortalDeregistrationAsync(Config.PushToken, ConnectDevice.UUID);
        if (response.IsSuccess)
        {
            EraseUserToken();
            EraseOutstandingPPI();
            OnNotificationsErased?.Invoke();
        }

        LastActivity = new AppActivity() { State = AppEvents.SubmitDeregistrationResult, Result = response };
        if (response.IsSuccess)
        {
            RegistrationState = RegistrationStates.NotRegistered;
        }

        return response;
    }

    #endregion

    #region Lifecycle

    protected override void OnStart()
    {
        Log.Info("App start.", false);

        // If there's already a push token, CrossFirebasePushNotification won't refresh it.
        // Instead, confirm it's still registered with the portal.
        Config = InitConfiguration();
        if (!string.IsNullOrWhiteSpace(Config.PushToken))
        {
            Task.Run(async () => { await CheckTokenWithPortalAsync(Config.PushToken); });
        }
    }

    protected override void OnSleep()
    {
        Log.Debug("App sleep.", false);
    }

    protected override void OnResume()
    {
        Log.Debug("App resume.", false);
        // If there's already a push token, CrossFirebasePushNotification won't refresh it.
        // Instead, confirm it's still registered with the portal.
        if (!string.IsNullOrWhiteSpace(Config.PushToken))
        {
            Task.Run(async () => { await CheckTokenWithPortalAsync(Config.PushToken); });
        }
    }

    #endregion
}

