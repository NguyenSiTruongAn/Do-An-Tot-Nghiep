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
    /// Interaction logic for PopupThuongPhat.xaml
    /// </summary>
    public partial class PopupThuongPhat : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupThuongPhat(MainWindow main, ListThuongPhat data, string pay_id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.data = data;
            this.pay_id = pay_id;
            dpNgay.SelectedDate = DateTime.Now;
            if (!string.IsNullOrEmpty(pay_id))
            {
                btnThem.Visibility = Visibility.Collapsed;
                btnSua.Visibility = Visibility.Visible;
                foreach(var item in data.dt_thuong)
                {
                    if(item.pay_id == pay_id)
                    {
                        tbInput.Text = item.pay_price;
                        rdThuong.IsChecked = true;
                        dpNgay.SelectedDate = DateTime.Parse(item.pay_day);
                        tbInput1.Text = item.pay_case;
                    }

                }
                foreach (var item in data.dt_phat)
                {
                    if (item.pay_id == pay_id)
                    {
                        tbInput.Text = item.pay_price;
                        rdPhat.IsChecked = true;
                        dpNgay.SelectedDate = DateTime.Parse(item.pay_day);
                        tbInput1.Text = item.pay_case;
                    }

                }
            }
        }

        private string pay_id;

        private ListThuongPhat _data;
        public ListThuongPhat data
        {
            get { return _data; }
            set
            {
                _data = value; OnPropertyChanged();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 980)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 980 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DtThuong data = (DtThuong)b.DataContext;
            pay_id = data.pay_id;
            tbInput.Text = data.pay_price;
            rdThuong.IsChecked = true;
            dpNgay.SelectedDate = DateTime.Parse(data.pay_day);
            tbInput1.Text = data.pay_case;
            btnThem.Visibility = Visibility.Collapsed;
            btnSua.Visibility = Visibility.Visible;
        }

        private void tbInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ThemThuongPhat(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateTien.Text = validateNgay.Text = validateLyDo.Text ="";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateTien.Text = "Vui lòng nhập số tiền thưởng phạt";
            }
            if(dpNgay.SelectedDate == null)
            {
                allow = false;
                validateNgay.Text = "Vui lòng chọn ngày áp dụng";
            }
            if(string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateLyDo.Text = "Vui lòng nhập lý do";
            }    
            if (allow)
            {
                using(WebClient web = new WebClient())
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id_emp", data.id);
                    web.QueryString.Add("amount", tbInput.Text);
                    string type = "2";
                    if(rdThuong.IsChecked == true)
                    {
                        type = "1";
                    }
                    web.QueryString.Add("type", type);
                    web.QueryString.Add("des", tbInput1.Text);
                    web.QueryString.Add("day", dpNgay.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.ThuongPhat(Main));
                                Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_punish_emp.php", web.QueryString);
                }
            }
        }

        private void SuaThuongPhat(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateTien.Text = validateNgay.Text = validateLyDo.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateTien.Text = "Vui lòng nhập số tiền thưởng phạt";
            }
            if (dpNgay.SelectedDate == null)
            {
                allow = false;
                validateNgay.Text = "Vui lòng chọn ngày áp dụng";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateLyDo.Text = "Vui lòng nhập lý do";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id", pay_id);
                    web.QueryString.Add("amount", tbInput.Text);
                    string type = "2";
                    if (rdThuong.IsChecked == true)
                    {
                        type = "1";
                    }
                    web.QueryString.Add("type", type);
                    web.QueryString.Add("des", tbInput1.Text);
                    web.QueryString.Add("day", dpNgay.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.ThuongPhat(Main));
                                Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_punish_emp.php", web.QueryString);
                }
            }
        }

        private void SuaPhat(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DtThuong data = (DtThuong)b.DataContext;
            pay_id = data.pay_id;
            tbInput.Text = data.pay_price;
            rdPhat.IsChecked = true;
            dpNgay.SelectedDate = DateTime.Parse(data.pay_day);
            tbInput1.Text = data.pay_case;
            btnThem.Visibility = Visibility.Collapsed;
            btnSua.Visibility = Visibility.Visible;
        }

        private void XoaThuongPhat(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DtThuong data = (DtThuong)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupXoaThuongPhat(Main, data.pay_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
