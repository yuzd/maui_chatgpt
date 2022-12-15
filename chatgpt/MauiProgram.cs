using Windows.UI.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using chatgpt.Services;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
using ServiceProvider = chatgpt.Services.ServiceProvider;
using WindowExtensions = chatgpt.Platforms.Windows.WindowExtensions;
using chatgpt.Platforms.Windows;
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
                            WindowExtensions.Hwnd = nativeWindowHandle;
                            WindowExtensions.Show = () =>
                            {
                                WindowExtensions.BringToFront();
                                var displyArea = DisplayArea.Primary;
                                var scalingFactor = ((MauiWinUIWindow)Application.Current?.Windows[0].Handler.PlatformView)?.GetDisplayDensity() ?? 1;
                                var width = 500 * scalingFactor;
                                var height = 700 * scalingFactor;
                                var startX = displyArea.WorkArea.Width - width;
                                var startY = displyArea.WorkArea.Height - height;
                                winuiAppWindow.MoveAndResize(new((int)startX, (int)startY, (int)width, (int)height), displyArea);
                                winuiAppWindow.Show();
                            };
                            WindowExtensions.Hide = () =>
                            {
                                winuiAppWindow.Hide();
                            };
                            WindowExtensions.Close = () =>
                            {
                                ((MauiWinUIWindow)Application.Current?.Windows[0].Handler.PlatformView)?.Close();
                            };


                            window.Activated += (_, _) =>
                            {
                                WindowExtensions.Hide();
                            };

                            windows.OnLaunched((_, _) =>
                            {

                            });

                            windows.OnClosed((_, args) =>
                            {
                                var trayService = ServiceProvider.GetService<ITrayService>();
                                if (!trayService.isDispose)
                                {
                                    args.Handled = true;
                                    WindowExtensions.Hide();
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
