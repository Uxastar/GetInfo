using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using ExtLib;
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
        public static ISettings settings = new Settings("1.6", null, null, 5006, true);
        public static ILogger logger = new Logger("log.txt", true);
        static TcpListener listener;

        public Form1()
        {

                      

            InitializeComponent();

            try
            {
                listener = new TcpListener(IPAddress.Any, settings.Port);
                listener.Start();
                logger.write("Listener start");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    logger.write("Client " + client.Client.AddressFamily.ToString() + " connect");
                    
                    Task clientTask = new Task(clientObject._Process);
                    clientTask.Start();
                    MemoryManagement.FlushMemory();
                }
            }
            catch (Exception ex)
            {
                logger.write(ex.Message);
            }
            finally
            {
                logger.write("End listener");
                if (listener != null)
                {
                    listener.Stop();
                    logger.write("Listener STOP");
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
