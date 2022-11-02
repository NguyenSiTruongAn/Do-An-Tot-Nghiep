using System;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppTinhLuong365.Views.BaoCaoCongLuong
{
    /// <summary>
    /// Interaction logic for BaoCaoCongLuong.xaml
    /// </summary>
    public partial class BaoCaoCongLuong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string month;
        string year;

        public BaoCaoCongLuong(MainWindow main)
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
            this.month = month;
            this.year = year;
            getData(month, year);
            searchBarYear.PlaceHolder = "Năm " + year;
            searchBarMonth.PlaceHolder = "Tháng " + month;
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public MainWindow Main;

        private int _IsSmallSize;

        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 800)
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

        private ItemCongLuong _congLuong;

        public ItemCongLuong congLuong
        {
            get { return _congLuong; }
            set
            {
                _congLuong = value;
                OnPropertyChanged();
            }
        }

        private void getData(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.UploadValuesCompleted += (s, e) =>
                {
                    Api_CongLuong api =
                        JsonConvert.DeserializeObject<Api_CongLuong>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        congLuong = api.data.list;
                    }

                    loading.Visibility = Visibility.Collapsed;
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_cong_luong.php",
                    web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(
                new Views.BaoCaoCongLuong.TongHopLuongNhanVienTheoChuKi(Main, month, year));
        }

        private void Border_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(
                new Views.BaoCaoCongLuong.TongHopThongTinBaoHiem(Main, month, year));
        }

        private void Border_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(
                new Views.BaoCaoCongLuong.TongHopThongTinThue(Main, month, year));
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "", id_pb = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");

            getData(month, year);
        }
    }
}