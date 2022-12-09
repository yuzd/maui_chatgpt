using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using WeatherTwentyOne.Services;
using WeatherTwentyOne.WinUI;

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
		builder.ConfigureLifecycleEvents(lifecycle => {
#if WINDOWS
			//lifecycle
			//    .AddWindows(windows =>
			//        windows.OnNativeMessage((app, args) => {
			//            if (WindowExtensions.Hwnd == IntPtr.Zero)
			//            {
			//                WindowExtensions.Hwnd = args.Hwnd;
			//                WindowExtensions.SetIcon("Platforms/Windows/trayicon.ico");
			//            }
			//        }));

			lifecycle.AddWindows(windows => windows.OnWindowCreated((del) => {
				del.ExtendsContentIntoTitleBar = true;
			}));
#endif
		});

		var services = builder.Services;
#if WINDOWS
		services.AddSingleton<ITrayService, TrayService>();
		services.AddSingleton<INotificationService, NotificationService>();
#elif MACCATALYST
            //services.AddSingleton<ITrayService, MacCatalyst.TrayService>();
            //services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();
#endif
		var app =  builder.Build();

		return app;
	}


	
}
