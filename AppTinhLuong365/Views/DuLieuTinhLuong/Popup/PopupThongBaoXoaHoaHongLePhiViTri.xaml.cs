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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThongBaoXoaHoaHongLePhiViTri.xaml
    /// </summary>
    public partial class PopupThongBaoXoaHoaHongLePhiViTri : Page
    {
        MainWindow Main;
        public PopupThongBaoXoaHoaHongLePhiViTri(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }
    }
}
