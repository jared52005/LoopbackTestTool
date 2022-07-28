namespace LTT
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_Count = new System.Windows.Forms.TextBox();
            this.textBox_EndSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_StartSize = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Com = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage_Ble = new System.Windows.Forms.TabPage();
            this.tabPage_Tcp = new System.Windows.Forms.TabPage();
            this.tabPage_Ws = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_Com.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox_Count);
            this.groupBox1.Controls.Add(this.textBox_EndSize);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_StartSize);
            this.groupBox1.Location = new System.Drawing.Point(12, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Packet Setup";
            // 
            // textBox_Count
            // 
            this.textBox_Count.Location = new System.Drawing.Point(64, 45);
            this.textBox_Count.Name = "textBox_Count";
            this.textBox_Count.Size = new System.Drawing.Size(100, 20);
            this.textBox_Count.TabIndex = 6;
            // 
            // textBox_EndSize
            // 
            this.textBox_EndSize.Location = new System.Drawing.Point(235, 19);
            this.textBox_EndSize.Name = "textBox_EndSize";
            this.textBox_EndSize.Size = new System.Drawing.Size(100, 20);
            this.textBox_EndSize.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "End Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Count";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Size";
            // 
            // textBox_StartSize
            // 
            this.textBox_StartSize.Location = new System.Drawing.Point(64, 19);
            this.textBox_StartSize.Name = "textBox_StartSize";
            this.textBox_StartSize.Size = new System.Drawing.Size(100, 20);
            this.textBox_StartSize.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_Com);
            this.tabControl1.Controls.Add(this.tabPage_Ble);
            this.tabControl1.Controls.Add(this.tabPage_Tcp);
            this.tabControl1.Controls.Add(this.tabPage_Ws);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(517, 152);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage_Com
            // 
            this.tabPage_Com.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Com.Controls.Add(this.button1);
            this.tabPage_Com.Controls.Add(this.label5);
            this.tabPage_Com.Controls.Add(this.textBox2);
            this.tabPage_Com.Controls.Add(this.label4);
            this.tabPage_Com.Controls.Add(this.textBox1);
            this.tabPage_Com.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Com.Name = "tabPage_Com";
            this.tabPage_Com.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Com.Size = new System.Drawing.Size(509, 126);
            this.tabPage_Com.TabIndex = 0;
            this.tabPage_Com.Text = "COM Port";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(64, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "COM Port";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(64, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 7;
            // 
            // tabPage_Ble
            // 
            this.tabPage_Ble.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Ble.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Ble.Name = "tabPage_Ble";
            this.tabPage_Ble.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Ble.Size = new System.Drawing.Size(509, 126);
            this.tabPage_Ble.TabIndex = 1;
            this.tabPage_Ble.Text = "BLE";
            // 
            // tabPage_Tcp
            // 
            this.tabPage_Tcp.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Tcp.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Tcp.Name = "tabPage_Tcp";
            this.tabPage_Tcp.Size = new System.Drawing.Size(509, 126);
            this.tabPage_Tcp.TabIndex = 2;
            this.tabPage_Tcp.Text = "TCP";
            // 
            // tabPage_Ws
            // 
            this.tabPage_Ws.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Ws.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Ws.Name = "tabPage_Ws";
            this.tabPage_Ws.Size = new System.Drawing.Size(509, 126);
            this.tabPage_Ws.TabIndex = 3;
            this.tabPage_Ws.Text = "WebSocket";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Baudrate";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Start COM Test";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 329);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Loopback Test Tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Com.ResumeLayout(false);
            this.tabPage_Com.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_StartSize;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Com;
        private System.Windows.Forms.TabPage tabPage_Ble;
        private System.Windows.Forms.TextBox textBox_Count;
        private System.Windows.Forms.TextBox textBox_EndSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage_Tcp;
        private System.Windows.Forms.TabPage tabPage_Ws;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}

