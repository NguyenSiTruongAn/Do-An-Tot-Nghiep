using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupAddFileNhanVien.xaml
    /// </summary>
    public partial class PopupAddFileNhanVien : Page
    {
        MainWindow Main;
        public PopupAddFileNhanVien(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void Brown_file_CLick(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog() == true)
            {

            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
