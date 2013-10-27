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
            //Parse p = new Parse(@"C:\Users\pok\Research\Experiment\mtk");
            //p.process();
            Console.WriteLine("Complete ...");
           // Console.ReadKey();
        }        
    }

    class Measure
    {
        //Set and start countdown dialog
        public int time = 70;
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
    /*
    public class Data
    {
        public ArrayList _dataArr = new ArrayList();

        string audio_on = "audio_on";
        string lcd_bright = "lcd_bright";
        string cpu_sys = "cpu_sys";
        string cpu_usr = "cpu_usr";
        string cpu_freq = "cpu_freq";
        string threeg_uplink_bytes = "3g_uplink_bytes";
        string threeg_downlink_bytes = "3g_downlink_bytes";
        string threeg_packets = "3g_packets";
        //string threeg_state = "";
        string wifi_packets = "wifi_packets";
        string wifi_uplink_bytes = "wifi_uplink_bytes";
        string wifi_downlink_bytes = "wifi_downlink_bytes";
        //string wifi_uplink = "";
        //string wifi_speed = "";
        //string wifi_state = "";
        Dictionary<string, string> dict = new Dictionary<string, string>();

        public Data()
        {
                 
            dict[cpu_sys] = "0";
            dict[cpu_usr] = "0";
            dict[cpu_freq] = "0";
            
            dict[lcd_bright] = "0";
            
            dict[audio_on] = "0";

            dict[threeg_packets] = "0";
            dict[threeg_uplink_bytes] = "0";
            dict[threeg_downlink_bytes] = "0";
            
            dict[wifi_packets] = "0";
            dict[wifi_uplink_bytes] = "0";
            dict[wifi_downlink_bytes] = "0";
        }

        public Dictionary<string, string> filterComp()
        {
            int len = _dataArr.Count;

            if (len <= 0) return null;

            for (int j = 0; j < len; j++)
            {
                string component = _dataArr[j].ToString();

                if (component.Contains("LCD-brightness"))
                {
                    string[] com = component.Split(' ');
                    dict[lcd_bright] = com[1];
                }
                else if (component.Contains("CPU-sys"))
                {
                    string[] com = component.Split(' ');
                    dict[cpu_sys] = com[1];
                }
                else if (component.Contains("CPU-usr"))
                {
                    string[] com = component.Split(' ');
                    dict[cpu_usr] = com[1];
                }

                else if (component.Contains("CPU-freq"))
                {
                    string[] com = component.Split(' ');
                    dict[cpu_freq] = com[1];
                }

                else if (component.Contains("3G-uplinkBytes"))
                {
                    string[] com = component.Split(' ');
                    dict[threeg_uplink_bytes] = com[1];
                }

                else if (component.Contains("3G-downlinkBytes"))
                {
                    string[] com = component.Split(' ');
                    dict[threeg_downlink_bytes] = com[1];
                }

                else if (component.Contains("3G-packets"))
                {
                    string[] com = component.Split(' ');
                    dict[threeg_packets] = com[1];
                }
                else if (component.Contains("Wifi-packets"))
                {
                    string[] com = component.Split(' ');
                    dict[wifi_packets] = com[1];
                }
                else if (component.Contains("Wifi-uplinkBytes"))
                {
                    string[] com = component.Split(' ');
                    dict[wifi_uplink_bytes] = com[1];
                }
                else if (component.Contains("Wifi-downlinkBytes"))
                {
                    string[] com = component.Split(' ');
                    dict[wifi_downlink_bytes] = com[1];
                }

                else if (component.Contains("Audio-on"))
                {
                    string[] com = component.Split(' ');

                    if (com[1].Equals("false"))
                        dict[audio_on] = "0";
                    else
                        dict[audio_on] = "1";
                }
            }

            return dict;
        }
    }
    */
    class TimerDemo : Form
    {
        System.Windows.Forms.Timer Clock;
        
        Label lbTime = new Label();

        public int start, stop;

        string savePath = @"C:\Users\pok\Research\Experiment\mtk";

        public TimerDemo() { }

        public TimerDemo(int start, int stop)
        {
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
                        //Depend on your power measure device.
                        break;

                    /* case "20": Console.WriteLine("Start benchmarked app");
                         new Thread(startBenchmarkApp).Start();
                         break; */

                    case "50":
                    
                        Thread t2 = new Thread(pullFile);
                        t2.Start();
                        t2.Join();
                        break;

                    case "60":
                        Thread t3 = new Thread(removeFile);
                        t3.Start();
                        t3.Join();
                        break;

                    case "70":

                        processSample();
                        break;

                }
            }
            
        }

        public void startSampling()
        {
            ProcessStartInfo sample = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"./data/local/tmp/sample 1 30 &\" | adb shell"); //25 mins
            sample.CreateNoWindow = true;
            sample.UseShellExecute = false;
            sample.RedirectStandardError = true;
            sample.RedirectStandardOutput = true;
            Process process = Process.Start(sample);
           
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

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

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