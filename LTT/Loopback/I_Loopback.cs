using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace LTT.Loopback
{
    interface I_Loopback : IDisposable
    {
        ConcurrentQueue<byte> ReceivedBytes { get; }
        AutoResetEvent ReadQueueMutex { get; }

        event EventHandler<string> OnError;
        event EventHandler OnAbort;

        void Init(string path);
        void Write(byte[] frame);
    }
}
