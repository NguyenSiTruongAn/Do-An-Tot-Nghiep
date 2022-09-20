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
    /// Interaction logic for PopupCaiDatHoaHongLePhiViTri.xaml
    /// </summary>
    public partial class PopupCaiDatHoaHongLePhiViTri : Page
    {
        MainWindow Main;
        public PopupCaiDatHoaHongLePhiViTri(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaHoaHongLePhiViTri(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnXoaHoaHongTien_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaHoaHongLePhiViTri(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnThem_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemHoaHongLePhiViTri(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
