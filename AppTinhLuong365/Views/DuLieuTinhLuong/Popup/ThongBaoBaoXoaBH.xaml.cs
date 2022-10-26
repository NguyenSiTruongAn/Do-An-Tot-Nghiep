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
    /// Interaction logic for ThongBaoBaoXoaBH.xaml
    /// </summary>
    public partial class ThongBaoBaoXoaBH : Page
    {
        MainWindow Main;
        private string cl_id;
        public ThongBaoBaoXoaBH(MainWindow main, string cl_id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.cl_id = cl_id;
        }
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void tieptuc(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id_insurrance", cl_id);
                }
                web.UploadValuesCompleted += (s, e1) =>
                {
                    API_XoaPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_XoaPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e1.Result));
                    if (api.data != null)
                    {
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(Main));
                        Main.sidebar.SelectedIndex = 8;
                        this.Visibility = Visibility.Collapsed;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/remove_insurrance.php", web.QueryString);
                
            }
        }
    }
}
