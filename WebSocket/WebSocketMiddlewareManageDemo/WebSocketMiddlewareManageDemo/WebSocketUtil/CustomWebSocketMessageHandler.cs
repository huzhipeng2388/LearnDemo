using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketMiddlewareManageDemo.WebSocketUtil
{
    public class CustomWebSocketMessageHandler : ICustomWebSocketMessageHandler
    {
        public async Task BroadcastAll(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var all = wsFactory.All();
            foreach (var uws in all)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task BroadcastOthers(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var others = wsFactory.Others(userWebSocket);
            foreach (var uws in others)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer,0,buffer.Length),WebSocketMessageType.Text,true,CancellationToken.None);
            }
        }

        public async Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            string msg = Encoding.UTF8.GetString(buffer);
            try
            {
                var message = JsonConvert.DeserializeObject<CustomWebSocketMessage>(msg);
                if (message.Type == WSMessageType.test1)
                {
                    await BroadcastOthers(buffer, userWebSocket, wsFactory);
                }
            }
            catch(Exception e)
            {
                await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), 
                    result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        }

        public async Task SendInitialMessages(CustomWebSocket userWebSocket)
        {
            WebSocket webSocket = userWebSocket.WebSocket;
            var msg = new CustomWebSocketMessage {
                MessagDateTime = DateTime.Now,
                Type = WSMessageType.test1,
                Text = "订阅成功！",
                Username = "system"
            };
            string serialisedMessage = JsonConvert.SerializeObject(msg);
            byte[] bytes = Encoding.UTF8.GetBytes(serialisedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
