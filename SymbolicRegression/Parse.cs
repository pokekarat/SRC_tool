using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SymbolicRegression
{
    class Parse
    {

        ArrayList list = new ArrayList();
        string folderName = "";

        public Parse(string folderName)
        {
            this.folderName = folderName;
        }

        public static ArrayList FreqParse(String folder)
        {
            string input = folder;


            string[] lines = File.ReadAllLines(input);
            ArrayList rets = new ArrayList();

            //int count = 0;
            foreach (string line in lines)
            {
                string lineTrim = line.Trim();
                if (!string.IsNullOrEmpty(lineTrim))
                {
                    double freq = double.Parse(lineTrim) / 1000;

                    if (freq < 1) freq = 0;
                    //freq = Math.Floor(freq);
                    rets.Add(freq);
                }
            }

            Console.WriteLine("Finish: Freq");
            return rets;
        }

        public static ArrayList MemoryParse(String folder)
        {
            string input = folder;
            string[] lines = File.ReadAllLines(input);
            ArrayList rets = new ArrayList();
            ArrayList list = new ArrayList();

            for (int i = 0; i < lines.Length; i++)
            {
                string lineTrim = lines[i].Trim();
                string extStr = Regex.Match(lineTrim, @"\d+").Value;
                list.Add(extStr);

                if (list.Count == 2)
                {
                    double total = Double.Parse(list[0].ToString());
                    double free = Double.Parse(list[1].ToString());
                    double memUsed = ((total - free) / total) * 100;
                    rets.Add(Math.Round(memUsed,2));
                    list.Clear();
                }

            }

            Console.WriteLine("Finish: Memory Parse");
            return rets;
        }

        public static ArrayList TotalCpuUtilParse(String file, String cpu_stat, String cpu_app, ref ArrayList apps)
        {

            ArrayList utils = new ArrayList();

            if (!File.Exists(file + cpu_stat))
            {
                return new ArrayList();
            }
            
            string[] cpu_stats = File.ReadAllLines(file + cpu_stat);
            string[] app_stats = File.ReadAllLines(file + cpu_app);

            ArrayList cpu_ar = new ArrayList();
            ArrayList app_ar = new ArrayList();

            for (int i = 0; i < cpu_stats.Length; i++)
            {
                string cpu = cpu_stats[i].Trim();
                if (!(cpu.Contains("cpu0") || cpu.Contains("cpu1") || cpu == ""))
                {
                    cpu_ar.Add(cpu);
                }
            }

            for (int j = 0; j < app_stats.Length; j++)
            {
                if (string.IsNullOrEmpty(app_stats[j]) || string.IsNullOrWhiteSpace(app_stats[j])) continue;
                app_ar.Add(app_stats[j].Trim());
            }

            ArrayList util_prev = new ArrayList();
            ArrayList util_cur = new ArrayList();
            double[] a = new double[7];
            double[] b = new double[7];
            double total_prev = 0;
            double idle_prev = 0;
            double idle_cur = 0;
            double total_cur = 0;

            //TextWriter wrt = new StreamWriter(@"D:\Research\Experiment\nexus_s\case0\App1_candy\test1\util_test.txt");
            double uTime_before = 0;
            double sTime_before = 0;
            for (int i = 0; i < cpu_ar.Count; i++)
            {

                string[] cpu_current = cpu_ar[i].ToString().Split(' ');

                for (int j = 0; j < cpu_current.Length; j++)
                {
                    if (!string.IsNullOrEmpty(cpu_current[j]) && cpu_current[j] != "cpu")
                        util_cur.Add(cpu_current[j]);
                }

                for (int k = 0; k < 7; k++)
                {
                    b[k] = double.Parse(util_cur[k].ToString());
                    total_cur += b[k];
                }

                idle_cur = b[3];

                double diff_idle = idle_cur - idle_prev;
                double diff_total = total_cur - total_prev;
                double diff_usage = (1000 * (diff_total - diff_idle) / diff_total) / 10;

                string[] appLines = app_ar[i].ToString().Split();
                double app_util = 0;
                if (appLines.Length > 1)
                {
                    double uTime = double.Parse(appLines[13]);
                    double sTime = double.Parse(appLines[14]);
                    double userUtil = 100 * (uTime - uTime_before) / diff_total;
                    double sysUtil = 100 * (sTime - sTime_before) / diff_total;
                    app_util = userUtil + sysUtil;

                    if (app_util > diff_usage)
                    {
                        Console.WriteLine("bug");
                        app_util = diff_usage;
                    }

                    uTime_before = uTime;
                    sTime_before = sTime;

                   
                }
                else
                {
                    app_util = -1;
                }

                apps.Add(app_util);



                utils.Add(Math.Round(diff_usage,2));

                idle_prev = idle_cur;
                total_prev = total_cur;
                total_cur = 0;
                util_cur.Clear();

                //  wrt.WriteLine(diff_usage);
            }

            //wrt.Close();

            return utils;
        }      

      
        public static bool isMem = false;
       
        public void process()
        {
            

            ArrayList appCpu = new ArrayList();
            ArrayList utils = TotalCpuUtilParse(this.folderName, @"\cpu_util.txt", @"\cpu_app.txt", ref appCpu);
            ArrayList freqs = FreqParse(this.folderName + @"\freq.txt");
            ArrayList freqs1 = FreqParse(this.folderName + @"\freq1.txt");
            ArrayList mems = MemoryParse(this.folderName + @"\mem_total.txt");

            Console.WriteLine("Processing complete....");
        }
      
    }


}