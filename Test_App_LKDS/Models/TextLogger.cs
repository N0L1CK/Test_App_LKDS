using System;
using System.IO;


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
