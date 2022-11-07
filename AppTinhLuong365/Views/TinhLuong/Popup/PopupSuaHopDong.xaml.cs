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

namespace AppTinhLuong365.Views.TinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupSuaHopDong.xaml
    /// </summary>
    public partial class PopupSuaHopDong : Page
    {
        public PopupSuaHopDong(MainWindow main, ContractWork data, string data1)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.data = data;
            this.data1 = data1;
            tbInput.Text = data.con_name;
            tbInput1.Text = data.con_salary_persent;
            dpNgayHieuLuc.SelectedDate = DateTime.Parse(data.con_time_up);
            if(data.con_time_end != "0000-00-00")
                dpNgayHetHan.SelectedDate = DateTime.Parse(data.con_time_end);
        }
        MainWindow Main;
        string data1;
        ContractWork data;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void SuaHopDong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateLuong.Text = validateNgay.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateLuong.Text = "Vui lòng nhập đầy đủ";
            }
            else if (int.Parse(tbInput1.Text) > 100)
            {
                allow = false;
                validateLuong.Text = "Vui lòng không nhập quá 100";
            }
            if (dpNgayHieuLuc.SelectedDate == null)
            {
                allow = false;
                validateNgay.Text = "Vui lòng chọn thời gian áp dụng";
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
                    web.QueryString.Add("id", data1);
                    web.QueryString.Add("id_cw", data.con_id);
                    web.QueryString.Add("name_ct", tbInput.Text);
                    web.QueryString.Add("salary", tbInput1.Text);
                    web.QueryString.Add("date_ct", dpNgayHieuLuc.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    if(dpNgayHetHan.SelectedDate != null)
                        web.QueryString.Add("date_ect_end", dpNgayHetHan.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data1));
                            Main.HomeSelectionPage.Visibility = Visibility.Visible;
                            this.Visibility = Visibility.Collapsed;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_contract_work.php", web.QueryString);
                }
            }
        }

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
