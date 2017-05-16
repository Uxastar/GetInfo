using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;

namespace GetInfoGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    

        
    public partial class MainWindow : Window
    {

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
        public static byte[] _img;


        static List<User.User> massiv = new List<User.User>();



        public MainWindow()
        {
            InitializeComponent();
            listBox.Items.Clear();

            
        }




        private void button_Click(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = null;
            massiv.Clear();

            if (text_ip.Text == "")
            {
                System.Windows.MessageBox.Show("Вы не указали сеть", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string str = text_ip.Text;
            string[] str_new = str.Split(new char[] { '.', '-' });
            List<String> ip_array = new List<string>();

            try
            {
                Convert.ToInt32(str_new[3]);

                for (int i = Convert.ToInt32(str_new[3]); i < Convert.ToInt32(str_new[7]); i++)
                    ip_array.Add(str_new[0] + "." + str_new[1] + "." + str_new[2] + "." + i);
            }
            catch
            {
                System.Windows.MessageBox.Show("Неверно указана сеть", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            foreach (string s in ip_array)
            {
                new Thread(() => Lis(Commands.GetInfoBin, s)).Start();
            }
            listBox.ItemsSource = massiv;
            System.Windows.MessageBox.Show("Сканирование завершено", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(massiv.Count.ToString());

        }
        private void StartCmd(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Элемент не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }





            Process.Start("PsExec.exe", "\\\\" + ((User.User)listBox.SelectedItem).PC + " cmd");
        }

        private void UpdateCmd(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Элемент не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Lis(Commands.GetUpdate, ((User.User)listBox.SelectedItem).IP);
        }

        private static void Lis2(string cmd, string ip)
        {

            TcpClient client = null;
            try
            {
                client = new TcpClient(ip, 5006);

                NetworkStream stream = client.GetStream();

                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(cmd);
                writer.Flush();

                BinaryReader reader = new BinaryReader(stream);
                int count = reader.ReadInt32();
                _img = reader.ReadBytes(count);

                reader.Close();

                writer.Close();



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (client != null)
                    client.Close();

            }


        }

        private static void NewLis(byte[] cmd, string ip)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(ip, 5006);

                BinaryWriter writer = new BinaryWriter(client.GetStream());
                writer.Write(cmd);
                writer.Flush();

                
                
                NetworkStream stream = client.GetStream();               
                

                BinaryFormatter formatter = new BinaryFormatter();
                massiv.Add((User.User)formatter.Deserialize(stream));
                MessageBox.Show(massiv[1].Version);


            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }

        }


        private static void Lis(Commands cmd, string ip)
        {
            
            TcpClient client = null;
            try
            {
                client = new TcpClient("127.0.0.1", 5006);

                //NetworkStream stream = client.GetStream();

                BinaryWriter writer = new BinaryWriter(client.GetStream());
                writer.Write((byte)cmd);


                BinaryFormatter formatter = new BinaryFormatter();
                Collection<byte> byte_massiv = new Collection<byte>();



                do
                {
                    byte_massiv.Add((byte)client.GetStream().ReadByte());

                }
                while (client.GetStream().DataAvailable);
                MessageBox.Show(byte_massiv.Count.ToString());


                // Old
                //string UserName = reader.ReadString();
                //string MachineName = reader.ReadString();
                //string _ip = reader.ReadString();
                //string _version_user = reader.ReadString();
                //massiv.Add(new User.User(UserName, MachineName, _ip, _version_user));


                

                byte[] array = new byte[byte_massiv.Count()];

                for (int i = 0; i < byte_massiv.Count(); i++)
                    array[i] = byte_massiv[i];

                try
                {
                    using (MemoryStream stream = new MemoryStream(array, 0, array.Length))
                    {
                        massiv.Add((User.User)formatter.Deserialize(stream));
                    }

                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    Bitmap bmp;
                    using (var ms = new MemoryStream(_img))
                    {
                        bmp = new Bitmap(ms);
                    }

                    
                    IntPtr hBitmap = bmp.GetHbitmap();
                    BitmapSource so = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                writer.Flush();
                writer.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (client != null)
                    client.Close();
            }


        }

        private void text_ip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            text_ip.Text = "";
        }




        private void Test_click(object sender, RoutedEventArgs e)
        {
            
        }

        private BitmapSource Screen(byte[] data)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(_img))
            {
                bmp = new Bitmap(ms);
            }
            IntPtr hBitmap = bmp.GetHbitmap();
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }


        private void ScreenCmd(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Элемент не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Lis2("screen", ((User.User)listBox.SelectedItem).IP);

            //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            Bitmap bmp;
            using (var ms = new MemoryStream(_img))
            {
                bmp = new Bitmap(ms);
            }


            

            Window2 w = new Window2(bmp);
            w.Show();
            IntPtr hBitmap = bmp.GetHbitmap();
            w.image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }




    }
}
