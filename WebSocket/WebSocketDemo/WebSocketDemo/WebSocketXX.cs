using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketDemo
{
    public class WebSocketXX
    {

        WebSocket socket;
        WebSocketXX(WebSocket socket)
        {
            this.socket = socket;
        }

        static List<WebSocket> list = new List<WebSocket>();

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private static async Task Acceptor(Microsoft.AspNetCore.Http.HttpContext httpContext, Func<Task> n)
        {

            if (!httpContext.WebSockets.IsWebSocketRequest)
                return;
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            list.Add(socket);
            var result = await RecvAsync(socket, CancellationToken.None);

        }

        /// <summary>
        /// 接收客户端数据
        /// </summary>
        /// <param name="webSocket">webSocket 对象</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string> RecvAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            string oldRequestParam = "";
            WebSocketReceiveResult result;
            do
            {
                var ms = new MemoryStream();
                var buffer = new ArraySegment<byte>(new byte[1024 * 8]);
                result = await webSocket.ReceiveAsync(buffer, cancellationToken);
                if (result.MessageType.ToString() == "Close")
                {
                    list.Remove(webSocket);
                    break;
                }
                ms.Write(buffer.Array, buffer.Offset, result.Count - buffer.Offset);
                ms.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(ms);
                var s = reader.ReadToEnd();
                reader.Dispose();
                ms.Dispose();
                if (!string.IsNullOrEmpty(s))
                {             
                    await SendAsync(s, webSocket);
                }
                oldRequestParam = s;

            } while (result.EndOfMessage);

            return "";
        }

        /// <summary>
        /// 向客户端发送数据 
        /// </summary>
        /// <param name="msg">数据</param>
        /// <param name="webSocket">socket对象  sleep 心跳周期</param>
        /// <returns></returns>
        public static async Task SendAsync(string msg, WebSocket webSocket)
        {
            try
            {   
                //业务逻辑
                CancellationToken cancellation = default(CancellationToken);
                var buf = Encoding.UTF8.GetBytes(msg+":WebSocket连接正常！");
                var segment = new ArraySegment<byte>(buf);
                await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, cancellation);
            }
            catch (Exception ex)
            {
                list.Remove(webSocket);
            }
        }

        /// <summary>
        /// 向客户端所有连接用户发送
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="webSocket"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task SendAsyncAll<TEntity>(TEntity entity)
        {
            foreach (WebSocket item in list)
            {
                var Json = JsonConvert.SerializeObject(entity);
                var bytes = Encoding.UTF8.GetBytes(Json);

                await item.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
            }      
        }

        /// 路由绑定处理
        /// </summary>
        /// <param name="app"></param>
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(WebSocketXX.Acceptor);
        }
    }
}
