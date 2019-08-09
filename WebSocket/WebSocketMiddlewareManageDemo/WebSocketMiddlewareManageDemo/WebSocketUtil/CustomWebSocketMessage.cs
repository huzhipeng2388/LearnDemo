using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketMiddlewareManageDemo.WebSocketUtil
{
    public class CustomWebSocketMessage
    {
        public string Text { get; set; }
        public DateTime MessagDateTime { get; set; }
        public string Username { get; set; }
        public WSMessageType Type { get; set; }
    }
}
