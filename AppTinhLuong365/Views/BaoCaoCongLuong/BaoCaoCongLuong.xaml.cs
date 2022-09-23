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

        public BaoCaoCongLuong(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <=12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            for (var i = 2022; i <= 2025; i++)
            {
                YearList.Add($"Năm {i}");
            }
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
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

        private ItemCongLuong _congLuong;

        public ItemCongLuong congLuong
        {
            get { return _congLuong; }
            set { _congLuong = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", "9");
                web.QueryString.Add("year", "2022");
                web.UploadValuesCompleted += (s, e) =>
                {
                    Api_CongLuong api = JsonConvert.DeserializeObject<Api_CongLuong>(UnicodeEncoding.UTF8.GetString(e.Result));
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_cong_luong.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.TongHopLuongNhanVienTheoChuKi(Main));
        }
    }
}
