using ConnectApp.Maui.Pages;
using ConnectApp.Maui.Pages.Models;

namespace ConnectApp.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		//BindingContext = new AppShellModel(Title, FlyoutIcon);
	}

	public async Task SwitchToPageAsync(PageTypes page)
	{
		await GoToAsync("//" + page.ToString().ToLower());
	}
}

