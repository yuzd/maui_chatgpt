using chatgpt.Platforms.Windows.NativeWindowing;
using chatgpt.Services;

namespace chatgpt.Platforms.Windows;

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
            Text = "退出",
			Command = new Command(() => {
				tray.dispose();
				dispose = true;
				ExistHandler?.Invoke();
			})
		});

        tray.ContextMenuStrip.Items.Add(new ContextMenuStrip.MenuItem
        {
            Icon = Resource.trayicon,
            Text = "打开",
            Command = new Command(() => {
                ClickHandler?.Invoke();
            })
        });

        tray.LeftClick = () => {
            ClickHandler?.Invoke();
        };
    }
}
