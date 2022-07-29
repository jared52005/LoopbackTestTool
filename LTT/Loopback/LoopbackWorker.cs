using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace LTT.Loopback
{
    class LoopbackWorker
    {
        BackgroundWorker _bw;
        LoopbackWorkerSetup _setup;
        List<LoopbackPacketItem> _tests;

        public bool IsRunning { get { return _bw.IsBusy; } }

        public Exception Error { get; private set; }

        /// <summary>
        /// List of all executed tests
        /// </summary>
        public List<LoopbackPacketItem> Tests { get { return _tests; } }

        public long TransferredBytes { get; private set; }

        public DateTime StartTime { get; private set; }

        public event EventHandler OnStart;
        public event EventHandler<Exception> OnEnd;
        public event EventHandler<int> OnProgress;

        public LoopbackWorker()
        {
            _bw = new BackgroundWorker();
            _bw.RunWorkerCompleted += WorkerCompleted;
            _bw.DoWork += DoWork;
            _tests = new List<LoopbackPacketItem>();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            int lastProgress = -1;
            _tests.Clear();
            TransferredBytes = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < _setup.Count; i++)
            {
                //Setup seize
                int size = _setup.PacketSizeStart;
                if(_setup.LoopbackType == LoopbackType.Dynamic)
                {
                    size += i;
                }
                if(size > _setup.PacketSizeEnd)
                {
                    size = _setup.PacketSizeEnd;
                }
                //Prepare packet
                LoopbackPacketItem lpi = new LoopbackPacketItem(size);
                sw.Restart();

                //Write packet
                _setup.Sink.Write(lpi.PacketTx);
                ReceivePacket(lpi);
                sw.Stop();
                lpi.Delay = sw.ElapsedMilliseconds;
                TransferredBytes = TransferredBytes + lpi.PacketTx.Length;

                Console.WriteLine(lpi);

                _tests.Add(lpi);

                progress = (_tests.Count * 100) / _setup.Count;
                if(progress != lastProgress)
                {
                    lastProgress = progress;
                    OnProgress?.Invoke(this, lastProgress);
                }
            }
        }

        private void ReceivePacket(LoopbackPacketItem lpi)
        {
            while (true)
            {
                if (_setup.Sink.ReadQueueMutex.WaitOne(1000))
                {
                    //Dequeue data
                    while (_setup.Sink.ReceivedBytes.Count > 0)
                    {
                        byte c;
                        while (!_setup.Sink.ReceivedBytes.TryDequeue(out c)) ;

                        //Avoid overflow
                        if(lpi.PacketRxPtr >= lpi.PacketRx.Length)
                        {
                            lpi.PacketRxPtr = lpi.PacketRx.Length - 1;
                        }

                        lpi.PacketRx[lpi.PacketRxPtr] = c;
                        lpi.PacketRxPtr++;
                    }

                    //Check if whole packet was received
                    if(lpi.PacketRxPtr == lpi.PacketTx.Length)
                    {
                        return;
                    }
                }
                else
                {
                    throw new Exception("Timeout");
                }
            }
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _setup.Sink.Dispose();
            Error = e.Error;
            OnEnd?.Invoke(this, Error);
        }

        public void Start(LoopbackWorkerSetup setup)
        {
            if(_bw.IsBusy)
            {
                return;
            }
            StartTime = DateTime.Now;
            _setup = setup;
            OnStart?.Invoke(this, null);
            _bw.RunWorkerAsync();
        }
    }

    class LoopbackPacketItem
    {
        /// <summary>
        /// Measure start and end of sent data
        /// </summary>
        public long Delay { get; set; }
        /// <summary>
        /// Packet transmitted into sink
        /// </summary>
        public byte[] PacketTx { get; }
        /// <summary>
        /// Data received from sink
        /// </summary>
        public byte[] PacketRx { get; }
        /// <summary>
        /// Position of last received byte
        /// </summary>
        public int PacketRxPtr { get; set; }

        public int Bps
        {
            get
            {
                if(Delay == 0)
                {
                    return 0;
                }

                //bps = (((TX+RX)*1000)/Delay[ms])

                int bps = (PacketTx.Length + PacketRx.Length) * 1000;
                bps = bps / (int)Delay;
                return bps;
            }
        }

        public bool IsValid
        {
            get
            {
                if(PacketTx == null || PacketRx == null)
                {
                    return false;
                }

                if(PacketTx.Length != PacketRx.Length)
                {
                    return false;
                }

                return PacketTx.SequenceEqual(PacketRx);
            }
        }

        public LoopbackPacketItem(int size)
        {
            PacketTx = new byte[size + 1];
            PacketRx = new byte[size + 1];
            PacketRxPtr = 0;
            Delay = 0;

            //Sequential generation of packet as 0x20 ~ 0x7E
            for (int i = 0; i < size; i++)
            {
                byte c = (byte)i;
                if (c > 0x7E)
                {
                    c %= 0x7E;
                }
                if (c < 0x20)
                {
                    c |= 0x20;
                }
                PacketTx[i] = c;
            }
            PacketTx[size] = 0x00; //Stop character.
        }

        public override string ToString()
        {
            string sBaudrate;
            if(Bps >= 1000)
            {
                sBaudrate = $"{Bps / 1000} kbps";
            }
            else
            {
                sBaudrate = $"{Bps} bps";
            }

            string sValid;
            if(IsValid)
            {
                sValid = "VALID";
            }
            else
            {
                sValid = "INVALID";
            }
            
            return $"[{Delay} ms][{PacketTx.Length} bytes][{sBaudrate}] - {sValid}";
        }
    }

    class LoopbackWorkerSetup
    {
        /// <summary>
        /// Loopback sink which we will use for testing
        /// </summary>
        public I_Loopback Sink { get; set; }
        /// <summary>
        /// Size of packet which we want to start with
        /// </summary>
        public int PacketSizeStart { get; set; }
        /// <summary>
        /// End size of packet
        /// </summary>
        public int PacketSizeEnd { get; set; }
        /// <summary>
        /// Loopback type
        /// </summary>
        public LoopbackType LoopbackType { get; set; }
        /// <summary>
        /// How many tests should be done
        /// </summary>
        public int Count { get; set; }
    }

    enum LoopbackType
    {
        /// <summary>
        /// Grow request packet from PacketSizeStart to PacketSizeEnd
        /// </summary>
        Dynamic,
        /// <summary>
        /// Keep request packet on PacketSizeStart
        /// </summary>
        Static,
    }
}
