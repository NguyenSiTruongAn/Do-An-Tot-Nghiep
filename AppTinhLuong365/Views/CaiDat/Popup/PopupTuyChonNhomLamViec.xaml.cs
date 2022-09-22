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
    /// Interaction logic for PopupTuyChonNhomLamViec.xaml
    /// </summary>
    public partial class PopupTuyChonNhomLamViec : Page
    {
        private string id, ten, mota;
        public PopupTuyChonNhomLamViec(MainWindow main, string ID, string name, string note)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            id = ID;
            ten = name;
            mota = note;
        }
        MainWindow Main;

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupThemNhanVien(Main, id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 495;
        }

        private void ChinhSuaThue_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupChinhSuaNhomLamViec(Main, id, ten, mota);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupThongBaoXoaNhom(Main, id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 252;
        }
    }
}
