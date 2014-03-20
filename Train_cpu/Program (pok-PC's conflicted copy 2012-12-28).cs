
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Collections;
using Parse;
using System.Text.RegularExpressions;

namespace Train_cpu
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(60 * 60 * 1000);

           //CPU utilization
           //start from 0...100%
           int[] freq = { 320000, 480000, 800000 };
           int[] utilize = { 50000, 45000, 40000, 35000, 30000, 25000, 20000, 15000, 10000, 5000, 0 };
           int[] brightness = { 0, 26, 51, 77, 102, 128, 153, 179, 204, 230, 255 };
          // int[] brightness = { 0, 51, 102, 153, 204, 255 };
           ArrayList measures = new ArrayList();
          // StreamWriter log = new StreamWriter(@"C:\Users\pok\Dropbox\Train_cpu\output\logfile.txt");
          //TextWriter tw = new StreamWriter(@"C:\Users\pok\Dropbox\Train_cpu\output\data.txt");
          //tw.WriteLine("power util freq bright");

           for (int i = 0; i < freq.Length; i++)
           {
               StreamWriter log = new StreamWriter(@"C:\Users\pok\Dropbox\Train_cpu\output\logfile_"+freq[i]+".txt");
               TextWriter tw = new StreamWriter(@"C:\Users\pok\Dropbox\Train_cpu\output\data_"+freq[i]+".txt");
               tw.WriteLine("power util freq bright");

               for (int j = 0; j < utilize.Length; j++)
               {

                   for (int k = 0; k < brightness.Length; k++)
                   {

                       ProcessStartInfo cpuInfo = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"./data/strc " + utilize[j] + " &\" | adb shell");
                       Console.WriteLine("echo sh -c \"./data/strc " + utilize[j] + " &\" | adb shell");
                       log.WriteLine("echo sh -c \"./data/strc " + utilize[j] + " &\" | adb shell");
                       cpuInfo.CreateNoWindow = true;
                       cpuInfo.UseShellExecute = false;
                       cpuInfo.RedirectStandardError = true;
                       cpuInfo.RedirectStandardOutput = true;
                       Process process = Process.Start(cpuInfo);
                       Thread.Sleep(3000);
                        if (!process.HasExited)
                           process.Kill();

                       string path = "";
                       string path2 = "";
                       ArrayList paths = new ArrayList();
                       for(int t=1; t<=5; t++){

                           ProcessStartInfo freqInfo = new ProcessStartInfo("cmd.exe", "/c " + "echo " + freq[i] + " > /sys/devices/system/cpu/cpu0/cpufreq/scaling_max_freq\" | adb shell");
                           Console.WriteLine("echo " + freq[i] + " > /sys/devices/system/cpu/cpu0/cpufreq/scaling_max_freq\" | adb shell");
                           log.WriteLine("echo " + freq[i] + " > /sys/devices/system/cpu/cpu0/cpufreq/scaling_max_freq\" | adb shell");
                           freqInfo.CreateNoWindow = true;
                           freqInfo.UseShellExecute = false;
                           freqInfo.RedirectStandardError = true;
                           freqInfo.RedirectStandardOutput = true;
                           Process process2 = Process.Start(freqInfo);
                           Thread.Sleep(2000);
                           if (!process2.HasExited)
                               process2.Kill();

                           ProcessStartInfo freqInfo2 = new ProcessStartInfo("cmd.exe", "/c " + "echo " + freq[i] + " > /sys/devices/system/cpu/cpu0/cpufreq/scaling_min_freq\" | adb shell");
                           Console.WriteLine("echo " + freq[i] + " > /sys/devices/system/cpu/cpu0/cpufreq/scaling_min_freq\" | adb shell");
                           log.WriteLine("echo " + freq[i] + " > /sys/devices/system/cpu/cpu0/cpufreq/scaling_min_freq\" | adb shell");
                           freqInfo2.CreateNoWindow = true;
                           freqInfo2.UseShellExecute = false;
                           freqInfo2.RedirectStandardError = true;
                           freqInfo2.RedirectStandardOutput = true;
                           Process process3 = Process.Start(freqInfo2);
                           Thread.Sleep(2000);
                           if (!process3.HasExited)
                               process3.Kill();


                           ProcessStartInfo brightInfo = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \" echo " + brightness[k] + " > /sys/class/leds/lcd-backlight/brightness\" | adb shell");
                           Console.WriteLine("echo sh -c \" echo " + brightness[k] + " > /sys/class/leds/lcd-backlight/brightness\" | adb shell");
                           log.WriteLine("echo sh -c \" echo " + brightness[k] + " > /sys/class/leds/lcd-backlight/brightness\" | adb shell");
                           brightInfo.CreateNoWindow = true;
                           brightInfo.UseShellExecute = false;
                           brightInfo.RedirectStandardError = true;
                           brightInfo.RedirectStandardOutput = true;
                           Process brightProcess = Process.Start(brightInfo);
                           Thread.Sleep(2000);
                           if (!brightProcess.HasExited)
                               brightProcess.Kill();

                           path = @"C:\Users\pok\Dropbox\Train_cpu\output\f_"+freq[i]+ @"\u_"+ (50000-utilize[j])/500 + @"\b_"+brightness[k];
                           path2 = @"\t_"+t;
                           Console.WriteLine(path + path2);
                           log.WriteLine(path + path2);
                           paths.Add(path + path2);
                           try
                           {
                               if (!Directory.Exists(path + path2))
                               {
                                   Directory.CreateDirectory(path + path2);
                               }
                               else
                               {
                                   Console.WriteLine(path + path2 + " is exist");
                                   //return;
                               }
                           }
                           catch (Exception)
                           {
                                Console.WriteLine("Cannot create folder " +path + path2 );
                           }


                           //Call Monsoon.
                           Process powerMonitor = new Process();
                           powerMonitor.StartInfo.FileName = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";
                           powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=3.70 /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + path + path2 + "\\power.pt4  /TRIGGER=DTXD50A"; //DTYD60A
                           powerMonitor.Start();
                           powerMonitor.WaitForExit();
                           Thread.Sleep(10000);

                       }

                       ProcessStartInfo killProcess = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"busybox kill $(busybox pidof strc)\" | adb shell");
                       Console.WriteLine("echo sh -c \"busybox kill $(busybox pidof strc)\" | adb shell");
                       log.WriteLine("echo sh -c \"busybox kill $(busybox pidof strc)\" | adb shell");
                       killProcess.CreateNoWindow = true;
                       killProcess.UseShellExecute = false;
                       killProcess.RedirectStandardError = true;
                       killProcess.RedirectStandardOutput = true;
                       Process kp = Process.Start(killProcess);
                       Thread.Sleep(3000);

                       if (!kp.HasExited)
                           kp.Kill();

                      
                       ArrayList data = new ArrayList();
                       for (int x = 0; x < 5; x++)
                       {
                           data.Add(PowerParse(paths[x].ToString()));
                           //tw.WriteLine(output);
                       }

                       data.Sort();
                       data.RemoveAt(0);
                       data.RemoveAt(data.Count - 1);
                       

                       //Average
                       float total = 0;
                       float avgData = 0;
                       int size = data.Count;
                       for (int a = 0; a < size; a++)
                       {
                           total += float.Parse(data[a].ToString());
                       }

                       avgData = total / size;
                       tw.WriteLine(avgData + " " + ((50000 - utilize[j]) / 500) + " " + (freq[i] / 1000) + " " + brightness[k]);
                       log.WriteLine(avgData + " " + ((50000 - utilize[j]) / 500) + " " + (freq[i] / 1000) + " " + brightness[k]);
                       //tw.Close();
                       Console.WriteLine("Charing");

                       //check battery
                       ProcessStartInfo battInfo = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \" cat /sys/class/power_supply/battery/capacity > /sdcard/batt_cap.txt \" | adb shell");
                       battInfo.CreateNoWindow = true;
                       battInfo.UseShellExecute = false;
                       battInfo.RedirectStandardError = true;
                       battInfo.RedirectStandardOutput = true;
                       Process battProcess = Process.Start(battInfo);
                       Thread.Sleep(2000);
                       if (!battProcess.HasExited)
                           battProcess.Kill();


                       ProcessStartInfo battpullInfo = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /sdcard/batt_cap.txt C:\\Users\\pok\\batt.txt");
                       battpullInfo.CreateNoWindow = true;
                       battpullInfo.UseShellExecute = false;
                       battpullInfo.RedirectStandardError = true;
                       battpullInfo.RedirectStandardOutput = true;
                       Process battpullProcess = Process.Start(battpullInfo);
                       Thread.Sleep(2000);
                       if (!battpullProcess.HasExited)
                           battpullProcess.Kill();

                       int batt_cap = 0;
                       if (File.Exists("C:\\Users\\pok\\batt.txt"))
                       {
                           var lines = File.ReadAllLines("C:\\Users\\pok\\batt.txt");
                           foreach (var line in lines)
                           {
                               batt_cap = int.Parse(line);
                           }
                       }

                       if (batt_cap <= 25)
                       {
                           Thread.Sleep(40 * 60 * 1000); //15 min
                       }
                       
                       //tw.Close();
                       //if(k==3 || k==7 || k==10)
                       //    Thread.Sleep(40 * 60 * 1000); //15 min
                       /*
                        if (k == 5)
                           Thread.Sleep(90 * 60 * 1000); //15 min

                        if(k == 10)
                           Thread.Sleep(60 * 60 * 1000); //15 min
                      */
                   }
                   
               }
               log.Close();
               tw.Close();
           }

           
        }

        public static double PowerParse(string folder)
        {
            string input = folder + @"\power.pt4";

            FileStream pt4Stream = File.Open(
                                                 input,
                                                  FileMode.Open,
                                                  FileAccess.Read,
                                                  FileShare.ReadWrite
                                              );

            //Console.WriteLine("File source " + args[1]);

            BinaryReader pt4Reader = new BinaryReader(pt4Stream);

            // reader the file header
            PT4.Pt4Header header = new PT4.Pt4Header();

            PT4.ReadHeader(pt4Reader, ref header);

            // read the Status Packet
            PT4.StatusPacket statusPacket = new PT4.StatusPacket();
            PT4.ReadStatusPacket(pt4Reader, ref statusPacket);

            // determine the number of samples in the file
            long sampleCount = PT4.SampleCount(pt4Reader, header.captureDataMask);

            // pre-position input file to the beginning of the sample // data (saves a lot of repositioning in the GetSample // routine)
            pt4Reader.BaseStream.Position = PT4.sampleOffset;
            // process the samples sequentially, beginning to end
            PT4.Sample sample = new PT4.Sample();

           
            double sum = 0;
            double powAvg = 0;
            double count = 0;
            for (long sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                if (sampleIndex < 50000) continue;
                PT4.GetSample(sampleIndex, header.captureDataMask, statusPacket, pt4Reader, ref sample);
                sum += (sample.mainCurrent * 3.7);//sample.mainVoltage);
                count++;
            }

            powAvg = sum / count;
            pt4Reader.Close();
           
            return powAvg;
        }
    }
}
