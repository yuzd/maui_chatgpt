#if WINDOWS

using Microsoft.Web.WebView2.Core;
#endif
using System.Diagnostics;
using System.Text;
using WeatherTwentyOne.Services;
namespace chatgpt;

public partial class MainPage : ContentPage
{
	static bool isSetup = false;

	public MainPage()
	{
		InitializeComponent();

		if (!isSetup)
		{
			isSetup = true;

			SetupTrayIcon();

			ModifyWebView();
		}


	}
	private void SetupTrayIcon()
	{
		var trayService = WeatherTwentyOne.ServiceProvider.GetService<ITrayService>();

		if (trayService != null)
		{
			trayService.Initialize();
			trayService.ClickHandler = () =>
				WeatherTwentyOne.ServiceProvider.GetService<INotificationService>()
					?.ShowNotification("Hello Build! 😻 From .NET MAUI", "How's your weather?  It's sunny where we are 🌞");
		}
	}

	void ModifyWebView()
	{
#if WINDOWS
		Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
		{

			var webView = handler.PlatformView; // Get the native android webview.
			CoreWebView2EnvironmentOptions Options = new CoreWebView2EnvironmentOptions();
			Options.AdditionalBrowserArguments = "--proxy-server=http://127.0.0.1:1080";
			var env = CoreWebView2Environment.CreateWithOptionsAsync(null, null, Options).GetResults();
			webView.EnsureCoreWebView2Async();
			var aa = webView.CoreWebView2;

			//string filter = "*://west-wind.com/*";   // or "*" for all requests
			//webView.CoreWebView2.AddWebResourceRequestedFilter(filter,
			//	CoreWebView2WebResourceContext.All);
			//webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;


		});


		void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
		{
			var headers = e.Request.Headers;
			string postData = null;
			var content = e.Request.Content.AsStreamForRead();

			// get content from stream
			if (content != null)
			{
				using (var ms = new MemoryStream())
				{
					content.CopyTo(ms);
					ms.Position = 0;
					postData = Encoding.UTF8.GetString(ms.ToArray());
				}
			}
			var url = e.Request.Uri.ToString();

			// collect the headers from the collection into a string buffer
			StringBuilder sb = new StringBuilder();
			foreach (var header in headers)
			{
				sb.AppendLine($"{header.Key}: {header.Value}");
			}

			// for demo write out captured string vals
			Debug.WriteLine($"{url}\n{sb.ToString()}\n{postData}\n---");
		}
#endif
	}

}

