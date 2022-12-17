
using chatgpt.Services.JSBridge;
using chatgpt.Services.ChatGpt;

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
                        case "chat":
                            if (ChatService.Setting != null)
                            {
                                try
                                {
                                    var requestData2 = requesut.GetOrParseRequest<ChatRequest>();
                                    var reply = await ChatService.GetResponseDataAsync(requestData2.Msg);
                                    var responseData2 = requesut.GetOrCreateResponse<string>();
                                    var choices = reply.Choices;
                                    var Text = choices?.FirstOrDefault()?.Text.Trim();
                                    responseData2.Data = Text;
                                    await responseData2.WriteToWebViewAsync(MyWebView);
                                }
                                catch (Exception exception)
                                {
                                    var responseData3 = requesut.GetOrCreateResponse<string>();
                                    responseData3.Data = exception.Message;
                                    await responseData3.WriteToWebViewAsync(MyWebView);
                                }
                            }
                            else
                            {
                                var responseData3 = requesut.GetOrCreateResponse<string>();
                                responseData3.Data = "chatgpt初始化失败";
                                await responseData3.WriteToWebViewAsync(MyWebView);
                            }
                            break;
                        case "chatinit":
                            try
                            {
                                var requestData3 = requesut.GetOrParseRequest<Setting>();
                                ChatService.Setting = requestData3;
                                var responseData3 = requesut.GetOrCreateResponse<string>();
                                responseData3.Data = "chatgpt初始化成功";
                                await responseData3.WriteToWebViewAsync(MyWebView);
                            }
                            catch (Exception exception)
                            {
                                var responseData3 = requesut.GetOrCreateResponse<string>();
                                responseData3.Data = exception.Message;
                                await responseData3.WriteToWebViewAsync(MyWebView);
                            }
                            break;
                    }
                }

                Dispatcher.Dispatch(Action);
            });
        }
    }
}


