using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;

using ZedGraph;

namespace AHRSInterface
{
    public partial class AHRSInterface : Form
    {

        int SC_H, SC_W;
        double formWidth, formHeight;
        int SC_Height;
        delegate void AppendTextCallback(string text, Color text_color);
        
        delegate void AppendLabel1TextCallback(string text);

        delegate void AppendLabel2TextCallback(string text);
        delegate void AppendLabel23TextCallback(string text);
		
        public AHRSInterface()
        {
            InitializeComponent();

            initializeSerialPort();

            sensor = new AHRS();
            

            // Set up event handlers
            sensor.PacketTimeoutEvent += new StateDelegate(TimeoutEventHandler);
            sensor.PacketReceivedEvent += new PacketDelegate(PacketReceivedEventHandler);
            sensor.DataReceivedEvent += new DataReceivedDelegate(DataReceivedEventHandler);
            sensor.PacketSentEvent += new PacketDelegate(PacketSentEventHandler);
            sensor.COMFailedEvent += new COMFailedDelegate(COMFailedEventHandler);
            sensor.PacketLabelEvent += new PacketLabel(PacketLabelHandler);

            renderTimer = new Timer();

            renderTimer.Interval = 20;
            renderTimer.Enabled = true;
            renderTimer.Tick += new System.EventHandler(OnRenderTimerTick);


            Pos_Pane = PosGraph.GraphPane;
            Pos_Pane.Title.Text = "Position controller";
            Pos_Pane.XAxis.Title.Text = "Time (s)";
            Pos_Pane.YAxis.Title.Text = "Output";

            Vel_Pane = VelGraph.GraphPane;
            Vel_Pane.Title.Text = "Velocity controller";
            Vel_Pane.XAxis.Title.Text = "Time (s)";
            Vel_Pane.YAxis.Title.Text = "Output";

            Cur_Pane = CurGraph.GraphPane;
            Cur_Pane.Title.Text = "Current controller";
            Cur_Pane.XAxis.Title.Text = "Time (s)";
            Cur_Pane.YAxis.Title.Text = "Output";

            Pos_Tar_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            Pos_Motor_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            
            Vel_Ext_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            Vel_Int_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            Vel_Motor_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            
            Cur_Ext_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            Cur_Int_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            Cur_Motor_List = new RollingPointPairList(SENSOR_GRAPH_POINTS);

            Pos_Tar_Line = Pos_Pane.AddCurve("Position Target", Pos_Tar_List, Color.Blue, SymbolType.None);
            Pos_Motor_Line = Pos_Pane.AddCurve("Motor position", Pos_Motor_List, Color.Red, SymbolType.None);

            Vel_Ext_Line = Vel_Pane.AddCurve("External Velocity", Vel_Ext_List, Color.Blue, SymbolType.None);
            Vel_Int_Line = Vel_Pane.AddCurve("Internal Velocity", Vel_Int_List, Color.Red, SymbolType.None);
            Vel_Motor_Line = Vel_Pane.AddCurve("Motor Velocity", Vel_Motor_List, Color.Green, SymbolType.None);

            Cur_Ext_Line = Cur_Pane.AddCurve("External Current", Cur_Ext_List, Color.Blue, SymbolType.None);
            Cur_Int_Line = Cur_Pane.AddCurve("Internal Current", Cur_Int_List, Color.Red, SymbolType.None);
            Cur_Motor_Line = Cur_Pane.AddCurve("Motor Current", Cur_Motor_List, Color.Green, SymbolType.None);
        }

        private void refreshGraphs()
        {
            
            PosGraph.AxisChange();
            PosGraph.Invalidate();
            VelGraph.AxisChange();
            VelGraph.Invalidate();

            CurGraph.AxisChange();
            CurGraph.Invalidate();

        }

