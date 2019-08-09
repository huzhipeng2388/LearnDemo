using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketMiddlewareManageDemo.WebSocketUtil
{
    public interface ICustomWebSocketFactory
    {
        void Add(CustomWebSocket uws);
        void Remove(string username);
        List<CustomWebSocket> All();
        List<CustomWebSocket> Others(CustomWebSocket client);
        CustomWebSocket Client(string username);
    }
}
