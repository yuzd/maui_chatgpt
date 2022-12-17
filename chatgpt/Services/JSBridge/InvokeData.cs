using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace chatgpt.Services.JSBridge
{
    internal class JsReqesut
    {
        [JsonIgnore]
        private object parsedObj = null;

        [JsonIgnore]
        private object response = null;

        public string Command { get; set; }
        public string Data { get; set; }
        public string Key { get; set; }


        public T GetOrParseRequest<T>()
        {
            parsedObj ??= typeof(T) == typeof(string) ? Data : JsonSerializer.Deserialize<T>(Data);
            return (T)parsedObj;
        }

        public JsResponse<T> GetOrCreateResponse<T>()
        {
            response ??= new JsResponse<T>(this);
            return (JsResponse<T>) response;
        }
    }

    internal class JsResponse<T>
    {
        [JsonIgnore]
        private readonly JsReqesut _reqesut;

        public JsResponse(JsReqesut reqesut)
        {
            _reqesut = reqesut;
        }

        public T Data { get; set; }
        public string ErrMessage { get; set; }

        /**
         * 写入结果到webview 防止出现乱码现象用base64传递
         */
        public Task WriteToWebViewAsync(WebView webView)
        {
            if (!string.IsNullOrEmpty(ErrMessage))
            {
                var err = Convert.ToBase64String(Encoding.UTF8.GetBytes("err:"+ ErrMessage));
                return webView.EvaluateJavaScriptAsync($"dotnet_response_csharp_set('{_reqesut.Key.Replace("request_csharp_", "response_csharp_")}','{err}')");
            }
            string jsonString = typeof(T) == typeof(string) ? Data.ToString() : JsonSerializer.Serialize(Data);
            var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
            return webView.EvaluateJavaScriptAsync($"dotnet_response_csharp_set('{_reqesut.Key.Replace("request_csharp_", "response_csharp_")}','{base64String}')");
        }
    }
}
