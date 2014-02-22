using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices; // SEHException
using AsyncProject;
using EureqaTestProject;
using ParseModelProject;
using ProcessSample;
using ShowResultsProject;

namespace SRC_GUI
{
    public partial class SRC_GUI : Form
    {
        delegate void SetItemCallback(string text);
        private int currentApp;

        public SRC_GUI()
        {
            InitializeComponent();
        }

        // Default Settings
        private void btnEkarat_Click(object sender, EventArgs e)
        {
            textPowerMonitorSRC.Text = @"C:\Program Files (x86)\Monsoon Solutions Inc\Power Monitor\PowerToolCmd.exe";
            textSampleRoot.Text = @"C:\Users\pok\Documents\GitHub\SRC_tool";
            comboSampleType.SelectedIndex = 7;
            comboBoxEuraqaServeIP.SelectedIndex = 1;
        }

        private void btnKohy_Click(object sender, EventArgs e)
        {
            textRScript.Text = @"C:\Users\raining\Desktop\Dropbox\NCTU\EmbeddedProject\Power\SRC_GUI\asynComponent.r";
            textPowerMonitorSRC.Text = @"C:\Program Files\Monsoon Solutions Inc\Power Monitor\PowerToolCmd.exe";
            textSampleRoot.Text = @"C:\ebl";
            comboSampleType.SelectedIndex = 7;
            comboBoxEuraqaServeIP.SelectedIndex = 0;
        }

        private void btnDropbox_Click(object sender, EventArgs e)
        {
            textRScript.Text = @"C:\Users\rain\Dropbox\NCTU\EmbeddedProject\Power\SRC_GUI\asynComponent.r";
            textPowerMonitorSRC.Text = @"C:\Program Files\Monsoon Solutions Inc\Power Monitor\PowerToolCmd.exe";
            textSampleRoot.Text = @"C:\Users\rain\Dropbox\NCTU\EmbeddedProject\Power\ebl";
            comboSampleType.SelectedIndex = 7;
            comboBoxEuraqaServeIP.SelectedIndex = 0;
        }

        // Directory Functions
        private void btnRScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog RScriptFile = new OpenFileDialog();
            RScriptFile.Filter = "RScript|*.r|All Files|*.*";
            RScriptFile.FilterIndex = 1;

            if (RScriptFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textRScript.Text = RScriptFile.FileName;
            }
        }

        private void btnPowerMonitorSRC_Click(object sender, EventArgs e)
        {
            OpenFileDialog PowerMonitorFile = new OpenFileDialog();
            PowerMonitorFile.Filter = "Application|*.exe|All Files|*.*";
            PowerMonitorFile.FilterIndex = 1;

            if (PowerMonitorFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textPowerMonitorSRC.Text = PowerMonitorFile.FileName;
            }
        }

        private void btnSampleRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderProjectSRC = new FolderBrowserDialog();
            folderProjectSRC.SelectedPath = textSampleRoot.Text;

