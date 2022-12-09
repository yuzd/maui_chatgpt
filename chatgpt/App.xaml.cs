namespace chatgpt;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);

		const int newWidth = 800;
		const int newHeight = 600;

		window.Width = newWidth;
		window.Height = newHeight;
		


		return window;
	}
}
