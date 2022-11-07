using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AppTinhLuong365.Views.TrangChu
{
    /// <summary>
    /// Interaction logic for PopupTaiKhoan.xaml
    /// </summary>
    public partial class PopupTaiKhoan : Page
    {
        MainWindow Main;
        public PopupTaiKhoan(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://quanlychung.timviec365.vn/quan-ly-thong-tin-tai-khoan-cong-ty.html");
        }

        private void DockPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://chamcong.timviec365.vn/quan-ly-cong-ty/gui-loi.html");
        }

        private void DockPanel_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://chamcong.timviec365.vn/quan-ly-cong-ty/danh-gia.html");
        }

        private void DockPanel_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            if (Main.LogOut != null) Main.LogOut();
        }
    }
}
