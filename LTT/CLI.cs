using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using LTT.Loopback;

namespace LTT
{
    internal class CLI
    {
        AutoResetEvent _readQueueMutex;
        string _com;

        public int Execute(Dictionary<ArgumentNames, string> args)
        {
            int baudrate = 115200;
            _com = args[ArgumentNames.ComPort];

            _readQueueMutex = new AutoResetEvent(false);
            I_Loopback vcp = new ComPort(_readQueueMutex, baudrate);
            vcp.OnError += VCP_OnError;

            Run(vcp);

            return 0;
        }

        public void Run(I_Loopback vcp)
        {
            byte[] data = new byte[] { 0xAA };
            Stopwatch sw = new Stopwatch();
            sw.Start();
            vcp.Init(_com);


            for (int i = 0; i < 25; i++)
            {
                sw.Restart();
                vcp.Write(data);

                if (_readQueueMutex.WaitOne(1000))
                {
                    //Deque until 0xAA is found
                    byte c;
                    while (!vcp.ReceivedBytes.TryDequeue(out c)) ;
                    if (c == 0xAA)
                    {
                        sw.Stop();
                        Console.WriteLine("Processing took {0} ms", sw.ElapsedMilliseconds);
                    }
                }
                else
                {
                    Console.WriteLine("Error: Timeout");
                    return;
                }
            }


            vcp.Dispose();
        }

        private void VCP_OnError(object sender, string e)
        {
            Console.WriteLine($"Error: {e}");
        }
    }
}