        public void OnRenderTimerTick(object source, EventArgs e)
        {
            //cube.Invalidate();
        }
        private void initializeSerialPort()
        {
            serialPort = new SerialPort();

            foreach (string s in SerialPort.GetPortNames())
            {
                serialPortCOMBox.Items.Add(s);
            }
            if (serialPortCOMBox.Items.Count == 0)
            {
                serialPortCOMBox.Items.Add("No Ports Avaliable");
                serialPortCOMBox.Enabled = false;
                serialConnectButton.Enabled = false;
            }

            serialPortCOMBox.SelectedIndex = 0;
            
            baudSelectBox.Items.Add(9600);
            baudSelectBox.Items.Add(56000);
            baudSelectBox.Items.Add(115200);
            baudSelectBox.Items.Add(1152000);
            baudSelectBox.Items.Add(2000000);
            baudSelectBox.Items.Add(2304000);
            baudSelectBox.Items.Add(3456000);
			baudSelectBox.Items.Add(4000000);
            baudSelectBox.Items.Add(4608000);
            baudSelectBox.Items.Add(4900000);
            baudSelectBox.Items.Add(5000000);
            baudSelectBox.Items.Add(5760000);
            baudSelectBox.Items.Add(6912000);
            
            baudSelectBox.SelectedIndex = 2;
        }
        private void InitTextBox()
        {
            PosKpBox_TextChanged(this, new EventArgs());
            PosKiBox_TextChanged(this, new EventArgs());
            PosKdBox_TextChanged(this, new EventArgs());
            PosKiSatBox_TextChanged(this, new EventArgs());
            VelKpBox_TextChanged(this, new EventArgs());
            VelKiBox_TextChanged(this, new EventArgs());
            VelKdBox_TextChanged(this, new EventArgs());
            VelKiSatBox_TextChanged(this, new EventArgs());
            CurKpBox_TextChanged(this, new EventArgs());
            CurKiBox_TextChanged(this, new EventArgs());
            CurKdBox_TextChanged(this, new EventArgs());
            CurKiSatBox_TextChanged(this, new EventArgs());
            SoftstartBox_TextChanged(this, new EventArgs());
            MaxVelBox_TextChanged(this, new EventArgs());
            MaxTorBox_TextChanged(this, new EventArgs());
            MaxOptBox_TextChanged(this, new EventArgs());
            ExtVelBox_TextChanged(this, new EventArgs());
            ExtTorBox_TextChanged(this, new EventArgs());
            ExtPosBox_TextChanged(this, new EventArgs());
            mChBox_TextChanged(this, new EventArgs());
        }
        private void AHRSInterface_Load(object sender, EventArgs e)
        {
            
            formWidth = this.Width;
            formHeight = this.Height;
            SC_H = Convert.ToInt16(formHeight);
            SC_W = Convert.ToInt16(formWidth);
            /*
            this.Text = (Convert.ToString((SC_H / 3) - 20));
            PosGraph.Height = Convert.ToInt16(this.Text) - 20;
            //VelGraph.Location.Y.ToString = 1;
            VelGraph.Location = new Point(12, Convert.ToInt16(this.Text) + 50);
            VelGraph.Height = Convert.ToInt16(this.Text) - 20;
            CurGraph.Location = new Point(12, Convert.ToInt16(this.Text) * 2 + 35);
            CurGraph.Height = Convert.ToInt16(this.Text) - 30;
            statusBox.Height = (CurGraph.Location.Y + CurGraph.Height) - 370;
             */
            SC_Height = Screen.PrimaryScreen.Bounds.Height;
            AHRSInterface_formResize();
            InitTextBox();

        }
        //public bool Graph_Sketch = true;
        //private bool Graph_Sketch = true;
        // Define AHRS object
        AHRS sensor;
        AHRSLog dataLog;
        Timer renderTimer;

        SerialPort serialPort;
        
        /*********************************************************************************
         * Variables for graph initialization
         * *******************************************************************************/
        GraphPane Pos_Pane,Vel_Pane,Cur_Pane;
        
        RollingPointPairList Pos_Tar_List, Pos_Motor_List;
        RollingPointPairList Vel_Ext_List, Vel_Int_List, Vel_Motor_List;       
        RollingPointPairList Cur_Ext_List, Cur_Int_List, Cur_Motor_List;
        
        LineItem Pos_Tar_Line, Pos_Motor_Line;
        LineItem Vel_Ext_Line, Vel_Int_Line, Vel_Motor_Line;
        LineItem Cur_Ext_Line, Cur_Int_Line, Cur_Motor_Line;

        private double time;

        const int SENSOR_GRAPH_POINTS = 1000;

        private int Motor_Channel = 0;




