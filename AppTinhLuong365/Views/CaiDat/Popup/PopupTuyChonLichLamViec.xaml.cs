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
    /// Interaction logic for PopupTuyChonLichLamViec.xaml
    /// </summary>
    public partial class PopupTuyChonLichLamViec : Page
    {
        MainWindow Main;
        private string id;
        public PopupTuyChonLichLamViec(MainWindow main, string cy_id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = cy_id;
        }

        private void Xoa_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThongBaoXoaLichLamViec(Main, id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void SaoChep_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupTuyChonSaoChepLichLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSua_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            /*var pop = new Views.CaiDat.Popup.PopupChinhSuaLichLamViec(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;*/
        }

        private void DanhSachNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupDSNVLichLamViec(Main, id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 1054;
        }

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupThemNhanVienVaoLichLamViec(Main, id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 495;
        }
    }
}
