using AppTinhLuong365.Core;
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
    /// Interaction logic for HoaHongDoanhThu.xaml
    /// </summary>
    public partial class HoaHongDoanhThu : Page, INotifyPropertyChanged
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

        public HoaHongDoanhThu(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid1.AutoReponsiveColumn(1);
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

            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            getData(month, year, "");
            getData1();
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private List<DSNVHoaHongDoanhThu> _listNVHHDoanhThu;

        public List<DSNVHoaHongDoanhThu> listNVHHDoanhThu
        {
            get { return _listNVHHDoanhThu; }
            set { _listNVHHDoanhThu = value; OnPropertyChanged(); }
        }

        private void getData(string month, string year, string user_id)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    using (WebClient web = new WebClient())
                    {
                        if (Main.MainType == 0)
                        {
                            web.QueryString.Add("token", Main.CurrentCompany.token);
                            web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                            web.QueryString.Add("time", year + "-" + month);
                            web.QueryString.Add("id_user", user_id);
                        }
                        web.UploadValuesCompleted += (s, e) =>
                        {
                            string x = UnicodeEncoding.UTF8.GetString(e.Result);
                            API_DSNVHoaHongDoanhThu api = JsonConvert.DeserializeObject<API_DSNVHoaHongDoanhThu>(x);
                            if (api.data != null)
                            {
                                listNVHHDoanhThu = api.data.list_rose;
                                if (listNVHHDoanhThu != null)
                                    foreach (var item in listNVHHDoanhThu)
                                    {
                                        if (item.ep_image == "/img/add.png")
                                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                    }
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_rose_dt.php", web.QueryString);
                    }
                });
            }
            catch
            {

            }
        }

        private List<ListEmployee> _listNV;

        public List<ListEmployee> listNV
        {
            get { return _listNV; }
            set
            {
                if (value == null) value = new List<ListEmployee>();
                value.Insert(0, new ListEmployee() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listNV = value; OnPropertyChanged();
            }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data.data != null)
                    {
                        listNV = api.data.data.items;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php", web.QueryString);
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
            if (this.ActualWidth > 1630)
            {
                DockPanel.SetDock(dockHoaHongCaNhan, Dock.Right);
                DockPanel.SetDock(dockHoaHongNhom, Dock.Right);

            }
            else
            {
                DockPanel.SetDock(dockHoaHongCaNhan, Dock.Bottom);
                DockPanel.SetDock(dockHoaHongNhom, Dock.Bottom);
            }
        }

        private void btnThemMoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemMoiHoaHongDoanhThu(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            if (pop.tabitem1.IsSelected == true)
                pop.Height = 571;
            else
                pop.Height = 728;
        }

        private void btnQuayLai_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            
            /*Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaHoaHongTien(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;*/
        }

        private void btnXoaHoaHongTien_Click(object sender, MouseButtonEventArgs e)
        {
            /*var pop = new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaHHT(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 203;*/
        }

        private void Select_Thang(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user);
        }
    }
}
