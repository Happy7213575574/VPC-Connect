using Android.App;
using Android.Runtime;
using Firebase;

namespace ConnectApp.Maui;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        //FirebaseApp.InitializeApp(this);
        base.OnCreate();
    }

}

