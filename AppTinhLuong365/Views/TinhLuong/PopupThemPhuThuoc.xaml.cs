﻿using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PopupThemPhuThuoc.xaml
    /// </summary>
    public partial class PopupThemPhuThuoc : Page
    {
        public PopupThemPhuThuoc(MainWindow main, string data)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            this.data = data;
        }
        string data;
        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void ThemGiaDinh(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateNgaySinh.Text = validateSDT.Text = validateQH.Text = validateNghe.Text = validateDiaChi.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput2.Text))
            {
                allow = false;
                validateSDT.Text = "Vui lòng nhập đầy đủ";
            }
            else if (tbInput2.Text.Length != 10)
            {
                allow = false;
                validateSDT.Text = "Vui lòng nhập đúng 10";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateQH.Text = "Vui lòng nhập đầy đủ";
            }
            if (dpNgaySinh.SelectedDate == null)
            {
                allow = false;
                validateNgaySinh.Text = "Vui lòng chọn ngày sinh";
            }
            if (string.IsNullOrEmpty(tbInput3.Text))
            {
                allow = false;
                validateNghe.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput4.Text))
            {
                allow = false;
                validateDiaChi.Text = "Vui lòng nhập đầy đủ";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id", data);
                    web.QueryString.Add("aname", tbInput.Text);
                    web.QueryString.Add("arela", tbInput1.Text);
                    web.QueryString.Add("aphone", tbInput2.Text);
                    web.QueryString.Add("ajob", tbInput3.Text);
                    web.QueryString.Add("aadd", tbInput4.Text);
                    web.QueryString.Add("adate", dpNgaySinh.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data));
                                Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_family_member.php", web.QueryString);
                }
            }
        }

        private void tbInput2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
