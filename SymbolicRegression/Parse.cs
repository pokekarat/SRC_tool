using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Parse;

namespace SymbolicRegression
{
    class Parse
    {

        ArrayList list = new ArrayList();
        public string folderName = "";
    
        public Parse() { }

        public static ArrayList PowerParse(string folder)
        {
            string input = folder + @"\power.pt4";

            ArrayList rets = new ArrayList();

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

            //TextWriter tw = new StreamWriter(folder + @"\power.csv");
            //Dictionary<double, double> powerTable = new Dictionary<double, double>();

            double sum = 0;
            double powAvg = 0;
            for (long sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                /*if (!isWifi)
                {
                    if (sampleIndex < (16 * 5000)) 
                        continue;
                }*/

                PT4.GetSample(sampleIndex, header.captureDataMask, statusPacket, pt4Reader, ref sample);
                //powerTable[sample.timeStamp] = sample.mainCurrent;
                sum += (sample.mainCurrent * 3.7); // * 3.7);//sample.mainVoltage);
                int sampleRate = 5000;
                if (sampleIndex % sampleRate == 0) // 5000 HZ : calculate every 1 second 50 for 100hz
                {

                    powAvg = sum / sampleRate;
                    rets.Add(Math.Round(powAvg, 2));
                    powAvg = 0;
                    sum = 0;
                }
            }

            // tw.Close();
            pt4Reader.Close();
            Console.WriteLine("Finish: Power");
            return rets;
        }

        public static ArrayList FreqParse(String input)
        {

            ArrayList rets = new ArrayList();

            if (!File.Exists(input))
                return rets;
            
            string[] lines = File.ReadAllLines(input);

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

        public static ArrayList manageCPUs(String file, String cpu_stat)
        {
            int core = 8;

            ArrayList utils = new ArrayList();

            if (!File.Exists(file + cpu_stat))
            {
                return new ArrayList();
            }

            string[] cpu_stats = File.ReadAllLines(file + cpu_stat);


            ArrayList app_ar = new ArrayList();
            ArrayList cleanCPUs = new ArrayList();

            //Clean not cpu
            for (int i = 0; i < cpu_stats.Length; i++)
            {
                string cpu = cpu_stats[i].Trim();

                if (cpu.Contains("cpu0") || cpu.Contains("cpu1") || cpu.Contains("cpu2") || cpu.Contains("cpu3") || cpu.Contains("cpu4") || cpu.Contains("cpu5") || cpu.Contains("cpu6") || cpu.Contains("cpu7"))
                {
                    cleanCPUs.Add(cpu);
                }
            }

            for (int t = 0; t < cleanCPUs.Count; t++)
            {
                string line = cleanCPUs[t].ToString();

            }

            //////////// add code //////////////////

            int num = 0;
            for (int i = 0; i < cleanCPUs.Count; i++)
            {
                if (cleanCPUs[i].ToString().Contains("cpu0")) ++num;
            }

            ArrayList indexs = new ArrayList();
            for (int j = 0; j < cleanCPUs.Count; j++)
            {
                string line = cleanCPUs[j].ToString();
                string[] lines = line.Split(' ');
                indexs.Add(lines[0]);
            }


            ArrayList arr2 = new ArrayList();
            for (int i = 0; i < (num * core); i++)
            {
                arr2.Add("cpu" + (i % core));
            }

            int x = 0;
            for (int i = 0; i < indexs.Count; i++)
            {
                if (!indexs[i].ToString().Equals(arr2[x]))
                {
                    do
                    {

                        arr2[x] = "";
                        ++x;

                    } while (!indexs[i].ToString().Equals(arr2[x]));
                }

                arr2[x] = cleanCPUs[i];
                ++x;

                if (i == (indexs.Count - 1))
                {
                    while (x < arr2.Count)
                    {
                        arr2[x] = "";
                        ++x;
                    }
                }
            }

            //////////// end code ///////////////////

            return arr2;
        }

        public static ArrayList TotalCpuUtilParse(ArrayList cpu)
        {
            ArrayList utils = new ArrayList();

            /// Compute utilization ///
            ArrayList util_prev = new ArrayList();
            ArrayList util_cur = new ArrayList();
            double[] a = new double[7];
            double[] b = new double[7];
            double total_prev = 0;
            double idle_prev = 0;
            double idle_cur = 0;
            double total_cur = 0;

           
           // double uTime_before = 0;
           // double sTime_before = 0;
            for (int i = 0; i < cpu.Count; i++)
            {

                string[] cpu_current = cpu[i].ToString().Split(' ');

                if (cpu_current.Length != 1)
                {
                    for (int j = 0; j < cpu_current.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(cpu_current[j]) && !cpu_current[j].Contains("cpu"))
                            util_cur.Add(cpu_current[j]);

                    }

                    for (int k = 0; k < 7; k++)
                    {
                        b[k] = double.Parse(util_cur[k].ToString());
                        total_cur += b[k];
                    }
                }
                else
                {
                    for (int k = 0; k < 7; k++)
                    {
                        b[k] = 0;
                        total_cur += b[k];
                    }
                }

             

                idle_cur = b[3];

                double diff_idle = idle_cur - idle_prev;
                double diff_total = total_cur - total_prev;
                double diff_usage = (1000 * (diff_total - diff_idle) / diff_total) / 10;

                if (double.IsNaN(diff_usage)) diff_usage = 0;

                utils.Add(Math.Round(diff_usage,2));

                idle_prev = idle_cur;
                total_prev = total_cur;
                total_cur = 0;
                util_cur.Clear();
            }

            return utils;
        }

        public static ArrayList utilParse(String file, String cpu_stat, String cpu_app, ref ArrayList apps)
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



                utils.Add(diff_usage);

