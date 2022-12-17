using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace chatgpt.Services.JSBridge
{
    internal static class WebViewExtentions
    {
        private const string LocalStorageProxy = @"
const signSetItem = localStorage.setItem
localStorage.setItem = function (key, val) {
    if (key.indexOf('response_csharp_') !== 0) {
        // 非csharp调用的直接return
        signSetItem.apply(this, arguments);
        return;
    }
    // 发送消息
    let event = new Event(key)
    event.key = key
    event.newValue = val
    window.dispatchEvent(event)
}
console.log('localStorage.setItem proxy success')
";

        public static void AddJsCallProxy(this WebView webview)
        {
            webview.Eval(LocalStorageProxy);
        }

        public static async Task<JsReqesut> GetJsCallRequestAsync(this WebView webview, string dataId)
        {
            var data = await webview.EvaluateJavaScriptAsync($"localStorage.getItem('{dataId}')");
            if (string.IsNullOrEmpty(data))
            {
                return new JsReqesut();
            }
            // 删除掉
            await webview.EvaluateJavaScriptAsync($"localStorage.removeItem('{dataId}')");
            var base64EncodedBytes = data.Replace("\\", "");
            if (base64EncodedBytes.StartsWith("\""))
            {
                base64EncodedBytes = base64EncodedBytes.Substring(1);
            }
            var realData = Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedBytes));
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var invokeData = JsonSerializer.Deserialize<JsReqesut>(realData, options);
            return invokeData;
        }
    }
}
