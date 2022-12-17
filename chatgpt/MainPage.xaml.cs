
using chatgpt.Services.JSBridge;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

namespace chatgpt;

public partial class MainPage : ContentPage
{



    public MainPage()
    {
        InitializeComponent();
        MyWebView.Navigated += MyWebViewOnNavigated;
        MyWebView.Navigating += WebBrowserNavigating;
    }

    private void MyWebViewOnNavigated(object sender, WebNavigatedEventArgs e)
    {
        MyWebView.AddJsCallProxy();
    }

    private void WebBrowserNavigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.Contains("/api/"))
        {
            // 拦截请求 拿到 参数id《参数的具体值存在localstorage里面》
            var dataId = e.Url.Substring(e.Url.IndexOf("/api/", StringComparison.Ordinal) + 5);
            e.Cancel = true;
            Task.Run(() =>
            {
                async void Action()
                {
                    var requesut = await MyWebView.GetJsCallRequestAsync(dataId);
                    switch (requesut.Command)
                    {
                        case "test":
                            // 拿到请求具体的内容
                            var requestData = requesut.GetOrParseRequest<string>();
                            var responseData = requesut.GetOrCreateResponse<string>();
                            await Task.Delay(Random.Shared.Next(1000,3000));
                            responseData.Data = requestData + "——改造" + DateTime.Now.ToString("HH:mm:ss");
                            await responseData.WriteToWebViewAsync(MyWebView);
                            break;
                    }
                }

                Dispatcher.Dispatch(Action);
            });
        }
    }
}


