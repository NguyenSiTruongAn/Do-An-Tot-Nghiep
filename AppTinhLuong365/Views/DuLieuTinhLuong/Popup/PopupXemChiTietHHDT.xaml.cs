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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupXemChiTietHHDT.xaml
    /// </summary>
    public partial class PopupXemChiTietHHDT : Page
    {
        public PopupXemChiTietHHDT(MainWindow main, string Mota)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            mota.Text = Mota;
        }

        MainWindow Main;

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }
    }
}
