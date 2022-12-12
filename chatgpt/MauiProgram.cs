using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
#if WINDOWS
using WeatherTwentyOne.Services;
using WeatherTwentyOne.WinUI;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.Maui.LifecycleEvents;
#endif

namespace chatgpt;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if WINDOWS



		builder.ConfigureLifecycleEvents(events =>
		{
			events.AddWindows(wndLifeCycleBuilder =>
			{
				wndLifeCycleBuilder.OnWindowCreated(window =>
				{

					IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
					WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
					AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

					//hide the minimize and maximize button in title bar
					window.ExtendsContentIntoTitleBar = false;

					if (winuiAppWindow.Presenter is OverlappedPresenter p)
					{
						//p.Maximize();
						p.IsAlwaysOnTop = false;
						p.IsResizable = false;
						p.IsMaximizable = false;
						p.IsMinimizable = false;


						//disable the close button action when clicked - close button still visible, but does not close the app
						events.AddWindows(windows =>
						{

							windows.OnLaunched((a1, args) =>
							{
								//window.GetAppWindow().Hide();
							});

							windows.OnClosed((window, args) =>
							{
								var trayService = WeatherTwentyOne.ServiceProvider.GetService<ITrayService>();
								if (!trayService.isDispose)
								{
									args.Handled = true;
									window.GetAppWindow().Hide();
								}

							});

						});
					}

					const int width = 500;
					const int height = 700;
					winuiAppWindow.MoveAndResize(new RectInt32(1920 / 2 - width / 2, 1080 / 2 - height / 2, width, height));
				});
			});
		});
#endif

		var services = builder.Services;
#if WINDOWS
		services.AddSingleton<ITrayService, TrayService>();
		services.AddSingleton<INotificationService, NotificationService>();
#elif MACCATALYST
            //services.AddSingleton<ITrayService, MacCatalyst.TrayService>();
            //services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();
#endif
		var app = builder.Build();
		return app;
	}




}
