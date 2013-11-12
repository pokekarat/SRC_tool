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
        string folderName = "";

        public Parse(string folderName)
        {
            this.folderName = folderName;
        }

        public static double PowerParse(string folder, int start)
        {
            string input = folder + @"\power.pt4";

            FileStream pt4Stream = File.Open(
                                                 input,
                                                  FileMode.Open,
                                                  FileAccess.Read,
                                                  FileShare.ReadWrite
                                              );


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
            for (long sampleIndex = start; sampleIndex < sampleCount; sampleIndex++)
            {
                PT4.GetSample(sampleIndex, header.captureDataMask, statusPacket, pt4Reader, ref sample);
                sum += (sample.mainCurrent * 3.7);
            }

            pt4Reader.Close();
            
            double avg = sum / ((sampleCount - start) + 1);
            
            return avg;
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

                if (cpu.Contains("cpu0") || cpu.Contains("cpu1") || cpu.Contains("cpu2") || cpu.Contains("cpu3"))
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
            for (int i = 0; i < (num * 4); i++)
            {
                arr2.Add("cpu" + (i % 4));
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

            //TextWriter wrt = new StreamWriter(@"D:\Research\Experiment\nexus_s\case0\App1_candy\test1\util_test.txt");
            double uTime_before = 0;
            double sTime_before = 0;
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

      
        public static bool isMem = false;
        public double[,] mat;
        public void process()
        {
            ArrayList appCpu = new ArrayList();
            //ArrayList utils = TotalCpuUtilParse(this.folderName, @"\cpu_util.txt", @"\cpu_app.txt", ref appCpu);
            
            ArrayList arr2 = manageCPUs(this.folderName, @"\cpu_util.txt");

            ArrayList cpu_ar1 = new ArrayList();
            ArrayList cpu_ar2 = new ArrayList();
            ArrayList cpu_ar3 = new ArrayList();
            ArrayList cpu_ar4 = new ArrayList();

            for (int i = 0; i < arr2.Count; i += 4)
            {
                cpu_ar1.Add(arr2[i]);
                cpu_ar2.Add(arr2[i + 1]);
                cpu_ar3.Add(arr2[i + 2]);
                cpu_ar4.Add(arr2[i + 3]);
            }

            ArrayList cpu1_utils = TotalCpuUtilParse(cpu_ar1);
            ArrayList cpu2_utils = TotalCpuUtilParse(cpu_ar2);
            ArrayList cpu3_utils = TotalCpuUtilParse(cpu_ar3);
            ArrayList cpu4_utils = TotalCpuUtilParse(cpu_ar4);
            ArrayList freqs1 = FreqParse(this.folderName + @"\freq1.txt");
            ArrayList freqs2 = FreqParse(this.folderName + @"\freq2.txt");
            ArrayList mems = MemoryParse(this.folderName + @"\mem_total.txt");
            
            TextWriter tw = new StreamWriter(this.folderName + @"\sample.txt");
            tw.WriteLine("cpu1 cpu2 cpu3 cpu4 freq1 freq2 mem");

            int numSample = cpu1_utils.Count;
            for (int s = 0; s < numSample; s++)
            {
                string dataLine = cpu1_utils[s] + " " + cpu2_utils[s] + " " + cpu3_utils[s] + " " + cpu4_utils[s] + " " + freqs1[s] + " " + freqs2[s] + " " + mems[s]; 
                tw.WriteLine(dataLine);
            }

            tw.Close();
            
            

           

           /* double pref = similarCompute(mat);

            string clusterOutput = this.folderName + @"\tmp";
            apcluster(this.folderName, pref, clusterOutput);

            //cluster1 considers power
            ArrayList cList1 = createCluster(clusterOutput);

            //cluster2 has no consider power_i
            ArrayList cList2 = createCluster(clusterOutput);

            int cList1Size = cList1.Count;
            int cList2Size = cList2.Count;
            */
            
            
            Console.WriteLine("Processing complete");
           // Console.ReadKey();
        }
/*
        private ArrayList createCluster(string clusterPath1)
        {
            string[] idx = File.ReadAllLines(clusterPath1+@"\idx.txt");
          
            ArrayList data = new ArrayList();
            for (int i = 0; i < idx.Length; i++)
            {
                if (String.IsNullOrEmpty(idx[i]))
                {
                    continue;
                }
                data.Add(idx[i]);
            }

            int numSample = data.Count;

            ArrayList cList = new ArrayList();

            bool isFound = false;
            for (int s = 0; s < numSample; s++)
            {
                int exemplar = int.Parse(data[s].ToString());
                if (cList.Count != 0)
                {
                    for (int i = 0; i < cList.Count; i++)
                    {
                        Cluster cTmp = (Cluster)cList[i];
                        if (cTmp.examplar == exemplar)
                        {
                            cTmp.data.Add(s);
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        Cluster c = new Cluster();
                        c.examplar = exemplar;
                        c.data.Add(s);
                        cList.Add(c);
                    }
                }
                else
                {
                    Cluster c = new Cluster();
                    c.examplar = exemplar;
                    c.data.Add(s);
                    cList.Add(c);
                }
                isFound = false;
            }

            return cList;
        }

        public double similarCompute(double[,] dat)
        {
            ArrayList results = new ArrayList();

            int rows = dat.GetLength(0);
            int cols = dat.GetLength(1);

            TextWriter tw = new StreamWriter(this.folderName + @"\Similarities.txt");

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (i == j) continue;

                    double sim = 0;
                    double sum = 0;
                    for (int k = 0; k < cols; k++)
                    {
                        sum += Math.Pow((dat[j,k]-dat[i, k]), 2);
                    }
                    sim = -1 * sum;
                    tw.WriteLine((i + 1) + " " + (j + 1) + " " + sim);
                    results.Add(sim);
                }
            }
            tw.Close();

            //Preferences
            results.Sort();
            double[] d = (double[])results.ToArray(typeof(double));
            return GetMedian(d);
           
        }

        private void apcluster(string path, double pref, string output)
        {

            Process apcluster = new Process();
            apcluster.StartInfo.FileName = path + @"\apcluster";
            apcluster.StartInfo.Arguments = path + @"\Similarities.txt " + pref + " " + output;
            apcluster.Start();
            apcluster.WaitForExit();
        
        }

        public double GetMedian(double[] Value)
        {
            double Median = 0;
            int size = Value.Length;
            int mid = size / 2;
            Median = (size % 2 != 0) ? Value[mid] : (Value[mid] + Value[mid + 1]) / 2;
            return Median;
        }
 */
    }

 /*   public class Cluster
    {
        public int examplar = -1;
        public ArrayList data;

        public Cluster() {
            data = new ArrayList();
        }
    }

    public class SubCluster
    {
        public ArrayList data;
        public SubCluster()
        {
            data = new ArrayList();
        }
    }*/
}