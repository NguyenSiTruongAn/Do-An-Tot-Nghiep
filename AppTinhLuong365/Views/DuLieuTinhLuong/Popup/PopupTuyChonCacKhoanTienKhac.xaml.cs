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
    /// Interaction logic for PopupTuyChonCacKhoanTienKhac.xaml
    /// </summary>
    public partial class PopupTuyChonCacKhoanTienKhac : Page
    {
        MainWindow Main;
        public PopupTuyChonCacKhoanTienKhac(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienCKTK(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 495;
        }

        private void DSNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.DSNhanVienADVaoCKTK(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 1054;
        }

        private void ChinhSuaThue_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCKTK(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 433;
        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaCKTK(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 252;
        }
    }
}
