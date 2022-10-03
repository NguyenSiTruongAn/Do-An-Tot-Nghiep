using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for CacKhoanTienKhac.xaml
    /// </summary>
    public partial class CacKhoanTienKhac : Page, INotifyPropertyChanged
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
        public CacKhoanTienKhac(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
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
            getData1();
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        private List<DSCacKhoanTienKhac> _listCacKhoanTienKhac;

        public List<DSCacKhoanTienKhac> listCacKhoanTienKhac
        {
            get { return _listCacKhoanTienKhac; }
            set { _listCacKhoanTienKhac = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSCacKhoanTienKhac api = JsonConvert.DeserializeObject<API_DSCacKhoanTienKhac>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listCacKhoanTienKhac = api.data.other_list;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_otherMoney.php", web.QueryString);
            }
        }

        private List<DSNVCacKhoanThienKhac> _listNVCacKhoanTienKhac;

        public List<DSNVCacKhoanThienKhac> listNVCacKhoanTienKhac
        {
            get { return _listNVCacKhoanTienKhac; }
            set { _listNVCacKhoanTienKhac = value; OnPropertyChanged(); }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("page", "0");
                    web.QueryString.Add("id_user", "0");
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSNVCacKhoanThienKhac api = JsonConvert.DeserializeObject<API_DSNVCacKhoanThienKhac>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNVCacKhoanTienKhac = api.data.ep_other_list;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_otherMoney.php", web.QueryString);
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
            if(this.ActualWidth > 1380)
            {
                DockPanel.SetDock(dockCacKhoanTienKhac, Dock.Right);
            }
            else
                DockPanel.SetDock(dockCacKhoanTienKhac, Dock.Bottom);

        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void ThemKhoanTienKhac_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemKhoanTienKhac(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 433;
        }

        private void TuyChinhCKTK_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTuyChonCacKhoanTienKhac(Main);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
