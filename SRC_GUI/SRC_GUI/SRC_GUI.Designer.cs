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
            this.comboBoxEuraqaServeIP = new System.Windows.Forms.ComboBox();
            this.btnViewAllResults = new System.Windows.Forms.Button();
            this.btnDropbox = new System.Windows.Forms.Button();
            this.btnAutoStartApp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTotalSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPowerOffest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAutoComplete
            // 
            this.btnAutoComplete.Location = new System.Drawing.Point(569, 292);
            this.btnAutoComplete.Name = "btnAutoComplete";
            this.btnAutoComplete.Size = new System.Drawing.Size(150, 23);
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
            this.comboSampleType.Location = new System.Drawing.Point(118, 101);
            this.comboSampleType.Name = "comboSampleType";
            this.comboSampleType.Size = new System.Drawing.Size(386, 20);
            this.comboSampleType.TabIndex = 2;
            this.comboSampleType.SelectedIndexChanged += new System.EventHandler(this.comboProjectType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Power Monitor SRC";
            // 
            // btnSample
            // 
            this.btnSample.Location = new System.Drawing.Point(569, 12);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(150, 23);
            this.btnSample.TabIndex = 4;
            this.btnSample.Text = "1. Start Sampling";
            this.btnSample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSample.UseVisualStyleBackColor = true;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // btnApplicationStarup
            // 
            this.btnApplicationStarup.Location = new System.Drawing.Point(569, 186);
            this.btnApplicationStarup.Name = "btnApplicationStarup";
            this.btnApplicationStarup.Size = new System.Drawing.Size(150, 23);
            this.btnApplicationStarup.TabIndex = 5;
            this.btnApplicationStarup.Text = "Application Startup";
            this.btnApplicationStarup.UseVisualStyleBackColor = true;
            this.btnApplicationStarup.Click += new System.EventHandler(this.btnApplicationStarup_Click);
            // 
            // btnPullData
            // 
            this.btnPullData.Location = new System.Drawing.Point(12, 378);
            this.btnPullData.Name = "btnPullData";
            this.btnPullData.Size = new System.Drawing.Size(95, 23);
            this.btnPullData.TabIndex = 5;
            this.btnPullData.Text = "Pull Data";
            this.btnPullData.UseVisualStyleBackColor = true;
            this.btnPullData.Click += new System.EventHandler(this.btnPullData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sample Root";
            // 
            // btnPowerMonitorSRC
            // 
            this.btnPowerMonitorSRC.Location = new System.Drawing.Point(511, 41);
            this.btnPowerMonitorSRC.Name = "btnPowerMonitorSRC";
            this.btnPowerMonitorSRC.Size = new System.Drawing.Size(52, 23);
            this.btnPowerMonitorSRC.TabIndex = 7;
            this.btnPowerMonitorSRC.Text = "Browse";
            this.btnPowerMonitorSRC.UseVisualStyleBackColor = true;
            this.btnPowerMonitorSRC.Click += new System.EventHandler(this.btnPowerMonitorSRC_Click);
            // 
            // btnSampleRoot
            // 
            this.btnSampleRoot.Location = new System.Drawing.Point(511, 70);
            this.btnSampleRoot.Name = "btnSampleRoot";
            this.btnSampleRoot.Size = new System.Drawing.Size(52, 23);
            this.btnSampleRoot.TabIndex = 8;
            this.btnSampleRoot.Text = "Browse";
            this.btnSampleRoot.UseVisualStyleBackColor = true;
            this.btnSampleRoot.Click += new System.EventHandler(this.btnSampleRoot_Click);
            // 
            // textPowerMonitorSRC
            // 
            this.textPowerMonitorSRC.Location = new System.Drawing.Point(118, 43);
            this.textPowerMonitorSRC.Name = "textPowerMonitorSRC";
            this.textPowerMonitorSRC.Size = new System.Drawing.Size(386, 22);
            this.textPowerMonitorSRC.TabIndex = 9;
            // 
            // textSampleRoot
            // 
            this.textSampleRoot.Location = new System.Drawing.Point(118, 72);
            this.textSampleRoot.Name = "textSampleRoot";
            this.textSampleRoot.Size = new System.Drawing.Size(386, 22);
            this.textSampleRoot.TabIndex = 10;
            // 
            // btnEkarat
            // 
            this.btnEkarat.Location = new System.Drawing.Point(569, 320);
            this.btnEkarat.Name = "btnEkarat";
            this.btnEkarat.Size = new System.Drawing.Size(150, 23);
            this.btnEkarat.TabIndex = 11;
            this.btnEkarat.Text = "Ekarat Defaults";
            this.btnEkarat.UseVisualStyleBackColor = true;
            this.btnEkarat.Click += new System.EventHandler(this.btnEkarat_Click);
            // 
            // btnKohy
            // 
            this.btnKohy.Location = new System.Drawing.Point(569, 350);
            this.btnKohy.Name = "btnKohy";
            this.btnKohy.Size = new System.Drawing.Size(150, 23);
            this.btnKohy.TabIndex = 12;
            this.btnKohy.Text = "Ko, Hsiang-Yu Defaults";
            this.btnKohy.UseVisualStyleBackColor = true;
            this.btnKohy.Click += new System.EventHandler(this.btnKohy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "Project Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 249);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "Working Directory";
            // 
            // labelWorkingDir
            // 
            this.labelWorkingDir.AutoSize = true;
            this.labelWorkingDir.Location = new System.Drawing.Point(116, 249);
            this.labelWorkingDir.Name = "labelWorkingDir";
            this.labelWorkingDir.Size = new System.Drawing.Size(36, 12);
            this.labelWorkingDir.TabIndex = 15;
            this.labelWorkingDir.Text = "Empty";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "Duration";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "Power Offset";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "Save Times";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(208, 133);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Euraqa Server IP";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(208, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 12);
            this.label9.TabIndex = 25;
            this.label9.Text = "WIFI";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(208, 191);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 12);
            this.label10.TabIndex = 26;
            this.label10.Text = "App";
            // 
            // textWifi
            // 
            this.textWifi.Location = new System.Drawing.Point(311, 160);
            this.textWifi.Name = "textWifi";
            this.textWifi.Size = new System.Drawing.Size(193, 22);
            this.textWifi.TabIndex = 27;
            this.textWifi.Text = "/sys/class/net/wlan0/";
            // 
            // textAppName
            // 
            this.textAppName.Location = new System.Drawing.Point(311, 188);
            this.textAppName.Name = "textAppName";
            this.textAppName.Size = new System.Drawing.Size(193, 22);
            this.textAppName.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 218);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 12);
            this.label11.TabIndex = 29;
            this.label11.Text = "Vout";
            // 
            // textVout
            // 
            this.textVout.Location = new System.Drawing.Point(118, 215);
            this.textVout.Name = "textVout";
            this.textVout.Size = new System.Drawing.Size(62, 22);
            this.textVout.TabIndex = 30;
            this.textVout.Text = "4.2";
            // 
            // textRScript
            // 
            this.textRScript.Location = new System.Drawing.Point(118, 14);
            this.textRScript.Name = "textRScript";
            this.textRScript.Size = new System.Drawing.Size(386, 22);
            this.textRScript.TabIndex = 33;
            // 
            // btnRScript
            // 
            this.btnRScript.Location = new System.Drawing.Point(511, 12);
            this.btnRScript.Name = "btnRScript";
            this.btnRScript.Size = new System.Drawing.Size(52, 23);
            this.btnRScript.TabIndex = 32;
            this.btnRScript.Text = "Browse";
            this.btnRScript.UseVisualStyleBackColor = true;
            this.btnRScript.Click += new System.EventHandler(this.btnRScript_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 12);
            this.label12.TabIndex = 31;
            this.label12.Text = "R Script SRC";
            // 
            // btnAsync
            // 
            this.btnAsync.Location = new System.Drawing.Point(569, 70);
            this.btnAsync.Name = "btnAsync";
            this.btnAsync.Size = new System.Drawing.Size(150, 23);
            this.btnAsync.TabIndex = 34;
            this.btnAsync.Text = "3. Async";
            this.btnAsync.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAsync.UseVisualStyleBackColor = true;
            this.btnAsync.Click += new System.EventHandler(this.btnAsync_Click);
            // 
            // btnEureqa
            // 
            this.btnEureqa.Location = new System.Drawing.Point(569, 99);
            this.btnEureqa.Name = "btnEureqa";
            this.btnEureqa.Size = new System.Drawing.Size(150, 23);
            this.btnEureqa.TabIndex = 35;
            this.btnEureqa.Text = "4. Eureqa";
            this.btnEureqa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEureqa.UseVisualStyleBackColor = true;
            this.btnEureqa.Click += new System.EventHandler(this.btnEureqa_Click);
            // 
            // btnParseModel
            // 
            this.btnParseModel.Location = new System.Drawing.Point(569, 128);
            this.btnParseModel.Name = "btnParseModel";
            this.btnParseModel.Size = new System.Drawing.Size(150, 23);
            this.btnParseModel.TabIndex = 36;
            this.btnParseModel.Text = "5. ParseModel";
            this.btnParseModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnParseModel.UseVisualStyleBackColor = true;
            this.btnParseModel.Click += new System.EventHandler(this.btnParseModel_Click);
            // 
            // btnProcessSample
            // 
            this.btnProcessSample.Location = new System.Drawing.Point(569, 41);
            this.btnProcessSample.Name = "btnProcessSample";
            this.btnProcessSample.Size = new System.Drawing.Size(150, 23);
            this.btnProcessSample.TabIndex = 37;
            this.btnProcessSample.Text = "2. ProcessSample";
            this.btnProcessSample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcessSample.UseVisualStyleBackColor = true;
            this.btnProcessSample.Click += new System.EventHandler(this.btnProcessSample_Click);
            // 
            // numericSampleNumber
            // 
            this.numericSampleNumber.Location = new System.Drawing.Point(510, 102);
            this.numericSampleNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSampleNumber.Name = "numericSampleNumber";
            this.numericSampleNumber.Size = new System.Drawing.Size(52, 22);
            this.numericSampleNumber.TabIndex = 38;
            this.numericSampleNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericTotalSample
            // 
            this.numericTotalSample.Location = new System.Drawing.Point(118, 189);
            this.numericTotalSample.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericTotalSample.Name = "numericTotalSample";
            this.numericTotalSample.Size = new System.Drawing.Size(62, 22);
            this.numericTotalSample.TabIndex = 38;
            this.numericTotalSample.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericPowerOffest
            // 
            this.numericPowerOffest.Location = new System.Drawing.Point(118, 161);
            this.numericPowerOffest.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPowerOffest.Name = "numericPowerOffest";
            this.numericPowerOffest.Size = new System.Drawing.Size(62, 22);
            this.numericPowerOffest.TabIndex = 38;
            this.numericPowerOffest.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericDuration
            // 
            this.numericDuration.Location = new System.Drawing.Point(118, 131);
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
            this.numericDuration.Size = new System.Drawing.Size(62, 22);
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
            this.label13.Location = new System.Drawing.Point(12, 278);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 12);
            this.label13.TabIndex = 15;
            this.label13.Text = "Status";
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.FormattingEnabled = true;
            this.listBoxStatus.ItemHeight = 12;
            this.listBoxStatus.Location = new System.Drawing.Point(113, 278);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(449, 124);
            this.listBoxStatus.TabIndex = 39;
            // 
            // comboBoxEuraqaServeIP
            // 
            this.comboBoxEuraqaServeIP.FormattingEnabled = true;
            this.comboBoxEuraqaServeIP.Items.AddRange(new object[] {
            "127.0.0.1",
            "140.113.88.194"});
            this.comboBoxEuraqaServeIP.Location = new System.Drawing.Point(311, 130);
            this.comboBoxEuraqaServeIP.Name = "comboBoxEuraqaServeIP";
            this.comboBoxEuraqaServeIP.Size = new System.Drawing.Size(193, 20);
            this.comboBoxEuraqaServeIP.TabIndex = 40;
            // 
            // btnViewAllResults
            // 
            this.btnViewAllResults.Location = new System.Drawing.Point(569, 262);
            this.btnViewAllResults.Name = "btnViewAllResults";
            this.btnViewAllResults.Size = new System.Drawing.Size(150, 23);
            this.btnViewAllResults.TabIndex = 41;
            this.btnViewAllResults.Text = "View All Results";
            this.btnViewAllResults.UseVisualStyleBackColor = true;
            this.btnViewAllResults.Click += new System.EventHandler(this.btnViewAllResults_Click);
            // 
            // btnDropbox
            // 
            this.btnDropbox.Location = new System.Drawing.Point(569, 378);
            this.btnDropbox.Name = "btnDropbox";
            this.btnDropbox.Size = new System.Drawing.Size(150, 23);
            this.btnDropbox.TabIndex = 42;
            this.btnDropbox.Text = "Ko, Hsiang-Yu Dropbox";
            this.btnDropbox.UseVisualStyleBackColor = true;
            this.btnDropbox.Click += new System.EventHandler(this.btnDropbox_Click);
            // 
            // btnAutoStartApp
            // 
            this.btnAutoStartApp.Location = new System.Drawing.Point(14, 349);
            this.btnAutoStartApp.Name = "btnAutoStartApp";
            this.btnAutoStartApp.Size = new System.Drawing.Size(93, 23);
            this.btnAutoStartApp.TabIndex = 43;
            this.btnAutoStartApp.Text = "Auto Start";
            this.btnAutoStartApp.UseVisualStyleBackColor = true;
            this.btnAutoStartApp.Click += new System.EventHandler(this.btnAutoStartApp_Click);
            // 
            // SRC_GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 414);
            this.Controls.Add(this.btnAutoStartApp);
            this.Controls.Add(this.btnDropbox);
            this.Controls.Add(this.btnViewAllResults);
            this.Controls.Add(this.comboBoxEuraqaServeIP);
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
        private System.Windows.Forms.ComboBox comboBoxEuraqaServeIP;
        private System.Windows.Forms.Button btnViewAllResults;
        private System.Windows.Forms.Button btnDropbox;
        private System.Windows.Forms.Button btnAutoStartApp;
    }
}