        /* **********************************************************************************
         * 
         * Function: void TimeoutEventHandler
         * Inputs: PName packet_type, int flags
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Handles timeout events generated by the AHRS class - a timeout event occurs if the
         * AHRS class attempts to communicate with the AHRS device and receives no response.
         * 
         * *********************************************************************************/
        void TimeoutEventHandler(StateName packet_type, int flags)
        {
            string message;

            message = "Timeout: ";
            message += System.Enum.Format(typeof(StateName), packet_type, "G");
            message += "\r\n";

            AppendStatusText(message, Color.Red);
        }

        void COMFailedEventHandler()
        {
//            AppendStatusText("Serial COM failed\r\n", Color.Red);
        }

        /* **********************************************************************************
        * 
        * Function: void PacketReceivedEventHandler
        * Inputs: PName packet_type, int flags
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles PacketReceived events generated by the AHRS.
        * 
        * *********************************************************************************/
        void PacketReceivedEventHandler(PName packet_type, int flags)
        {
            string message;

            if (packet_type == PName.CMD_NO_SUPPORT)
            {
                message = "Command Failed: ";
                message += System.Enum.Format(typeof(PName), flags, "G");
                message += "\r\n";
            }
            else if (packet_type == PName.CMD_COMPLETE)
            {
                message = "Command Complete: ";
                message += System.Enum.Format(typeof(PName), flags, "G");
                message += "\r\n";
            }
            else if (packet_type == PName.CMD_CRC_FAILED)
            {
                if (flags == -1)
                {
                    message = "Device CRC Failed:: ";
                }
                else
                {
                    message = "Command CRC Failed:: ";
                }
                message += System.Enum.Format(typeof(PName), 0, "G");
                message += "\r\n";

            }
            else if (packet_type == PName.CMD_OVER_DATA_LENGTH)
            {
                if (flags == -1)
                {
                    message = "Device Over Data Length: ";
                }
                else
                {
                    message = "Command Over Data Length: ";
                }                         
                message += System.Enum.Format(typeof(PName), 0, "G");
                message += "\r\n";
            }
            else
            {
                message = "Received ";
                message += System.Enum.Format(typeof(PName), packet_type, "G");
                message += " packet\r\n";
            }

            AppendStatusText(message, Color.Green);
        }

        /* **********************************************************************************
        * 
        * Function: void PacketSentEventHandler
        * Inputs: PName packet_type, int flags
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles PacketReceived events generated by the AHRS.
        * 
        * *********************************************************************************/
        void PacketSentEventHandler(PName packet_type, int flags)
        {
            string message;

            message = "Sent ";
            message += System.Enum.Format(typeof(PName), packet_type, "G");
            message += " packet\r\n";

            AppendStatusText(message, Color.Blue);            
        }

        /* **********************************************************************************
        * 
        * Function: void PacketLabelHandler
        * Inputs: byte[] data, int flags
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles PacketReceived events generated by the AHRS.
        * 
        * *********************************************************************************/
        void PacketLabelHandler(byte[] data, int flags)
        {
            string message;
            int i = data.Length;
            

            if (flags == 1)
            {
            
                message = "Model Name: ";
                for (i = 0; i < data.Length; i++)
                {
                    message += Convert.ToChar(data[i]);
                }
                AppendLabel1Text( message);
            }
            else if(flags == 2)
            {
                message = "F/W Version: ";
                for (i = 0; i < data.Length; i++)
                {
                    message += Convert.ToChar(data[i]);
                }

                AppendLabel2Text( message);
            }
            else if(flags == 23)
            {
                message = "Sensor ADC: 0x";
                for (i = 0; i < (data.Length - 1); i++)
                {
                    message += data[i].ToString("X2");
                }

                AppendLabel23Text( message);
            }
			
            else
            {
                // to do something
            }

        }

        

