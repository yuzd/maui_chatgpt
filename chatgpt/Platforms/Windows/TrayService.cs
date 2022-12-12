using chatgpt;
using Hardcodet.Wpf.TaskbarNotification.Interop;
using Microsoft.UI.Xaml;
using WeatherTwentyOne.Services;

namespace WeatherTwentyOne.WinUI;

public class TrayService : ITrayService
{
    WindowsTrayIcon tray;

	volatile bool dispose = false;

    public Action ClickHandler { get; set; }

	public Action ExistHandler { get; set; }

	public bool isDispose => dispose;

	public void Initialize()
    {
        tray = new WindowsTrayIcon(Resource.trayicon);

		tray.ContextMenuStrip.Items.Add(new ContextMenuStrip.MenuItem { 
            Icon = Resource.trayicon,
            Text = "Exit",
			Command = new Command(() => {
				tray.dispose();
				dispose = true;
				ExistHandler?.Invoke();
			})
		});

		tray.LeftClick = () => {
            WindowExtensions.BringToFront();
            ClickHandler?.Invoke();
        };
    }
}
