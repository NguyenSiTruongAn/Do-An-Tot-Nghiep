using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemMoiPhuCapTheoCa.xaml
    /// </summary>
    public partial class PopupThemMoiPhuCapTheoCa : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public PopupThemMoiPhuCapTheoCa(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            getData();
        }
        MainWindow Main;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSCaLamViec> _listCa;

        public List<DSCaLamViec> listCa
        {
            get { return _listCa; }
            set
            {
                /*if (value == null) value = new List<DSCaLamViec>();
                value.Insert(0,new DSCaLamViec() { shift_id="-1",shift_name="Tất cả các ca" });*/
                _listCa = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_com", Main.CurrentCompany.com_id);
                        web.Headers.Add("Authorization", Main.CurrentCompany.token);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_DSCaLamViec api = JsonConvert.DeserializeObject<API_DSCaLamViec>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listCa = api.data.items;
                        }
                    };
                    web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_shift.php", web.QueryString);
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }

        private void dteSelectedMonth1_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
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
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
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

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }

        private void ThemMoiPhuCapTheoCa(object sender, MouseButtonEventArgs e)
        {
            DSCaLamViec ca =  (DSCaLamViec)cbCaLamViec.SelectedItem;
            var allow = true;
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if(string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhập đầy đủ thông tin";
            }
            if (allow)
            {
                string day_end = "";
                if (textDenThang.Text != "--------- ----")
                    day_end = dteSelectedMonth1.DisplayDate.ToString("yyyy/MM/dd");
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("id_shift", ca.shift_id);
                        web.QueryString.Add("month_start", dteSelectedMonth.DisplayDate.ToString("yyyy/MM/dd"));
                        web.QueryString.Add("month_end", day_end);
                        web.QueryString.Add("salary", tbInput1.Text);
                    }
                    web.UploadValuesCompleted += (s, e1) =>
                    {
                        API_ThemMoiPhuCaoTheoCa api = JsonConvert.DeserializeObject<API_ThemMoiPhuCaoTheoCa>(UnicodeEncoding.UTF8.GetString(e1.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/create_welfare_shift.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                Main.sidebar.SelectedIndex = 5;
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
