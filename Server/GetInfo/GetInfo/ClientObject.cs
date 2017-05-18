using System;
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
using ExtLib;

namespace UdpClientApp
{
    public class ClientObject
    {
        public TcpClient client;

        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
            
        }


        

        protected void Sender(TcpClient client, byte[] data)
        {
            try
            {
                Form1.logger.write("Sender Ok");
                BinaryWriter writer = new BinaryWriter(client.GetStream());                
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                Form1.logger.write(e.Message);
            }
        }


        

        protected byte[] _Info ()
        {
            return new User(Environment.UserName, Environment.MachineName, GetIp(), Form1.settings.Version, _Screen()).GetBinary();
            
        }

        

        protected byte[] _Info(string type)
        {
            switch (type)
            {
                case "bin": return new User(Environment.UserName, Environment.MachineName, GetIp(), Form1.settings.Version, _Screen()).GetBinary();
                case "json": return new User(Environment.UserName, Environment.MachineName, GetIp(), Form1.settings.Version).GetJSON();
                case "xml": return new User(Environment.UserName, Environment.MachineName, GetIp(), Form1.settings.Version).GetXML();
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

            Form1.logger.write("Command from server: Update");

            try
            {
                string fileName = "Update.exe", myStringWebResource = null;
                WebClient myWebClient = new WebClient();
                myStringWebResource = Form1.settings.UrlUpdate + fileName;
                myWebClient.DownloadFile(myStringWebResource, fileName);
                Process.Start("Update.exe", process.Id.ToString());
            }
            catch (Exception e)
            {
                Form1.logger.write(e.Message);
            }
            finally
            {
                Form1.logger.write("Command end");                
            }          
        }

        public void _Process()
        {
            try
            {
                BinaryReader reader = new BinaryReader(this.client.GetStream());
                byte cmd = reader.ReadByte();

                Form1.logger.write(cmd.ToString());

                switch ((Commands)cmd)
                {
                    case Commands.GetInfoBin: Sender(this.client, _Info("bin")); break;
                    case Commands.GetInfoJSON: Sender(this.client, _Info("json")); break;
                    case Commands.GetInfoXML: Sender(this.client, _Info("xml")); break;                                      
                    case Commands.GetScreen: Sender(this.client, _Screen()); break;
                    case Commands.GetUpdate: CmdUpdate(Process.GetCurrentProcess()); break;
                    case Commands.GetTest: Sender(this.client, _Test()); break;
                    default: Form1.logger.write("Incorrect server command "); break;
                }
                
                reader.Close();
            }
            catch (Exception e)
            {
                Form1.logger.write(e.Message + "  _Process");
            }
            finally
            {
                Form1.logger.write("Client close connect");
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