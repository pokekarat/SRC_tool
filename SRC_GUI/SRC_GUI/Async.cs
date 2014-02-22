using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace AsyncProject
{
    class Async
    {
		public int status;
		private string _RScriptSRC;
		private string _WorkingDir;
		private string _SampleNumber;
		
        //Before run this program, you have to success install R step by step as follow:
        //1. Install R 2.15.3 from this website http://cran.r-project.org/bin/windows/base/old/2.15.3/
        //2. Set path to the bin folder of R 2.15.3 in order to let R program visible to every path
        //3. Test if path is set correctly by run RScript in any path in cmd

        public Async(string RScriptSRC, string WorkingDir, string SampleNumber)
        {
			status = -1;
			
			_RScriptSRC = RScriptSRC;
			_WorkingDir = WorkingDir;
			_SampleNumber = SampleNumber;
			status = StartProcessing();
        }

        private int StartProcessing()
        {
            // string pathToRScript = Config.PROJECTROOT + @"\asynComponent.r";
            // string pathToSample = Config.WORKINGDIR + @"\" + Config.MODLEFOLDER + @"\"; //sample var points to "sample.txt" in folder 3.
            string parameters = _RScriptSRC + @" " + _WorkingDir + @"\" + _SampleNumber + @"\";
            Console.WriteLine(parameters);

            if (!File.Exists(_RScriptSRC))
            {
                throw(new FileNotFoundException("File not found : " + _RScriptSRC));
            }

            if (!Directory.Exists(_WorkingDir))
            {
                throw(new DirectoryNotFoundException("Directory not exists : " + _WorkingDir));
            }

            Process asynProc = new Process();
            asynProc.StartInfo.FileName = "Rscript";
            asynProc.StartInfo.Arguments = parameters;
            asynProc.Start();
            asynProc.WaitForExit();
            //Console.WriteLine(@"RScript Processing Status : " + asynProc.ExitCode);

            return asynProc.ExitCode;
		}
    }
}
