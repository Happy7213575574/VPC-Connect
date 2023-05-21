using System;
namespace ConnectApp.Maui.Pages.Models
{
	public class AppShellModel : AbstractBaseModel
	{
		public ImageSource FlyoutIcon { get; set; }
		public string Title { get; set; }

		public AppShellModel(string title, ImageSource flyout) : base()
		{
			Title = title;
			FlyoutIcon = flyout;
		}
	}
}

