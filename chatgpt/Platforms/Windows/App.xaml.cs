using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using WeatherTwentyOne.Services;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace chatgpt.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    private bool isShow = true;
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();


	protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
		base.OnLaunched(args);
		WeatherTwentyOne.WindowExtensions.Hwnd = ((MauiWinUIWindow)chatgpt.App.Current.Windows[0].Handler.PlatformView).WindowHandle;
		SetupTrayIcon();
	}

	private void SetupTrayIcon()
	{
		var trayService = WeatherTwentyOne.ServiceProvider.GetService<ITrayService>();

		if (trayService != null)
		{
			trayService.Initialize();
			trayService.ExistHandler = () => {

				((MauiWinUIWindow)chatgpt.App.Current.Windows[0].Handler.PlatformView).Close();
			};


			trayService.ClickHandler = () =>
            {
                if (isShow)
                {
                    isShow = false;
                    WeatherTwentyOne.WindowExtensions.BringToFront();
                    var nativeWindowHandle = ((MauiWinUIWindow) chatgpt.App.Current.Windows[0].Handler.PlatformView).WindowHandle;
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);
                    winuiAppWindow.Show(true);
                }
                else
                {
                    isShow = true;
                    ((MauiWinUIWindow)chatgpt.App.Current.Windows[0].Handler.PlatformView).Close();
                }
                

                //WeatherTwentyOne.ServiceProvider.GetService<INotificationService>()
                //    ?.ShowNotification("Hello Build! 😻 From .NET MAUI",
                //        "How's your weather?  It's sunny where we are 🌞");
            };
		}
	}

}

