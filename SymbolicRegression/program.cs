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

namespace SymbolicRegression
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ...");
            new Measure();
            //new TimerDemo().ProcessPowerTraceFile();
            //Parse p = new Parse(@"C:\Experiment\mtk");
            //p.process();
            Console.WriteLine("Complete ...");
           // Console.ReadKey();
        }        
    }

    class Measure
    {
        //Set and start countdown dialog
        public int time = 650;
        public TimerDemo timer;
        public void CountDown()
        {
            timer = new TimerDemo(0, time);
            Application.Run(timer);
        }

        public Measure()
        {
            new Thread(CountDown).Start();
        }
    }
   
    class TimerDemo : Form
    {
        System.Windows.Forms.Timer Clock;
        
        Label lbTime = new Label();

        public int start, stop;

        string savePath = @"C:\Users\pok\Research\Experiment\Dropbox\Project2\test1\highCpu";

        public TimerDemo() { }

        public TimerDemo(int start, int stop)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            this.start = start;
            this.stop = stop;
            Clock = new System.Windows.Forms.Timer();
            Clock.Interval = 1000;
            Clock.Start();
            Clock.Tick += new EventHandler(Timer_Tick);

            this.Controls.Add(lbTime);
            lbTime.BackColor = Color.Black;
            lbTime.ForeColor = Color.White;

            lbTime.Font = new Font("Times New Roman", 100);
            
            lbTime.Width = 220;
            lbTime.Height = 180;
            lbTime.Location = new Point(20, 20);
            lbTime.Text = GetTime();
        }

        public string GetTime()
        {
            string TimeInString = "";
            TimeInString += this.start;
            this.start++;
            if (this.start > this.stop)
            {
                Clock.Tick -= new System.EventHandler(Timer_Tick);
            }

           // Console.WriteLine(TimeInString);

            return TimeInString;
        }

        public void Timer_Tick(object sender, EventArgs eArgs)
        {
            if (sender == Clock)
            {
                lbTime.Text = GetTime();
                Console.WriteLine("Time " + lbTime.Text);

                String s = lbTime.Text.ToString();

                switch (s)
                {
                    case "1":
                        Console.WriteLine("Start sampling");
                        Thread t1 = new Thread(startSampling);
                        t1.Start();
                        //t1.Join();
                        break;

                    case "10": Console.WriteLine("Start Power sampling");
                        Thread monsoon = new Thread(StartMonsoon);
                        monsoon.Start();
                        //Depend on your power measure device.
                        break;

                    /* case "20": Console.WriteLine("Start benchmarked app");
                         new Thread(startBenchmarkApp).Start();
                         break; */

                    case "650":
                    
                        Thread t2 = new Thread(pullFile);
                        t2.Start();
                        t2.Join();
                        break;

                  /*  case "330":
                        Thread t3 = new Thread(removeFile);
                        t3.Start();
                        t3.Join();
                        break; */

                   /* case "340":
                        processSample();
                        break; */
                }
            }
        }

        public void startSampling()
        {
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"./data/local/tmp/nexus 1 "+(this.stop - 50)+" & \" | adb shell"); //25 mins
            sample.CreateNoWindow = true;
            sample.UseShellExecute = false;
            sample.RedirectStandardError = true;
            sample.RedirectStandardOutput = true;
            Process process = Process.Start(sample);
        }

        public void StartMonsoon()
        {
            //int measureDuration = 150; //seconds
            Process powerMonitor = new Process();
            powerMonitor.StartInfo.FileName = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";
            powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=3.70 /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + savePath + "\\power.pt4  /TRIGGER=DTXD" + (this.stop - 60) + "A"; //DTYD60A
            powerMonitor.Start();
            powerMonitor.WaitForExit();
        }

       /* public void startBenchmarkApp()
        {
            ProcessStartInfo bApp = new ProcessStartInfo("cmd.exe", "/c " + "adb shell am start -n edu.umich.PowerTutor/edu.umich.PowerTutor.ui.UMLogger"); //25 mins
            bApp.CreateNoWindow = true;
            bApp.UseShellExecute = false;
            bApp.RedirectStandardError = true;
            bApp.RedirectStandardOutput = true;
            Process process = Process.Start(bApp);
        } */

        public void pullFile()
        {
            Console.WriteLine("Start Pull files");

           
            ProcessStartInfo pullFile = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /data/local/tmp/stat " + savePath); 
            pullFile.CreateNoWindow = true;
            pullFile.UseShellExecute = false;
            pullFile.RedirectStandardError = true;
            pullFile.RedirectStandardOutput = true;
            Process process = Process.Start(pullFile);

            Console.WriteLine("Finish pull trace file");

        }

        public void removeFile()
        {
          Console.WriteLine("Start delete files");
          ProcessStartInfo rmFile = new ProcessStartInfo("cmd.exe", "/c " + "adb shell rm /data/local/tmp/stat/*.txt");
          rmFile.CreateNoWindow = true;
          rmFile.UseShellExecute = false;
          rmFile.RedirectStandardError = true;
          rmFile.RedirectStandardOutput = true;
          Process process2 = Process.Start(rmFile);
          Console.WriteLine("Finish delete files");
          
        }

        public void processSample() 
        {
            string[] files = Directory.GetFiles(savePath);

            while (files.Length <= 0)
            {
                Console.WriteLine("No files to process yet");
                Thread.Sleep(1000);
            }

            Parse p = new Parse(savePath);
            p.process();
        }
    }
}