            if (folderProjectSRC.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textSampleRoot.Text = folderProjectSRC.SelectedPath;
            }
        }

        private void comboProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentApp = comboSampleType.SelectedIndex;
            switch (comboSampleType.Text)
            {
                case @"skype\idle":
                case @"skype\voice":
                case @"skype\video":
                    textAppName.Text = "com.skype.raider";
                    Config.PACKAGE = "com.skype.rover";
                    Config.ACTIVITY = "com.skype.android.app.main.SplashActivity";
                    break;
                case @"line\idle":
                case @"line\voice":
                case @"line\video":
                    textAppName.Text = "jp.naver.line.android";
                    Config.PACKAGE = "";
                    Config.ACTIVITY = "";
                    break;
                case @"candycrush":
                    textAppName.Text = "com.king.candycrushsaga";
                    Config.PACKAGE = "com.king.candycrushsaga";
                    Config.ACTIVITY = "com.king.candycrushsaga.CandyCrushSagaActivity";
                    break;
                case @"pokopang":
                    textAppName.Text = "jp.naver.SJLGPP";
                    Config.PACKAGE = "jp.naver.SJLGPP";
                    Config.ACTIVITY = "com.treenod.android.UnityPlayerActivity";
                    break;
                default:
                    textAppName.Text = "none";
                    Config.PACKAGE = "none";
                    Config.ACTIVITY = "none";
                    break;
            }
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
            updateConfig();
            new Thread(SampleStart).Start();
        }

        private void btnProcessSample_Click(object sender, EventArgs e)
        {
            updateConfig();

            try
            {
                updateStatus("Start processing all samples : totally #" + Config.TOTALSAMPLE + " samples.");
                Parse sample = new Parse(Config.POWEROFFSET, Config.VOLT, Config.WORKINGDIR, Config.TOTALSAMPLE);
                updateStatus("Step2 : Process Sample Processed Successfully.");
            }
            catch (ApplicationException msg)
            {
                updateStatus(msg.Message);
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            int status = -1;

            updateConfig();

            try
            {
				Async sample = new Async(Config.RSCRIPTSRC, Config.WORKINGDIR, Config.SAMPLENUMBER);
				status = sample.status;
				//labelStatus.Text = "RScript Processing Status : " + status;
				updateStatus("Step 3 : Async Processing Status : " + status);
            }
			catch(IOException msg){
                updateStatus(msg.Message);
			}
        }

        private void btnEureqa_Click(object sender, EventArgs e)
        {
            updateConfig();

            try
            {
                EureqaTest sample = new EureqaTest(Config.WORKINGDIR, Config.SAMPLENUMBER, Config.IP_EUREQA_SERVER, 10000);
                updateStatus("Step 4 : Eureqa Processed Successfully.");
            }
            catch(ApplicationException msg) {
                updateStatus(msg.Message);
            }
			catch(ArgumentException msg){
                updateStatus(msg.Message);
			}
			catch(IOException msg){
                updateStatus(msg.Message);
			}
			catch(SEHException msg){
                updateStatus(msg.Message);
			}
        }

        private void btnParseModel_Click(object sender, EventArgs e)
        {
            updateConfig();

            try
            {
				ParseModel sample = new ParseModel(Config.WORKINGDIR, Config.TOTALSAMPLE);
				sample.ShowDialog();
                updateStatus("Step 5 : ParseModel Processed Successfully.");
            }
			catch(IOException msg){
                updateStatus(msg.Message);
            }
            catch (ArgumentException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"model.txt\" needs to be updated.");
            }
        }

        private void btnAutoComplete_Click(object sender, EventArgs e)
        {
        }

        private void updateConfig()
        {
            labelWorkingDir.Text = textSampleRoot.Text + @"\" + comboSampleType.Text;

            Config.DURATION = Convert.ToInt32(numericDuration.Value);
            Config.POWEROFFSET = Convert.ToInt32(numericPowerOffest.Value);
            Config.TOTALSAMPLE = Convert.ToInt32(numericTotalSample.Value);
            Config.SAMPLENUMBER = numericSampleNumber.Value.ToString();
            Config.MODLEFOLDER = "3";

            Config.POWERMETER = textPowerMonitorSRC.Text;
            Config.RSCRIPTSRC = textRScript.Text;
            Config.SAMPLEROOT = textSampleRoot.Text;
            Config.SAMPLESUBDIR = comboSampleType.Text;
            Config.WORKINGDIR = labelWorkingDir.Text;

            Config.IP_EUREQA_SERVER = comboBoxEuraqaServeIP.Text;
            Config.WIFI = textWifi.Text;
            Config.APP2TEST = textAppName.Text;
            Config.VOLT = Convert.ToDouble(textVout.Text);
        }

        private void SampleStart()
        {
            Measure sample;
            sample = new Measure(0, Config.DURATION, Config.PACKAGE, Config.ACTIVITY);
            Application.Run(sample);
        }

        private void btnAutoStartApp_Click(object sender, EventArgs e)
        {
            updateConfig();

            updateStatus("Auto Start App");
            updateStatus("/c " + "adb shell am start -n " + Config.PACKAGE + @"/" + Config.ACTIVITY);
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", "/c " + "adb shell am start -n " + Config.PACKAGE + @"/" + Config.ACTIVITY);
            sample.CreateNoWindow = false;
            sample.UseShellExecute = false;
            sample.RedirectStandardError = true;
            sample.RedirectStandardOutput = true;
            Process process = Process.Start(sample);
        }

        private void btnPullData_Click(object sender, EventArgs e)
        {
            updateConfig();

            string path = Config.WORKINGDIR + @"\" + Config.SAMPLENUMBER;
            updateStatus("Start pulling files from \"" + path + "\"");

            //string path = "/c " + "adb pull /data/local/tmp/stat " + savePath;
            ProcessStartInfo pullFile = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /data/local/tmp/stat " + path);
            pullFile.CreateNoWindow = false;
            pullFile.UseShellExecute = false;
            pullFile.RedirectStandardError = true;
            pullFile.RedirectStandardOutput = true;
            Process process = Process.Start(pullFile);

            if (process != null)
            {
                process.WaitForExit();
                updateStatus("Finished pulling trace files.");
            }
            else
            {
                updateStatus("Failed to pull files from target.");
            }
        }

        private void btnApplicationStarup_Click(object sender, EventArgs e)
        {
            updateConfig();

            string para;

            para = "adb shell am start -n " + Config.PACKAGE + @"/" + Config.ACTIVITY;
            updateStatus("Starting application from target.");
            updateStatus("==> " + para);

            //string path = "/c " + "adb pull /data/local/tmp/stat " + savePath;
            ProcessStartInfo pullFile = new ProcessStartInfo("cmd.exe", "/c " + para);
            pullFile.CreateNoWindow = false;
            pullFile.UseShellExecute = false;
            pullFile.RedirectStandardError = true;
            pullFile.RedirectStandardOutput = true;
            Process process = Process.Start(pullFile);

            if (process != null)
            {
                process.WaitForExit();
                updateStatus("Application Started.");
            }
            else
            {
                updateStatus("Failed to connecting to target.");
            }
        }

        private void btnViewAllResults_Click(object sender, EventArgs e)
        {
            updateConfig();

            try
            {
                updateStatus("Show All Results.");
                ShowResults sample = new ShowResults(textSampleRoot.Text.ToString());
                sample.ShowDialog();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
			catch (FormatException msg)
			{
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
            }
        }

        private void updateStatus(string status)
        {
            if (this.listBoxStatus.InvokeRequired)
            {
                SetItemCallback d = new SetItemCallback(updateStatus);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                this.listBoxStatus.Items.Add(status);
                listBoxStatus.SelectedIndex = listBoxStatus.Items.Count - 1;
                listBoxStatus.SelectedIndex = -1;
            }
        }
    }

    public class Config
    {
        public static int DURATION = 210;
        public static int POWEROFFSET = 10; //time after start sampling (seconds)
        public static int TOTALSAMPLE = 5;
        public static string SAMPLENUMBER = "";
        public static string MODLEFOLDER = "";

        // Path
        public static string POWERMETER = "";
        public static string RSCRIPTSRC = "";
        public static string SAMPLEROOT = "";
        public static string SAMPLESUBDIR = "";
        public static string WORKINGDIR = "";

        // IP
        public static string IP_EUREQA_SERVER = "";

        //Nexus S, Galaxy S4, Fame, S2
        public static string WIFI = "";

        //Example app "com.google.android.youtube";
        //com.skype.raider
        public static string APP2TEST = "";
        public static string PACKAGE = "";
        public static string ACTIVITY = "";

        public static double VOLT = 4.2;
    }
}
