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
        public static int POWEROFFSET = 0; //time after start sampling (seconds)
        public static string ROOTPATH = @"C:\ebl\";
        public static string SAVEFOLDER = @"skype\";
        public static string SAVETIMES = @"2";
        public static string POWERMETER = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";

        //Nexus S, Galaxy S4, Fame, S2
        public static string WIFI = @"/sys/class/net/wlan0/";

        //Example app "com.google.android.youtube";
        //com.skype.raider
        public static string APP2TEST = "com.skype.raider";

        public static double VOLT = 3.7;
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ...");

            new Measure();
            //new TimerDemo().processSample();
            //new TimerDemo().pullFile();
            //new TimerDemo().processSample();
            //Console.WriteLine("Complete ...");
           
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

        public TimerDemo() {
            savePath = savePath + Config.SAVEFOLDER + Config.SAVETIMES;
        }

        public TimerDemo(int start, int stop)
        {

            cleanFile();

            //trainPath = savePath + Config.TESTFOLDER + @"\train\";
            savePath = savePath + Config.SAVEFOLDER + Config.SAVETIMES;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
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
                        break;

                    case "10": 
                        Console.WriteLine("Start power meter");
                        Thread monsoon = new Thread(StartMonsoon);
                        monsoon.Start();
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
                        Console.WriteLine("Power should stop");
                        break;

                    case "80":
                        Thread t2 = new Thread(pullFile);
                        t2.Start();
                        break;

                    case "100":
                        Console.WriteLine("Finish job and process sampling files");
                        processSample();
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
            string path = "/c " + "adb shell ./data/local/tmp/sample 1 "+ (Config.DURATION-40) + " " + Config.WIFI + " " + Config.APP2TEST + " &";
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", path );
            sample.CreateNoWindow = true;
            sample.UseShellExecute = false;
            sample.RedirectStandardError = true;
            sample.RedirectStandardOutput = true;
            Process process = Process.Start(sample);
            
        }

        public void StartMonsoon()
        {
          
                Console.WriteLine("Start Power sampling");
                //int measureDuration = 150; //seconds
                Process powerMonitor = new Process();
                powerMonitor.StartInfo.FileName = Config.POWERMETER;
                powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=" + Config.VOLT + " /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + savePath + "\\power.pt4  /TRIGGER=DTXD"+(Config.DURATION-40)+"A"; //60 seconds
                powerMonitor.Start();
                //powerMonitor.WaitForExit();
           
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
            
            //string path = "/c " + "adb pull /data/local/tmp/stat " + savePath;
            ProcessStartInfo pullFile = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /data/local/tmp/stat " + savePath); 
            pullFile.CreateNoWindow = true;
            pullFile.UseShellExecute = false;
            pullFile.RedirectStandardError = true;
            pullFile.RedirectStandardOutput = true;
            Process process = Process.Start(pullFile);
           
            try
            {
                 if(process != null){
                    process.WaitForExit();
                }
            }
            catch (InvalidOperationException i)
            {

            }

            Console.WriteLine("Finish pull trace file");

            //Console.ReadKey();
            //cleanFile();
           
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
            p.folderName = savePath;
            p.processTrain();
          
        }
    }
}