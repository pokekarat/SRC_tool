using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Parse p = new Parse();
            p.folderName = @"d:\skype\1";
            p.processTrain();
        }

    }
}
