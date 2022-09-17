using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Shared;
using Plugin.Firebase.CloudMessaging;
using ConnectApp.Maui.Services;
#if IOS
using Plugin.Firebase.iOS;
#else
using Plugin.Firebase.Android;
#endif

namespace ConnectApp.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        return MauiApp
            .CreateBuilder()
            .UseMauiApp<App>()
            .RegisterFonts()
            .RegisterServices()
            .RegisterFirebaseServices()
            .Build();
	}

    private static MauiAppBuilder RegisterFonts(this MauiAppBuilder builder)
    {
        return builder.ConfigureFonts(fonts =>
             {
                 fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                 fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
             });
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IPushNotificationService, PushNotificationService>();
        return builder;
    }

    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events => {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                CrossFirebase.Initialize(app, launchOptions, CreateCrossFirebaseSettings());
                return false;
            }));
#else
            events.AddAndroid(android => android.OnCreate((activity, state) =>
                CrossFirebase.Initialize(activity, state, CreateCrossFirebaseSettings())));
#endif
        });
        
        builder.Services.AddSingleton(_ => CrossFirebaseCloudMessaging.Current);

        return builder;
    }

    private static CrossFirebaseSettings CreateCrossFirebaseSettings()
    {
        return new CrossFirebaseSettings(
            isAnalyticsEnabled: true,
            isCloudMessagingEnabled: true);
    }
}

