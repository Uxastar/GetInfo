using Microsoft.Win32;
using System.Drawing;
using System.Windows;

namespace GetInfoGUI
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Bitmap BitImg;

        public Window2(Bitmap bmp)
        {
            this.BitImg = new Bitmap(bmp);
            InitializeComponent();           
            
        }

        private void MenuItem_Click_save(object sender, RoutedEventArgs e)
        {           
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG|*.jpg";
            if (saveFileDialog.ShowDialog() == true)                
            {
                BitImg.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("File saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
