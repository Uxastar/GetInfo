using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using User;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UdpClientApp;

namespace GetInfo
{
    public partial class Form1 : Form
    {
        
        static TcpListener listener;

        public Form1()
        {

            Settings.Init("1.0", null, "http://url", 5006, true);

            InitializeComponent();

            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), Settings.Port);
                listener.Start();
                Logger.add("Listener start");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    Logger.add("Client " + client.Client.AddressFamily.ToString() + " connect");
                    
                    Task clientTask = new Task(clientObject._Process);
                    clientTask.Start();
                    MemoryManagement.FlushMemory();
                }
            }
            catch (Exception ex)
            {
                Logger.add(ex.Message + " 0x0f");
            }
            finally
            {
                Logger.add("End listener");
                if (listener != null)
                {
                    listener.Stop();
                    Logger.add("Listener STOP");
                }
                    

            }
        }

        static string GetIp()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    
}
