﻿using System;
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
        private System.Windows.Forms.Timer Clock;
        delegate void SetItemCallback(string text);

        string savePath = "";

        public Measure(int start, int stop)
        {
            InitializeComponent();

            savePath = Config.WORKINGDIR + @"\" + Config.SAMPLENUMBER;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //chkBusybox();
            cleanFile();

            this.currentTime = start;
            this.stopTime = stop;
            Clock = new System.Windows.Forms.Timer();
            turbo = 5;
            StartTimer(1000 / turbo);
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

        private bool StopTimer()
        {
            if (this.currentTime > this.stopTime)
            {
                //Clock.Tick -= new System.EventHandler(Timer_Tick);
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
                if (StopTimer()) return;

                // Show current time
                lbTime.Text = this.currentTime.ToString();
                // updateStatus("Time " + this.currentTime.ToString());

                switch (this.currentTime)
                {
                    case 10:
                        Thread t1 = new Thread(startSampling);
                        t1.Start();
                        break;
                    case 20:
                        updateStatus("Start power meter");
                        //Thread monsoon = new Thread(StartMonsoon);
                        //monsoon.Start();
                        break;
                    case 50:
                        updateStatus("Start testing app");
                        break;
                    case 150:
                        updateStatus("Stop testing app");
                        break;
                    case 180:
                        updateStatus("Sample should stop");
                        break;
                    case 200:
                        updateStatus("Power should stop");
                        break;
                    case 210:
                        Thread t2 = new Thread(pullFile);
                        t2.Start();
                        break;
                }

                // Tick
                this.currentTime++;
            }
        }

        private void cleanFile()
        {
            updateStatus("Clean files");
            ProcessStartInfo rmFile = new ProcessStartInfo("cmd.exe", "/c " + "adb shell rm /data/local/tmp/stat/*.txt");
            rmFile.CreateNoWindow = false;
            rmFile.UseShellExecute = false;
            rmFile.RedirectStandardError = true;
            rmFile.RedirectStandardOutput = true;
            Process process2 = Process.Start(rmFile);
            updateStatus("Finish clean files in /data/local/tmp/stat");
        }

        private void startSampling()
        {
            updateStatus("Start sampling");
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"./data/local/tmp/a.out 1 170 " + Config.WIFI + " " + Config.APP2TEST + " &\" | adb shell");
            sample.CreateNoWindow = false;
            sample.UseShellExecute = false;
            sample.RedirectStandardError = true;
            sample.RedirectStandardOutput = true;
            Process process = Process.Start(sample);

        }

        private void StartMonsoon()
        {
            updateStatus("Start Power sampling");
            //int measureDuration = 150; //seconds
            Process powerMonitor = new Process();
            powerMonitor.StartInfo.FileName = Config.POWERMETER;
            powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=" + Config.VOLT + " /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + savePath + @"\power.pt4  /TRIGGER=DTXD180A"; //60 seconds
            powerMonitor.Start();
            powerMonitor.WaitForExit();

        }

        public void pullFile()
        {
            updateStatus("Start Pull files");

            //string path = "/c " + "adb pull /data/local/tmp/stat " + savePath;
            ProcessStartInfo pullFile = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /data/local/tmp/stat " + savePath);
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