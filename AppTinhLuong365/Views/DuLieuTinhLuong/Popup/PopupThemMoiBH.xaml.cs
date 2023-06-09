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
    /// Interaction logic for PopupThemMoiBH.xaml
    /// </summary>
    public partial class PopupThemMoiBH : Page
    {
        public PopupThemMoiBH(MainWindow main, string name, string note, string name_ct, string ct, string ct_hs)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbInput.Text = name;
            tbInput1.Text = note;
            ct1 = ct;
            name_ct1 = name_ct;
            ct_hs1 = ct_hs;
            name1 = name;
            note1 = note;
        }

        MainWindow Main;
        private string name1, name_ct1, ct1, ct_hs1, note1;

        private void ThemKhoanTienKhac(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(ct1))
            {
                allow = false;
                txtValuedate.Text = "Vui lòng thiết lập công thức";
            }
            if (string.IsNullOrEmpty(name1))
            {
                allow = false;
                txtValuedateName.Text = "Vui lòng nhập tên bảo hiểm";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("name", tbInput.Text);
                        web.QueryString.Add("des", tbInput1.Text);
                        web.QueryString.Add("name_recipe", name_ct1);
                        web.QueryString.Add("recipe", ct1);
                        web.QueryString.Add("type_data", ct_hs1);
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(Main));
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/create_insurrance.php", web.QueryString);
                }
                
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void ThietLapCongThuc_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            name1 = tbInput.Text;
            note1 = tbInput1.Text;
            var pop = new Views.TinhLuong.PopupChinhSuaThue(Main, "2", name1, note1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 565;
            pop.Height = 481;
        }
    }
}
