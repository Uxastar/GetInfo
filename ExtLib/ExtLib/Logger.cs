using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExtLib
{

    public interface ILogger
    {
        string FileName { get; set; }
        bool Enable { get; set; }

        void write(string str_log);
        
    }

    public class Logger : ILogger
    {
        Stack<string> Massiv = new Stack<string>();
        public string FileName { get; set; }
        public bool Enable { get; set; }

        public Logger(string fileName, bool enable)
        {
            this.FileName = fileName;
            this.Enable = enable;
        }

        public void write(string str_log)
        {
            this.Massiv.Push(Time() + "     " + str_log);
            if (Enable)
            {
                File.AppendAllLines(this.FileName, this.Massiv);
                this.Massiv.Clear();
            }                
        }

        private static string Time()
        {
            return DateTime.Now.ToString();
        }
        

    }

    //static class Logger
    //{
    //    static Stack<string> log_massiv = new Stack<string>();
    //    static string logFile = "log.txt";

    //    static public void add(string str)
    //    {
            
    //        log_massiv.Push(time() + " - " + str);

    //        write(log_massiv, logFile, Settings.Log);
    //    }

    //    private static void write(Stack<string> strs, string file, bool log)
    //    {
    //        if (log)
    //        {
    //            File.AppendAllLines(file, strs);
    //            log_massiv.Clear();
    //        }
    //    }

    //    private static string time()
    //    {
    //        return DateTime.Now.ToString();
    //    }
    //}
}
