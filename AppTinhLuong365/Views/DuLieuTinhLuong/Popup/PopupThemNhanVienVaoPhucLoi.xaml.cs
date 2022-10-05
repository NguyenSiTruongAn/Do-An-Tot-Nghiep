﻿using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for PopupThemNhanVienVaoPhucLoi.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoPhucLoi : Page, INotifyPropertyChanged
    {
        private DateTime day1, day_end1;
        private string id1;
        bool setDayEnd = false;
        public PopupThemNhanVienVaoPhucLoi(MainWindow main, string id, string day, string day_end)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            DateTime.TryParse(day, out day1);
            DateTime.TryParse(day_end, out day_end1);
            id1 = id;
            if (day_end != null)
            {
                DateTime.TryParse(day_end, out day_end1);
                setDayEnd = true;
            }
            getData();
        }

        MainWindow Main;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSNVThemMoiVaoPhucLoi_PhuCap> _listNV;

        public List<DSNVThemMoiVaoPhucLoi_PhuCap> listNV
        {
            get { return _listNV; }
            set 
            {
                if (value == null) value = new List<DSNVThemMoiVaoPhucLoi_PhuCap>();
                value.Insert(0, new DSNVThemMoiVaoPhucLoi_PhuCap() { ep_id = "", ep_name = "Tất cả nhân viên", ep_image = "", dep_name = "" });
                _listNV = value; OnPropertyChanged(); 
            }
        }

        private List<DSNVThemMoiVaoPhucLoi_PhuCap> _listNV1;

        public List<DSNVThemMoiVaoPhucLoi_PhuCap> listNV1
        {
            get { return _listNV1; }
            set { _listNV1 = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_wf", id1);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSNVThemMoiVaoPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_DSNVThemMoiVaoPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNV1= listNV = api.data.list;
                    }
                    foreach (DSNVThemMoiVaoPhucLoi_PhuCap item in listNV)
                    {
                        if (item.ep_image == "")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                        else item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_other.php", web.QueryString);
            }
        }

        private void ThemNhanVienVaoPhucLoi(object sender, MouseButtonEventArgs e)
        {
            List<string> nv = new List<string>();
            foreach(var item in listNV1)
            {
                if (item.status == true)
                    nv.Add(item.ep_id);
            }
            bool allow = true;
            if (nv.Count <= 0)
            {
                allow = false;
            }

            if ( valuedate_day.CompareTo(valuedate_day_end) > 0 && valuedate_day_end != "")
            {
                validateDateEnd.Text = "Tháng kết thúc không được nhỏ hơn tháng bắt đầu";
                allow = false;
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
                    int dem = 0;
                    foreach(var item in nv)
                    {
                        string idname = "id[" + dem + "]";
                        web.QueryString.Add(idname, item);
                        dem++;
                    }
                    web.QueryString.Add("id_welf", id1);
                    web.QueryString.Add("wf_time", day1.ToString("yyyy/MM/dd"));
                    if(setDayEnd)
                        web.QueryString.Add("wf_time_end", day_end1.ToString("yyyy/MM/dd"));
                    else web.QueryString.Add("wf_time_end", "");
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemNhanVienVaoPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_ThemNhanVienVaoPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_welfare.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x => x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSNVThemMoiVaoPhucLoi_PhuCap data = (DSNVThemMoiVaoPhucLoi_PhuCap)cb.DataContext;
            if (data.ep_id.Equals(""))
            {
                foreach (var item in listNV1)
                {
                    item.status = true;
                }
            }
            else data.status = true;
        }

        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (dteSelectedMonth.DisplayDate == null) dteSelectedMonth1.DisplayDateStart = day1;
            else
                dteSelectedMonth1.DisplayDateStart = dteSelectedMonth.DisplayDate;
            if (setDayEnd)
                dteSelectedMonth1.DisplayDateEnd = day_end1;
            flag = 1;
        }

        private string valuedate_day_end;
        private string valuedate_day;


        private void dteSelectedMonth1_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            valuedate_day_end = dteSelectedMonth1.DisplayDate.ToString("yyyy/MM");
            if (flag1 == 0)
            {
                x = "";
                valuedate_day_end = "";
            }    
            else
            {
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
                valuedate_day_end = dteSelectedMonth1.DisplayDate.ToString("yyyy/MM");
            }
            if (textDenThang != null && !string.IsNullOrEmpty(x))
            {
                textDenThang.Text = x;
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag1 > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        int flag = 0;
        int flag1 = 0;
        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
            {
                valuedate_day = dteSelectedMonth.DisplayDate.ToString("yyyy/MM");
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            }
            if (textThangAD != null && !string.IsNullOrEmpty(x))
            {
                textThangAD.Text = x;
            }
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag += 1;
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSNVThemMoiVaoPhucLoi_PhuCap data = (DSNVThemMoiVaoPhucLoi_PhuCap)cb.DataContext;
            if (data.ep_id.Equals(""))
            {
                foreach (var item in listNV1)
                {
                    item.status = false;
                }
            }
            else
            {
                data.status = false;
            }
        }

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            dteSelectedMonth.DisplayDateStart = day1;
            if (dteSelectedMonth1.DisplayDate != null)
                dteSelectedMonth.DisplayDateEnd = dteSelectedMonth1.DisplayDate;
            else if (setDayEnd)
                dteSelectedMonth.DisplayDateEnd = day_end1;
            flag = 1;
        }
    }
}