using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel; // Needed by BackgroundWorker

namespace SRC_GUI
{
    public partial class Measure : Form
    {
        private int turbo;
        private int currentTime, stopTime;
        private string _package;
        private string _activity;
        private System.Windows.Forms.Timer Clock;
        private System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"beep.wav");
        delegate void SetItemCallback(string text);

        string savePath = "";

        int innerStep = 1;
        int maxInnerStep = 5;
        int outerStep = 1;
        int maxOuterStep = 40;
        int currentUtil = 0;

        public Measure(int startTime, int stopTime, String package, String activity, int mode)
        {

            InitializeComponent();

            this.currentTime = startTime;
            this.stopTime = 800; // stopTime;
            Clock = new System.Windows.Forms.Timer();
            turbo = 1;

            if (Config.isCPU)
            {
                this.stopTime = 800;
            }
            else if (Config.isLCD)
            {
                this.stopTime = 800;
            }

            //Set training app (Android)
            _package = "com.example.semionline";
            _activity = "com.example.semionline.MainActivity";
            savePath = @"D:\SemiOnline\";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //chkBusybox();

            cleanFile(" /sdcard/semionline/*.txt");
            StartTimer2(1000 / turbo);
                        
        }

        public Measure(int start, int stop, string package, string activity)
        {
            InitializeComponent();

            //Set training app (Android)
            _package = package;
            _activity = activity;
            savePath = Config.WORKINGDIR + @"\" + Config.SAMPLENUMBER;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //chkBusybox();
            cleanFile(" /data/local/tmp/stat/*.txt");
            StartTimer(1000 / turbo);

            this.currentTime = start;
            this.stopTime = stop;
            Clock = new System.Windows.Forms.Timer();
            turbo = 1;

        }

        private void chkBusybox()
        {
            //Check whether busybox exists in /data/local/tmp/ or not.
            ProcessStartInfo chkBusybox = new ProcessStartInfo("cmd.exe", "/c " + "adb shell ls /data/local/tmp/busybox > " + Config.SAMPLEROOT + @"\check.txt");
            chkBusybox.CreateNoWindow = true;
            chkBusybox.UseShellExecute = false;
            chkBusybox.RedirectStandardError = true;
            chkBusybox.RedirectStandardOutput = true;
            Process process = Process.Start(chkBusybox);
            process.WaitForExit();

            while (!File.Exists(Config.SAMPLEROOT + @"\check.txt")) { updateStatus("Wait check.txt to save"); }

            string[] data = File.ReadAllLines(Config.SAMPLEROOT + @"\check.txt");
        }

        private void StartTimer(int interval)
        {
            Clock.Interval = interval;
            Clock.Tick += new EventHandler(Timer_Tick);
            Clock.Start();
        }

        private void StartTimer2(int interval)
        {

            Clock.Interval = interval;
            Clock.Tick += new EventHandler(Timer_Tick2);
            Clock.Start();
            
        }

        private bool StopTimer()
        {
            if (this.currentTime > this.stopTime)
            {
                Clock.Tick -= new System.EventHandler(Timer_Tick2);
                Clock.Stop();
                return true;
            }

            return false;
        }

        private void Timer_Tick(object sender, EventArgs eArgs)
        {
            if (sender == Clock)
            {
                // Terminate clock
                if (StopTimer()) this.Close();

                // Show current time
                lbTime.Text = this.currentTime.ToString();
                // updateStatus("Time " + this.currentTime.ToString());

                switch (this.currentTime)
                {
                    case 10:
                        Thread t1 = new Thread(startSamplingApp);
                        t1.Start();
                        break;
                    case 20:
                        updateStatus("Start power meter");
                        Thread monsoon = new Thread(StartMonsoon);
                        monsoon.Start();
                        break;
                    case 45:
                        player.Play();
                        break;
                    case 50:
                        //Thread t2 = new Thread(startApp);
                        //t2.Start();
                        updateStatus("Start testing app");
                        break;
                    case 150:
                        updateStatus("Stop testing app");
                        break;
                    case 153:
                        player.Play();
                        break;
                    case 180:
                        updateStatus("Sample should stop");
                        break;
                    case 200:
                        updateStatus("Power should stop");
                        break;
                    case 210:
                        //Thread t3 = new Thread(pullFile);
                        //t3.Start();
                        break;
                }

                // Tick
                this.currentTime++;
            }
        }
       
       
        private void Timer_Tick2(object sender, EventArgs eArgs)
        {
            if (sender == Clock)
            {
                // Terminate clock
                if (StopTimer())
                {
                    if (Config.isCPU)
                    {
                        //this.Close();
                        if (outerStep < maxOuterStep)
                        {
                            this.currentTime = 0;
                            cleanFile(" /sdcard/semionline/*.txt");

                            if (innerStep < maxInnerStep)
                            {
                                ++innerStep;
                            }
                            else
                            {
                                innerStep = 1;
                                currentUtil += 10;
                            }

                            ++outerStep;
                            StartTimer2(1000);

                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else if (Config.isLCD)
                    {

                    }
                }

                // Show current time
                lbTime.Text = this.currentTime.ToString();
                // updateStatus("Time " + this.currentTime.ToString());

                if (this.currentTime == 1)
                {
                    string path = "";

                    if (Config.isCPU)
                    {
                        path = savePath + "util" + currentUtil + "_" + innerStep;
                    }
                    else if (Config.isLCD)
                    {
                        path = savePath + "lcd";
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Thread t1 = new Thread(this.startApp);
                    t1.Start();
                    //this.pullFile("/sdcard/semionline/", "pullFolder");
                }
                else if (this.currentTime == 30)
                {
                    Thread t2 = new Thread(this.StartMonsoon);
                    t2.Start();
                }
                else if (this.currentTime == (this.stopTime-20))
                {
                    this.pullFile("/sdcard/semionline/", "util" + currentUtil + "_" + innerStep);
                }
 
                // Tick
                this.currentTime++;
            }
        }

        private void cleanFile(String target)
        {
          callProcess(Config.ADB," shell rm "+target);
        }

        private void startSamplingApp()
        {
           
            callProcess("cmd.exe","/c " + "echo sh -c \"./data/local/tmp/a.out 1 170 " + Config.WIFI + " " + Config.APP2TEST + " &\" | adb shell");
        }

        private void startAppMonsoon()
        {
            this.startApp();
            this.StartMonsoon();
        }

        private void startApp()
        {
            callProcess(Config.ADB, " shell am start -n " + _package + @"/" + _activity + " --es extraKey "+currentUtil);            
        }

        private void killApp()
        {
            callProcess(Config.ADB, " shell am force-stop " + _package);
        }

        private void StartMonsoon()
        {
            int offset = 0;

            if (Config.isCPU)
            { 
                offset = 150; 
            }
            else if (Config.isLCD)
            { 
                offset = 50;
            }

            callProcess(Config.POWERMETER, "/USBPASSTHROUGH=AUTO /VOUT=" + Config.VOLT + " /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + savePath + "util" + currentUtil + "_" + innerStep + @"\power.pt4  /TRIGGER=DTXD" + (this.stopTime - offset) + "A");   
        }

        public void pullFile(String target, String host)
        {
            String saveTarget = savePath + host;
            if (!Directory.Exists(saveTarget))
                Directory.CreateDirectory(saveTarget);

            callProcess(Config.ADB, " pull " + target + " " + saveTarget);

            killApp();

        }

        private void callProcess(string fileName, string argues)
        {
            updateStatus("Start " + fileName + " " + argues);
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = argues;
            proc.Start();
            updateStatus("Finish call process");
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
}