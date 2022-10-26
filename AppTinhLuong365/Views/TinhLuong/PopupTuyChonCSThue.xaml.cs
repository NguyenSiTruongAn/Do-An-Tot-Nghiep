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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupTuyChonCSThue.xaml
    /// </summary>
    public partial class PopupTuyChonCSThue : Page
    {
        MainWindow Main;
        public PopupTuyChonCSThue(MainWindow main, ChinhSachThue cs)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.cs = cs;
            if(cs.cl_active == "1" || cs.cl_id=="1")
            {
                ChinhSua.Visibility = Visibility.Collapsed;
                Xoa.Visibility = Visibility.Collapsed;
            }
        }

        ChinhSachThue cs;

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupThemNhanVienVaoThue(Main,cs.cl_id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void DSNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupDSNhanVienADThue(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 1054;
        }

        private void ChinhSuaThue_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupThue(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 433;
        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupThongBaoXoa(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 203;
        }
    }
}