                idle_prev = idle_cur;
                total_prev = total_cur;
                total_cur = 0;
                util_cur.Clear();

                //  wrt.WriteLine(diff_usage);
            }

            //wrt.Close();

            return utils;
        }

        public static ArrayList wifiParse(String folder)
        {
            string file = folder;
            //string rx = folder + @"\wifi_rx.txt";

            string[] data = File.ReadAllLines(file);
            ArrayList rets = new ArrayList();

            int prev = int.Parse(data[0]);

            for (int i = 0; i < data.Length; i++)
            {
                if (string.IsNullOrEmpty(data[i]) || string.IsNullOrWhiteSpace(data[i])) continue; 

                int curr = int.Parse(data[i]);
                rets.Add(curr - prev);
                prev = curr;
            }

            
            return rets;
        }
      
        public void processTrain()
        {
           // ArrayList appCpu = new ArrayList();
            //ArrayList utils = TotalCpuUtilParse(this.folderName, @"\cpu_util.txt", @"\cpu_app.txt", ref appCpu);
            
            ArrayList arr = manageCPUs(this.folderName, @"\cpu_util.txt");

            ArrayList cpu_ar1 = new ArrayList();
            ArrayList cpu_ar2 = new ArrayList();
            ArrayList cpu_ar3 = new ArrayList();
            ArrayList cpu_ar4 = new ArrayList();
            ArrayList cpu_ar5 = new ArrayList();
            ArrayList cpu_ar6 = new ArrayList();
            ArrayList cpu_ar7 = new ArrayList();
            ArrayList cpu_ar8 = new ArrayList();

            for (int i = 0; i < arr.Count; i += 8)
            {
                cpu_ar1.Add(arr[i]);
                cpu_ar2.Add(arr[i + 1]);
                cpu_ar3.Add(arr[i + 2]);
                cpu_ar4.Add(arr[i + 3]);
                cpu_ar5.Add(arr[i + 4]);
                cpu_ar6.Add(arr[i + 5]);
                cpu_ar7.Add(arr[i + 6]);
                cpu_ar8.Add(arr[i + 7]);
            }

            ArrayList cpu1_utils = TotalCpuUtilParse(cpu_ar1);
            ArrayList cpu2_utils = TotalCpuUtilParse(cpu_ar2);
            ArrayList cpu3_utils = TotalCpuUtilParse(cpu_ar3);
            ArrayList cpu4_utils = TotalCpuUtilParse(cpu_ar4);
            ArrayList cpu5_utils = TotalCpuUtilParse(cpu_ar4);
            ArrayList cpu6_utils = TotalCpuUtilParse(cpu_ar4);
            ArrayList cpu7_utils = TotalCpuUtilParse(cpu_ar4);
            ArrayList cpu8_utils = TotalCpuUtilParse(cpu_ar4);

            ArrayList freqs1 = FreqParse(this.folderName + @"\freq0.txt");
            ArrayList freqs2 = FreqParse(this.folderName + @"\freq1.txt");
            ArrayList freqs3 = FreqParse(this.folderName + @"\freq2.txt");
            ArrayList freqs4 = FreqParse(this.folderName + @"\freq3.txt");
            ArrayList freqs5 = FreqParse(this.folderName + @"\freq4.txt");
            ArrayList freqs6 = FreqParse(this.folderName + @"\freq5.txt");
            ArrayList freqs7 = FreqParse(this.folderName + @"\freq6.txt");
            ArrayList freqs8 = FreqParse(this.folderName + @"\freq7.txt");

            ArrayList mems = MemoryParse(this.folderName + @"\mem_total.txt");

            ArrayList rx_pks = wifiParse(this.folderName + @"\wifi_rx_pk.txt");
            ArrayList rx_bytes = wifiParse(this.folderName + @"\wifi_rx_byte.txt");
            ArrayList tx_pks = wifiParse(this.folderName + @"\wifi_tx_pk.txt");
            ArrayList tx_bytes = wifiParse(this.folderName + @"\wifi_tx_byte.txt");

            ArrayList powers = PowerParse(this.folderName);
            
            TextWriter tw = new StreamWriter(this.folderName + @"\sample.txt");
            tw.WriteLine("cpu1 cpu2 cpu3 cpu4 cpu5 cpu6 cpu7 cpu8 freq1 freq2 freq3 freq4 freq5 freq6 freq7 freq8 mem bright rx_pk rx_byte tx_pk tx_byte power");

            int numSample = cpu1_utils.Count;

            int matchTimeAndPower = 10;
            
            for (int s = matchTimeAndPower; s < numSample; s++)
            {
                string dataLine = cpu1_utils[s] + " " + cpu2_utils[s] + " " + cpu3_utils[s] + " " + cpu4_utils[s] + " " + cpu5_utils[s] + " " + cpu6_utils[s] + " " + cpu7_utils[s] + " " + cpu8_utils[s] + " "
                                  + freqs1[s] + " " + freqs2[s] + " " + freqs3[s] + " " + freqs4[s] + " " + freqs5[s] + " " + freqs6[s] + " " + freqs7[s] + " " + freqs8[s] + " "
                                  + mems[s] + " " + "255" + " "
                                  + rx_pks[s] + " " + rx_bytes[s] + " " + tx_pks[s] + " " + tx_bytes[s] + " "
                                  + powers[(s - matchTimeAndPower)+2]; // we skip first two sampling power as it causes by start monsoon.

                tw.WriteLine(dataLine);
            }

            tw.Close();
            
            Console.WriteLine("Processing complete");
            Console.ReadKey();
        }


        public void processTest()
        {
            ArrayList uid_rcvs = wifiParse(this.folderName + @"\uid_rcv.txt");
            ArrayList uid_snds = wifiParse(this.folderName + @"\uid_snd.txt");
        }
    }
}