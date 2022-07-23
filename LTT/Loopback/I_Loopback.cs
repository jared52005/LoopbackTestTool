﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTT.Loopback
{
    interface I_Loopback : IDisposable
    {
        ConcurrentQueue<byte> ReceivedBytes { get; }

        event EventHandler<string> OnError;
        event EventHandler OnByteReceived;
        event EventHandler OnAbort;

        void Init(string path);
        void Write(byte[] frame);
    }
}
