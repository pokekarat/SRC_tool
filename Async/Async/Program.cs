using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Async
{
    class Program
    {
        //Before run this program, you have to success install R step by step as follow:
        //1. Install R 2.15.3 from this website http://cran.r-project.org/bin/windows/base/old/2.15.3/
        //2. Set path to the bin folder of R 2.15.3 in order to let R program visible to every path
        //3. Test if path is set correctly by run RScript in any path in cmd
        
        static void Main(string[] args)
        {
            string path2r = @"C:\Users\USER\Documents\GitHub\SRC_tool\asynComponent.r";
            string path2sample = @"D:\candycrush\1\"; //sample var points to "sample.txt" in folder 3.
            string parameters = path2r+" "+path2sample;

            Process asynProc = new Process();
            asynProc.StartInfo.FileName = "Rscript";
            asynProc.StartInfo.Arguments = parameters;
            asynProc.Start();
            asynProc.WaitForExit();

           // Console.ReadKey();
        }
    }
}
