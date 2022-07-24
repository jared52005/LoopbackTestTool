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
        public int Execute(Dictionary<ArgumentNames, object> args)
        {           
            I_Loopback vcp = new ComPort((int)args[ArgumentNames.Baudrate]);
            vcp.OnError += VCP_OnError;
            vcp.Init(args[ArgumentNames.ComPort].ToString());

            LoopbackWorker lw = new LoopbackWorker();
            LoopbackWorkerSetup lws = new LoopbackWorkerSetup()
            {
                Sink = vcp,
                LoopbackType = (LoopbackType)args[ArgumentNames.LoopbackType],
                PacketSizeStart = (int)args[ArgumentNames.PacketSizeStart],
                PacketSizeEnd = (int)args[ArgumentNames.PacketSizeEnd],
                Count = (int)args[ArgumentNames.Count],
            };

            lw.Start(lws);

            //Race condition
            do
            {
                Thread.Sleep(500);
            }
            while (lw.IsRunning);

            int exitCode = 0;
            if(lw.Error != null)
            {
                Console.WriteLine("Error: " + lw.Error.Message);
                exitCode = -1;
            }

            return exitCode;
        }

        private void VCP_OnError(object sender, string e)
        {
            Console.WriteLine($"Error: {e}");
        }
    }
}
