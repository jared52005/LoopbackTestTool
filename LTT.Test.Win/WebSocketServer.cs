using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace LTT.Test.Win
{
    internal class WebSocketServer
    {
        Thread _threadServer;

        public WebSocketServer()
        {
            _threadServer = new Thread(Server);
            _threadServer.Start();
        }

        public void Connect()
        {
            using (var ws = new WebSocket("ws://localhost:4649/Echo"))
            {
                // Set the WebSocket events.

                ws.OnOpen += (sender, e) => ws.Send("Hi, there!");

                ws.OnMessage += (sender, e) => {
                    var fmt = "[WebSocket Message] {0}";
                    var body = !e.IsPing ? e.Data : "A ping was received.";

                    Console.WriteLine(fmt, body);
                };

                ws.OnError += (sender, e) => {
                    var fmt = "[WebSocket Error] {0}";

                    Console.WriteLine(fmt, e.Message);
                };

                ws.OnClose += (sender, e) => {
                    var fmt = "[WebSocket Close ({0})] {1}";

                    Console.WriteLine(fmt, e.Code, e.Reason);
                };

                ws.Connect();

                Console.WriteLine("\nType 'exit' to exit.\n");

                while (true)
                {
                    Thread.Sleep(1000);
                    Console.Write("> ");

                    var msg = Console.ReadLine();

                    if (msg == "exit")
                        break;

                    // Send a text message.
                    ws.Send(msg);
                }
            }
        }

        public void Server()
        {
            var wssv = new WebSocketSharp.Server.WebSocketServer(4649);
            wssv.AddWebSocketService<Echo>("/Echo");
            wssv.Start();

            if (wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);

                foreach (var path in wssv.WebSocketServices.Paths)
                {
                    Console.WriteLine("- {0}", path);
                }
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            //wssv.Stop();
        }
    }

    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }
    }
}
