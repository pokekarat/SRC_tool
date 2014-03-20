using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Train_cpu
{
    public class Train_system
    {

        public Train_system()
        {}

        public void execute()
        {
           
            int[] freq = { 100000, 200000, 400000, 800000, 1000000}; //245760, 320000, 480000, 800000
        
            ArrayList measures = new ArrayList();
            TextWriter tw;
            String path = @"C:\Users\pok\Research\Experiment\Dropbox\Project1_SRC\Experiment";
            int delayAfterMonsoon = 10000;

            for (int f = 0; f < freq.Length; f++)
            {
                
                for (int u = 0; u < 100; u+=10)
                {

                    for (int i = 1; i <= 5; i++)
                    {
                        tw = new StreamWriter(path + @"\freq_" + freq[f] + "_util_" + u + "_"+i+".txt");

                        ProcessStartInfo cpuInfo = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"./data/strc " + u + " " + freq[f] + " &\" | adb shell");
                        cpuInfo.CreateNoWindow = true;
                        cpuInfo.UseShellExecute = false;
                        cpuInfo.RedirectStandardError = true;
                        cpuInfo.RedirectStandardOutput = true;
                        Process process = Process.Start(cpuInfo);
                        Thread.Sleep(5000);
                        if (!process.HasExited)
                            process.Kill();

                        //Call Monsoon.
                        Process powerMonitor = new Process();
                        powerMonitor.StartInfo.FileName = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";
                        powerMonitor.StartInfo.Arguments = "/USBPASSTHROUGH=AUTO /VOUT=3.70 /KEEPPOWER /NOEXITWAIT /SAVEFILE=" + path + "\\power.pt4  /TRIGGER=DTXD60A"; //DTYD60A
                        powerMonitor.Start();
                        powerMonitor.WaitForExit();
                        Thread.Sleep(delayAfterMonsoon);

                        Console.WriteLine("End loops");
                    }
                }

                checkBattStatus(path);
            }
                       
            /* 
             
            ProcessStartInfo killProcess = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \"busybox kill $(busybox pidof strc)\" | adb shell");
            Console.WriteLine("echo sh -c \"busybox kill $(busybox pidof strc)\" | adb shell");
            killProcess.CreateNoWindow = true;
            killProcess.UseShellExecute = false;
            killProcess.RedirectStandardError = true;
            killProcess.RedirectStandardOutput = true;
            Process kp = Process.Start(killProcess);
            Thread.Sleep(3000);

            if (!kp.HasExited)
                kp.Kill();
 
            ArrayList data = new ArrayList();
            for (int x = 0; x < 3; x++)
            {
                data.Add(Tool.PowerParse(paths[x].ToString()));
                //tw.WriteLine(output);
            }

            //Average
            float total = 0;
            float avgData = 0;
            int size = data.Count;
            for (int a = 0; a < size; a++)
            {
                Console.WriteLine(data[a].ToString());
                total += float.Parse(data[a].ToString());
                log.WriteLine(a + " = " + data[a].ToString());
            }

            avgData = total / size;
            Console.WriteLine("avg " + avgData);
            tw.WriteLine(avgData + " " + ((50000 - utilize[u]) / 500) + " " + (freq[f] / 1000) + " " + brightness[b]);
            log.WriteLine("Average = " + avgData); // + " " + ((50000 - utilize[j]) / 500) + " " + (freq[i] / 1000) + " " + brightness[k]);
            
            */
        }


        public void checkBattStatus(string path)
        {

            //check battery
            ProcessStartInfo battInfo = new ProcessStartInfo("cmd.exe", "/c " + "echo sh -c \" cat /sys/class/power_supply/battery/capacity > /data/local/tmp/batt_cap.txt \" | adb shell");
            battInfo.CreateNoWindow = true;
            battInfo.UseShellExecute = false;
            battInfo.RedirectStandardError = true;
            battInfo.RedirectStandardOutput = true;
            Process battProcess = Process.Start(battInfo);
            Thread.Sleep(2000);
            if (!battProcess.HasExited)
                battProcess.Kill();

            ProcessStartInfo battpullInfo = new ProcessStartInfo("cmd.exe", "/c " + "adb pull /data/local/tmp/batt_cap.txt "+path+"\\batt.txt");
            battpullInfo.CreateNoWindow = true;
            battpullInfo.UseShellExecute = false;
            battpullInfo.RedirectStandardError = true;
            battpullInfo.RedirectStandardOutput = true;
            Process battpullProcess = Process.Start(battpullInfo);
            Thread.Sleep(2000);
            if (!battpullProcess.HasExited)
                battpullProcess.Kill();

            int batt_cap = 0;
            if (File.Exists(path + "\\batt.txt"))
            {
                var lines = File.ReadAllLines(path + "\\batt.txt");
                foreach (var line in lines)
                {
                    batt_cap = int.Parse(line);
                }
            }

            if (batt_cap <= 20)
            {
                Console.WriteLine("Charing");
                Thread.Sleep(3 * 60 * 1000); //3 hours
            }
            Console.WriteLine("Full charge");
        }
    }
}
