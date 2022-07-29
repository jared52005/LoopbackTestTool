using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using LTT.Loopback;

namespace LTT
{
    public partial class Form1 : Form
    {
        LoopbackWorker _lw;
        Dictionary<string, object> _calculatedValues;

        public Form1()
        {
            
            InitializeComponent();

            _lw = new LoopbackWorker();

            //List elements
            _calculatedValues = new Dictionary<string, object>();
            _calculatedValues.Add("Throughput [req/s]", (float)0); //How many datagrams per second are we able to push through
            _calculatedValues.Add("Average response [ms]", (float)0); //Average response of device to request
            _calculatedValues.Add("Throughput [kByte/s]", (float)0); //How many kilobytes per second are we able to push through
            _calculatedValues.Add("Transferred [kByte]", (float)0);
            _calculatedValues.Add("Processed datagrams", (int)0); //How many datagrams did we processed
            _calculatedValues.Add("Request size", (int)0); //Size of a symbol in bytes
            _calculatedValues.Add("Run Time [minutes]", (int)0);

            //Load elements into list view
            SetDoubleBuffered(listView1);
            listView1.Items.Clear();
            foreach (string key in _calculatedValues.Keys)
            {
                ListViewItem lvi = new ListViewItem(key);
                lvi.SubItems.Add(_calculatedValues[key].ToString());
                listView1.Items.Add(lvi);
            }

            timer1.Start();
        }

        public static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void ShowError(string text)
        {
            MessageBox.Show(text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private int ConvertNumberWithSuffix(TextBox tx)
        {
            tx.BackColor = Color.White;
            try
            {
                //Check if ends with k/m/g => add proper amount of zeros instead, kilo-mega-giga
                string sVal = tx.Text.ToLower();
                sVal = sVal.Replace("k", "000");
                sVal = sVal.Replace("m", "000000");
                sVal = sVal.Replace("g", "000000000");
                return Convert.ToInt32(sVal);

            }
            catch
            {
                tx.BackColor = Color.Yellow;
                ShowError($"Error: '{tx.Text}' is not a number");
                return -1;
            }
        }

        private void button_ComTest_Click(object sender, EventArgs e)
        {
            if(_lw.IsRunning)
            {
                ShowError("Loopback testing is already running");
                return;
            }

            //Load variables from GUI

            int start = ConvertNumberWithSuffix(textBox_StartSize);
            if(start < 0)
            {
                return;
            }
            int end = ConvertNumberWithSuffix(textBox_EndSize);
            if (end < 0)
            {
                return;
            }
            int count = ConvertNumberWithSuffix(textBox_Count);
            if (count < 0)
            {
                return;
            }

            int baudrate = ConvertNumberWithSuffix(textBox_baudrate);
            if (baudrate < 0)
            {
                return;
            }

            //VCP setup

            I_Loopback vcp = new ComPort(baudrate);
            //vcp.OnError += VCP_OnError;
            try
            {
                vcp.Init(textBox_comPort.Text);
            }
            catch (Exception ex)
            {
                vcp.Dispose();
                ShowError(ex.Message);
                return;
            }

            LoopbackWorkerSetup lws = new LoopbackWorkerSetup()
            {
                Sink = vcp,
                LoopbackType = LoopbackType.Dynamic,
                PacketSizeStart = start,
                PacketSizeEnd = end,
                Count = count,
            };

            _lw.Start(lws);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_lw.IsRunning)
            {
                return;
            }

            //Update dictionary with calcualted values

            int packetTransferred = _lw.Tests.Count;
            long transferredBytes = _lw.TransferredBytes;

            TimeSpan ts = DateTime.Now - _lw.StartTime;
            _calculatedValues["Processed datagrams"] = packetTransferred.ToString();
            float symbolsPerSecond = (packetTransferred / ((float)ts.TotalMilliseconds / 1000));
            _calculatedValues["Throughput [req/s]"] = symbolsPerSecond.ToString();
            float averageResponseTime = (((float)ts.TotalMilliseconds) / packetTransferred);
            _calculatedValues["Average response [ms]"] = averageResponseTime.ToString();

            float kbps = ((transferredBytes / ((float)ts.TotalMilliseconds / 1000)) / 1024);
            _calculatedValues["Throughput [kByte/s]"] = kbps.ToString();

            _calculatedValues["Transferred [kByte]"] = transferredBytes / 1024;
            _calculatedValues["Run Time [minutes]"] = (long)(ts.TotalMilliseconds / 60000);

            _calculatedValues["Request size"] = "0x" + _lw.Tests[packetTransferred - 1].PacketTx.Length.ToString("X");

            //Update list view with new values
            foreach (ListViewItem lvi in listView1.Items)
            {
                UpdateSubitem(lvi, _calculatedValues);
            }
        }

        private void UpdateSubitem(ListViewItem item, Dictionary<string, object> values)
        {
            item.SubItems[1].Text = values[item.SubItems[0].Text].ToString();
        }
    }
}
