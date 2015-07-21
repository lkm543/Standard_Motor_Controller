using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace AHRSInterface
{
    // Delegate types for interfacing with AHRS class events
    // The PacketDelegate type is used to create handlers for 'PacketReceived' events
    // and for 'COMFailed' events.
    public delegate void PacketDelegate(PName packet_type, int flags);

    public delegate void StateDelegate(StateName state_type, int flags);

    // The DataReceivedDelegate is used to create handlers for 'DataReceived' events
    public delegate void DataReceivedDelegate(int active_channels);

    public delegate void COMFailedDelegate();

    public delegate void PacketLabel(byte[] data, int flags);

    // Define a struct for converting a 4-byte data sequences into different data types
    [StructLayout(LayoutKind.Explicit)]
    struct byte_conversion_array
    {
        // Four Bytes
        [FieldOffset(0)]
        public byte byte3;

        [FieldOffset(1)]
        public byte byte2;

        [FieldOffset(2)]
        public byte byte1;

        [FieldOffset(3)]
        public byte byte0;

        // One Float
        [FieldOffset(0)]
        public float float0;

        // One 32-bit integer
        [FieldOffset(0)]
        public Int32 int32;

        // One 32-bit unsigned integer
        [FieldOffset(0)]
        public UInt32 uint32;

        // Two 16-bit integers
        [FieldOffset(0)]
        public Int16 int16_1;

        [FieldOffset(2)]
        public Int16 int16_0;

        // Two 16-bit unsigned integers
        [FieldOffset(0)]
        public UInt16 uint16_1;

        [FieldOffset(2)]
        public UInt16 uint16_0;
    }

    public enum PName
    {
		 WHO_AM_I			= 0,
		 FIRMWARE_VERSION,	   
		 ID_STATUS,			   
		 PWM_PARAMETER,		   
		 CMD_CRC_FAILED, 	   
		 CMD_NO_SUPPORT, 	   
		 CMD_OVER_DATA_LENGTH,  
		 CMD_COMPLETE,
		 REPORT_SENSOR_ADC,
		 CONTROLLER_STATUS,	   
							   
		 RESET_CONTROLLER,	   
		 KICK_OFF_CONTROLLER,   
		 REPORT_MCU_INFORMATION,
		 HOME_CMD,
		 PAUSE_REPORT_INFO,
		 CONTINUE_REPORT_INFO,
		 
		 ALL_OF_PID_PARAM,	   
		 POSITION_OF_PID_PARAM, 
		 VELOCITY_OF_PID_PARAM, 
		 TORQUE_OF_PID_PARAM,   
							   
		 MAX_OF_POSITION_CMD,   
		 MAX_OF_VELOCITY_CMD,   
		 MAX_OF_TORQUE_CMD,	   
		 MAX_OF_PWM_DUTYCYCLE,  
							   
		 POSITION_TARGET_CMD,   
		 VELOCITY_EXT_CMD,	   
		 TORQUE_EXT_CMD, 	   
		 DEBUG_MODE, 		   
    }

    public enum StateName
    {
        STATE_WHO_AM_I  = 0,      
        STATE_FIRMWARE_VERSION,    
        STATE_ID_STATUS,            
        STATE_PWM_PARAMETER,
        STATE_REPORT_SENSOR_ADC,
        STATE_CONTROLLER_STATUS,    

        STATE_RESET_CONTROLLER,     
        STATE_KICK_OFF_CONTROLLER,
        STATE_REPORT_MCU_INFORMATION,
		STATE_HOME_CMD,
		STATE_PAUSE_REPORT_INFO,
		STATE_CONTINUE_REPORT_INFO,
			
        STATE_ALL_OF_PID_PARAM,     
        STATE_POSITION_OF_PID_PARAM, 
        STATE_VELOCITY_OF_PID_PARAM, 
        STATE_TORQUE_OF_PID_PARAM,   
                              
        STATE_MAX_OF_POSITION_CMD,   
        STATE_MAX_OF_VELOCITY_CMD,   
        STATE_MAX_OF_TORQUE_CMD,    
        STATE_MAX_OF_PWM_DUTYCYCLE,  
                              
        STATE_POSITION_TARGET_CMD,   
        STATE_VELOCITY_EXT_CMD,     
        STATE_TORQUE_EXT_CMD,       
        STATE_DEBUG_MODE,           
    }


    public struct PID_PARAM
    {
        public Int32  Kp,
                      Ki,
                      Kd,
                      Int_sat; // integrator saturation
    }
        
    public struct MOTOR_MEMBER
    {
        public byte ticker;
        
        public UInt32 Position_Target,
                      QEI32;
        
        public Int16  Velocity_External,
                      Velocity_Internal,
                      QEI_Diff16;
        
        public Int16 Torque_External,
                     Torque_Internal,
                     Motor_Current;
    
        public Int16 PWM_Output;
        
    }

    public struct MOTOR_PID_MEMBER
    {
        public PID_PARAM Pos;  // position
        public PID_PARAM Vel;  // velocity
        public PID_PARAM Tor;  // torque
    }    

    public class AHRS
    {
        // Default constructor
        public AHRS()
        {
            connected = false;

            // Fill arrays used for AHRS COM
            int packet_count = PName.GetValues( typeof( PName ) ).Length;
            PID = new byte[packet_count];

            serialPort = new SerialPort();
            PacketTimer = new Timer();
            
            int state_count = StateName.GetValues(typeof(StateName)).Length;
            UpdatePending = new bool[state_count];
            DataPending = new bool[state_count];
            MaxDelay = new int[state_count];
            ElapsedTime = new int[state_count];

            // Commands that can be sent to the AHRS device
			PID[(int)PName.WHO_AM_I]				= 0x00;
			PID[(int)PName.FIRMWARE_VERSION]		= 0x01;
			PID[(int)PName.ID_STATUS]				= 0x03;
			PID[(int)PName.PWM_PARAMETER]			= 0x04;
			PID[(int)PName.CMD_CRC_FAILED]			= 0x05;
			PID[(int)PName.CMD_NO_SUPPORT]			= 0x06;
			PID[(int)PName.CMD_OVER_DATA_LENGTH]	= 0x07;
			PID[(int)PName.CMD_COMPLETE]			= 0x08;
			PID[(int)PName.REPORT_SENSOR_ADC]		= 0x0E;
			PID[(int)PName.CONTROLLER_STATUS]		= 0x0F;
			
			PID[(int)PName.RESET_CONTROLLER]		= 0x10;
			PID[(int)PName.KICK_OFF_CONTROLLER] 	= 0x11;
			PID[(int)PName.REPORT_MCU_INFORMATION]	= 0x12;
			PID[(int)PName.HOME_CMD] 				= 0x13;
			PID[(int)PName.PAUSE_REPORT_INFO]		= 0x14;
			PID[(int)PName.CONTINUE_REPORT_INFO]	= 0x15;
			
			PID[(int)PName.ALL_OF_PID_PARAM]		= 0x20;
			PID[(int)PName.POSITION_OF_PID_PARAM]	= 0x21;
			PID[(int)PName.VELOCITY_OF_PID_PARAM]	= 0x22;
			PID[(int)PName.TORQUE_OF_PID_PARAM] 	= 0x23;
			
			PID[(int)PName.MAX_OF_POSITION_CMD] 	= 0x30;
			PID[(int)PName.MAX_OF_VELOCITY_CMD] 	= 0x31;
			PID[(int)PName.MAX_OF_TORQUE_CMD]		= 0x32;
			PID[(int)PName.MAX_OF_PWM_DUTYCYCLE]	= 0x33;
			
			PID[(int)PName.POSITION_TARGET_CMD] 	= 0x40;
			PID[(int)PName.VELOCITY_EXT_CMD]		= 0x41;
			PID[(int)PName.TORQUE_EXT_CMD]			= 0x42;
			PID[(int)PName.DEBUG_MODE]				= 0xFF;


            // Set AHRS class parameters so that on the next call to 'synch', the class
            // will attempt to acquire all internal states of the actual AHRS device.
            Invalidate();

            // Setup timer for keeping track of time elapsed between packet transmission and reception of response.
            PacketTimer.Interval = 50;      // 10 ms delay
            PacketTimer.Enabled = true;
            PacketTimer.Tick += new System.EventHandler(OnPacketTimerTick);

            RXbuffer = new byte[RX_BUF_SIZE];

            m_Motor_Member = new MOTOR_MEMBER[Motor_Device_SIZE];
            m_Motor_PID_Member = new MOTOR_PID_MEMBER[Motor_Device_SIZE];
            
            m_Pos_SoftStart = new Int16[Motor_Device_SIZE];
            m_Max_Vel_Cmd = new Int16[Motor_Device_SIZE];
            m_Max_Tor_Cmd = new Int16[Motor_Device_SIZE];
            m_Max_PWM_Cmd = new Int16[Motor_Device_SIZE];
            m_Position_Target = new UInt32[Motor_Device_SIZE];
            m_Velocity_External = new Int16[Motor_Device_SIZE];
            m_Torque_External = new Int16[Motor_Device_SIZE];


        }

        // Destructor
        ~AHRS()
        {
            // If the serial port is open, close it.
            if (connected)
            {
                serialPort.Close();
            }

            PacketTimer.Stop();
            PacketTimer.Dispose();
        }

        /* **********************************************************************************
         * 
         * Private member variables
         * 
         * *********************************************************************************/
        
        // Events for interfacing with AHRS class
        // A PacketReveivedEvent occurs when a new packet is received and parsed.
        public event PacketDelegate PacketReceivedEvent;
        public event PacketDelegate PacketSentEvent;
        public event COMFailedDelegate COMFailedEvent;
        // A DataReceivedEvent occurs when new sensor data arrives from the AHRS.
        public event DataReceivedDelegate DataReceivedEvent;
        // A COMFailedEvent occurs when the AHRS class expects to receive data from the
        // AHRS device, but no data is received in the max. allowed time.
        public event StateDelegate PacketTimeoutEvent;
        public event PacketLabel PacketLabelEvent;

        // Data for communication
        private bool connected;
        private byte bitstatue=0;
        private byte mChBoxch = 0;
        public bool graph_Sketch = true;

        private SerialPort serialPort;
        private Timer PacketTimer;

        const int RX_BUF_SIZE = 30000;
        private byte[] RXbuffer { get; set;  }
        private int RXbufPtr = 0;
        const int MAX_PACKET_SIZE = 60;



        // Variables for storing the state of the Motor controller
        const int Motor_Device_SIZE = 10;

        private int Motor_Channel = 0;//Convert.to 

        private MOTOR_MEMBER[] m_Motor_Member;
        private MOTOR_PID_MEMBER[] m_Motor_PID_Member;

        
        private Int16[] m_Pos_SoftStart;
        private Int16[] m_Max_Vel_Cmd;
        private Int16[] m_Max_Tor_Cmd;
        private Int16[] m_Max_PWM_Cmd;
        private UInt32[] m_Position_Target;
        private Int16[]  m_Velocity_External;
        private Int16[] m_Torque_External;


        public MOTOR_MEMBER[]  Motor_Member
        {
            get { return m_Motor_Member; }
        }

        public MOTOR_PID_MEMBER[]  Motor_PID_Member
        {
            get { return m_Motor_PID_Member; }
            set { m_Motor_PID_Member = value;
                  //UpdatePending[(int)StateName.STATE_ALL_OF_PID_PARAM] = true;
                }
        }

        public UInt32[] Position_Target
        {
            get { return m_Position_Target; }
            set { m_Position_Target = value;
                  //UpdatePending[(int)StateName.STATE_POSITION_TARGET_CMD] = true;
                }
        }

        public Int16[] Velocity_External
        {
            get { return m_Velocity_External; }
            set { m_Velocity_External = value;
                  //UpdatePending[(int)StateName.STATE_VELOCITY_EXT_CMD] = true;
                }
        }

        public Int16[] Torque_External
        {
            get { return m_Torque_External; }
            set { m_Torque_External = value;
                  //UpdatePending[(int)StateName.STATE_TORQUE_EXT_CMD] = true;
                }
        }
  

        public Int16[] Pos_SoftStart
        {
            get { return m_Pos_SoftStart; }
            set { m_Pos_SoftStart = value;
                  //UpdatePending[(int)StateName.STATE_MAX_OF_POSITION_CMD] = true;
                }
        }

        public Int16[] Max_Vel_Cmd
        {
            get { return m_Max_Vel_Cmd; }
            set { m_Max_Vel_Cmd = value;
                  //UpdatePending[(int)StateName.STATE_MAX_OF_VELOCITY_CMD] = true;
                }
        }

        public Int16[] Max_Tor_Cmd
        {
            get { return m_Max_Tor_Cmd; }
            set { m_Max_Tor_Cmd = value;
                  //UpdatePending[(int)StateName.STATE_MAX_OF_TORQUE_CMD] = true;
                }
        }

        public Int16[] Max_PWM_Cmd
        {
            get { return m_Max_PWM_Cmd; }
            set { m_Max_PWM_Cmd = value;
                  //UpdatePending[(int)StateName.STATE_MAX_OF_PWM_DUTYCYCLE] = true;
                }
        }
         
        public bool[] UdtPending
        {
            get { return UpdatePending; }
            set { UpdatePending = value;}
        }
        
        public bool IsConnected
        {
            get { return connected; }
        }

        public byte  Bit_Statis
        {
            get { return bitstatue; }
            set { bitstatue = value; }
        }
        public byte mChBox_CH
        {
            get { return mChBoxch; }
            set { mChBoxch = value; }
        }
        public bool Graph_Sketch
        {
            get { return graph_Sketch; }
            set { graph_Sketch = value; }
        }
        // Arrays for definition of the AHRS COM protocol.  The enum PName references the names
        // of all possible packets that can be transmitted and received by the AHRS class.
        // The PID array stores the packet IDs associated with each packet name in PName.
        Byte[] PID;
        // The 'DataPending' flag is 'true' when a packet has been sent to the AHRS and the
        // AHRS class is awaiting a response.  This applies for both data requests and for
        // commands.
        bool[] DataPending;
        // For each packet sent, there is a MaxDelay in milliseconds that the AHRS will wait
        // to receive a response from the AHRS before assuming that the AHRS did not receive
        // the packet.  The 'ElapsedTime' array is used to store the amount of time that has
        // passed since the initiating packet was sent.
        int[] MaxDelay;
        int[] ElapsedTime;

        // The 'UpdatePending' flah is 'true' when the internal class state has been changed,
        // but the changes have not been transmitted to the AHRS itself.
        private bool[] UpdatePending;

        /* **********************************************************************************
         * 
         * Function: private UInt16 Invalidate()
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS class to assume that none of its internal data is correct.
         * After Invalidate has been called, a call to 'synch' will cause the class
         * to re-acquire all data from the AHRS device.
         * 
         * *********************************************************************************/
        public void Invalidate()
        {
            // Clear 'update' and 'measured' data members.
            for (int i = 0; i < UpdatePending.Length; i++)
            {
                UpdatePending[i] = false;

                // Set maximum delay in milliseconds.  Set to 200 ms for each packet - special cases (ie. packets that take longer) are
                // set later.
                MaxDelay[i] = 1000;
                ElapsedTime[i] = 0;
                DataPending[i] = false;
            }

        }

        /* **********************************************************************************
         * 
         * Function: public static void OnPacketTimerTick
         * Inputs: object source, EventArgs e
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * On each timer "tick," the DataPending array is checked to determine which packets
         * have been sent and are awaiting a response.  For each pending response, the amount
         * of time that has elapsed since the packet was sent is checked.  If more time has
         * elapsed than the time (in ms) specified by MaxDelay[], then assume the operation
         * has failed.  Trigger a COMFailed event for the packet.
         * 
         * *********************************************************************************/
        public void OnPacketTimerTick(object source, EventArgs e)
        {
            int i;
            for (i = 0; i < ElapsedTime.Length; i++)
            {
                if (DataPending[i])
                {
                    ElapsedTime[i] += PacketTimer.Interval;
                    if (ElapsedTime[i] >= MaxDelay[i])
                    {
                        // Set DataPending flag to false, reset elapsed time.
                        DataPending[i] = false;
                        ElapsedTime[i] = 0;

                        // Trigger a COMFailed event
                        PacketTimeoutEvent((StateName)i, 0);
                    }
                }
            }
        }

        /* **********************************************************************************
         * 
         * Function: public bool connect( String portname, int baudrate)
         * Inputs: None
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Connects to the specified serial port.  Returns true on success, false on failure
         * 
         * *********************************************************************************/
        public bool connect(String portname, int baudrate)
        {
            //set the properties of the SerialPort Object
            serialPort.PortName = portname;
            serialPort.BaudRate = baudrate;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;

            try
            {
                //now open the port
                serialPort.Open();

                connected = true;

                // Add event handler
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                //start send
                UpdatePending[(int)StateName.STATE_WHO_AM_I] = true;
                UpdatePending[(int)StateName.STATE_FIRMWARE_VERSION] = true;
                //UpdatePending[(int)StateName.STATE_ID_STATUS] = true;
                //UpdatePending[(int)StateName.STATE_PWM_PARAMETER] = true;
                
                return true;
            }
            catch
            {
                return false;
            }
          
        }

        /* **********************************************************************************
         * 
         * Function: public bool Disconnect( )
         * Inputs: None
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Disconnects from the serial port
         * 
         * *********************************************************************************/
        public bool Disconnect()
        {
            if (connected)
            {
                connected = false;
                serialPort.Dispose(); 
                serialPort.Close();
            }

            return true;
        }

        /* **********************************************************************************
         * 
         * Function: private void serialPort_DataReceived
         * Inputs: object sender, SerialDataReceivedEventArgs e
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Event handler for serial communication
         * 
         * *********************************************************************************/
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int continue_parsing = 1;
            int bytes_to_read;

            try
            {
                bytes_to_read = serialPort.BytesToRead;

                if ((RXbufPtr + bytes_to_read) >= RX_BUF_SIZE)
                {
                    RXbufPtr = 0;
                }

                if (bytes_to_read >= RX_BUF_SIZE)
                {
                    bytes_to_read = RX_BUF_SIZE - 1;
                }
            
                // Get serial data
                serialPort.Read(RXbuffer, RXbufPtr, bytes_to_read);

                RXbufPtr += bytes_to_read;
            }
            catch
            {
                COMFailedEvent();
                return;
            }

            bool found_packet;
            int packet_start_index;
            int packet_index;

            // If there are enough bytes in the buffer to construct a full packet, then check data.
            // There are RXbufPtr bytes in the buffer at any given time
            while (RXbufPtr >= 8 && (continue_parsing == 1))
            {
                // Search for the packet start sequence
                found_packet = false;
                packet_start_index = 0;
                for (packet_index = 0; packet_index < (RXbufPtr - 2); packet_index++)
                {
                    if (RXbuffer[packet_index] == 'E' && RXbuffer[packet_index + 1] == 'C' && RXbuffer[packet_index + 2] == 'S')
                    {
                        found_packet = true;
                        packet_start_index = packet_index;

                        break;
                    }
                }

                // If start sequence found, try to recover all the data in the packet
                if (found_packet && ((RXbufPtr - packet_start_index) >= 8))
                {
                	int i;
                	Packet DataPacket = new Packet();
                    DataPacket.PacketType = RXbuffer[packet_start_index + 3];
					DataPacket.Ch_Status  = RXbuffer[packet_start_index + 4];
                    DataPacket.DataLength = RXbuffer[packet_start_index + 5];
					DataPacket.Data = new byte[DataPacket.DataLength];

                    // Only process packet if data_size is not too large.
                    if (DataPacket.DataLength <= MAX_PACKET_SIZE)
                    {

                        // If a full packet has been received, then the full packet size should be
                        // 3 + 1 + 1 + 1 + [data_size] + 1
                        // that is, 3 bytes for the start sequence, 1 byte for type, 1 byte for status, 1 byte for data length, 
                        // data_size bytes for packet data inculde 1 bytes for the CRC-8.
                        // If enough data has been received, go ahead and recover the packet.  If not, wait until the
                        // rest of the data arrives
                        int buffer_length = (RXbufPtr - packet_start_index);
                        int packet_length = (6 + DataPacket.DataLength);
                        if (buffer_length >= packet_length)
                        {
                            if (DataPacket.DataLength == 0)
                            {
                                // this packet length is wrong!!!
                            }
                            else
                            {
                                // A full packet has been received.  Retrieve the data.
                                for (i = 0; i < (DataPacket.DataLength - 1); i++)
                                {
                                    DataPacket.Data[i] = RXbuffer[packet_start_index + 6 + i];
                                }
    							DataPacket.CRC8 = RXbuffer[packet_start_index + 6 + i];

                                handle_packet(DataPacket);
                            }
                            // Copy all received bytes that weren't part of this packet into the beginning of the
                            // buffer.  Then, reset RXbufPtr.
                            for (int index = 0; index < (buffer_length - packet_length); index++)
                            {
                                RXbuffer[index] = RXbuffer[(packet_start_index + packet_length) + index];
                            }

                            RXbufPtr = (buffer_length - packet_length);
                        }
                        else
                        {
                            continue_parsing = 0;
                        }
                    }
                    else
                    {
                        // data_size was too large - the packet data is invalid.  Clear the RX buffer.
                        RXbufPtr = 0;
                        continue_parsing = 0;
                        PacketReceivedEvent(PName.CMD_OVER_DATA_LENGTH, -1);
                    }
                }
                else
                {
                    continue_parsing = 0;
                }
            }
        }

        /* **********************************************************************************
         * 
         * Function: private int getTypeIndex
         * Inputs: byte type
         * Outputs: None
         * Return Value: The index of the PID in 'byte type'
         * Dependencies: None
         * Description: 
         * 
         * Finds the index of the packet type of the packet specified by 'type'.  If no
         * packet was found, returns -1.
         * 
         * *********************************************************************************/
        private int getTypeIndex(byte type)
        {
            int type_index = -1;

            // Iterate through the PID array to determine which packet was received
            // (ie. which enum PName was received).
            for (int i = 0; i < PID.Length; i++)
            {
                if (PID[i] == type)
                {
                    type_index = i;
                    break;
                }
            }

            return type_index;
        }

        /* **********************************************************************************
         * 
         * Function: private void updatePacketSynch
         * Inputs: PName packet
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * The AHRS class keeps track of packets it has sent and received - if a packet requesting
         * data is sent to the AHRS device, then the class will watch to receive the data.
         * If the data is not received in MaxDelay milliseconds, then a PacketTimeoutEvent is
         * fired (from elsewhere in the class, not from the updatePacketSynch function).
         * 
         * Timing is tracked using several arrays:
         * The ElapsedTime[] array keeps track of how much time has elapsed since a packet
         * requesting data was sent.
         * The DataPending[] array keeps track of which channels have "pending" data - ie.
         * which channels the AHRS class is expecting to receive data on.
         * The Measured[] array keeps track of which data has been received at least once
         * from the AHRS - this allows the AHRS class to synchronize itself with the device.
         * The UpdatePending[] arrays keeps track of which internal class data has been changed,
         * but has not yet been written to the AHRS.
         * 
         * Whenever a new packet is received from the AHRS device, the DataPending array
         * needs to be updated to reflect the data that was just received - if data was received,
         * then it is no longer "pending."
         * 
         * The updatePacketSynch function performs the aforementioned tasks.
         * 
         * *********************************************************************************/
        private void updatePacketSynch(PName packet, byte[] data)
        {
            switch (packet)
            {
				case PName.WHO_AM_I:
					 updatePacketSynchHelper(StateName.STATE_WHO_AM_I);
					 break;
                     
                case PName.FIRMWARE_VERSION:
                    updatePacketSynchHelper(StateName.STATE_FIRMWARE_VERSION);
                break;
                
                case PName.ID_STATUS:
                    updatePacketSynchHelper(StateName.STATE_ID_STATUS);
                break;
                
                case PName.PWM_PARAMETER:
                    updatePacketSynchHelper(StateName.STATE_PWM_PARAMETER);
                break;

                case PName.REPORT_SENSOR_ADC:
                    updatePacketSynchHelper(StateName.STATE_REPORT_SENSOR_ADC);
                break;

                case PName.CMD_COMPLETE:
                    if (data[0] == PID[(int)PName.KICK_OFF_CONTROLLER])
                    {
                        updatePacketSynchHelper(StateName.STATE_KICK_OFF_CONTROLLER);
                    }
                    else if (data[0] == PID[(int)PName.RESET_CONTROLLER])
                    {
                        updatePacketSynchHelper(StateName.STATE_RESET_CONTROLLER);
                    }
                    else if (data[0] == PID[(int)PName.HOME_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_HOME_CMD);
                    }
                    else if (data[0] == PID[(int)PName.PAUSE_REPORT_INFO])
                    {
                        updatePacketSynchHelper(StateName.STATE_PAUSE_REPORT_INFO);
                    }
                    else if (data[0] == PID[(int)PName.CONTINUE_REPORT_INFO])
                    {
                        updatePacketSynchHelper(StateName.STATE_CONTINUE_REPORT_INFO);
                    }
                    else if (data[0] == PID[(int)PName.ALL_OF_PID_PARAM])
                    {
                        updatePacketSynchHelper(StateName.STATE_ALL_OF_PID_PARAM);
                    }
                    else if (data[0] == PID[(int)PName.MAX_OF_POSITION_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_MAX_OF_POSITION_CMD);
                    }
                    else if (data[0] == PID[(int)PName.MAX_OF_VELOCITY_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_MAX_OF_VELOCITY_CMD);
                    }
                    else if (data[0] == PID[(int)PName.MAX_OF_TORQUE_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_MAX_OF_TORQUE_CMD);
                    }
                    else if (data[0] == PID[(int)PName.MAX_OF_PWM_DUTYCYCLE])
                    {
                        updatePacketSynchHelper(StateName.STATE_MAX_OF_PWM_DUTYCYCLE);
                    }
                    else if (data[0] == PID[(int)PName.POSITION_TARGET_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_POSITION_TARGET_CMD);
                    }
                    else if (data[0] == PID[(int)PName.VELOCITY_EXT_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_VELOCITY_EXT_CMD);
                    }                   
                    else if (data[0] == PID[(int)PName.TORQUE_EXT_CMD])
                    {
                        updatePacketSynchHelper(StateName.STATE_TORQUE_EXT_CMD);
                    }
                    
                break;

                default:
                    break;
            }

        }

        private void updatePacketSynchHelper(StateName state)
        {
            DataPending[(int)state] = false;
            ElapsedTime[(int)state] = 0;
            UpdatePending[(int)state] = false;
        }

        /* **********************************************************************************
         * 
         * Function: private void handle_packet
         * Inputs: byte type, int length, byte[] data
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Handles data packets received over the serial port.
         * 
         * *********************************************************************************/
        private void handle_packet( Packet DataPacket)
        {
            int type_index = -1;
			byte CRC8_Temp;
            byte_conversion_array DConvert = new byte_conversion_array();

            type_index = getTypeIndex(DataPacket.PacketType);

            // For the packet received, update 'dataPending' and 'ElapsedTime' flags
            if (type_index != -1)
            {
            
                CRC8_Temp = Check_CRC8(DataPacket); // calculation CRC-8

                if ( CRC8_Temp != DataPacket.CRC8)
                {
                    PacketReceivedEvent(PName.CMD_CRC_FAILED, -1);
                }
                else
                {
                    updatePacketSynch((PName)type_index, DataPacket.Data);
                }
            }
            else
            {
                // Generate a COMMAND_COMPLETE event with a -1 flag.  The -1 indicates that the packet
                // wasn't recognized.
                PacketReceivedEvent(PName.CMD_COMPLETE, -1);
                return;
            }

            switch (type_index)
            {
                case (int)PName.WHO_AM_I:
                    PacketLabelEvent(DataPacket.Data , 1);
                    PacketReceivedEvent(PName.WHO_AM_I, type_index);
					break;

                case (int)PName.FIRMWARE_VERSION:
                    PacketLabelEvent(DataPacket.Data , 2);
                    PacketReceivedEvent(PName.FIRMWARE_VERSION, type_index);
					break;
					
                case (int)PName.REPORT_SENSOR_ADC:
                    PacketLabelEvent(DataPacket.Data , 23);
                    PacketReceivedEvent(PName.REPORT_SENSOR_ADC, type_index);
					break;
					
                case (int)PName.ID_STATUS:
                
                    PacketReceivedEvent(PName.ID_STATUS, type_index);
                    break;
                
                case (int)PName.PWM_PARAMETER:
                
                    PacketReceivedEvent(PName.PWM_PARAMETER, type_index);
                    break;
       
                case (int)PName.CMD_COMPLETE:

                    type_index = getTypeIndex(DataPacket.Data[0]);

                    PacketReceivedEvent(PName.CMD_COMPLETE, type_index);

                    break;

                case (int)PName.CMD_NO_SUPPORT:

                    PacketReceivedEvent(PName.CMD_NO_SUPPORT, DataPacket.Data[0]);

                    break;

                case (int)PName.CMD_CRC_FAILED:

                    PacketReceivedEvent(PName.CMD_CRC_FAILED, 0);

                    break;

                case (int)PName.CMD_OVER_DATA_LENGTH:

                    PacketReceivedEvent(PName.CMD_OVER_DATA_LENGTH, 0);

                    break;

                case (int)PName.CONTROLLER_STATUS:

                    if (DataPacket.DataLength != 23)
                    {
                        // this packet is wrong!!!
                        return;
                    }
                    byte ch_tmp;
                    ch_tmp = Convert.ToByte(Convert.ToByte(DataPacket.Ch_Status) & 0xf);
                    switch (ch_tmp)
                    {
                        case 0:
                            m_Motor_Member[0].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[0].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[0].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[0].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[0].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[0].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[0].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[0].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[0].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[0].PWM_Output = DataPacket.Data[21];
                            break;
                        case 1:
                            m_Motor_Member[1].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[1].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[1].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[1].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[1].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[1].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[1].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[1].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[1].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[1].PWM_Output = DataPacket.Data[21];
                            break;
                        case 2:
                            m_Motor_Member[2].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[2].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[2].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[2].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[2].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[2].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[2].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[2].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[2].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[2].PWM_Output = DataPacket.Data[21];
                            break;
                        case 3:
                            m_Motor_Member[3].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[3].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[3].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[3].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[3].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[3].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[3].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[3].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[3].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[3].PWM_Output = DataPacket.Data[21];
                            break;
                        case 4:
                            m_Motor_Member[4].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[4].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[4].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[4].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[4].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[4].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[4].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[4].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[4].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[4].PWM_Output = DataPacket.Data[21];
                            break;
                        case 5:
                            m_Motor_Member[5].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[5].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[5].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[5].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[5].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[5].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[5].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[5].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[5].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[5].PWM_Output = DataPacket.Data[21];
                            break;
                        case 6:
                            m_Motor_Member[6].ticker = DataPacket.Data[0];

                            DConvert.byte0 = DataPacket.Data[1];
                            DConvert.byte1 = DataPacket.Data[2];
                            DConvert.byte2 = DataPacket.Data[3];
                            DConvert.byte3 = DataPacket.Data[4];
                            m_Motor_Member[6].Position_Target = DConvert.uint32; // command

                            DConvert.byte0 = DataPacket.Data[5];
                            DConvert.byte1 = DataPacket.Data[6];
                            DConvert.byte2 = DataPacket.Data[7];
                            DConvert.byte3 = DataPacket.Data[8];
                            m_Motor_Member[6].QEI32 = DConvert.uint32;

                            DConvert.byte0 = DataPacket.Data[9];
                            DConvert.byte1 = DataPacket.Data[10];
                            m_Motor_Member[6].Velocity_External = DConvert.int16_0; // command

                            DConvert.byte0 = DataPacket.Data[11];
                            DConvert.byte1 = DataPacket.Data[12];
                            m_Motor_Member[6].Velocity_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[13];
                            DConvert.byte1 = DataPacket.Data[14];
                            m_Motor_Member[6].QEI_Diff16 = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[15];
                            DConvert.byte1 = DataPacket.Data[16];
                            m_Motor_Member[6].Torque_External = DConvert.int16_0; //command

                            DConvert.byte0 = DataPacket.Data[17];
                            DConvert.byte1 = DataPacket.Data[18];
                            m_Motor_Member[6].Torque_Internal = DConvert.int16_0;

                            DConvert.byte0 = DataPacket.Data[19];
                            DConvert.byte1 = DataPacket.Data[20];
                            m_Motor_Member[6].Motor_Current = DConvert.int16_0;

                            m_Motor_Member[6].PWM_Output = DataPacket.Data[21];
                            break;

                        default:

                            break;

                    }
                    /*
                    m_Motor_Member[mChBox_CH].ticker = DataPacket.Data[0];

                    DConvert.byte0 = DataPacket.Data[1];
                    DConvert.byte1 = DataPacket.Data[2];
                    DConvert.byte2 = DataPacket.Data[3];
                    DConvert.byte3 = DataPacket.Data[4];
                    m_Motor_Member[mChBox_CH].Position_Target = DConvert.uint32; // command
                    
                    DConvert.byte0 = DataPacket.Data[5];
                    DConvert.byte1 = DataPacket.Data[6];
                    DConvert.byte2 = DataPacket.Data[7];
                    DConvert.byte3 = DataPacket.Data[8];
                    m_Motor_Member[mChBox_CH].QEI32 = DConvert.uint32;

                    DConvert.byte0 = DataPacket.Data[9];
                    DConvert.byte1 = DataPacket.Data[10];
                    m_Motor_Member[mChBox_CH].Velocity_External = DConvert.int16_0; // command

                    DConvert.byte0 = DataPacket.Data[11];
                    DConvert.byte1 = DataPacket.Data[12];
                    m_Motor_Member[mChBox_CH].Velocity_Internal = DConvert.int16_0;
                    
                    DConvert.byte0 = DataPacket.Data[13];
                    DConvert.byte1 = DataPacket.Data[14];
                    m_Motor_Member[mChBox_CH].QEI_Diff16 = DConvert.int16_0;

                    DConvert.byte0 = DataPacket.Data[15];
                    DConvert.byte1 = DataPacket.Data[16];
                    m_Motor_Member[mChBox_CH].Torque_External = DConvert.int16_0; //command

                    DConvert.byte0 = DataPacket.Data[17];
                    DConvert.byte1 = DataPacket.Data[18];
                    m_Motor_Member[mChBox_CH].Torque_Internal = DConvert.int16_0;
             
                    DConvert.byte0 = DataPacket.Data[19];
                    DConvert.byte1 = DataPacket.Data[20];
                    m_Motor_Member[mChBox_CH].Motor_Current = DConvert.int16_0;

                    m_Motor_Member[mChBox_CH].PWM_Output = DataPacket.Data[21];
                    
                    */
                    PacketReceivedEvent(PName.CONTROLLER_STATUS, 0);
                   
                    bitstatue = Convert.ToByte(Convert.ToByte(DataPacket.Ch_Status) & 0xf0);
                    //bitstatue +=1;

                    DataReceivedEvent(ch_tmp);
                    
                    break;

					
                 default:
                    
                    break;

            }

        }

        /* **********************************************************************************
         * 
         * Function: public bool synch()
         * Inputs: None
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Synchronizes the stored AHRS states with actual states on  the device.  There are
         * two flags used by the AHRS class to keep track of synchronization.  For each setting,
         * the _measured flag indicates that the current AHRS state has been retrieved and stored
         * by the class.  The _updated flag indicates that the class' internal state has changed,
         * but that the AHRS itself has not yet been updated with the new data.
         * 
         * The 'synch' function first updates the AHRS by sending COM packets to set all
         * data for which the _updated flag has been set.  Then, all data that has not yet been
         * measured is requested.
         * 
         * If synchronization succeeds, the function returns 'true'.  'false' otherwise.
         * 
         * *********************************************************************************/

        public bool synch()
        {
            bool complete = true;

            if (!connected)
                return false;

            // First, iterate through the PUpdatePending array to determine which AHRS states need
            // to be updated.
            for (int i = 0; i < UpdatePending.Length; i++)
            {
                if (UpdatePending[i])
                {
                    DataPending[i] = true;
                    
                    // Call UpdateAHRS to send packet to the AHRS to synchronize data
                    if (!updateAHRS(i))
                    {
                        complete = false;
                        DataPending[i] = false;
                        
                    }
                }
            }

            return complete;
        }

        /* **********************************************************************************
         * 
         * Function: private bool updateAHRS
         * Inputs: int index
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Causes a packet to be sent to the AHRS to update its state to conform with the internal
         * state of the AHRS class.
         * 
         * Returns 'true' on success, 'false' otherwise
         * 
         * *********************************************************************************/
        private bool updateAHRS(int index)
        {
            Packet AHRSPacket = new Packet();
            byte_conversion_array DConvert = new byte_conversion_array();
            AHRSPacket.Ch_Status = mChBox_CH;

            if (index == (int)StateName.STATE_WHO_AM_I)
            {
                AHRSPacket.PacketType = PID[(int)PName.REPORT_MCU_INFORMATION];
                AHRSPacket.DataLength = 2;

                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
				AHRSPacket.Data[0] = PID[(int)PName.WHO_AM_I];
  
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_FIRMWARE_VERSION)
            {
                AHRSPacket.PacketType = PID[(int)PName.REPORT_MCU_INFORMATION];
                AHRSPacket.DataLength = 2;

                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
				AHRSPacket.Data[0] = PID[(int)PName.FIRMWARE_VERSION];
  
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_ID_STATUS)
            {
                AHRSPacket.PacketType = PID[(int)PName.REPORT_MCU_INFORMATION];
                AHRSPacket.DataLength = 2;

                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
				AHRSPacket.Data[0] = PID[(int)PName.ID_STATUS];
  
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_PWM_PARAMETER)
            {
                AHRSPacket.PacketType = PID[(int)PName.REPORT_MCU_INFORMATION];
                AHRSPacket.DataLength = 2;

                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
				AHRSPacket.Data[0] = PID[(int)PName.PWM_PARAMETER];
  
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_KICK_OFF_CONTROLLER)
            {
                AHRSPacket.PacketType = PID[(int)PName.KICK_OFF_CONTROLLER];
                AHRSPacket.DataLength = 1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_RESET_CONTROLLER)
            {
                AHRSPacket.PacketType = PID[(int)PName.RESET_CONTROLLER];
                AHRSPacket.DataLength = 1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_REPORT_SENSOR_ADC)
            {
                AHRSPacket.PacketType = PID[(int)PName.REPORT_MCU_INFORMATION];
                AHRSPacket.DataLength = 2;

                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
				AHRSPacket.Data[0] = PID[(int)PName.REPORT_SENSOR_ADC];
  
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_HOME_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.HOME_CMD];
                AHRSPacket.DataLength = 1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_PAUSE_REPORT_INFO)
            {
                AHRSPacket.PacketType = PID[(int)PName.PAUSE_REPORT_INFO];
                AHRSPacket.DataLength = 1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_CONTINUE_REPORT_INFO)
            {
                AHRSPacket.PacketType = PID[(int)PName.CONTINUE_REPORT_INFO];
                AHRSPacket.DataLength = 1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_ALL_OF_PID_PARAM)
            {
                AHRSPacket.PacketType = PID[(int)PName.ALL_OF_PID_PARAM];
                AHRSPacket.DataLength = 49;

                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Pos.Kp;
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
				AHRSPacket.Data[2] = DConvert.byte2;
				AHRSPacket.Data[3] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Pos.Ki;
				AHRSPacket.Data[4] = DConvert.byte0;
				AHRSPacket.Data[5] = DConvert.byte1;
				AHRSPacket.Data[6] = DConvert.byte2;
				AHRSPacket.Data[7] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Pos.Kd;
				AHRSPacket.Data[8] = DConvert.byte0;
				AHRSPacket.Data[9] = DConvert.byte1;
				AHRSPacket.Data[10] = DConvert.byte2;
				AHRSPacket.Data[11] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Pos.Int_sat;
				AHRSPacket.Data[12] = DConvert.byte0;
				AHRSPacket.Data[13] = DConvert.byte1;
				AHRSPacket.Data[14] = DConvert.byte2;
				AHRSPacket.Data[15] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Vel.Kp;
				AHRSPacket.Data[16] = DConvert.byte0;
				AHRSPacket.Data[17] = DConvert.byte1;
				AHRSPacket.Data[18] = DConvert.byte2;
				AHRSPacket.Data[19] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Vel.Ki;
				AHRSPacket.Data[20] = DConvert.byte0;
				AHRSPacket.Data[21] = DConvert.byte1;
				AHRSPacket.Data[22] = DConvert.byte2;
				AHRSPacket.Data[23] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Vel.Kd;
				AHRSPacket.Data[24] = DConvert.byte0;
				AHRSPacket.Data[25] = DConvert.byte1;
				AHRSPacket.Data[26] = DConvert.byte2;
				AHRSPacket.Data[27] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Vel.Int_sat;
				AHRSPacket.Data[28] = DConvert.byte0;
				AHRSPacket.Data[29] = DConvert.byte1;
				AHRSPacket.Data[30] = DConvert.byte2;
				AHRSPacket.Data[31] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Tor.Kp;
				AHRSPacket.Data[32] = DConvert.byte0;
				AHRSPacket.Data[33] = DConvert.byte1;
				AHRSPacket.Data[34] = DConvert.byte2;
				AHRSPacket.Data[35] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Tor.Ki;
				AHRSPacket.Data[36] = DConvert.byte0;
				AHRSPacket.Data[37] = DConvert.byte1;
				AHRSPacket.Data[38] = DConvert.byte2;
				AHRSPacket.Data[39] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Tor.Kd;
				AHRSPacket.Data[40] = DConvert.byte0;
				AHRSPacket.Data[41] = DConvert.byte1;
				AHRSPacket.Data[42] = DConvert.byte2;
				AHRSPacket.Data[43] = DConvert.byte3;

                DConvert.int32 = m_Motor_PID_Member[mChBox_CH].Tor.Int_sat;
				AHRSPacket.Data[44] = DConvert.byte0;
				AHRSPacket.Data[45] = DConvert.byte1;
				AHRSPacket.Data[46] = DConvert.byte2;
				AHRSPacket.Data[47] = DConvert.byte3;
				//AHRSPacket.Data[48] = 0x00;
				//AHRSPacket.Data[49] = 0x00;

                AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8
                
                if (!sendPacket(AHRSPacket))
                    return false;                    

            }
            else if (index == (int)StateName.STATE_MAX_OF_POSITION_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.MAX_OF_POSITION_CMD];
                AHRSPacket.DataLength = 3;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.int16_0 = m_Pos_SoftStart[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_MAX_OF_VELOCITY_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.MAX_OF_VELOCITY_CMD];
                AHRSPacket.DataLength = 3;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.int16_0 = m_Max_Vel_Cmd[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_MAX_OF_TORQUE_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.MAX_OF_TORQUE_CMD];
                AHRSPacket.DataLength = 3;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.int16_0 = m_Max_Tor_Cmd[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_MAX_OF_PWM_DUTYCYCLE)
            {
                AHRSPacket.PacketType = PID[(int)PName.MAX_OF_PWM_DUTYCYCLE];
                AHRSPacket.DataLength = 3;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.int16_0 = m_Max_PWM_Cmd[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_POSITION_TARGET_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.POSITION_TARGET_CMD];
                AHRSPacket.DataLength = 5;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.uint32 = m_Position_Target[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
				AHRSPacket.Data[2] = DConvert.byte2;
				AHRSPacket.Data[3] = DConvert.byte3;
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_VELOCITY_EXT_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.VELOCITY_EXT_CMD];
                AHRSPacket.DataLength = 3;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.int16_0 = m_Velocity_External[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;

				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }
            else if (index == (int)StateName.STATE_TORQUE_EXT_CMD)
            {
                AHRSPacket.PacketType = PID[(int)PName.TORQUE_EXT_CMD];
                AHRSPacket.DataLength = 3;
                
                AHRSPacket.Data = new byte[AHRSPacket.DataLength - 1];
                DConvert.int16_0 = m_Torque_External[mChBox_CH];
				AHRSPacket.Data[0] = DConvert.byte0;
				AHRSPacket.Data[1] = DConvert.byte1;
                
				AHRSPacket.CRC8 = Check_CRC8(AHRSPacket); // calculation CRC-8

				if (!sendPacket(AHRSPacket))
					return false;                    
 
            }            
           


            return true;
        }
        /* **********************************************************************************
         * 
         * Function: private bool sendPacket
         * Inputs: Packet AHRSPacket - the packet to be transmitted
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Sends the specified packet to the AHRS.
         * 
         * Returns 'true' on success, 'false' otherwise
         * 
         * *********************************************************************************/
        private bool sendPacket( Packet AHRSPacket )
        {
            int i;

            if (!connected)
                return false;

			if (AHRSPacket.DataLength == 0)
			{
				// this command is failed!!!
				return false;
			}

            byte[] packet = new byte[AHRSPacket.DataLength + 6];

            // Build packet header
            packet[0] = (byte)'E';
            packet[1] = (byte)'C';
            packet[2] = (byte)'S';
            packet[3] = AHRSPacket.PacketType;
            packet[4] = AHRSPacket.Ch_Status; // channle/status
            packet[5] = AHRSPacket.DataLength;

            // Fill data section
            for (i = 0; i < (AHRSPacket.DataLength - 1); i++)
            {
                packet[6 + i] = AHRSPacket.Data[i];
            }

            // Add CRC-8 to end of packet
            packet[6 + i] = AHRSPacket.CRC8;

            // Now write the packet to the serial port
            try
            {
                serialPort.Write(packet, 0, AHRSPacket.DataLength + 6);
                
                PacketSentEvent((PName)getTypeIndex(AHRSPacket.PacketType), 0);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        /* **********************************************************************************
         * 
         * Function: public void Kick_Off
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to execute a Kick-Off command
         * 
         * *********************************************************************************/
        public void Kick_Off()
        {
            UpdatePending[(int)StateName.STATE_KICK_OFF_CONTROLLER] = true;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void Home_Cmd
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to execute a Home command
         * 
         * *********************************************************************************/
        public void Home_Cmd()
        {
            UpdatePending[(int)StateName.STATE_HOME_CMD] = true;

            synch();
        }		

        /* **********************************************************************************
         * 
         * Function: public void Pause_Report_Info
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to execute a Pause_Report_Info command
         * 
         * *********************************************************************************/
        public void Pause_Report_Info()
        {
            UpdatePending[(int)StateName.STATE_PAUSE_REPORT_INFO] = true;

            synch();
        }	

        /* **********************************************************************************
         * 
         * Function: public void Continue_Report_Info
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to execute a Continue_Report_Info command
         * 
         * *********************************************************************************/
        public void Continue_Report_Info()
        {
            UpdatePending[(int)StateName.STATE_CONTINUE_REPORT_INFO] = true;

            synch();
        }	

        /* **********************************************************************************
         * 
         * Function: public void Report_Sensor_ADC
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to execute a Report_Sensor_ADC command
         * 
         * *********************************************************************************/
        public void Report_Sensor_ADC()
        {
            UpdatePending[(int)StateName.STATE_REPORT_SENSOR_ADC] = true;

            synch();
        }	

        /* **********************************************************************************
         * 
         * Function: private UInt16 ComputeChecksum
         * Inputs: byte packet
         * Outputs: None
         * Return Value: A two-byte checksum
         * Dependencies: None
         * Description: 
         * 
         * Computes the sum of all bytes in the packet and returns a two byte value.
         * 
         * 
         * *********************************************************************************/
        private UInt16 ComputeChecksum(byte[] packet, int length)
        {
            UInt16 sum = 0;
            int i;

            for (i = 0; i < length; i++)
            {
                sum += packet[i];
            }

            return sum;
        }


        /* **********************************************************************************
         * 
         * Function: private byte Check_CRC8
         * Inputs: byte packet
         * Outputs: None
         * Return Value: A CRC-8 value
         * Dependencies: None
         * Description: 
         * 
         * Computes the CRC-8 of all bytes in the packet and returns a byte value.
         * 
         * 
         * *********************************************************************************/
		
		private byte[] crc8tab = new byte[256] 
		{
		    0x00, 0x07, 0x0E, 0x09, 0x1C, 0x1B, 0x12, 0x15,
			0x38, 0x3F, 0x36, 0x31, 0x24, 0x23, 0x2A, 0x2D,
			0x70, 0x77, 0x7E, 0x79, 0x6C, 0x6B, 0x62, 0x65,
			0x48, 0x4F, 0x46, 0x41, 0x54, 0x53, 0x5A, 0x5D,
			0xE0, 0xE7, 0xEE, 0xE9, 0xFC, 0xFB, 0xF2, 0xF5,
			0xD8, 0xDF, 0xD6, 0xD1, 0xC4, 0xC3, 0xCA, 0xCD,
			0x90, 0x97, 0x9E, 0x99, 0x8C, 0x8B, 0x82, 0x85,
			0xA8, 0xAF, 0xA6, 0xA1, 0xB4, 0xB3, 0xBA, 0xBD,
			0xC7, 0xC0, 0xC9, 0xCE, 0xDB, 0xDC, 0xD5, 0xD2,
			0xFF, 0xF8, 0xF1, 0xF6, 0xE3, 0xE4, 0xED, 0xEA,
			0xB7, 0xB0, 0xB9, 0xBE, 0xAB, 0xAC, 0xA5, 0xA2,
			0x8F, 0x88, 0x81, 0x86, 0x93, 0x94, 0x9D, 0x9A,
			0x27, 0x20, 0x29, 0x2E, 0x3B, 0x3C, 0x35, 0x32,
			0x1F, 0x18, 0x11, 0x16, 0x03, 0x04, 0x0D, 0x0A,
			0x57, 0x50, 0x59, 0x5E, 0x4B, 0x4C, 0x45, 0x42,
			0x6F, 0x68, 0x61, 0x66, 0x73, 0x74, 0x7D, 0x7A,
			0x89, 0x8E, 0x87, 0x80, 0x95, 0x92, 0x9B, 0x9C,
			0xB1, 0xB6, 0xBF, 0xB8, 0xAD, 0xAA, 0xA3, 0xA4,
			0xF9, 0xFE, 0xF7, 0xF0, 0xE5, 0xE2, 0xEB, 0xEC,
			0xC1, 0xC6, 0xCF, 0xC8, 0xDD, 0xDA, 0xD3, 0xD4,
			0x69, 0x6E, 0x67, 0x60, 0x75, 0x72, 0x7B, 0x7C,
			0x51, 0x56, 0x5F, 0x58, 0x4D, 0x4A, 0x43, 0x44,
			0x19, 0x1E, 0x17, 0x10, 0x05, 0x02, 0x0B, 0x0C,
			0x21, 0x26, 0x2F, 0x28, 0x3D, 0x3A, 0x33, 0x34,
			0x4E, 0x49, 0x40, 0x47, 0x52, 0x55, 0x5C, 0x5B,
			0x76, 0x71, 0x78, 0x7F, 0x6A, 0x6D, 0x64, 0x63,
			0x3E, 0x39, 0x30, 0x37, 0x22, 0x25, 0x2C, 0x2B,
			0x06, 0x01, 0x08, 0x0F, 0x1A, 0x1D, 0x14, 0x13,
			0xAE, 0xA9, 0xA0, 0xA7, 0xB2, 0xB5, 0xBC, 0xBB,
			0x96, 0x91, 0x98, 0x9F, 0x8A, 0x8D, 0x84, 0x83,
			0xDE, 0xD9, 0xD0, 0xD7, 0xC2, 0xC5, 0xCC, 0xCB,
			0xE6, 0xE1, 0xE8, 0xEF, 0xFA, 0xFD, 0xF4, 0xF3
		};

		private byte updcrc8(byte crc, byte data)
		{

		    crc = crc8tab[ crc ^ data]; //look-up CRC-8 table
		    return crc;
		}

		
        private byte Check_CRC8(Packet packet)
        {
			byte CRC8_Temp = 0;
			byte Index;
				
			CRC8_Temp = updcrc8( CRC8_Temp, (byte)'E');
			CRC8_Temp = updcrc8( CRC8_Temp, (byte)'C');
			CRC8_Temp = updcrc8( CRC8_Temp, (byte)'S');
			
			CRC8_Temp = updcrc8( CRC8_Temp, packet.PacketType);
			CRC8_Temp = updcrc8( CRC8_Temp, packet.Ch_Status);
			CRC8_Temp = updcrc8( CRC8_Temp, packet.DataLength);
			
			for ( Index = 0; Index < (packet.DataLength - 1); Index++ )
			{
				CRC8_Temp = updcrc8( CRC8_Temp, packet.Data[Index]);
			}
			
			return CRC8_Temp;

        }		


    }
}
