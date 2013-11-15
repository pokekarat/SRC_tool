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


    public class Config
    {
        public static int DURATION = 100;
        public static string ROOTPATH = @"C:\ebl";
        public static string TESTCASE = @"\skype";
        public static string TESTTIME = @"1";

        //Nexus S, Galaxy S4
        public static string WIFI = @"/sys/class/net/wlan0/";

        //Example app "com.google.youtube.com";
        public static string APP2TEST = "com.google.android.youtube";

        public static int MODE = 1; //1= train, 2 = test
        
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ...");

            //new Measure();
            new TimerDemo().processSample();
            //Parse p = new Parse(@"C:\Experiment\mtk");
            //p.process();
            Console.WriteLine("Complete ...");
           
            // Console.ReadKey();
        }        
    }

    class Measure
    {
        //Set and start countdown dialog
        public int duration = Config.DURATION;
        public TimerDemo timer;
        public void CountDown()
        {
            timer = new TimerDemo(0, duration);
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

        public int startTime, stopTime;

        string savePath = Config.ROOTPATH;
        string trainPath = "";
        string testPath = "";

        public TimerDemo() { }

        public TimerDemo(int start, int stop)
        {

            trainPath = savePath + Config.TESTCASE + @"\train\";
            testPath = savePath + Config.TESTCASE + @"\test\" + Config.TESTTIME;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(trainPath);
                Directory.CreateDirectory(testPath);
            }

            this.startTime = start;
            this.stopTime = stop;
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
            TimeInString += this.startTime;
            this.startTime++;
            if (this.startTime > this.stopTime)
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
                    /*case "0":
                        Thread cleanFiles = new Thread(cleanFile);
                        cleanFiles.Start();
                         break;*/
                   
                    case "1":
                        Thread t1 = new Thread(startSampling);
                        t1.Start();
                        //t1.Join();
                        break;

                    case "10": 
                        Thread monsoon = new Thread(StartMonsoon);
                        monsoon.Start();
                        //Depend on your power measure device.
                        break;

                    case "20":
                        Console.WriteLine("Start testing app");
                        break;

                    case "50":
                        Console.WriteLine("Stop testing app");
                        break;

                    case "60":
                        Console.WriteLine("Sample should stop");
                        break;

                    case "70":
                        if (Config.APP2TEST == "") Console.WriteLine("Power should stop");
                        break;

                    case "80":
                        Thread t2 = new Thread(pullFile);
                        t2.Start();
                        break;

                    case "100":
                        Console.WriteLine("Finish job");
                        break;

                    /*  case "330":
                    */
                }
            }
        }

        public void startSampling()
        {
            Console.WriteLine("Start sampling");
            //string path = "/c " + "echo sh -c \"./data/local/tmp/sample 1 60 "+Config.WIFI+" "+Config.APP2TEST+" & \" | adb shell";
            string path = "/c " + "adb shell ./data/local/tmp/sample 1 60 " + Config.WIFI + " " + Config.APP2TEST + " &";
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", path );
            sample.CreateNoWindow = true;
            sample.UseShellExecute = false;
            sample.RedirectStandardError = true;
            sample.RedirectStandardOutput = true;
            Process process = Process.Start(sample);
        }

        public void StartMonsoon()
        {
            if (Config.MODE == 1)
            {
                Console.WriteLine("Start Power sampling");
                //int measureDuration = 150; //seconds
                Process powerMonitor = new Process();
                powerMonitor.StartInfo.FileName = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";
                powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=4.20 /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + trainPath + "\\power.pt4  /TRIGGER=DTXD60A"; //60 seconds
                powerMonitor.Start();
                powerMonitor.WaitForExit();
            }
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

            string path = "";
            if (Config.MODE == 1)
            {
                path = trainPath;
            }
            else
            {
                path = testPath;
            }
           
            ProcessStartInfo pullFile = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /data/local/tmp/stat " + path); 
            pullFile.CreateNoWindow = true;
            pullFile.UseShellExecute = false;
            pullFile.RedirectStandardError = true;
            pullFile.RedirectStandardOutput = true;
            Process process = Process.Start(pullFile);

            Console.WriteLine("Finish pull trace file");

            string[] files = Directory.GetFiles(savePath, "*.txt");
            
            while (files.Length == 0)
            {
                Console.WriteLine("Files are not pulled yet ...");
                files = Directory.GetFiles(savePath, "*.txt");
            }

            cleanFile();
            //processSample();
        }

        public void cleanFile()
        {
          Console.WriteLine("Clean files");
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
            Parse p = new Parse();

            if (Config.MODE == 1)
            {
                p.folderName = Config.ROOTPATH + Config.TESTCASE + @"\train";
                p.processTrain();
            }
            else
            {
                p.folderName = testPath;
                p.processTest();
            }
        }
    }
}