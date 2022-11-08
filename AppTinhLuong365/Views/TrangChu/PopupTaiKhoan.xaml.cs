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
            if (Main.MainType == 1)
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.CurrentEmployee.ep_id + "." + Main.CurrentEmployee.pass + "&link=https://quanlychung.timviec365.vn/quan-ly-thong-tin-tai-khoan-cong-ty.html");
            else
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.CurrentCompany.com_id + "." + Main.CurrentCompany.pass + "&link=https://quanlychung.timviec365.vn/quan-ly-thong-tin-tai-khoan-cong-ty.html");
        }

        private void DockPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (Main.MainType == 1)
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.CurrentEmployee.ep_id + "." + Main.CurrentEmployee.pass + "&link=https://chamcong.timviec365.vn/quan-ly-cong-ty/gui-loi.html");
            else
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.CurrentCompany.com_id + "." + Main.CurrentCompany.pass + "&link=https://chamcong.timviec365.vn/quan-ly-cong-ty/gui-loi.html");
        }

        private void DockPanel_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            if (Main.MainType == 1)
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.CurrentEmployee.ep_id + "." + Main.CurrentEmployee.pass + "&link=https://chamcong.timviec365.vn/quan-ly-cong-ty/danh-gia.html");
            else
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.CurrentCompany.com_id + "." + Main.CurrentCompany.pass + "&link=https://chamcong.timviec365.vn/quan-ly-cong-ty/danh-gia.html");
        }

        private void DockPanel_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            if (Main.LogOut != null) Main.LogOut();
        }
    }
}
