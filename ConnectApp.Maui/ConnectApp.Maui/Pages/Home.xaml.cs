using Plugin.Firebase.CloudMessaging;

namespace ConnectApp.Maui.Pages;

public partial class Home : ContentPage
{
	int count;
	bool enabled;

	public Home()
	{
		InitializeComponent();
		count = Preferences.Get("count", 0); // init from persisted
    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		Preferences.Set("count", count); // persist across user sessions

		SemanticScreenReader.Announce(CounterBtn.Text);

        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        await DisplayAlert("FCM token", token, "OK");
    }
}
