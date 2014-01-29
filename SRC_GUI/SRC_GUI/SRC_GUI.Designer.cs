namespace SRC_GUI
{
    partial class SRC_GUI
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
            this.btnAutoComplete = new System.Windows.Forms.Button();
            this.comboSampleType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSample = new System.Windows.Forms.Button();
            this.btnApplicationStarup = new System.Windows.Forms.Button();
            this.btnPullData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPowerMonitorSRC = new System.Windows.Forms.Button();
            this.btnSampleRoot = new System.Windows.Forms.Button();
            this.textPowerMonitorSRC = new System.Windows.Forms.TextBox();
            this.textSampleRoot = new System.Windows.Forms.TextBox();
            this.btnEkarat = new System.Windows.Forms.Button();
            this.btnKohy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelWorkingDir = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textEuraqaServeIP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textWifi = new System.Windows.Forms.TextBox();
            this.textAppName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textVout = new System.Windows.Forms.TextBox();
            this.textRScript = new System.Windows.Forms.TextBox();
            this.btnRScript = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAsync = new System.Windows.Forms.Button();
            this.btnEureqa = new System.Windows.Forms.Button();
            this.btnParseModel = new System.Windows.Forms.Button();
            this.btnProcessSample = new System.Windows.Forms.Button();
            this.numericSampleNumber = new System.Windows.Forms.NumericUpDown();
            this.numericTotalSample = new System.Windows.Forms.NumericUpDown();
            this.numericPowerOffest = new System.Windows.Forms.NumericUpDown();
            this.numericDuration = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.listBoxStatus = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTotalSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPowerOffest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAutoComplete
            // 
            this.btnAutoComplete.Location = new System.Drawing.Point(569, 348);
            this.btnAutoComplete.Name = "btnAutoComplete";
            this.btnAutoComplete.Size = new System.Drawing.Size(150, 25);
            this.btnAutoComplete.TabIndex = 0;
            this.btnAutoComplete.Text = "Auto Complete";
            this.btnAutoComplete.UseVisualStyleBackColor = true;
            this.btnAutoComplete.Click += new System.EventHandler(this.btnAutoComplete_Click);
            // 
            // comboSampleType
            // 
            this.comboSampleType.FormattingEnabled = true;
            this.comboSampleType.Items.AddRange(new object[] {
            "skype\\idle",
            "skype\\voice",
            "skype\\video",
            "line\\idle",
            "line\\voice",
            "line\\video",
            "candycrush",
            "pokopang"});
            this.comboSampleType.Location = new System.Drawing.Point(118, 109);
            this.comboSampleType.Name = "comboSampleType";
            this.comboSampleType.Size = new System.Drawing.Size(386, 21);
            this.comboSampleType.TabIndex = 2;
            this.comboSampleType.SelectedIndexChanged += new System.EventHandler(this.comboProjectType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Power Monitor SRC";
            // 
            // btnSample
            // 
            this.btnSample.Location = new System.Drawing.Point(569, 13);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(150, 25);
            this.btnSample.TabIndex = 4;
            this.btnSample.Text = "Start Sampling";
            this.btnSample.UseVisualStyleBackColor = true;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // btnApplicationStarup
            // 
            this.btnApplicationStarup.Location = new System.Drawing.Point(569, 170);
            this.btnApplicationStarup.Name = "btnApplicationStarup";
            this.btnApplicationStarup.Size = new System.Drawing.Size(150, 25);
            this.btnApplicationStarup.TabIndex = 5;
            this.btnApplicationStarup.Text = "Application Startup";
            this.btnApplicationStarup.UseVisualStyleBackColor = true;
            this.btnApplicationStarup.Click += new System.EventHandler(this.btnApplicationStarup_Click);
            // 
            // btnPullData
            // 
            this.btnPullData.Location = new System.Drawing.Point(569, 202);
            this.btnPullData.Name = "btnPullData";
            this.btnPullData.Size = new System.Drawing.Size(150, 25);
            this.btnPullData.TabIndex = 5;
            this.btnPullData.Text = "Pull Data";
            this.btnPullData.UseVisualStyleBackColor = true;
            this.btnPullData.Click += new System.EventHandler(this.btnPullData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Project SRC";
            // 
            // btnPowerMonitorSRC
            // 
            this.btnPowerMonitorSRC.Location = new System.Drawing.Point(511, 44);
            this.btnPowerMonitorSRC.Name = "btnPowerMonitorSRC";
            this.btnPowerMonitorSRC.Size = new System.Drawing.Size(52, 25);
            this.btnPowerMonitorSRC.TabIndex = 7;
            this.btnPowerMonitorSRC.Text = "Browse";
            this.btnPowerMonitorSRC.UseVisualStyleBackColor = true;
            this.btnPowerMonitorSRC.Click += new System.EventHandler(this.btnPowerMonitorSRC_Click);
            // 
            // btnSampleRoot
            // 
            this.btnSampleRoot.Location = new System.Drawing.Point(511, 76);
            this.btnSampleRoot.Name = "btnSampleRoot";
            this.btnSampleRoot.Size = new System.Drawing.Size(52, 25);
            this.btnSampleRoot.TabIndex = 8;
            this.btnSampleRoot.Text = "Browse";
            this.btnSampleRoot.UseVisualStyleBackColor = true;
            this.btnSampleRoot.Click += new System.EventHandler(this.btnSampleRoot_Click);
            // 
            // textPowerMonitorSRC
            // 
            this.textPowerMonitorSRC.Location = new System.Drawing.Point(118, 47);
            this.textPowerMonitorSRC.Name = "textPowerMonitorSRC";
            this.textPowerMonitorSRC.Size = new System.Drawing.Size(386, 20);
            this.textPowerMonitorSRC.TabIndex = 9;
            // 
            // textSampleRoot
            // 
            this.textSampleRoot.Location = new System.Drawing.Point(118, 78);
            this.textSampleRoot.Name = "textSampleRoot";
            this.textSampleRoot.Size = new System.Drawing.Size(386, 20);
            this.textSampleRoot.TabIndex = 10;
            // 
            // btnEkarat
            // 
            this.btnEkarat.Location = new System.Drawing.Point(569, 379);
            this.btnEkarat.Name = "btnEkarat";
            this.btnEkarat.Size = new System.Drawing.Size(150, 25);
            this.btnEkarat.TabIndex = 11;
            this.btnEkarat.Text = "Ekarat Defaults";
            this.btnEkarat.UseVisualStyleBackColor = true;
            this.btnEkarat.Click += new System.EventHandler(this.btnEkarat_Click);
            // 
            // btnKohy
            // 
            this.btnKohy.Location = new System.Drawing.Point(569, 411);
            this.btnKohy.Name = "btnKohy";
            this.btnKohy.Size = new System.Drawing.Size(150, 25);
            this.btnKohy.TabIndex = 12;
            this.btnKohy.Text = "Ko, Hsiang-Yu Defaults";
            this.btnKohy.UseVisualStyleBackColor = true;
            this.btnKohy.Click += new System.EventHandler(this.btnKohy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Project Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 270);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Working Directory";
            // 
            // labelWorkingDir
            // 
            this.labelWorkingDir.AutoSize = true;
            this.labelWorkingDir.Location = new System.Drawing.Point(116, 270);
            this.labelWorkingDir.Name = "labelWorkingDir";
            this.labelWorkingDir.Size = new System.Drawing.Size(36, 13);
            this.labelWorkingDir.TabIndex = 15;
            this.labelWorkingDir.Text = "Empty";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Duration";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Power Offset";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Save Times";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(208, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Euraqa Server IP";
            // 
            // textEuraqaServeIP
            // 
            this.textEuraqaServeIP.Location = new System.Drawing.Point(311, 141);
            this.textEuraqaServeIP.Name = "textEuraqaServeIP";
            this.textEuraqaServeIP.Size = new System.Drawing.Size(193, 20);
            this.textEuraqaServeIP.TabIndex = 24;
            this.textEuraqaServeIP.Text = "140.113.88.194";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(208, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "WIFI";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(208, 207);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "App";
            // 
            // textWifi
            // 
            this.textWifi.Location = new System.Drawing.Point(311, 173);
            this.textWifi.Name = "textWifi";
            this.textWifi.Size = new System.Drawing.Size(193, 20);
            this.textWifi.TabIndex = 27;
            this.textWifi.Text = "/sys/class/net/wlan0/";
            // 
            // textAppName
            // 
            this.textAppName.Location = new System.Drawing.Point(311, 204);
            this.textAppName.Name = "textAppName";
            this.textAppName.Size = new System.Drawing.Size(193, 20);
            this.textAppName.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 236);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Vout";
            // 
            // textVout
            // 
            this.textVout.Location = new System.Drawing.Point(118, 233);
            this.textVout.Name = "textVout";
            this.textVout.Size = new System.Drawing.Size(62, 20);
            this.textVout.TabIndex = 30;
            this.textVout.Text = "4.2";
            // 
            // textRScript
            // 
            this.textRScript.Location = new System.Drawing.Point(118, 15);
            this.textRScript.Name = "textRScript";
            this.textRScript.Size = new System.Drawing.Size(386, 20);
            this.textRScript.TabIndex = 33;
            // 
            // btnRScript
            // 
            this.btnRScript.Location = new System.Drawing.Point(511, 13);
            this.btnRScript.Name = "btnRScript";
            this.btnRScript.Size = new System.Drawing.Size(52, 25);
            this.btnRScript.TabIndex = 32;
            this.btnRScript.Text = "Browse";
            this.btnRScript.UseVisualStyleBackColor = true;
            this.btnRScript.Click += new System.EventHandler(this.btnRScript_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "R Script SRC";
            // 
            // btnAsync
            // 
            this.btnAsync.Location = new System.Drawing.Point(569, 76);
            this.btnAsync.Name = "btnAsync";
            this.btnAsync.Size = new System.Drawing.Size(150, 25);
            this.btnAsync.TabIndex = 34;
            this.btnAsync.Text = "Async";
            this.btnAsync.UseVisualStyleBackColor = true;
            this.btnAsync.Click += new System.EventHandler(this.btnAsync_Click);
            // 
            // btnEureqa
            // 
            this.btnEureqa.Location = new System.Drawing.Point(569, 107);
            this.btnEureqa.Name = "btnEureqa";
            this.btnEureqa.Size = new System.Drawing.Size(150, 25);
            this.btnEureqa.TabIndex = 35;
            this.btnEureqa.Text = "Eureqa";
            this.btnEureqa.UseVisualStyleBackColor = true;
            this.btnEureqa.Click += new System.EventHandler(this.btnEureqa_Click);
            // 
            // btnParseModel
            // 
            this.btnParseModel.Location = new System.Drawing.Point(569, 139);
            this.btnParseModel.Name = "btnParseModel";
            this.btnParseModel.Size = new System.Drawing.Size(150, 25);
            this.btnParseModel.TabIndex = 36;
            this.btnParseModel.Text = "ParseModel";
            this.btnParseModel.UseVisualStyleBackColor = true;
            this.btnParseModel.Click += new System.EventHandler(this.btnParseModel_Click);
            // 
            // btnProcessSample
            // 
            this.btnProcessSample.Location = new System.Drawing.Point(569, 44);
            this.btnProcessSample.Name = "btnProcessSample";
            this.btnProcessSample.Size = new System.Drawing.Size(150, 25);
            this.btnProcessSample.TabIndex = 37;
            this.btnProcessSample.Text = "ProcessSample";
            this.btnProcessSample.UseVisualStyleBackColor = true;
            this.btnProcessSample.Click += new System.EventHandler(this.btnProcessSample_Click);
            // 
            // numericSampleNumber
            // 
            this.numericSampleNumber.Location = new System.Drawing.Point(510, 111);
            this.numericSampleNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSampleNumber.Name = "numericSampleNumber";
            this.numericSampleNumber.Size = new System.Drawing.Size(52, 20);
            this.numericSampleNumber.TabIndex = 38;
            this.numericSampleNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericTotalSample
            // 
            this.numericTotalSample.Location = new System.Drawing.Point(118, 205);
            this.numericTotalSample.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericTotalSample.Name = "numericTotalSample";
            this.numericTotalSample.Size = new System.Drawing.Size(62, 20);
            this.numericTotalSample.TabIndex = 38;
            this.numericTotalSample.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericPowerOffest
            // 
            this.numericPowerOffest.Location = new System.Drawing.Point(118, 174);
            this.numericPowerOffest.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPowerOffest.Name = "numericPowerOffest";
            this.numericPowerOffest.Size = new System.Drawing.Size(62, 20);
            this.numericPowerOffest.TabIndex = 38;
            this.numericPowerOffest.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericDuration
            // 
            this.numericDuration.Location = new System.Drawing.Point(118, 142);
            this.numericDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericDuration.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericDuration.Name = "numericDuration";
            this.numericDuration.Size = new System.Drawing.Size(62, 20);
            this.numericDuration.TabIndex = 38;
            this.numericDuration.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 301);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "Status";
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.FormattingEnabled = true;
            this.listBoxStatus.Location = new System.Drawing.Point(113, 301);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(449, 134);
            this.listBoxStatus.TabIndex = 39;
            // 
            // SRC_GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 449);
            this.Controls.Add(this.listBoxStatus);
            this.Controls.Add(this.numericDuration);
            this.Controls.Add(this.numericPowerOffest);
            this.Controls.Add(this.numericTotalSample);
            this.Controls.Add(this.numericSampleNumber);
            this.Controls.Add(this.btnProcessSample);
            this.Controls.Add(this.btnParseModel);
            this.Controls.Add(this.btnEureqa);
            this.Controls.Add(this.btnAsync);
            this.Controls.Add(this.textRScript);
            this.Controls.Add(this.btnRScript);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textVout);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textAppName);
            this.Controls.Add(this.textWifi);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textEuraqaServeIP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.labelWorkingDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnKohy);
            this.Controls.Add(this.btnEkarat);
            this.Controls.Add(this.textSampleRoot);
            this.Controls.Add(this.textPowerMonitorSRC);
            this.Controls.Add(this.btnSampleRoot);
            this.Controls.Add(this.btnPowerMonitorSRC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnPullData);
            this.Controls.Add(this.btnApplicationStarup);
            this.Controls.Add(this.btnSample);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboSampleType);
            this.Controls.Add(this.btnAutoComplete);
            this.Name = "SRC_GUI";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTotalSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPowerOffest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAutoComplete;
        private System.Windows.Forms.ComboBox comboSampleType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Button btnApplicationStarup;
        private System.Windows.Forms.Button btnPullData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPowerMonitorSRC;
        private System.Windows.Forms.Button btnSampleRoot;
        private System.Windows.Forms.TextBox textPowerMonitorSRC;
        private System.Windows.Forms.TextBox textSampleRoot;
        private System.Windows.Forms.Button btnEkarat;
        private System.Windows.Forms.Button btnKohy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelWorkingDir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textEuraqaServeIP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textWifi;
        private System.Windows.Forms.TextBox textAppName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textVout;
        private System.Windows.Forms.TextBox textRScript;
        private System.Windows.Forms.Button btnRScript;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnAsync;
        private System.Windows.Forms.Button btnEureqa;
        private System.Windows.Forms.Button btnParseModel;
        private System.Windows.Forms.Button btnProcessSample;
        private System.Windows.Forms.NumericUpDown numericSampleNumber;
        private System.Windows.Forms.NumericUpDown numericTotalSample;
        private System.Windows.Forms.NumericUpDown numericPowerOffest;
        private System.Windows.Forms.NumericUpDown numericDuration;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox listBoxStatus;
    }
}

