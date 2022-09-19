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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemLichLamViec.xaml
    /// </summary>
    public partial class PopupThemLichLamViec : Page
    {
        public PopupThemLichLamViec(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
        }
        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void BtnTiepTuc_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupChonCaLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
