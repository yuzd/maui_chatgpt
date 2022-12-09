using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Internals;
using WeatherTwentyOne.Services;
using Application = Microsoft.Maui.Controls.Application;
using WindowsConfiguration = Microsoft.Maui.Controls.PlatformConfiguration.Windows;
namespace chatgpt;

public partial class MainPage : ContentPage
{
	static bool isSetup = false;

	public MainPage()
	{
		InitializeComponent();

		if (!isSetup)
		{
			isSetup = true;

			SetupTrayIcon();
		}
	}
	private void SetupTrayIcon()
	{
		var trayService = WeatherTwentyOne.ServiceProvider.GetService<ITrayService>();

		if (trayService != null)
		{
			trayService.Initialize();
			trayService.ClickHandler = () =>
				WeatherTwentyOne.ServiceProvider.GetService<INotificationService>()
					?.ShowNotification("Hello Build! 😻 From .NET MAUI", "How's your weather?  It's sunny where we are 🌞");
		}
	}
}

