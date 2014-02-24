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

            p.folderName = @"d:\pokopang\1";
            /*p.folderName = @"c:\ebl\skype2\1";
            p.folderName = @"c:\ebl\skype2\1";
            p.folderName = @"c:\ebl\skype2\1";*/
            p.processTrain();
        }

    }
}
