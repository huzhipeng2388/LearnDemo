using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocketMiddlewareManageDemo.WebSocketUtil
{
    public class CustomWebSocket
    {
        public WebSocket WebSocket { get; set; }
        public string Username { get; set; }
    }
}
