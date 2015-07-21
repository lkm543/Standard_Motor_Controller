using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
namespace AHRSInterface
{
    public partial class AHRSLog : Form
    {
        string path;
        public AHRSLog(AHRS sensor)
        {
            InitializeComponent();

            //loggingEnabled = false;
            loggingEnabled = true;
            this.sensor = sensor;

            sensor.DataReceivedEvent += new DataReceivedDelegate(DataReceivedEventHandler);
        }

        ~AHRSLog()
        {
            if (loggingEnabled)
            {
                logfile.Close();
            }

            sensor.DataReceivedEvent -= new DataReceivedDelegate(DataReceivedEventHandler);
        }

        StreamWriter logfile;

        string logdate;
        DateTime dt = DateTime.Now;
        StreamWriter swch0;
        StreamWriter swch1;
        StreamWriter swch2;
        StreamWriter swch3;
        StreamWriter swch4;
        StreamWriter swch5;
        StreamWriter swch6;

        bool loggingEnabled;
        SaveFileDialog saveFileDialog;

        AHRS sensor;

        DateTime startTime;
        DateTime currentTime;

        void DataReceivedEventHandler(int active_channels)
        {
            if (loggingEnabled)
            {
                currentTime = DateTime.Now;

                TimeSpan deltaT = currentTime - startTime;
                channels_fog(active_channels);
                /*
               try
               {
                   
                   logfile.Write(deltaT.TotalMilliseconds);
                   logfile.Write("\t");
                    
                   logfile.Write(sensor.Motor_Member[0].ticker);
                   logfile.Write("\t");

                   logfile.Write(sensor.Motor_Member[0].Position_Target);
                   logfile.Write("\t");
                   logfile.Write(sensor.Motor_Member[0].QEI32);
                   logfile.Write("\t");
                    
                   logfile.Write(sensor.Motor_Member[0].Velocity_External);
                   logfile.Write("\t");
                   logfile.Write(sensor.Motor_Member[0].Velocity_Internal);
                   logfile.Write("\t");
                   logfile.Write(sensor.Motor_Member[0].QEI_Diff16);
                   logfile.Write("\t");
                    
                   logfile.Write(sensor.Motor_Member[0].Torque_External);
                   logfile.Write("\t");
                   logfile.Write(sensor.Motor_Member[0].Torque_Internal);
                   logfile.Write("\t");
                   logfile.WriteLine(sensor.Motor_Member[0].Motor_Current);
               }
               catch
               {
               }
                    */
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
          

            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
    
            //設定起始目錄為C:\
        
            if (saveFileDialog.ShowDialog() == DialogResult.OK )
            {

                path = saveFileDialog.SelectedPath;
                filenameBox.Text = path;
            }
        }


        void channels_fog(int ch)
        {
            try
            {
                switch (ch)
                {
                    case 0:
                        swch0.Write(sensor.Motor_Member[0].ticker);
                        swch0.Write("\t");

                        swch0.Write(sensor.Motor_Member[0].Position_Target);
                        swch0.Write("\t");
                        swch0.Write(sensor.Motor_Member[0].QEI32);
                        swch0.Write("\t");

                        swch0.Write(sensor.Motor_Member[0].Velocity_External);
                        swch0.Write("\t");
                        swch0.Write(sensor.Motor_Member[0].Velocity_Internal);
                        swch0.Write("\t");
                        swch0.Write(sensor.Motor_Member[0].QEI_Diff16);
                        swch0.Write("\t");

                        swch0.Write(sensor.Motor_Member[0].Torque_External);
                        swch0.Write("\t");
                        swch0.Write(sensor.Motor_Member[0].Torque_Internal);
                        swch0.Write("\t");
                        swch0.WriteLine(sensor.Motor_Member[0].Motor_Current);
                        break;
                    case 1:
                        swch1.Write(sensor.Motor_Member[1].ticker);
                        swch1.Write("\t");

                        swch1.Write(sensor.Motor_Member[1].Position_Target);
                        swch1.Write("\t");
                        swch1.Write(sensor.Motor_Member[1].QEI32);
                        swch1.Write("\t");

                        swch1.Write(sensor.Motor_Member[1].Velocity_External);
                        swch1.Write("\t");
                        swch1.Write(sensor.Motor_Member[1].Velocity_Internal);
                        swch1.Write("\t");
                        swch1.Write(sensor.Motor_Member[1].QEI_Diff16);
                        swch1.Write("\t");

                        swch1.Write(sensor.Motor_Member[1].Torque_External);
                        swch1.Write("\t");
                        swch1.Write(sensor.Motor_Member[1].Torque_Internal);
                        swch1.Write("\t");
                        swch1.WriteLine(sensor.Motor_Member[1].Motor_Current);
                        break;
                    case 2:
                        swch2.Write(sensor.Motor_Member[2].ticker);
                        swch2.Write("\t");

                        swch2.Write(sensor.Motor_Member[2].Position_Target);
                        swch2.Write("\t");
                        swch2.Write(sensor.Motor_Member[2].QEI32);
                        swch2.Write("\t");

                        swch2.Write(sensor.Motor_Member[2].Velocity_External);
                        swch2.Write("\t");
                        swch2.Write(sensor.Motor_Member[2].Velocity_Internal);
                        swch2.Write("\t");
                        swch2.Write(sensor.Motor_Member[2].QEI_Diff16);
                        swch2.Write("\t");

                        swch2.Write(sensor.Motor_Member[2].Torque_External);
                        swch2.Write("\t");
                        swch2.Write(sensor.Motor_Member[2].Torque_Internal);
                        swch2.Write("\t");
                        swch2.WriteLine(sensor.Motor_Member[2].Motor_Current);
                        break;
                    case 3:
                        swch3.Write(sensor.Motor_Member[3].ticker);
                        swch3.Write("\t");

                        swch3.Write(sensor.Motor_Member[3].Position_Target);
                        swch3.Write("\t");
                        swch3.Write(sensor.Motor_Member[3].QEI32);
                        swch3.Write("\t");

                        swch3.Write(sensor.Motor_Member[3].Velocity_External);
                        swch3.Write("\t");
                        swch3.Write(sensor.Motor_Member[3].Velocity_Internal);
                        swch3.Write("\t");
                        swch3.Write(sensor.Motor_Member[3].QEI_Diff16);
                        swch3.Write("\t");

                        swch3.Write(sensor.Motor_Member[3].Torque_External);
                        swch3.Write("\t");
                        swch3.Write(sensor.Motor_Member[3].Torque_Internal);
                        swch3.Write("\t");
                        swch3.WriteLine(sensor.Motor_Member[3].Motor_Current);
                        break;
                    case 4:
                        swch4.Write(sensor.Motor_Member[4].ticker);
                        swch4.Write("\t");

                        swch4.Write(sensor.Motor_Member[4].Position_Target);
                        swch4.Write("\t");
                        swch4.Write(sensor.Motor_Member[4].QEI32);
                        swch4.Write("\t");

                        swch4.Write(sensor.Motor_Member[4].Velocity_External);
                        swch4.Write("\t");
                        swch4.Write(sensor.Motor_Member[4].Velocity_Internal);
                        swch4.Write("\t");
                        swch4.Write(sensor.Motor_Member[4].QEI_Diff16);
                        swch4.Write("\t");

                        swch4.Write(sensor.Motor_Member[4].Torque_External);
                        swch4.Write("\t");
                        swch4.Write(sensor.Motor_Member[4].Torque_Internal);
                        swch4.Write("\t");
                        swch4.WriteLine(sensor.Motor_Member[4].Motor_Current);
                        break;
                    case 5:
                        swch5.Write(sensor.Motor_Member[5].ticker);
                        swch5.Write("\t");

                        swch5.Write(sensor.Motor_Member[5].Position_Target);
                        swch5.Write("\t");
                        swch5.Write(sensor.Motor_Member[5].QEI32);
                        swch5.Write("\t");

                        swch5.Write(sensor.Motor_Member[5].Velocity_External);
                        swch5.Write("\t");
                        swch5.Write(sensor.Motor_Member[5].Velocity_Internal);
                        swch5.Write("\t");
                        swch5.Write(sensor.Motor_Member[5].QEI_Diff16);
                        swch5.Write("\t");

                        swch5.Write(sensor.Motor_Member[5].Torque_External);
                        swch5.Write("\t");
                        swch5.Write(sensor.Motor_Member[5].Torque_Internal);
                        swch5.Write("\t");
                        swch5.WriteLine(sensor.Motor_Member[5].Motor_Current);
                        break;
                    case 6:
                        swch6.Write(sensor.Motor_Member[6].ticker);
                        swch6.Write("\t");

                        swch6.Write(sensor.Motor_Member[6].Position_Target);
                        swch6.Write("\t");
                        swch6.Write(sensor.Motor_Member[6].QEI32);
                        swch6.Write("\t");

                        swch6.Write(sensor.Motor_Member[6].Velocity_External);
                        swch6.Write("\t");
                        swch6.Write(sensor.Motor_Member[6].Velocity_Internal);
                        swch6.Write("\t");
                        swch6.Write(sensor.Motor_Member[6].QEI_Diff16);
                        swch6.Write("\t");

                        swch6.Write(sensor.Motor_Member[6].Torque_External);
                        swch6.Write("\t");
                        swch6.Write(sensor.Motor_Member[6].Torque_Internal);
                        swch6.Write("\t");
                        swch6.WriteLine(sensor.Motor_Member[6].Motor_Current);
                        break;
                    default:

                        break;

                }
            }
            catch
            {
            }
            
        }

        private void startLoggingButton_Click(object sender, EventArgs e)
        {
            
            loggingEnabled = true;
            sensor.Graph_Sketch = false;
            stopLoggingButton.Enabled = true;
            startLoggingButton.Enabled = false;
            browseButton.Enabled = false;

          //  logfile = new StreamWriter(saveFileDialog.FileName, true);        
            


            dt = DateTime.Now;
            logdate = Convert.ToString(String.Format("{0:yyyyMMddhhmmss}", dt));
            if (path == null )
            {
                if (!Directory.Exists(@"log\")) // if not path, make is.
                    Directory.CreateDirectory("log");

                swch0 = new StreamWriter(@"log\" + logdate + "00.txt");
                swch1 = new StreamWriter(@"log\" + logdate + "01.txt");
                swch2 = new StreamWriter(@"log\" + logdate + "02.txt");
                swch3 = new StreamWriter(@"log\" + logdate + "03.txt");
                swch4 = new StreamWriter(@"log\" + logdate + "04.txt");
                swch5 = new StreamWriter(@"log\" + logdate + "05.txt");
                swch6 = new StreamWriter(@"log\" + logdate + "06.txt");

            }
            else
            {
                swch0 = new StreamWriter(path + @"\"+ logdate + @"00.txt");
                swch1 = new StreamWriter(path + @"\" + logdate + @"01.txt");
                swch2 = new StreamWriter(path + @"\" + logdate + @"02.txt");
                swch3 = new StreamWriter(path + @"\" + logdate + @"03.txt");
                swch4 = new StreamWriter(path + @"\" + logdate + @"04.txt");
                swch5 = new StreamWriter(path + @"\" + logdate + @"05.txt");
                swch6 = new StreamWriter(path + @"\" + logdate + @"06.txt");
            }
            swch0.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");
            swch1.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");
            swch2.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");
            swch3.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");
            swch4.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");
            swch5.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");
            swch6.WriteLine("Ticker\tPos_Target\tMotor_Pos\tVel_Ext\tVel_Int\tPMotor_Vel\tTor_Ext\tTor_Int\tMotor_Tor");

        }

        private void stopLoggingButton_Click(object sender, EventArgs e)
        {
            loggingEnabled = false;
            sensor.Graph_Sketch = true;
            stopLoggingButton.Enabled = false;
            startLoggingButton.Enabled = true;
            browseButton.Enabled = true;

            //logfile.Close();
            
            swch0.Close();
            swch1.Close();
            swch2.Close();
            swch3.Close();
            swch4.Close();
            swch5.Close();
            swch6.Close();
        }

        private void AHRSLog_Load(object sender, EventArgs e)
        {

        }



    }
}
