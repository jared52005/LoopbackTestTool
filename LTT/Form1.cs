using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LTT.Loopback;

namespace LTT
{
    public partial class Form1 : Form
    {
        LoopbackWorker _lw;

        public Form1()
        {
            _lw = new LoopbackWorker();
            InitializeComponent();
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
        }
    }
}
