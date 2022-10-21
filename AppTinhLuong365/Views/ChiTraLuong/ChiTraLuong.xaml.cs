using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.ChiTraLuong
{
    /// <summary>
    /// Interaction logic for ChiTraLuong.xaml
    /// </summary>
    public partial class ChiTraLuong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChiTraLuong(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            var c = DateTime.Now.Year;
            if (c != null)
            {
                YearList.Add($"Năm {c - 1}");
                YearList.Add($"Năm {c}");
                YearList.Add($"Năm {c + 1}");
            }
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            getData(month, year);
            Year.PlaceHolder = "Năm " + year;
            Month.PlaceHolder = "Tháng " + month;
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public MainWindow Main;

        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 900)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 900 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.ChiTraLuong.PopupChiTraLuong(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<Item_pay> _listPay;

        public List<Item_pay> listPay
        {
            get { return _listPay; }
            set
            {
                _listPay = value;
                OnPropertyChanged();
            }
        }

        private void getData(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_pay api =
                        JsonConvert.DeserializeObject<API_List_pay>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listPay = api.data.list;
                        DateTime aDateTime;
                        foreach (var a in listPay)
                        {
                            DateTime.TryParse(a.pay_time_start, out aDateTime);
                            a.pay_time_start = aDateTime.ToString("dd/MM/yyyy");
                            DateTime.TryParse(a.pay_time_end, out aDateTime);
                            a.pay_time_end = aDateTime.ToString("dd/MM/yyyy");
                            DateTime.TryParse(a.pay_for_time, out aDateTime);
                            a.pay_for_time = "Tháng " + aDateTime.ToString("MM/yyyy");
                        }
                    }
                    //foreach (EpLate item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_pay.php",
                    web.QueryString);
            }
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "";
            if (Year.SelectedItem != null)
                year = Year.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (Month.SelectedIndex != -1)
                month = (Month.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            getData(month, year);
        }

        private void Sua(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            Item_pay data = (Item_pay)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.ChiTraLuong.PopupSua(Main, data.pay_id, data.pay_name, data.pay_for_time, data.pay_time_start, data.pay_time_end, data.pay_unit));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Xoa(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            Item_pay data = (Item_pay)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.ChiTraLuong.PopupXoa(Main, data.pay_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Detail(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            Item_pay data = (Item_pay)tb.DataContext;
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTietChiTraLuong(Main, data.pay_id));
        }
    }
}
