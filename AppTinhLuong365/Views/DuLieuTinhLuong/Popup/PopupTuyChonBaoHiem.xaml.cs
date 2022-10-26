using AppTinhLuong365.Model.APIEntity;
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
    /// Interaction logic for PopupTuyChonBaoHiem.xaml
    /// </summary>
    public partial class PopupTuyChonBaoHiem : Page
    {
        MainWindow Main;
        public PopupTuyChonBaoHiem(MainWindow main, ListBaoHiem data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.data = data;
            if (data.cl_active == "1")
            {
                ChinhSua.Visibility = Visibility.Collapsed;
                Xoa.Visibility = Visibility.Collapsed;
            }
        }
        ListBaoHiem data;

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienVaoBaoHiem(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 495;
        }

        private void DSNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupDSNhanVienADBaoHiem(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 1054;
        }

        private void ChinhSuaThue_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCSBH(Main, data.cl_id,data.cl_name,data.cl_note,data.fs_name,data.fs_repica,data.fs_data,"");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.ThongBaoBaoXoaBH(Main, data.cl_id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 252;
        }
    }
}
