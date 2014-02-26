using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Parse;

namespace ProcessSample
{
    public class Config
    {
        public static int DURATION = 100;
        public static int POWEROFFSET = 10; //time after start sampling (seconds)
        public static string ROOTPATH = @"D:\";
        public static string SAVEFOLDER = @"pokopang\";
        public static string SAVETIMES = @"1";
        public static string POWERMETER = "C:\\Program Files (x86)\\Monsoon Solutions Inc\\PowerMonitor\\PowerToolCmd";
        public static string SRC_TOOL_PATH = "C:\\Users\\pok\\Documents\\GitHub\\SRC_tool\\";
        public static string IP_EUREQA_SERVER = "140.113.88.194";

        //Nexus S, Galaxy S4, Fame, S2
        public static string WIFI = @"/sys/class/net/wlan0/";

        //Example app "com.google.android.youtube";
        //com.skype.raider
        public static string APP2TEST = "com.skype.raider";

        public static double VOLT = 3.7;
    }

    class Parse
    {

        ArrayList list = new ArrayList();
        public string folderName = "";
        public static int CPU_cores = 8;

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

                PT4.GetSample(sampleIndex, header.captureDataMask, statusPacket, pt4Reader, ref sample);
                //powerTable[sample.timeStamp] = sample.mainCurrent;
                sum += (sample.mainCurrent * Config.VOLT);
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
                    rets.Add(Math.Round(freq, 2));
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
                    rets.Add(Math.Round(memUsed, 2));
                    list.Clear();
                }

            }

            Console.WriteLine("Finish: Memory Parse");
            return rets;
        }

        public static ArrayList manageCpuApp(String file, String cpu_app_stat)
        {
            ArrayList ret = new ArrayList();
            ArrayList ret2 = new ArrayList();
            int countLine = Parse.CPU_cores;

            string[] cpu_app_stats = File.ReadAllLines(file + cpu_app_stat);

            bool inLoop = false;
            for (int i = 0; i < cpu_app_stats.Length; i++)
            {

                if (cpu_app_stats[i].Contains("+++"))
                {
                    inLoop = true;
                    continue;
                }
                
                if (cpu_app_stats[i].Trim().Equals("")) 
                {
                    inLoop = false;

                    //check cpu num match
                    int len1 = ret.Count;
                    HashSet<string> cpuSets = new HashSet<string>();
                    for (int b = 0; b < len1; b++)
                    {
                        string[] str = ret[b].ToString().Split(' ');
                        string cpuNum = str[38];
                        cpuSets.Add(cpuNum);
                    }

                    Dictionary<string, int[]> match = new Dictionary<string, int[]>();
                    //initial match
                    for (int c = 0; c < cpuSets.Count; c++)
                    {
                        match[cpuSets.ElementAt<string>(c)] = new int[] { 0, 0 };
                    }


                    for (int c = 0; c < cpuSets.Count; c++)
                    {
                        string cpuType = cpuSets.ElementAt<string>(c);

                        for (int d = 0; d < ret.Count; d++)
                        {
                            string[] str = ret[d].ToString().Split(' ');
                            string cpuNum = str[38];
                            if (cpuNum.Equals(cpuType))
                            {
                                int[] s = new int[2];
                                s[0] = int.Parse(str[13]);
                                s[1] = int.Parse(str[14]);
                                match[cpuType][0] += s[0];
                                match[cpuType][1] += s[1];
                            }
                        }
                    }


                    for (int a = 0; a < 8; a++)
                    {
                        if (match.ContainsKey(a.ToString()))
                        {
                            string s = match[a.ToString()][0] + " " + match[a.ToString()][1];
                           /* if (s.Contains('\t'))
                            {
                                s.Split('\t');               
                            } */

                            ret2.Add(s);
                        }
                        else
                        {
                            ret2.Add("0");
                        }
                    }

                    ret.Clear();

                    continue;
                }

                if (inLoop)
                {
                    ret.Add(cpu_app_stats[i]);
                }
            }

            return ret2;
        }

        public static ArrayList manageCPUs(String file, String cpu_stat)
        {


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
            for (int i = 0; i < (num * Parse.CPU_cores); i++)
            {
                arr2.Add("cpu" + (i % Parse.CPU_cores));
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

        public static ArrayList TotalCpuUtilParse(ArrayList cpu, ArrayList app, ref ArrayList app_utils)
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
            double uTime_before = 0;
            double sTime_before = 0;

            for (int i = 0; i < cpu.Count; i++)
            {
                if (i == 45)
                    Console.WriteLine("test");

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


                /******** app **********/

                string[] appLines = app[i].ToString().Split();
                double app_util = 0;
                if (appLines.Length > 1)
                {
                    double uTime = double.Parse(appLines[0]);
                    double sTime = double.Parse(appLines[1]);
                    double userUtil = 100 * (uTime - uTime_before) / diff_total;
                    double sysUtil = 100 * (sTime - sTime_before) / diff_total;

                    app_util = userUtil + sysUtil;

                    if (app_util < 0)
                    {
                        app_util = 0;
                        //Console.WriteLine("test");
                    }

                    if (app_util > diff_usage)
                    {
                        //Console.WriteLine("bug");
                        app_util = diff_usage;
                    }

                    uTime_before = uTime;
                    sTime_before = sTime;


                }
                else
                {
                    app_util = 0;
                }

            

                app_utils.Add(Math.Round(app_util, 2));

                /************************/

                utils.Add(Math.Round(diff_usage, 2));

                idle_prev = idle_cur;
                total_prev = total_cur;
                total_cur = 0;
                util_cur.Clear();
            }

            return utils;
        }

        public static ArrayList wifiParseApp(String folder)
        {
            string file = folder;
            //string rx = folder + @"\wifi_rx.txt";

            string[] data = File.ReadAllLines(file);
            ArrayList cleanData = new ArrayList();
            //Clean data
            for (int d = 0; d < data.Length; d++)
            {
                if (String.IsNullOrWhiteSpace(data[d]) || String.IsNullOrEmpty(data[d])) continue;
                cleanData.Add(data[d]);
            }


            ArrayList rets = new ArrayList();

            string currIndex = ""; // cleanData[0].ToString();
            string nextIndex = ""; // cleanData[1].ToString();
            for (int i = 0; i < cleanData.Count - 1; i++)
            {
                currIndex = cleanData[i].ToString();
                nextIndex = cleanData[i + 1].ToString();

                if (currIndex.Contains("+++"))
                {
                    if (nextIndex.Contains("+++"))
                    {
                        rets.Add("0");
                    }
                    else
                    {
                        rets.Add(nextIndex);
                    }
                }
            }

            ArrayList rets2 = new ArrayList();

            /*for (int j = 0; j < rets.Count; j++)
            {
                rets2.Add(int.Parse(rets[j].ToString()) / 1000);
            }*/

            int prev = int.Parse(rets[0].ToString());

            for (int i = 1; i < rets.Count; i++)
            {
                int curr = int.Parse(rets[i].ToString());

                int deltaByte = 0;
                
                if(curr >= prev)
                   deltaByte = curr - prev;

                rets2.Add(deltaByte);
                prev = curr;
            }

            return rets2;
        }

        public static ArrayList wifiParse(String folder)
        {
            string file = folder;
            //string rx = folder + @"\wifi_rx.txt";

            string[] data = File.ReadAllLines(file);
            ArrayList rets = new ArrayList();

            int prev = int.Parse(data[0]);

            for (int i = 1; i < data.Length; i++)
            {
                if (string.IsNullOrEmpty(data[i]) || string.IsNullOrWhiteSpace(data[i])) continue;

                int curr = int.Parse(data[i]);
                int deltaByte = 0;

                if (curr >= prev)
                    deltaByte = curr - prev;
                rets.Add(deltaByte);
                prev = curr;
            }

            return rets;
        }

        public static ArrayList appFreq(ArrayList freqs, ArrayList app_utils)
        {
            ArrayList rets = new ArrayList();
            for (int i = 0; i < freqs.Count; i++)
            {
                double fVal = double.Parse(freqs[i].ToString());
                double aUtil = double.Parse(app_utils[i].ToString());
                rets.Add(Math.Round(fVal * (aUtil / 100), 2));
            }
            return rets;
        }

        public static ArrayList estimateAppWifiPk(ArrayList pk, ArrayList size, ArrayList appSize)
        {
            int size1 = appSize.Count;

            ArrayList ret = new ArrayList();

            for (int i = 0; i < size1; i++)
            {
               

                float numPk = float.Parse(pk[i].ToString());
                float numByte = float.Parse(size[i].ToString());
                float appNumByte = float.Parse(appSize[i].ToString());

                if (numByte == 0)
                {
                    ret.Add(0);
                    continue;
                }

                float val = (numPk / numByte) * appNumByte;
                ret.Add((int)val);
            }

            
            return ret;
        }

        public void processTrain()
        {
            //ArrayList appCpu = new ArrayList();
            //ArrayList utils = TotalCpuUtilParse(this.folderName, @"\cpu_util.txt", @"\cpu_app.txt", ref appCpu);

            ArrayList arr = manageCPUs(this.folderName, @"\cpu_util.txt");
            ArrayList arr_app = manageCpuApp(this.folderName, @"\cpu_app.txt");

            ArrayList cpu_ar1 = new ArrayList();
            ArrayList cpu_ar2 = new ArrayList();
            ArrayList cpu_ar3 = new ArrayList();
            ArrayList cpu_ar4 = new ArrayList();
            ArrayList cpu_ar5 = new ArrayList();
            ArrayList cpu_ar6 = new ArrayList();
            ArrayList cpu_ar7 = new ArrayList();
            ArrayList cpu_ar8 = new ArrayList();

            ArrayList cpu_app_ar1 = new ArrayList();
            ArrayList cpu_app_ar2 = new ArrayList();
            ArrayList cpu_app_ar3 = new ArrayList();
            ArrayList cpu_app_ar4 = new ArrayList();
            ArrayList cpu_app_ar5 = new ArrayList();
            ArrayList cpu_app_ar6 = new ArrayList();
            ArrayList cpu_app_ar7 = new ArrayList();
            ArrayList cpu_app_ar8 = new ArrayList();

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

                cpu_app_ar1.Add(arr_app[i]);
                cpu_app_ar2.Add(arr_app[i + 1]);
                cpu_app_ar3.Add(arr_app[i + 2]);
                cpu_app_ar4.Add(arr_app[i + 3]);
                cpu_app_ar5.Add(arr_app[i + 4]);
                cpu_app_ar6.Add(arr_app[i + 5]);
                cpu_app_ar7.Add(arr_app[i + 6]);
                cpu_app_ar8.Add(arr_app[i + 7]);
            }

            ArrayList cpu1_app_utils = new ArrayList();
            ArrayList cpu2_app_utils = new ArrayList();
            ArrayList cpu3_app_utils = new ArrayList();
            ArrayList cpu4_app_utils = new ArrayList();
            ArrayList cpu5_app_utils = new ArrayList();
            ArrayList cpu6_app_utils = new ArrayList();
            ArrayList cpu7_app_utils = new ArrayList();
            ArrayList cpu8_app_utils = new ArrayList();

            ArrayList cpu1_utils = TotalCpuUtilParse(cpu_ar1, cpu_app_ar1, ref cpu1_app_utils);
            ArrayList cpu2_utils = TotalCpuUtilParse(cpu_ar2, cpu_app_ar2, ref cpu2_app_utils);
            ArrayList cpu3_utils = TotalCpuUtilParse(cpu_ar3, cpu_app_ar3, ref cpu3_app_utils);
            ArrayList cpu4_utils = TotalCpuUtilParse(cpu_ar4, cpu_app_ar4, ref cpu4_app_utils);
            ArrayList cpu5_utils = TotalCpuUtilParse(cpu_ar5, cpu_app_ar5, ref cpu5_app_utils);
            ArrayList cpu6_utils = TotalCpuUtilParse(cpu_ar6, cpu_app_ar6, ref cpu6_app_utils);
            ArrayList cpu7_utils = TotalCpuUtilParse(cpu_ar7, cpu_app_ar7, ref cpu7_app_utils);
            ArrayList cpu8_utils = TotalCpuUtilParse(cpu_ar8, cpu_app_ar8, ref cpu8_app_utils);


            ArrayList freqs1 = FreqParse(this.folderName + @"\freq0.txt");
            ArrayList freqs2 = FreqParse(this.folderName + @"\freq1.txt");
            ArrayList freqs3 = FreqParse(this.folderName + @"\freq2.txt");
            ArrayList freqs4 = FreqParse(this.folderName + @"\freq3.txt");
            ArrayList freqs5 = FreqParse(this.folderName + @"\freq4.txt");
            ArrayList freqs6 = FreqParse(this.folderName + @"\freq5.txt");
            ArrayList freqs7 = FreqParse(this.folderName + @"\freq6.txt");
            ArrayList freqs8 = FreqParse(this.folderName + @"\freq7.txt");

            ArrayList appFreq1 = appFreq(freqs1, cpu1_app_utils);
            ArrayList appFreq2 = appFreq(freqs2, cpu2_app_utils);
            ArrayList appFreq3 = appFreq(freqs3, cpu3_app_utils);
            ArrayList appFreq4 = appFreq(freqs4, cpu4_app_utils);
            ArrayList appFreq5 = appFreq(freqs5, cpu5_app_utils);
            ArrayList appFreq6 = appFreq(freqs6, cpu6_app_utils);
            ArrayList appFreq7 = appFreq(freqs7, cpu7_app_utils);
            ArrayList appFreq8 = appFreq(freqs8, cpu8_app_utils);

            //ArrayList mems = MemoryParse(this.folderName + @"\mem_total.txt");

            ArrayList rx_pks = wifiParse(this.folderName + @"\wifi_rx_pk.txt");
            ArrayList rx_bytes = wifiParse(this.folderName + @"\wifi_rx_byte.txt");
            ArrayList tx_pks = wifiParse(this.folderName + @"\wifi_tx_pk.txt");
            ArrayList tx_bytes = wifiParse(this.folderName + @"\wifi_tx_byte.txt");

            ArrayList uid_rcvs = wifiParseApp(this.folderName + @"\uid_rcv.txt");
            ArrayList uid_rcvs_pks = estimateAppWifiPk(rx_pks, rx_bytes, uid_rcvs);
            
            ArrayList uid_snds = wifiParseApp(this.folderName + @"\uid_snd.txt");
            ArrayList uid_snds_pks = estimateAppWifiPk(tx_pks, tx_bytes, uid_snds);

            ArrayList powers = PowerParse(this.folderName);
            /*ArrayList powers = new ArrayList(cpu1_utils.Count);//PowerParse(this.folderName);
            for (int i = 0; i < powers.Count; i++)
                powers[i] = 0;
            */

            TextWriter tw = new StreamWriter(this.folderName + @"\sample.txt");
            //tw.WriteLine("cpu1 cpu2 cpu3 cpu4 cpu5 cpu6 cpu7 cpu8 freq1 freq2 freq3 freq4 freq5 freq6 freq7 freq8 bright rx_pk rx_byte tx_pk tx_byte power");
            tw.WriteLine("cpu1 cpu2 cpu3 cpu4 cpu5 cpu6 cpu7 cpu8 freq1 freq2 freq3 freq4 freq5 freq6 freq7 freq8 bright rx_pk tx_pk power");

            int numSample = uid_rcvs.Count;

            //Use Config
            int matchTimeAndPower = Config.POWEROFFSET;

            for (int s = matchTimeAndPower; s < numSample; s++)
            {
                string dataLine = cpu1_utils[s] + " " + cpu2_utils[s] + " " + cpu3_utils[s] + " " + cpu4_utils[s] + " " + cpu5_utils[s] + " " + cpu6_utils[s] + " " + cpu7_utils[s] + " " + cpu8_utils[s] + " "
                                  + freqs1[s] + " " + freqs2[s] + " " + freqs3[s] + " " + freqs4[s] + " " + freqs5[s] + " " + freqs6[s] + " " + freqs7[s] + " " + freqs8[s] + " "
                                  + " " + "255" + " " + rx_pks[s] + " " + tx_pks[s] + " " + powers[(s - matchTimeAndPower)+3];

                tw.WriteLine(dataLine);
            }

            tw.Close();

            TextWriter tw2 = new StreamWriter(this.folderName + @"\test.txt");
            tw2.WriteLine("cpu1 cpu2 cpu3 cpu4 cpu5 cpu6 cpu7 cpu8 freq1 freq2 freq3 freq4 freq5 freq6 freq7 freq8 bright rx_pk tx_pk");

            for (int t = 0; t < numSample; t++)
            {
                tw2.WriteLine(
                        cpu1_app_utils[t] + " " +
                        cpu2_app_utils[t] + " " +
                        cpu3_app_utils[t] + " " +
                        cpu4_app_utils[t] + " " +
                        cpu5_app_utils[t] + " " +
                        cpu6_app_utils[t] + " " +
                        cpu7_app_utils[t] + " " +
                        cpu8_app_utils[t] + " " +
                        appFreq1[t] + " " +
                        appFreq2[t] + " " +
                        appFreq3[t] + " " +
                        appFreq4[t] + " " +
                        appFreq5[t] + " " +
                        appFreq6[t] + " " +
                        appFreq7[t] + " " +
                        appFreq8[t] + " " +
                        255 + " " +
                        uid_rcvs_pks[t] + " " +
                        uid_snds_pks[t]
                    );
            }

            tw2.Close();

            Console.WriteLine("Processing complete");
            //Console.ReadKey();
        }
    }
}