using Windows.UI.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using PInvoke;
using Windows.UI.WindowManagement;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
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
                        //p.Minimize(false);
                        p.IsAlwaysOnTop = false;
						p.IsResizable = false;
						p.IsMaximizable = false;
						p.IsMinimizable = false;
                     

                        //disable the close button action when clicked - close button still visible, but does not close the app
                        events.AddWindows(windows =>
                        {

						

                            window.Activated += (sender, args) =>
                            {
                                window.GetAppWindow()?.Hide();
                            } ;
						
                            windows.OnLaunched((a1, args) =>
							{

                                WeatherTwentyOne.WindowExtensions.Hwnd = nativeWindowHandle;
                                WeatherTwentyOne.WindowExtensions.Show = () =>
                                {
                                    WeatherTwentyOne.WindowExtensions.BringToFront();
                                    var displyArea = Microsoft.UI.Windowing.DisplayArea.Primary;
                                    var scalingFactor = ((MauiWinUIWindow)chatgpt.App.Current.Windows[0].Handler.PlatformView).GetDisplayDensity();
                                    var width = 500* scalingFactor;
                                    var height = 700 * scalingFactor;
                                    var startX = displyArea.WorkArea.Width - width;
                                    var startY = displyArea.WorkArea.Height - height;
                                    winuiAppWindow.MoveAndResize(new ((int)startX, (int)startY, (int)width, (int)height), displyArea);
                                    winuiAppWindow.Show();
                                };
                                WeatherTwentyOne.WindowExtensions.Hide = () =>
                                {
                                    winuiAppWindow.Hide();
                                };

                            });

							windows.OnClosed((window, args) =>
							{
								var trayService = WeatherTwentyOne.ServiceProvider.GetService<ITrayService>();
								if (!trayService.isDispose)
								{
									args.Handled = true;
                                    WeatherTwentyOne.WindowExtensions.Hide();
								}
							});

						});
					}
					winuiAppWindow.Hide();
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
