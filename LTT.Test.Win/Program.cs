using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTT.Test.Win
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TcpServer server = new TcpServer();
            System.Threading.Thread.Sleep(500);
            //server.Connect("127.0.0.1", "Test Message");

            WebSocketServer server = new WebSocketServer();
            System.Threading.Thread.Sleep(500);
            server.Connect();
        }
    }
}