        /* **********************************************************************************
        * 
        * Function: void DataReceivedEventHandler
        * Inputs: None
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles DataReceived events generated by the AHRS object
        * 
        * *********************************************************************************/
        void DataReceivedEventHandler(int active_channels)
        {
            //time += timer1.Interval / 1000.0;
            if ( sensor.Graph_Sketch == true)
            {
                time++;

                Pos_Tar_List.Add(time, sensor.Motor_Member[Motor_Channel].Position_Target);
                Pos_Motor_List.Add(time, sensor.Motor_Member[Motor_Channel].QEI32);

                Vel_Ext_List.Add(time, sensor.Motor_Member[Motor_Channel].Velocity_External);
                Vel_Int_List.Add(time, sensor.Motor_Member[Motor_Channel].Velocity_Internal);
                Vel_Motor_List.Add(time, sensor.Motor_Member[Motor_Channel].QEI_Diff16);

                Cur_Ext_List.Add(time, sensor.Motor_Member[Motor_Channel].Torque_External);
                Cur_Int_List.Add(time, sensor.Motor_Member[Motor_Channel].Torque_Internal);
                Cur_Motor_List.Add(time, sensor.Motor_Member[Motor_Channel].Motor_Current);
                
                refreshGraphs();
                DataReceivedbitStatus(sensor.Bit_Statis);//bit status grhp
            }
        }
        private void DataReceivedbitStatus(byte chstatus)
        {
            bool ptbit4, ptbit5, ptbit6, ptbit7;

            ptbit4 = Convert.ToBoolean(Convert.ToByte(sensor.Bit_Statis) & 0x10);
            ptbit5 = Convert.ToBoolean(Convert.ToByte(sensor.Bit_Statis) & 0x20);
            ptbit6 = Convert.ToBoolean(Convert.ToByte(sensor.Bit_Statis) & 0x40);
            ptbit7 = Convert.ToBoolean(Convert.ToByte(sensor.Bit_Statis) & 0x80);
            if (ptbit4)
                picbit4.BackColor = Color.GreenYellow ;
            else
                picbit4.BackColor = Color.Red;
            if (ptbit5)
                picbit5.BackColor = Color.GreenYellow;
            else
                picbit5.BackColor = Color.Red;
            if (ptbit6)
                picbit6.BackColor = Color.GreenYellow;
            else
                picbit6.BackColor = Color.Red;
            if (ptbit7)
                picbit7.BackColor = Color.GreenYellow;
            else
                picbit7.BackColor = Color.Red;
        }
        private void AppendStatusText(string text, Color text_color)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.statusBox.InvokeRequired)
                {
                    AppendTextCallback d = new AppendTextCallback(AppendStatusText);
                    this.Invoke(d, new object[] { text, text_color });
                }
                else
                {
                    this.statusBox.SelectionColor = text_color;
                    this.statusBox.AppendText(text);
                    this.statusBox.ScrollToCaret();
                }
            }
            catch{}
        }

        private void AppendLabel1Text(string text)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.label1.InvokeRequired)
                {
                    AppendLabel1TextCallback d = new AppendLabel1TextCallback(AppendLabel1Text);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.label1.Text = text;
                }
            }
            catch{}
        }

        private void AppendLabel2Text(string text)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.label2.InvokeRequired)
                {
                    AppendLabel2TextCallback d = new AppendLabel2TextCallback(AppendLabel2Text);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.label2.Text = text;
                }
            }
            catch{}
        }        

        private void AppendLabel23Text(string text)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.label23.InvokeRequired)
                {
                    AppendLabel23TextCallback d = new AppendLabel23TextCallback(AppendLabel23Text);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.label23.Text = text;
                }
            }
            catch{}
        }    

        private void SynchButton_Click(object sender, EventArgs e)
        {
            sensor.synch();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //data_display = new InfoDump(sensor);

            //data_display.Show();
        }

        // "Connect" button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Pos_Tar_List.Clear();
            Pos_Motor_List.Clear();

            Vel_Ext_List.Clear();
            Vel_Int_List.Clear();
            Vel_Motor_List.Clear();

            Cur_Ext_List.Clear();
            Cur_Int_List.Clear();
            Cur_Motor_List.Clear();
            refreshGraphs();
            try
            {
                // Connect to the serial port
                if (!sensor.connect(serialPortCOMBox.SelectedItem.ToString(), (int)baudSelectBox.SelectedItem))
                {
                    AppendStatusText("Failed to connect to serial port\r\n", Color.Red);
                }
                else
                {
                    AppendStatusText("Connected to " + serialPortCOMBox.SelectedItem.ToString() + "\r\n", Color.Blue);

                    serialDisconnectButton.Enabled = true;
                    serialConnectButton.Enabled = false;
                    magCalibrationToolStripMenuItem.Enabled = true;
                    configToolStripMenuItem.Enabled = true;
                    logDataToolstripItem.Enabled = true;
                    sensor.Graph_Sketch = true;

                    sensor.synch();
                }
            }
            catch{}
        }

        private void serialDisconnectButton_Click(object sender, EventArgs e)
        {
            try
            {

                //Pos_Pane.CurveList.Clear();
                //Pos_Pane.GraphObjList.Clear();

                //Vel_Pane.CurveList.Clear();
                //Vel_Pane.GraphObjList.Clear();

                //Cur_Pane.CurveList.Clear();
                //Cur_Pane.GraphObjList.Clear();
                //refreshGraphs();
                sensor.Graph_Sketch = false;
                sensor.Disconnect();
                sensor.Invalidate();

                serialDisconnectButton.Enabled = false;
                serialConnectButton.Enabled = true;
                magCalibrationToolStripMenuItem.Enabled = false;
                configToolStripMenuItem.Enabled = false;
                logDataToolstripItem.Enabled = false;

                AppendStatusText("Disconnected from serial port\r\n", Color.Blue);
            }
            catch{ }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch
            {
                //If there was an exception, then close the handle to 
                //  the device and assume that the device was removed
                exitToolStripMenuItem_Click(this, null);
            }      
        }

        private void magCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //magCalibration = new MagCal(sensor);

            //magCalibration.Show();
        }

        private void logDataToolstripItem_Click(object sender, EventArgs e)
        {
            dataLog = new AHRSLog(sensor);

            dataLog.Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void KickOff_Click(object sender, EventArgs e)
        {
            InitTextBox();
            //timer1.Enabled = Enabled;
            sensor.Kick_Off();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            //refreshGraphs();

        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            if ( sensor.Graph_Sketch == true)
            {
                sensor.Graph_Sketch = false;
                PauseBtn.Text = "Resume";
            }
            else
            {
                sensor.Graph_Sketch = true;
                PauseBtn.Text = "Pause";
            }

        }

        private void PosKpBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(PosKpBox.Text))
            {
            }
            else
            {
                PosKpBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Pos.Kp = PID_Param_Handle(PosKpBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }        

        private void PosKiBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(PosKiBox.Text))
            {
            }
            else
            {
                PosKiBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Pos.Ki = PID_Param_Handle(PosKiBox.Text);                
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void PosKdBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(PosKdBox.Text))
            {
            }
            else
            {
                PosKdBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Pos.Kd = PID_Param_Handle(PosKdBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void PosKiSatBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(PosKiSatBox.Text) && Convert.ToInt32(PosKiSatBox.Text) >= 0)
            {
            }
            else
            {
                PosKiSatBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Pos.Int_sat = Convert.ToInt32(PosKiSatBox.Text);                
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void VelKpBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(VelKpBox.Text))
            {
            }
            else
            {
                VelKpBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Vel.Kp = PID_Param_Handle(VelKpBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void VelKiBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(VelKiBox.Text))
            {
            }
            else
            {
                VelKiBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Vel.Ki = PID_Param_Handle(VelKiBox.Text);                
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void VelKdBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(VelKdBox.Text))
            {
            }
            else
            {
                VelKdBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Vel.Kd = PID_Param_Handle(VelKdBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void VelKiSatBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(VelKiSatBox.Text))
            {
            }
            else
            {
                VelKiSatBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Vel.Int_sat = Convert.ToInt32(VelKiSatBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void CurKpBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(CurKpBox.Text))
            {
            }
            else
            {
                CurKpBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Tor.Kp = PID_Param_Handle(CurKpBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void CurKiBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(CurKiBox.Text))
            {
            }
            else
            {
                CurKiBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Tor.Ki = PID_Param_Handle(CurKiBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void CurKdBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(CurKdBox.Text))
            {
            }
            else
            {
                CurKdBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Tor.Kd = PID_Param_Handle(CurKdBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void CurKiSatBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsFloat(CurKiSatBox.Text))
            {
            }
            else
            {
                CurKiSatBox.Text = "0";
            }
            sensor.Motor_PID_Member[Motor_Channel].Tor.Int_sat = Convert.ToInt32(CurKiSatBox.Text);
            sensor.UdtPending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true; 
        }

        private void SoftstartBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(SoftstartBox.Text) && Convert.ToInt32(SoftstartBox.Text) >= 0)
            {
            }
            else
            {
                SoftstartBox.Text = "0";
            }
            sensor.Pos_SoftStart[Motor_Channel] = (Int16)Convert.ToInt32(SoftstartBox.Text);
            sensor.UdtPending[(int)StateName.STATE_MAX_OF_POSITION_CMD] = true; 
        }

        private void MaxVelBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(MaxVelBox.Text) && Convert.ToInt32(MaxVelBox.Text) >= 0)
            {
            }
            else
            {
                MaxVelBox.Text = "0";
            }
            sensor.Max_Vel_Cmd[Motor_Channel] = (Int16)Convert.ToInt32(MaxVelBox.Text);
            sensor.UdtPending[(int)StateName.STATE_MAX_OF_VELOCITY_CMD] = true; 
        }

        private void MaxTorBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(MaxTorBox.Text) && Convert.ToInt32(MaxTorBox.Text) >= 0)
            {
            }
            else
            {
                MaxTorBox.Text = "0";
            }
            sensor.Max_Tor_Cmd[Motor_Channel] = (Int16)Convert.ToInt32(MaxTorBox.Text);
            sensor.UdtPending[(int)StateName.STATE_MAX_OF_TORQUE_CMD] = true; 
        }

        private void MaxOptBox_TextChanged(object sender, EventArgs e)
        {            
            if ( IsInt(MaxOptBox.Text) && Convert.ToInt32(MaxOptBox.Text) >= 0)
            {
            }
            else
            {
                MaxOptBox.Text = "0";
            }
            sensor.Max_PWM_Cmd[Motor_Channel] = (Int16)Convert.ToInt32(MaxOptBox.Text);
            sensor.UdtPending[(int)StateName.STATE_MAX_OF_PWM_DUTYCYCLE] = true;            
        }

        private void ExtVelBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(ExtVelBox.Text))
            {
            }
            else
            {
                ExtVelBox.Text = "0";
            }
            sensor.Velocity_External[Motor_Channel] = (Int16)Convert.ToInt32(ExtVelBox.Text);
            sensor.UdtPending[(int)StateName.STATE_VELOCITY_EXT_CMD] = true; 
        }

        private void ExtTorBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(ExtTorBox.Text))
            {
            }
            else
            {
                ExtTorBox.Text = "0";
            }
            sensor.Torque_External[Motor_Channel] = (Int16)Convert.ToInt32(ExtTorBox.Text);
            sensor.UdtPending[(int)StateName.STATE_TORQUE_EXT_CMD] = true; 
        }

        private void ExtPosBox_TextChanged(object sender, EventArgs e)
        {
            if ( IsInt(ExtPosBox.Text))
            {
            }
            else
            {
                ExtPosBox.Text = "0";
            }
            sensor.Position_Target[Motor_Channel] = (UInt32)Convert.ToInt32(ExtPosBox.Text);
            sensor.UdtPending[(int)StateName.STATE_POSITION_TARGET_CMD] = true; 
        }
    
    
        public bool IsInt(string TextBoxValue)
        {
            try
            {
                int i = Convert.ToInt32(TextBoxValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private static bool IsFloat(string TextBoxValue)
        {
            try
            {
                double i = Convert.ToDouble(TextBoxValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static Int32 PID_Param_Handle(string TextBoxValue)
        {
            double Temp;
            Temp = Convert.ToDouble(TextBoxValue) * 65536;
            return (Int32)Temp;

        }
        



        private void AHRSInterface_Resize(object sender, EventArgs e)
        {
            if (this.Height <= 600) { this.Height = 600; } //set 600 pix is min size.
            else if (this.Height >= SC_Height) { this.Height = SC_Height; }
            formWidth = this.Width;
            formHeight = this.Height;

            AHRSInterface_formResize();
            //label18.Text = Convert.ToString(CurGraph.Location.Y + CurGraph.Height - 400);

        }
       private void AHRSInterface_formResize() // zoom box size
       {
           int tmp;
        SC_H = Convert.ToInt16(formHeight);
        SC_W = Convert.ToInt16(formWidth);
        //tmp = (Convert.ToString((SC_H / 3) - 20)); 
        tmp = ((SC_H / 3) - 20);
        PosGraph.Height = tmp - 20;
        VelGraph.Location = new Point(12, tmp + 50);
        VelGraph.Height =tmp - 20;
        CurGraph.Location = new Point(12, tmp * 2 + 35);
        CurGraph.Height = tmp - 30;
        statusBox.Height = (CurGraph.Location.Y + CurGraph.Height)-370  ;
       }

       private void mChBox_TextChanged(object sender, EventArgs e)
       {

           Pos_Tar_List.Clear();
           Pos_Motor_List.Clear();

           Vel_Ext_List.Clear();
           Vel_Int_List.Clear();
           Vel_Motor_List.Clear();

           Cur_Ext_List.Clear();
           Cur_Int_List.Clear();
           Cur_Motor_List.Clear();
           refreshGraphs();

           if (mChBox.Text == "")
               mChBox.Text = Convert.ToString(0);

           Motor_Channel = Convert.ToByte(mChBox.Text);
           sensor.mChBox_CH = Convert.ToByte(mChBox.Text);
       }

       private void toolStripButton1_Click_1(object sender, EventArgs e)
       {

           sensor.Graph_Sketch = false;
           sensor.Disconnect();
           sensor.Invalidate();

           serialDisconnectButton.Enabled = false;
           serialConnectButton.Enabled = true;
           magCalibrationToolStripMenuItem.Enabled = false;
           configToolStripMenuItem.Enabled = false;
           logDataToolstripItem.Enabled = false;

           AppendStatusText("Disconnected from serial port\r\n", Color.Blue);

           //建立OpenFileDialog
           OpenFileDialog dialog = new OpenFileDialog();

           //設定Filter，過濾檔案 
           dialog.Filter = "open files (*.txt)|*.txt";

           //設定起始目錄為C:\
           dialog.InitialDirectory = "";

           //設定起始目錄為程式目錄
           dialog.InitialDirectory = Application.StartupPath;

           //設定dialog的Title
           dialog.Title = "Select a log file";

           //假如使用者按下OK鈕，則將檔案名稱顯示於TextBox1上
           if (dialog.ShowDialog() == DialogResult.OK)
           {
               //textBox1.Text = dialog.FileName;
               Pos_Tar_List.Clear();
               Pos_Motor_List.Clear();

               Vel_Ext_List.Clear();
               Vel_Int_List.Clear();
               Vel_Motor_List.Clear();

               Cur_Ext_List.Clear();
               Cur_Int_List.Clear();
               Cur_Motor_List.Clear();


               try
               { // Create an instance of StreamReader to read from a file.
                   // The using statement also closes the StreamReader.
                   using (StreamReader sr = new StreamReader(dialog.FileName))     //小寫TXT
                   {
                       String line;


                       sr.BaseStream.Position = 82;
                       UInt64 count = 0;
                       // Read and display lines from the file until the end of 
                       // the file is reached.
                       while ((line = sr.ReadLine()) != null)
                       {
                           //if (line.IndexOf("_") <0)
                           // {
                           count += 1;
                           char[] fit = new char[] { '\t' };
                           string[] fileline = line.Split(fit);
                           Pos_Tar_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[1]));
                           Pos_Motor_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[2]));

                           Vel_Ext_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[3]));
                           Vel_Int_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[4]));
                           Vel_Motor_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[5]));

                           Cur_Ext_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[6]));
                           Cur_Int_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[7]));
                           Cur_Motor_List.Add(Convert.ToDouble(count), Convert.ToDouble(fileline[8]));

                           //}
                       }
                   }
               }
               catch
               {
               }


               refreshGraphs();
           }

        

       }

       private void Home_Click(object sender, EventArgs e)
       {

           sensor.Home_Cmd();
       }

       private void Sensor_btn_Click(object sender, EventArgs e)
       {
           sensor.Report_Sensor_ADC();
       }

       private void statusBox_TextChanged(object sender, EventArgs e)
       {

       }



 





            
    }
}
