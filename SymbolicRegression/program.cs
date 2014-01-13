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
        public static int DURATION = 215;
        public static int POWEROFFSET = 10; //time after start sampling (seconds)
        public static string ROOTPATH = @"C:\ebl\";

        public static string SAVEFOLDER = @"skype\";

    

        public static string SAVETIMES = @"1";
        public static string POWERMETER = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";
        public static string SRC_TOOL_PATH =  "C:\\Users\\pok\\Documents\\GitHub\\SRC_tool\\";
        public static string IP_EUREQA_SERVER = "140.113.88.194";

        //Nexus S, Galaxy S4, Fame, S2
        public static string WIFI = @"/sys/class/net/wlan0/";

        //Example app "com.google.android.youtube";
        //com.skype.raider
        public static string APP2TEST = "com.skype.raider";

        public static double VOLT = 4.2;
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ...");

            new Measure();
            //new TimerDemo().pullFile();
            //new TimerDemo().processSample();
            //Console.WriteLine("Complete ...");
           
            Console.ReadKey();
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
            chkBusybox();

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

        private static void chkBusybox()
        {
            //Check whether busybox exists in /data/local/tmp/ or not.
            ProcessStartInfo chkBusybox = new ProcessStartInfo("cmd.exe", "/c " + "adb shell ls /data/local/tmp/busybox > "+ Config.ROOTPATH +"check.txt");
            chkBusybox.CreateNoWindow = true;
            chkBusybox.UseShellExecute = false;
            chkBusybox.RedirectStandardError = true;
            chkBusybox.RedirectStandardOutput = true;
            Process process = Process.Start(chkBusybox);
            process.WaitForExit();

            while (!File.Exists(Config.ROOTPATH + "check.txt")) { Console.WriteLine("Wait check.txt to save"); }

            string[] data = File.ReadAllLines(Config.ROOTPATH+"check.txt");
            if (data[0].Contains("No such file"))
            {
                Console.WriteLine("set up busybox");
                //Add busybox and then chmod to 777
                ProcessStartInfo pushBusybox = new ProcessStartInfo("cmd.exe", "/c " + "adb push "+ Config.ROOTPATH +"busybox /data/local/tmp/");
                pushBusybox.CreateNoWindow = true;
                pushBusybox.UseShellExecute = false;
                pushBusybox.RedirectStandardError = true;
                pushBusybox.RedirectStandardOutput = true;
                Process process2 = Process.Start(pushBusybox);
                process2.WaitForExit();

                //Add busybox and then chmod to 777
                ProcessStartInfo modeBusybox = new ProcessStartInfo("cmd.exe", "/c " + "adb shell chmod 777 /data/local/tmp/busybox");
                modeBusybox.CreateNoWindow = true;
                modeBusybox.UseShellExecute = false;
                modeBusybox.RedirectStandardError = true;
                modeBusybox.RedirectStandardOutput = true;
                Process process3 = Process.Start(modeBusybox);
                process3.WaitForExit();
            }

            Console.WriteLine("Success busybox");

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
                   
                    case "10":
                        Thread t1 = new Thread(startSampling);
                        t1.Start();
                        break;

                    case "20": 
                        Console.WriteLine("Start power meter");
                        Thread monsoon = new Thread(StartMonsoon);
                        monsoon.Start();
                        break;

                    case "50":
                        Console.WriteLine("Start testing app");
                        break;

                    case "150":
                        Console.WriteLine("Stop testing app");
                        break;

                    case "180":
                        Console.WriteLine("Sample should stop");
                        break;

                    case "200":
                        Console.WriteLine("Power should stop");
                        break;

                    case "210":
                        Thread t2 = new Thread(pullFile);
                        t2.Start();
                        break;

                   /* case "220":/
                        Console.WriteLine("Finish job and process sampling files");
                        processSample();
                        break; */

                    /*  case "330":
                    */
                }
            }
        }

        public void startSampling()
        {
            Console.WriteLine("Start sampling");
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"./data/local/tmp/a.out 1 170 " + Config.WIFI + " " +Config.APP2TEST+ " &\" | adb shell");           
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
                powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=" + Config.VOLT + " /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + savePath + "\\power.pt4  /TRIGGER=DTXD180A"; //60 seconds
                powerMonitor.Start();
                powerMonitor.WaitForExit();
           
        }    

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
            Console.ReadKey();
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
          Console.WriteLine("Finish clean files in /data/local/tmp/stat");
          
        }

        public void AsynProcess()
        {
            Console.WriteLine("Start async processing...");

            //Build power model in folder 1
            ProcessStartInfo asynProc = new ProcessStartInfo("cmd.exe", "/c " + "Rscript " + Config.SRC_TOOL_PATH + "asynComponent.r " + Config.ROOTPATH + Config.SAVEFOLDER + @"1\");
            asynProc.CreateNoWindow = true;
            asynProc.UseShellExecute = false;
            asynProc.RedirectStandardError = true;
            asynProc.RedirectStandardOutput = true;
            Process process2 = Process.Start(asynProc);

            if (process2 != null)
            {
                process2.WaitForExit();
            }

            Console.WriteLine("Finish async processing...");
        }

        /*public void processSample() 
        {
            Parse p = new Parse();
            p.folderName = savePath;
            p.processTrain();

            if (Config.SAVETIMES.Equals("5"))
            {
               // AsynProcess();

                Eureqa modelProcss = new Eureqa();
                string model = "power = f(cpu1,cpu2,cpu3,cpu4,cpu5,cpu6,cpu7,cpu8,freq1,freq2,freq3,freq4,freq5,freq6,freq7,freq8,bright,rx_pk,rx_byte,tx_pk,tx_byte)";
                modelProcss.Run(Config.ROOTPATH + Config.SAVEFOLDER + @"1\modifyPower.txt", model, Config.IP_EUREQA_SERVER);
            }
        }*/

        
    }
}