using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInfo
{
    static class Logger
    {
        static Stack<string> log_massiv = new Stack<string>();
        static string logFile = "log.txt";

        static public void add(string str)
        {
            log_massiv.Push(time() + " - " + str);

            write(log_massiv, logFile, Settings.Log);
        }

        private static void write(Stack<string> strs, string file, bool log)
        {
            if (log)
            {
                File.AppendAllLines(file, strs);
                log_massiv.Clear();
            }            
        }

        private static string time()
        {
            return 
                DateTime.Now.Day + "." + 
                DateTime.Now.Month + "." + 
                DateTime.Now.Year + " " + 
                DateTime.Now.Hour + ":" + 
                DateTime.Now.Minute + ":" + 
                DateTime.Now.Second;
        }
    }
}
