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
        private string id1, name1, name_ct1, note1, ct1, ct_hs1, fs_id1;
        public PopupTuyChonCacKhoanTienKhac(MainWindow main, string id, string name, string mota, string congthuc, string name_ct, string ct_hs, string fs_id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id1 = id;
            name1 = name;
            note1 = mota;
            ct1 = congthuc;
            name_ct1 = name_ct;
            ct_hs1 = ct_hs;
            fs_id1 = fs_id;
        }

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienCKTK(Main, id1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void DSNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.DSNhanVienADVaoCKTK(Main, id1, name1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 1054;
        }

        private void ChinhSuaThue_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCKTK(Main, id1, name1, note1, name_ct1, ct1, ct_hs1, fs_id1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 433;
        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaCKTK(Main, id1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 252;
        }
    }
}
