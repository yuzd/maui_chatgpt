using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatgpt.Services.ChatGpt
{
    public class Setting
    {
        public string ApiKey { get; set; }
        public string Temperature { get; set; }
        public string MaxTokens { get; set; }
    }

    public class ChatRequest
    {
        public string Msg { get; set; }
    }
}
