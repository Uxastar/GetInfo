namespace ExtLib
{

    public interface ISettings
    {
        string Version { get; set; }
        string Key { set; get; }
        string UrlUpdate { get; set; }
        int Port { get; set; }
        bool Log { get; set; }

        //void Init(string version, string key, string urlUpdate, int port, bool log);
        //void Init();
    }


    public class Settings : ISettings
    {
        public string Version { get; set; }
        public string Key { set; get; }
        public string UrlUpdate { get; set; }
        public int Port { get; set; }
        public bool Log { get; set; }


        public Settings(string version, string key, string urlUpdate, int port, bool log)
        {
            this.Version = version;
            this.Key = key;
            this.UrlUpdate = urlUpdate;
            this.Port = port;
            this.Log = log;
        }        
    }



    //static public class Settings
    //{
    //    static public string Version { get; set; }
    //    static public string Key { set; get; }
    //    static public string UrlUpdate { get; set; }
    //    static public int Port { get; set; }
    //    static public bool Log { get; set; }

    //    static public void Init(string version, string key, string urlUpdate, int port, bool log)
    //    {
    //        Version = version;
    //        Key = key;
    //        UrlUpdate = urlUpdate;
    //        Port = port;
    //        Log = log;
    //    }
    //}
}
