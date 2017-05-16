using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInfo
{
    static public class Settings
    {
        static public string Version { get; set; }
        static public string Key { set; get; }
        static public string UrlUpdate { get; set; }
        static public int Port { get; set; }
        static public bool Log { get; set; }

        static public void Init(string version, string key, string urlUpdate, int port, bool log)
        {
            Version = version;
            Key = key;
            UrlUpdate = urlUpdate;
            Port = port;
            Log = log;
        }
    }





}
