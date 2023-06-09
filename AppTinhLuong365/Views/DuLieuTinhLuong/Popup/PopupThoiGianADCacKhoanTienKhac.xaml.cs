﻿using AppTinhLuong365.Model.APIEntity;
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
    /// Interaction logic for PopupThoiGianADCacKhoanTienKhac.xaml
    /// </summary>
    public partial class PopupThoiGianADCacKhoanTienKhac : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        private string id1;
        private List<DSNVThemVaoCKTK> _dsnv1;

        public List<DSNVThemVaoCKTK> dsnv1
        {
            get { return _dsnv1; }
            set { _dsnv1 = value; OnPropertyChanged(); }
        }

        public PopupThoiGianADCacKhoanTienKhac(MainWindow main, string id, List<DSNVThemVaoCKTK> dsnv)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id1 = id;
            dsnv1 = dsnv;
            dteSelectedMonth = new Calendar();
            dteSelectedMonth.Visibility = Visibility.Collapsed;
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            dteSelectedMonth.MouseLeftButtonDown += Select_thang;
            dteSelectedMonth.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged;
            dteSelectedMonth1 = new Calendar();
            dteSelectedMonth1.Visibility = Visibility.Collapsed;
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            dteSelectedMonth1.MouseLeftButtonDown += Select_thang_end;
            dteSelectedMonth1.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged1;
            cl = new List<Calendar>();
            cl.Add(dteSelectedMonth);
            cl = cl.ToList();
            cl1 = new List<Calendar>();
            cl1.Add(dteSelectedMonth1);
            cl1 = cl1.ToList();
        }

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }

        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag1 = 1;
        }

        int flag = 0;
        int flag1 = 0;

        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThangAD != null && !string.IsNullOrEmpty(x))
            {
                textThangAD.Text = x;
                valuedateDay = dteSelectedMonth.DisplayDate.ToString("yyyy/MM");
                DateTime a = DateTime.Parse(x);
            }
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag += 1;
        }

        private void dteSelectedMonth_DisplayModeChanged1(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (textDenThang != null && !string.IsNullOrEmpty(x))
            {
                textDenThang.Text = x;
                valuedateDayEnd = dteSelectedMonth1.DisplayDate.ToString("yyyy/MM");
                DateTime a = DateTime.Parse(x);
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag1 > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        private string valuedateDay = "", valuedateDayEnd = "";

        Calendar dteSelectedMonth { get; set; }
        Calendar dteSelectedMonth1 { get; set; }

        private List<Calendar> _cl;

        public List<Calendar> cl
        {
            get { return _cl; }
            set
            {
                _cl = value; OnPropertyChanged();
            }
        }

        private List<Calendar> _cl1;

        public List<Calendar> cl1
        {
            get { return _cl1; }
            set
            {
                _cl1 = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Btn_QuayLai_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienCKTK(Main,id1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }

        private void Btn_LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateDate.Text = "";
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (valuedateDayEnd != "" && valuedateDay.CompareTo(valuedateDayEnd) > 0)
            {
                allow = false;
                validateDateEnd.Text = "Tháng kết thúc phải lớn hơn tháng bắt đầu";
                validateDateEnd.TextTrimming = TextTrimming.CharacterEllipsis;
            }
            if (allow)
            {
                string day_end = "";
                DateTime date_end;
                if (textDenThang.Text != "--------- ----")
                {
                    DateTime.TryParse(textDenThang.Text, out date_end);
                    day_end = date_end.ToString("yyyy-MM");
                }

                string listID ="";
                for(int i=0; i< dsnv1.Count; i++)
                {
                    if (i < dsnv1.Count - 1)
                        listID += dsnv1[i].ep_id + ",";
                    else
                        listID += dsnv1[i].ep_id;
                }    
                DateTime.TryParse(textThangAD.Text, out date_end);
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id_list", id1);
                    web.QueryString.Add("arr_user", listID);
                    web.QueryString.Add("time_bg", date_end.ToString("yyyy-MM"));
                    web.QueryString.Add("time_kt", day_end);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_otherMoney.php", web.QueryString);
                }
                /*Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(Main));
                Main.sidebar.SelectedIndex = 7;*/
            }
        }
    }
}
