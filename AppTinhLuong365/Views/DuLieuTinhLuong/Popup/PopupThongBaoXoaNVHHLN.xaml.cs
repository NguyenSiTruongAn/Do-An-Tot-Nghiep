﻿using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for PopupThongBaoXoaNVHHLN.xaml
    /// </summary>
    public partial class PopupThongBaoXoaNVHHLN : Page
    {
        public PopupThongBaoXoaNVHHLN(MainWindow main, string ro_id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = ro_id;
        }
        MainWindow Main;
        private string id, type;

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void TiepTuc(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id_rose", id);
                }
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_rose_loinhuan.php", web.QueryString);
            }
            var pop = new Views.DuLieuTinhLuong.HoaHongLoiNhuan(Main);
            Main.HomeSelectionPage.NavigationService.Navigate(pop);
            Main.sidebar.SelectedIndex = -1;
            if (type == "1")
                pop.tb.SelectedIndex = 1;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
