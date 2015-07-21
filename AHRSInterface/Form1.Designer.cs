namespace AHRSInterface
{
    partial class AHRSInterface
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AHRSInterface));
            this.statusBox = new System.Windows.Forms.RichTextBox();
            this.SynchButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dialogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logDataToolstripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.serialPortCOMBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.baudSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.serialConnectButton = new System.Windows.Forms.ToolStripButton();
            this.serialDisconnectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.KickOff = new System.Windows.Forms.Button();
            this.PosGraph = new ZedGraph.ZedGraphControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.VelGraph = new ZedGraph.ZedGraphControl();
            this.CurGraph = new ZedGraph.ZedGraphControl();
            this.mChBox = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CurKiSatBox = new System.Windows.Forms.TextBox();
            this.CurKdBox = new System.Windows.Forms.TextBox();
            this.CurKiBox = new System.Windows.Forms.TextBox();
            this.CurKpBox = new System.Windows.Forms.TextBox();
            this.VelKiSatBox = new System.Windows.Forms.TextBox();
            this.VelKdBox = new System.Windows.Forms.TextBox();
            this.VelKiBox = new System.Windows.Forms.TextBox();
            this.VelKpBox = new System.Windows.Forms.TextBox();
            this.PosKiSatBox = new System.Windows.Forms.TextBox();
            this.PosKdBox = new System.Windows.Forms.TextBox();
            this.PosKiBox = new System.Windows.Forms.TextBox();
            this.PosKpBox = new System.Windows.Forms.TextBox();
            this.ExtVelBox = new System.Windows.Forms.TextBox();
            this.ExtTorBox = new System.Windows.Forms.TextBox();
            this.ExtPosBox = new System.Windows.Forms.TextBox();
            this.MaxOptBox = new System.Windows.Forms.TextBox();
            this.MaxVelBox = new System.Windows.Forms.TextBox();
            this.MaxTorBox = new System.Windows.Forms.TextBox();
            this.SoftstartBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.picbit7 = new System.Windows.Forms.PictureBox();
            this.picbit6 = new System.Windows.Forms.PictureBox();
            this.picbit5 = new System.Windows.Forms.PictureBox();
            this.picbit4 = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.Home = new System.Windows.Forms.Button();
            this.Sensor_btn = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbit4)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(863, 369);
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(272, 272);
            this.statusBox.TabIndex = 0;
            this.statusBox.Text = "";
            this.statusBox.TextChanged += new System.EventHandler(this.statusBox_TextChanged);
            // 
            // SynchButton
            // 
            this.SynchButton.Location = new System.Drawing.Point(191, 52);
            this.SynchButton.Name = "SynchButton";
            this.SynchButton.Size = new System.Drawing.Size(75, 21);
            this.SynchButton.TabIndex = 1;
            this.SynchButton.Text = "Synch";
            this.SynchButton.UseVisualStyleBackColor = true;
            this.SynchButton.Click += new System.EventHandler(this.SynchButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dialogsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1147, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dialogsToolStripMenuItem
            // 
            this.dialogsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.magCalibrationToolStripMenuItem,
            this.logDataToolstripItem});
            this.dialogsToolStripMenuItem.Name = "dialogsToolStripMenuItem";
            this.dialogsToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.dialogsToolStripMenuItem.Text = "Dialogs";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Enabled = false;
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // magCalibrationToolStripMenuItem
            // 
            this.magCalibrationToolStripMenuItem.Enabled = false;
            this.magCalibrationToolStripMenuItem.Name = "magCalibrationToolStripMenuItem";
            this.magCalibrationToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.magCalibrationToolStripMenuItem.Text = "Mag. Calibration";
            this.magCalibrationToolStripMenuItem.Click += new System.EventHandler(this.magCalibrationToolStripMenuItem_Click);
            // 
            // logDataToolstripItem
            // 
            this.logDataToolstripItem.Enabled = false;
            this.logDataToolstripItem.Name = "logDataToolstripItem";
            this.logDataToolstripItem.Size = new System.Drawing.Size(171, 22);
            this.logDataToolstripItem.Text = "Log";
            this.logDataToolstripItem.Click += new System.EventHandler(this.logDataToolstripItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.serialPortCOMBox,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.baudSelectBox,
            this.toolStripSeparator2,
            this.serialConnectButton,
            this.serialDisconnectButton,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1147, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel1.Text = "Port:";
            // 
            // serialPortCOMBox
            // 
            this.serialPortCOMBox.Name = "serialPortCOMBox";
            this.serialPortCOMBox.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(40, 22);
            this.toolStripLabel2.Text = "Baud:";
            // 
            // baudSelectBox
            // 
            this.baudSelectBox.Name = "baudSelectBox";
            this.baudSelectBox.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // serialConnectButton
            // 
            this.serialConnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.serialConnectButton.Image = ((System.Drawing.Image)(resources.GetObject("serialConnectButton.Image")));
            this.serialConnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.serialConnectButton.Name = "serialConnectButton";
            this.serialConnectButton.Size = new System.Drawing.Size(23, 22);
            this.serialConnectButton.Text = "c";
            this.serialConnectButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // serialDisconnectButton
            // 
            this.serialDisconnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.serialDisconnectButton.Enabled = false;
            this.serialDisconnectButton.Image = ((System.Drawing.Image)(resources.GetObject("serialDisconnectButton.Image")));
            this.serialDisconnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.serialDisconnectButton.Name = "serialDisconnectButton";
            this.serialDisconnectButton.Size = new System.Drawing.Size(23, 22);
            this.serialDisconnectButton.Text = "c";
            this.serialDisconnectButton.Click += new System.EventHandler(this.serialDisconnectButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "c";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(421, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(598, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // KickOff
            // 
            this.KickOff.Location = new System.Drawing.Point(1054, 340);
            this.KickOff.Name = "KickOff";
            this.KickOff.Size = new System.Drawing.Size(75, 23);
            this.KickOff.TabIndex = 6;
            this.KickOff.Text = "Start";
            this.KickOff.UseVisualStyleBackColor = true;
            this.KickOff.Click += new System.EventHandler(this.KickOff_Click);
            // 
            // PosGraph
            // 
            this.PosGraph.Location = new System.Drawing.Point(12, 64);
            this.PosGraph.Name = "PosGraph";
            this.PosGraph.ScrollGrace = 0D;
            this.PosGraph.ScrollMaxX = 200D;
            this.PosGraph.ScrollMaxY = 0D;
            this.PosGraph.ScrollMaxY2 = 0D;
            this.PosGraph.ScrollMinX = 0D;
            this.PosGraph.ScrollMinY = 0D;
            this.PosGraph.ScrollMinY2 = 0D;
            this.PosGraph.Size = new System.Drawing.Size(845, 260);
            this.PosGraph.TabIndex = 7;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // VelGraph
            // 
            this.VelGraph.Location = new System.Drawing.Point(12, 331);
            this.VelGraph.Name = "VelGraph";
            this.VelGraph.ScrollGrace = 0D;
            this.VelGraph.ScrollMaxX = 0D;
            this.VelGraph.ScrollMaxY = 0D;
            this.VelGraph.ScrollMaxY2 = 0D;
            this.VelGraph.ScrollMinX = 0D;
            this.VelGraph.ScrollMinY = 0D;
            this.VelGraph.ScrollMinY2 = 0D;
            this.VelGraph.Size = new System.Drawing.Size(845, 260);
            this.VelGraph.TabIndex = 8;
            // 
            // CurGraph
            // 
            this.CurGraph.Location = new System.Drawing.Point(12, 598);
            this.CurGraph.Name = "CurGraph";
            this.CurGraph.ScrollGrace = 0D;
            this.CurGraph.ScrollMaxX = 0D;
            this.CurGraph.ScrollMaxY = 0D;
            this.CurGraph.ScrollMaxY2 = 0D;
            this.CurGraph.ScrollMinX = 0D;
            this.CurGraph.ScrollMinY = 0D;
            this.CurGraph.ScrollMinY2 = 0D;
            this.CurGraph.Size = new System.Drawing.Size(845, 260);
            this.CurGraph.TabIndex = 9;
            // 
            // mChBox
            // 
            this.mChBox.Location = new System.Drawing.Point(793, 27);
            this.mChBox.Mask = "0";
            this.mChBox.Name = "mChBox";
            this.mChBox.Size = new System.Drawing.Size(28, 22);
            this.mChBox.TabIndex = 10;
            this.mChBox.Text = "0";
            this.mChBox.TextChanged += new System.EventHandler(this.mChBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(740, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "Channel:";
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(966, 340);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(75, 23);
            this.PauseBtn.TabIndex = 12;
            this.PauseBtn.Text = "Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CurKiSatBox);
            this.groupBox1.Controls.Add(this.CurKdBox);
            this.groupBox1.Controls.Add(this.CurKiBox);
            this.groupBox1.Controls.Add(this.CurKpBox);
            this.groupBox1.Controls.Add(this.VelKiSatBox);
            this.groupBox1.Controls.Add(this.VelKdBox);
            this.groupBox1.Controls.Add(this.VelKiBox);
            this.groupBox1.Controls.Add(this.VelKpBox);
            this.groupBox1.Controls.Add(this.PosKiSatBox);
            this.groupBox1.Controls.Add(this.PosKdBox);
            this.groupBox1.Controls.Add(this.PosKiBox);
            this.groupBox1.Controls.Add(this.PosKpBox);
            this.groupBox1.Location = new System.Drawing.Point(863, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 120);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PID parameter";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(222, 5);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(50, 24);
            this.label17.TabIndex = 25;
            this.label17.Text = "integrator\r\nsaturatin";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(172, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 12);
            this.label16.TabIndex = 24;
            this.label16.Text = "Kd";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(123, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 12);
            this.label15.TabIndex = 23;
            this.label15.Text = "Ki";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(59, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 12);
            this.label14.TabIndex = 22;
            this.label14.Text = "Kp";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "Torque:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "Velocity:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "Postion:";
            // 
            // CurKiSatBox
            // 
            this.CurKiSatBox.Location = new System.Drawing.Point(221, 93);
            this.CurKiSatBox.Name = "CurKiSatBox";
            this.CurKiSatBox.Size = new System.Drawing.Size(51, 22);
            this.CurKiSatBox.TabIndex = 11;
            this.CurKiSatBox.Text = "32767";
            this.CurKiSatBox.TextChanged += new System.EventHandler(this.CurKiSatBox_TextChanged);
            // 
            // CurKdBox
            // 
            this.CurKdBox.Location = new System.Drawing.Point(164, 93);
            this.CurKdBox.Name = "CurKdBox";
            this.CurKdBox.Size = new System.Drawing.Size(51, 22);
            this.CurKdBox.TabIndex = 10;
            this.CurKdBox.Text = "0";
            this.CurKdBox.TextChanged += new System.EventHandler(this.CurKdBox_TextChanged);
            // 
            // CurKiBox
            // 
            this.CurKiBox.Location = new System.Drawing.Point(107, 93);
            this.CurKiBox.Name = "CurKiBox";
            this.CurKiBox.Size = new System.Drawing.Size(51, 22);
            this.CurKiBox.TabIndex = 9;
            this.CurKiBox.Text = "0";
            this.CurKiBox.TextChanged += new System.EventHandler(this.CurKiBox_TextChanged);
            // 
            // CurKpBox
            // 
            this.CurKpBox.Location = new System.Drawing.Point(50, 93);
            this.CurKpBox.Name = "CurKpBox";
            this.CurKpBox.Size = new System.Drawing.Size(51, 22);
            this.CurKpBox.TabIndex = 8;
            this.CurKpBox.Text = "0";
            this.CurKpBox.TextChanged += new System.EventHandler(this.CurKpBox_TextChanged);
            // 
            // VelKiSatBox
            // 
            this.VelKiSatBox.Location = new System.Drawing.Point(221, 62);
            this.VelKiSatBox.Name = "VelKiSatBox";
            this.VelKiSatBox.Size = new System.Drawing.Size(51, 22);
            this.VelKiSatBox.TabIndex = 7;
            this.VelKiSatBox.Text = "32767";
            this.VelKiSatBox.TextChanged += new System.EventHandler(this.VelKiSatBox_TextChanged);
            // 
            // VelKdBox
            // 
            this.VelKdBox.Location = new System.Drawing.Point(164, 62);
            this.VelKdBox.Name = "VelKdBox";
            this.VelKdBox.Size = new System.Drawing.Size(51, 22);
            this.VelKdBox.TabIndex = 6;
            this.VelKdBox.Text = "0";
            this.VelKdBox.TextChanged += new System.EventHandler(this.VelKdBox_TextChanged);
            // 
            // VelKiBox
            // 
            this.VelKiBox.Location = new System.Drawing.Point(103, 65);
            this.VelKiBox.Name = "VelKiBox";
            this.VelKiBox.Size = new System.Drawing.Size(51, 22);
            this.VelKiBox.TabIndex = 5;
            this.VelKiBox.Text = "0";
            this.VelKiBox.TextChanged += new System.EventHandler(this.VelKiBox_TextChanged);
            // 
            // VelKpBox
            // 
            this.VelKpBox.Location = new System.Drawing.Point(50, 62);
            this.VelKpBox.Name = "VelKpBox";
            this.VelKpBox.Size = new System.Drawing.Size(51, 22);
            this.VelKpBox.TabIndex = 4;
            this.VelKpBox.Text = "0";
            this.VelKpBox.TextChanged += new System.EventHandler(this.VelKpBox_TextChanged);
            // 
            // PosKiSatBox
            // 
            this.PosKiSatBox.Location = new System.Drawing.Point(221, 32);
            this.PosKiSatBox.Name = "PosKiSatBox";
            this.PosKiSatBox.Size = new System.Drawing.Size(51, 22);
            this.PosKiSatBox.TabIndex = 3;
            this.PosKiSatBox.Text = "32767";
            this.PosKiSatBox.TextChanged += new System.EventHandler(this.PosKiSatBox_TextChanged);
            // 
            // PosKdBox
            // 
            this.PosKdBox.Location = new System.Drawing.Point(164, 32);
            this.PosKdBox.Name = "PosKdBox";
            this.PosKdBox.Size = new System.Drawing.Size(51, 22);
            this.PosKdBox.TabIndex = 2;
            this.PosKdBox.Text = "0";
            this.PosKdBox.TextChanged += new System.EventHandler(this.PosKdBox_TextChanged);
            // 
            // PosKiBox
            // 
            this.PosKiBox.Location = new System.Drawing.Point(107, 32);
            this.PosKiBox.Name = "PosKiBox";
            this.PosKiBox.Size = new System.Drawing.Size(51, 22);
            this.PosKiBox.TabIndex = 1;
            this.PosKiBox.Text = "0";
            this.PosKiBox.TextChanged += new System.EventHandler(this.PosKiBox_TextChanged);
            // 
            // PosKpBox
            // 
            this.PosKpBox.Location = new System.Drawing.Point(50, 32);
            this.PosKpBox.Name = "PosKpBox";
            this.PosKpBox.Size = new System.Drawing.Size(51, 22);
            this.PosKpBox.TabIndex = 0;
            this.PosKpBox.Text = "0";
            this.PosKpBox.TextChanged += new System.EventHandler(this.PosKpBox_TextChanged);
            // 
            // ExtVelBox
            // 
            this.ExtVelBox.Location = new System.Drawing.Point(50, 21);
            this.ExtVelBox.Name = "ExtVelBox";
            this.ExtVelBox.Size = new System.Drawing.Size(81, 22);
            this.ExtVelBox.TabIndex = 18;
            this.ExtVelBox.Text = "0";
            this.ExtVelBox.TextChanged += new System.EventHandler(this.ExtVelBox_TextChanged);
            // 
            // ExtTorBox
            // 
            this.ExtTorBox.Location = new System.Drawing.Point(191, 21);
            this.ExtTorBox.Name = "ExtTorBox";
            this.ExtTorBox.Size = new System.Drawing.Size(81, 22);
            this.ExtTorBox.TabIndex = 17;
            this.ExtTorBox.Text = "0";
            this.ExtTorBox.TextChanged += new System.EventHandler(this.ExtTorBox_TextChanged);
            // 
            // ExtPosBox
            // 
            this.ExtPosBox.Location = new System.Drawing.Point(89, 51);
            this.ExtPosBox.Name = "ExtPosBox";
            this.ExtPosBox.Size = new System.Drawing.Size(92, 22);
            this.ExtPosBox.TabIndex = 16;
            this.ExtPosBox.Text = "0";
            this.ExtPosBox.TextChanged += new System.EventHandler(this.ExtPosBox_TextChanged);
            // 
            // MaxOptBox
            // 
            this.MaxOptBox.Location = new System.Drawing.Point(191, 49);
            this.MaxOptBox.Name = "MaxOptBox";
            this.MaxOptBox.Size = new System.Drawing.Size(81, 22);
            this.MaxOptBox.TabIndex = 15;
            this.MaxOptBox.Text = "32767";
            this.MaxOptBox.TextChanged += new System.EventHandler(this.MaxOptBox_TextChanged);
            // 
            // MaxVelBox
            // 
            this.MaxVelBox.Location = new System.Drawing.Point(191, 21);
            this.MaxVelBox.Name = "MaxVelBox";
            this.MaxVelBox.Size = new System.Drawing.Size(81, 22);
            this.MaxVelBox.TabIndex = 14;
            this.MaxVelBox.Text = "32767";
            this.MaxVelBox.TextChanged += new System.EventHandler(this.MaxVelBox_TextChanged);
            // 
            // MaxTorBox
            // 
            this.MaxTorBox.Location = new System.Drawing.Point(50, 49);
            this.MaxTorBox.Name = "MaxTorBox";
            this.MaxTorBox.Size = new System.Drawing.Size(81, 22);
            this.MaxTorBox.TabIndex = 13;
            this.MaxTorBox.Text = "32767";
            this.MaxTorBox.TextChanged += new System.EventHandler(this.MaxTorBox_TextChanged);
            // 
            // SoftstartBox
            // 
            this.SoftstartBox.Location = new System.Drawing.Point(50, 21);
            this.SoftstartBox.Name = "SoftstartBox";
            this.SoftstartBox.Size = new System.Drawing.Size(81, 22);
            this.SoftstartBox.TabIndex = 12;
            this.SoftstartBox.Text = "5";
            this.SoftstartBox.TextChanged += new System.EventHandler(this.SoftstartBox_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.SoftstartBox);
            this.groupBox2.Controls.Add(this.MaxVelBox);
            this.groupBox2.Controls.Add(this.MaxTorBox);
            this.groupBox2.Controls.Add(this.MaxOptBox);
            this.groupBox2.Location = new System.Drawing.Point(863, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 81);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Saturation";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(139, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 12);
            this.label10.TabIndex = 25;
            this.label10.Text = "Max Opt:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "Max Tor:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Max Vel:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "Softstart:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.ExtPosBox);
            this.groupBox3.Controls.Add(this.ExtTorBox);
            this.groupBox3.Controls.Add(this.ExtVelBox);
            this.groupBox3.Controls.Add(this.SynchButton);
            this.groupBox3.Location = new System.Drawing.Point(863, 255);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(272, 79);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Command";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(137, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 12);
            this.label13.TabIndex = 27;
            this.label13.Text = "Ext Tor:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 12);
            this.label12.TabIndex = 26;
            this.label12.Text = "Ext Vel:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "Tartget Position:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // picbit7
            // 
            this.picbit7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picbit7.Location = new System.Drawing.Point(906, 28);
            this.picbit7.Name = "picbit7";
            this.picbit7.Size = new System.Drawing.Size(15, 15);
            this.picbit7.TabIndex = 16;
            this.picbit7.TabStop = false;
            // 
            // picbit6
            // 
            this.picbit6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picbit6.Location = new System.Drawing.Point(926, 28);
            this.picbit6.Name = "picbit6";
            this.picbit6.Size = new System.Drawing.Size(15, 15);
            this.picbit6.TabIndex = 16;
            this.picbit6.TabStop = false;
            // 
            // picbit5
            // 
            this.picbit5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picbit5.Location = new System.Drawing.Point(946, 28);
            this.picbit5.Name = "picbit5";
            this.picbit5.Size = new System.Drawing.Size(15, 15);
            this.picbit5.TabIndex = 16;
            this.picbit5.TabStop = false;
            // 
            // picbit4
            // 
            this.picbit4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picbit4.Location = new System.Drawing.Point(966, 28);
            this.picbit4.Name = "picbit4";
            this.picbit4.Size = new System.Drawing.Size(15, 15);
            this.picbit4.TabIndex = 16;
            this.picbit4.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(868, 30);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 12);
            this.label18.TabIndex = 11;
            this.label18.Text = "Status:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(968, 9);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(9, 12);
            this.label19.TabIndex = 17;
            this.label19.Text = "I";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(947, 9);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(13, 12);
            this.label20.TabIndex = 18;
            this.label20.Text = "R";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(928, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(12, 12);
            this.label21.TabIndex = 19;
            this.label21.Text = "L";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(908, 9);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(13, 12);
            this.label22.TabIndex = 20;
            this.label22.Text = "H";
            // 
            // Home
            // 
            this.Home.Location = new System.Drawing.Point(866, 340);
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(75, 23);
            this.Home.TabIndex = 21;
            this.Home.Text = "Home";
            this.Home.UseVisualStyleBackColor = true;
            this.Home.Click += new System.EventHandler(this.Home_Click);
            // 
            // Sensor_btn
            // 
            this.Sensor_btn.Location = new System.Drawing.Point(1054, 26);
            this.Sensor_btn.Name = "Sensor_btn";
            this.Sensor_btn.Size = new System.Drawing.Size(75, 23);
            this.Sensor_btn.TabIndex = 22;
            this.Sensor_btn.Text = "Sensor";
            this.Sensor_btn.UseVisualStyleBackColor = true;
            this.Sensor_btn.Click += new System.EventHandler(this.Sensor_btn_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(1010, 9);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(11, 12);
            this.label23.TabIndex = 23;
            this.label23.Text = "0";
            // 
            // AHRSInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 650);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.Sensor_btn);
            this.Controls.Add(this.Home);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.picbit4);
            this.Controls.Add(this.picbit5);
            this.Controls.Add(this.picbit6);
            this.Controls.Add(this.picbit7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mChBox);
            this.Controls.Add(this.CurGraph);
            this.Controls.Add(this.VelGraph);
            this.Controls.Add(this.PosGraph);
            this.Controls.Add(this.KickOff);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AHRSInterface";
            this.Text = "Motor Controller Interface";
            this.Load += new System.EventHandler(this.AHRSInterface_Load);
            this.ResizeEnd += new System.EventHandler(this.OnRenderTimerTick);
            this.Resize += new System.EventHandler(this.AHRSInterface_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbit4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox statusBox;
        private System.Windows.Forms.Button SynchButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dialogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magCalibrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logDataToolstripItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox serialPortCOMBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox baudSelectBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton serialConnectButton;
        private System.Windows.Forms.ToolStripButton serialDisconnectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button KickOff;
        private ZedGraph.ZedGraphControl PosGraph;
        private System.Windows.Forms.Timer timer1;
        private ZedGraph.ZedGraphControl VelGraph;
        private ZedGraph.ZedGraphControl CurGraph;
        private System.Windows.Forms.MaskedTextBox mChBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button PauseBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PosKpBox;
        private System.Windows.Forms.TextBox CurKiSatBox;
        private System.Windows.Forms.TextBox CurKdBox;
        private System.Windows.Forms.TextBox CurKiBox;
        private System.Windows.Forms.TextBox CurKpBox;
        private System.Windows.Forms.TextBox VelKiSatBox;
        private System.Windows.Forms.TextBox VelKdBox;
        private System.Windows.Forms.TextBox VelKiBox;
        private System.Windows.Forms.TextBox VelKpBox;
        private System.Windows.Forms.TextBox PosKiSatBox;
        private System.Windows.Forms.TextBox PosKdBox;
        private System.Windows.Forms.TextBox PosKiBox;
        private System.Windows.Forms.TextBox MaxOptBox;
        private System.Windows.Forms.TextBox MaxVelBox;
        private System.Windows.Forms.TextBox MaxTorBox;
        private System.Windows.Forms.TextBox SoftstartBox;
        private System.Windows.Forms.TextBox ExtVelBox;
        private System.Windows.Forms.TextBox ExtTorBox;
        private System.Windows.Forms.TextBox ExtPosBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox picbit7;
        private System.Windows.Forms.PictureBox picbit6;
        private System.Windows.Forms.PictureBox picbit5;
        private System.Windows.Forms.PictureBox picbit4;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button Home;
        private System.Windows.Forms.Button Sensor_btn;
        private System.Windows.Forms.Label label23;
    }
}

