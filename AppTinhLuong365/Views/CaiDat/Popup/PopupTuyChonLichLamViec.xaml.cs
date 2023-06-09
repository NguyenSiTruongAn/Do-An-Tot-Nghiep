﻿using System;
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
using AppTinhLuong365.Model.APIEntity;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupTuyChonLichLamViec.xaml
    /// </summary>
    public partial class PopupTuyChonLichLamViec : Page
    {
        MainWindow Main;
        private string id;
        string ap_month;
        string cy_name;
        public PopupTuyChonLichLamViec(MainWindow main, string cy_id, string ap_month, string name)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = cy_id;
            this.ap_month = ap_month;
            this.cy_name = name;
        }

        private void Xoa_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThongBaoXoaLichLamViec(Main, id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void SaoChep_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupSaoChepLichDon(Main, id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSua_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupChinhSuaLichLamViec(Main, id, null, ap_month);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void DanhSachNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupDSNVLichLamViec(Main, id, cy_name);
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
