using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_App_LKDS
{
    public class TextLogger : ILogger
    {
        string Path { get; set; }
        public TextLogger(string path) 
        {
            Path = path;

        }
        public void Log(string eventName)
        {
            using (StreamWriter logger = new StreamWriter(Path, true))
            {
                logger.WriteLine(DateTime.Now.ToLongTimeString() + " - " + eventName);
                
            }
        }
    }
}
