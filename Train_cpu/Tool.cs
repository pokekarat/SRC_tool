using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Parse;
namespace Train_cpu
{
    class Tool
    {
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
                sum += (sample.mainCurrent);//sample.mainVoltage);
                count++;
            }

            powAvg = sum / count;
            pt4Reader.Close();

            return powAvg;
        }
    }
}
