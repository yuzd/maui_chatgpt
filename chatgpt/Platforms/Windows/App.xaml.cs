#nullable enable
using chatgpt.Services;
using Microsoft.UI.Xaml;
using ServiceProvider = chatgpt.Services.ServiceProvider;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace chatgpt.Platforms.Windows;

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
		var trayService = ServiceProvider.GetService<ITrayService>();

		if (trayService != null)
		{
			trayService.Initialize();
			trayService.ExistHandler = () => {

                WindowExtensions.Close();
			};


			trayService.ClickHandler = () =>
            {
				
                if (!openFlag)
                {
                    if (!WindowExtensions.Visible)
                    {
                        WindowExtensions.Show();
                        openFlag = false;
                        WindowExtensions.Visible = true;
                    }
                    else
                    {
                        openFlag = true;
                        WindowExtensions.Hide();
                    }
                }
                else
                {
                    openFlag = false;
                    WindowExtensions.Show();
                    WindowExtensions.Visible = true;
                }
                

                //WeatherTwentyOne.ServiceProvider.GetService<INotificationService>()
                //    ?.ShowNotification("Hello Build! 😻 From .NET MAUI",
                //        "How's your weather?  It's sunny where we are 🌞");
            };
		}
	}

}

