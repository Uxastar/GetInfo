using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ExtLib;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.Net;

namespace GetInfoGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    

        
    public partial class MainWindow : Window
    {
        static List<User> massiv = new List<User>();

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
                MessageBox.Show("Вы не указали сеть", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Неверно указана сеть", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            foreach (string s in ip_array)
            {
                new Thread(() => Lis(Commands.GetInfoBin, s)).Start();                
            }
            
            MessageBox.Show("Сканирование завершено", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            listBox.Dispatcher.BeginInvoke(new Action(delegate () { this.listBox.ItemsSource = massiv; }));



        }

        private void StartCmd(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Элемент не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Process.Start("PsExec.exe", "\\\\" + ((User)listBox.SelectedItem).PC + " cmd");
        }

        private void UpdateCmd(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Элемент не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Lis(Commands.GetUpdate, ((User)listBox.SelectedItem).IP);
        }
        


        private static void Lis(Commands cmd, string ip)
        {
            TcpClient client = null;
            try
            {
                
                client = new TcpClient(ip, 5006);                

                BinaryWriter writer = new BinaryWriter(client.GetStream());
                writer.Write((byte)cmd);

                BinaryFormatter formatter = new BinaryFormatter();
                Collection<byte> byte_massiv = new Collection<byte>();                

                do
                {
                    byte_massiv.Add((byte)client.GetStream().ReadByte());
                }
                while (client.GetStream().DataAvailable);             

                byte[] array = new byte[byte_massiv.Count()];

                for (int i = 0; i < byte_massiv.Count(); i++)
                    array[i] = byte_massiv[i];

                try
                {
                    using (MemoryStream stream = new MemoryStream(array, 0, array.Length))
                    {
                        massiv.Add((User)formatter.Deserialize(stream));
                    }
                    

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Deserialize", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private BitmapSource Screen(byte[] data)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(data))
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
                MessageBox.Show("Элемент не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Bitmap bmp;               
            using (MemoryStream stream = new MemoryStream(((User)listBox.SelectedItem).Screen))
            {
                bmp = new Bitmap(stream);
            }
            

            Window2 w = new Window2(bmp);
            w.Show();
            IntPtr hBitmap = bmp.GetHbitmap();
            w.image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }




    }
}
