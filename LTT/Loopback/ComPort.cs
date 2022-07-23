using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace LTT.Loopback
{
    class ComPort : I_Loopback
    {
        #region Fields
        int _baudrate;
        SerialPort _serialPort; //Serial port, which we are using for communication with Cheetah protocol based Cable
        AutoResetEvent _readQueueMutex; //Mutex to signal queue reader that there are new data in queue
        Thread _threadReadSerialPort;
        CancellationTokenSource _readSerialPortCancel;
        ConcurrentQueue<byte> _rxBuffer;
        const int TIMEOUT_CLOSE_SECONDS = 1; //If COM port is not closed within N seconds, send the signal again
        #endregion

        /// <summary>
        /// Reference on RX buffer
        /// </summary>
        public ConcurrentQueue<byte> ReceivedBytes
        {
            get { return _rxBuffer; }
        }

        public event EventHandler<string> OnError;
        public event EventHandler OnByteReceived;
        public event EventHandler OnAbort;

        /// <summary>
        /// Create new VCP sink. Mutex is set always when byte is received on rxBuffer
        /// </summary>
        /// <param name="readQueueMutex"></param>
        /// <param name="rxBuffer"></param>
        /// <param name="baudrate">Set baudrate or use default 460800</param>
        public ComPort(AutoResetEvent readQueueMutex, int baudrate)
        {
            _readQueueMutex = readQueueMutex;
            _rxBuffer = new ConcurrentQueue<byte>();
            _baudrate = baudrate;
        }

        public void Init(string comPortName)
        {
            //Already inited
            if (_serialPort != null && _serialPort.IsOpen)
            {
                throw new Exception("Already connected");
            }

            if (string.IsNullOrEmpty(comPortName))
            {
                throw new Exception("Argument COM port name missing");
            }

            try
            {
                _serialPort = new SerialPort(comPortName);
                _serialPort.ErrorReceived += SerialPort_ErrorReceived;
                _serialPort.WriteTimeout = 1000;
                _serialPort.BaudRate = _baudrate;
                _serialPort.Open();
                bool result = _serialPort.IsOpen;

                //Serial port reading setup
                if (result)
                {
                    _readSerialPortCancel = new CancellationTokenSource();
                    _threadReadSerialPort = new Thread(() => ReadSerialBytesAsync(_readSerialPortCancel.Token));
                    _threadReadSerialPort.Start();
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(this, "Unable to open COM port. " + ex.Message);

                try
                {
                    _serialPort.Close();
                }
                catch { }
                finally
                {
                    _serialPort.Dispose();
                }

                //Check if given port still exist
                string[] ports = SerialPort.GetPortNames();
                if (!ports.Contains(comPortName))
                {
                    throw new Exception($"Can't continue to init again. {comPortName} does not seem to exist");
                }
                Thread.Sleep(2000);
            }
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            OnError?.Invoke(this, "[VCP Error] " + e.EventType.ToString());
        }

        private async void ReadSerialBytesAsync(CancellationToken ct)
        {
            if (!_serialPort.IsOpen)
            {
                return;
            }
            _serialPort.BaseStream.ReadTimeout = 0;
            int bytesToRead = 4096;
            byte[] receiveBuffer = new byte[bytesToRead];

            try
            {
                while ((!ct.IsCancellationRequested) && _serialPort.IsOpen)
                {
                    var numBytesRead = await _serialPort.BaseStream.ReadAsync(receiveBuffer, 0, bytesToRead, ct);
                    if (numBytesRead == 0)
                    {
                        continue;
                    }

                    //Copy data into buffer
                    for (int i = 0; i < numBytesRead; i++)
                    {
                        _rxBuffer.Enqueue(receiveBuffer[i]);
                        OnByteReceived?.Invoke(this, null);
                    }
                    //Signal thread which is reading from buffer, that data are ready
                    _readQueueMutex.Set();
                }
            }
            catch (System.IO.IOException ioex)
            {
                //When I will disconnect via Dispose(), IsCancellationRequested flag will be raised
                //When it won't happen, it is an error (i.e. User pulled out the device from USB)
                if (!ct.IsCancellationRequested)
                {
                    //Signal error
                    OnError?.Invoke(this, $"ReadSerialBytesAsync: {ioex}");
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"ReadSerialBytesAsync: ({ex.GetType()}) - {ex}");
            }

            //If serial port is not open anymre, but nobody requested cancellation
            //Then this is abort condition
            if(!_serialPort.IsOpen && (!ct.IsCancellationRequested))
            {
                OnAbort?.Invoke(this, null);
            }

            if (_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                }
                catch
                {
                    //Sometimes carshes
                }
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                int secondsWaitingOnComClose = 0;
                //Signal to reading thread that it is necessary to close
                if (_readSerialPortCancel != null)
                {
                    _readSerialPortCancel.Cancel();
                }
                else
                {
                    //Close it from this thread
                    secondsWaitingOnComClose = TIMEOUT_CLOSE_SECONDS;
                }

                //Wait until serial COM port is closed
                while (_serialPort.IsOpen)
                {
                    //Send again signal to close the COM port
                    if (secondsWaitingOnComClose >= TIMEOUT_CLOSE_SECONDS)
                    {
                        try
                        {
                            _serialPort.Close();
                        }
                        catch
                        {
                            //Sometimes carshes
                        }
                        OnAbort?.Invoke(this, null);
                        secondsWaitingOnComClose = 0;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        secondsWaitingOnComClose++;
                    }
                }

                _serialPort.Dispose();
            }
        }

        /// <summary>
        /// Write data on VCP
        /// </summary>
        /// <param name="frame"></param>
        public void Write(byte[] frame)
        {
            _serialPort.Write(frame, 0, frame.Length);
        }
    }
}
