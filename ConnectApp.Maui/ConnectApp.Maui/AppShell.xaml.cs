using ConnectApp.Maui.Pages;

namespace ConnectApp.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

	public async Task SwitchToPageAsync(PageTypes page)
	{
		// TODO: implement switch to page
		await GoToAsync("//" + page.ToString().ToLower());
	}

	public string FooterText => "ABC";
}

