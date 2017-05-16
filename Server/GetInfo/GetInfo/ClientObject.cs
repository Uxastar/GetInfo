using System;
using User;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using GetInfo;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Text;

namespace UdpClientApp
{
    public class ClientObject
    {
        public TcpClient client;

        [Flags]
        enum Commands : byte
        {
            GetInfoBin = 0x0a,
            GetInfoJSON = 0x0b,
            GetInfoXML = 0x0c,
            GetScreen = 0x14,
            GetUpdate = 0x15,
            GetTest = 0xff
        }
        


        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
            
        }


        

        protected void Sender(TcpClient client, byte[] data)
        {
            try
            {
                Logger.add("Sender OK 0xFF");
                BinaryWriter writer = new BinaryWriter(client.GetStream());                
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                Logger.add(e.Message + "0xFF");
            }
        }


        

        protected byte[] _Info ()
        {
            return new User.User(Environment.UserName, Environment.MachineName, GetIp(), Settings.Version, _Screen()).GetBinary();            
        }

        

        protected byte[] _Info(string type)
        {
            User.User tmp = new User.User(Environment.UserName, Environment.MachineName, GetIp(), Settings.Version);
            
            switch (type)
            {
                case "bin": return tmp.GetBinary();
                case "json": return tmp.GetJSON();
                case "xml": return tmp.GetXML();
            }

            return (new byte[1] { 0x00 });
        }

        

        protected byte[] _Screen()
        {
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics gr = Graphics.FromImage(bm as Image);
            gr.CopyFromScreen(0, 0, 0, 0, bm.Size);

            using (MemoryStream stream = new MemoryStream())
            {
                bm.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }        

        protected byte[] _Test()
        {
            return Encoding.UTF8.GetBytes("Test send from server");
        }

        public void CmdUpdate(Process process)
        {

            Logger.add("Command from server: Update");

            try
            {
                string fileName = "Update.exe", myStringWebResource = null;
                WebClient myWebClient = new WebClient();
                myStringWebResource = Settings.UrlUpdate + fileName;
                myWebClient.DownloadFile(myStringWebResource, fileName);
                Process.Start("Update.exe", process.Id.ToString());
            }
            catch (Exception e)
            {
                Logger.add(e.Message);
            }
            finally
            {
                Logger.add("Command end");                
            }          
        }

        public void _Process()
        {
            try
            {
                BinaryReader reader = new BinaryReader(this.client.GetStream());
                byte cmd = reader.ReadByte();

                Logger.add(cmd.ToString());

                switch ((Commands)cmd)
                {
                    case Commands.GetInfoBin: Sender(this.client, _Info("bin")); break;
                    case Commands.GetInfoJSON: Sender(this.client, _Info("json")); break;
                    case Commands.GetInfoXML: Sender(this.client, _Info("xml")); break;                                      
                    case Commands.GetScreen: Sender(this.client, _Screen()); break;
                    case Commands.GetUpdate: CmdUpdate(Process.GetCurrentProcess()); break;
                    case Commands.GetTest: Sender(this.client, _Test()); break;
                    default: Logger.add("Incorrect server command "); break;
                }
                
                reader.Close();
            }
            catch (Exception e)
            {
                Logger.add(e.Message + " 0x2F");
            }
            finally
            {
                Logger.add("Client close connect");
                this.client.Close();
                MemoryManagement.FlushMemory();
            }
        }

        static string GetIp()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}