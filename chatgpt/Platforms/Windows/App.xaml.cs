using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using WeatherTwentyOne.Services;
using Windows.Graphics;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace chatgpt.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
	private  bool openFlag = true;
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    static Mutex? __SingleMutex;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
        if (!IsSingleInstance())
        {
            //Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
            return;
        }

        base.OnLaunched(args);
        SetupTrayIcon();
	}
    static bool IsSingleInstance()
    {
        const string applicationId = "813342EB-7796-4B13-98F1-14C99E778C6E";
        __SingleMutex = new Mutex(false, applicationId);
        GC.KeepAlive(__SingleMutex);

        try
        {
            return __SingleMutex.WaitOne(0, false);
        }
        catch (Exception)
        {
            __SingleMutex.ReleaseMutex();
            return __SingleMutex.WaitOne(0, false);
        }

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
				
                if (!openFlag)
                {
                    if (!WeatherTwentyOne.WindowExtensions.Visible)
                    {
                        WeatherTwentyOne.WindowExtensions.Show();
                        openFlag = false;
                        WeatherTwentyOne.WindowExtensions.Visible = true;
                    }
                    else
                    {
                        openFlag = true;
                        WeatherTwentyOne.WindowExtensions.Hide();
                    }
                }
                else
                {
                    openFlag = false;
                    WeatherTwentyOne.WindowExtensions.Show();
                    WeatherTwentyOne.WindowExtensions.Visible = true;
                }
                

                //WeatherTwentyOne.ServiceProvider.GetService<INotificationService>()
                //    ?.ShowNotification("Hello Build! 😻 From .NET MAUI",
                //        "How's your weather?  It's sunny where we are 🌞");
            };
		}
	}

}

