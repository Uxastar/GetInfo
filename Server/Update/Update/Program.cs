using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Update
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Update program: GetInfo");
            Console.WriteLine("Process id: " + args[0]);
            Console.WriteLine("End current process, plase wait.........");
            Process.GetProcessById(Convert.ToInt32(args[0])).Kill();
            Thread.Sleep(3000);



            string remoteUri = "http://url";
            string fileName = "GetInfo.exe", myStringWebResource = null;
            WebClient myWebClient = new WebClient();
            myStringWebResource = remoteUri + fileName;
            Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
            myWebClient.DownloadFile(myStringWebResource, fileName);
            Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
            Console.WriteLine("Start new process.....");

            Process.Start("GetInfo.exe");
            return;

        }
    }
}
