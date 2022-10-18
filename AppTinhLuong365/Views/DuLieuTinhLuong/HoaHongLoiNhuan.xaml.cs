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
    /// Interaction logic for HoaHongLoiNhuan.xaml
    /// </summary>
    public partial class HoaHongLoiNhuan : Page, INotifyPropertyChanged
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

        public HoaHongLoiNhuan(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
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
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            cbThang1.PlaceHolder = "Tháng " + month;
            cbNam1.PlaceHolder = "Năm " + year;
            cbThang2.PlaceHolder = "Tháng " + month;
            cbNam2.PlaceHolder = "Năm " + year;
            Main = main;
            getData(month,year,"");
            getData1();
            getData2(month, year, "");
            getData3();
            getData4(month, year, "");
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private List<DSNVHoaHongLoiNhuan> _listNVHHLoiNhuan;

        public List<DSNVHoaHongLoiNhuan> listNVHHLoiNhuan
        {
            get { return _listNVHHLoiNhuan; }
            set { _listNVHHLoiNhuan = value; OnPropertyChanged(); }
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
                            API_DSNVHoaHongLoiNhuan api = JsonConvert.DeserializeObject<API_DSNVHoaHongLoiNhuan>(x);
                            if (api.data != null)
                            {
                                listNVHHLoiNhuan = api.data.list_rose;
                                if (listNVHHLoiNhuan != null)
                                    foreach (var item in listNVHHLoiNhuan)
                                    {
                                        if (item.ep_image == "/img/add.png")
                                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                    }
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_rose_loinhuan.php", web.QueryString);
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

        private List<DSNhomHHLoiNhuan> _listNhomHHLoiNhuan;

        public List<DSNhomHHLoiNhuan> listNhomHHLoiNhuan
        {
            get { return _listNhomHHLoiNhuan; }
            set { _listNhomHHLoiNhuan = value; OnPropertyChanged(); }
        }

        private void getData2(string month, string year, string gr_id)
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
                            web.QueryString.Add("time", year + "-" + month + "-01");
                            web.QueryString.Add("id_gr", gr_id);
                        }
                        web.UploadValuesCompleted += (s, e) =>
                        {
                            string x = UnicodeEncoding.UTF8.GetString(e.Result);
                            API_DSNhomHHLoiNhuan api = JsonConvert.DeserializeObject<API_DSNhomHHLoiNhuan>(x);
                            if (api.data != null)
                            {
                                listNhomHHLoiNhuan = api.data.list_rose;
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_group_rose_loinhuan.php", web.QueryString);
                    }
                });
            }
            catch
            {

            }
        }

        private List<ListGroup> _listGR;

        public List<ListGroup> listGR
        {
            get { return _listGR; }
            set
            {
                _listGR = value; OnPropertyChanged();
            }
        }

        private void getData3()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listGR = api.data.list_group;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        private List<DSTongHHLoiNhuan> _listTongHHLoiNhuan;

        public List<DSTongHHLoiNhuan> listTongHHLoiNhuan
        {
            get { return _listTongHHLoiNhuan; }
            set { _listTongHHLoiNhuan = value; OnPropertyChanged(); }
        }

        private void getData4(string month, string year, string user_id)
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
                            web.QueryString.Add("month", month);
                            web.QueryString.Add("year", year);
                            web.QueryString.Add("id_user", user_id);
                        }
                        web.UploadValuesCompleted += (s, e) =>
                        {
                            string x = UnicodeEncoding.UTF8.GetString(e.Result);
                            API_DSTongHHLoiNhuan api = JsonConvert.DeserializeObject<API_DSTongHHLoiNhuan>(x);
                            if (api.data != null)
                            {
                                listTongHHLoiNhuan = api.data.list;
                                if (listTongHHLoiNhuan != null)
                                    foreach (var item in listTongHHLoiNhuan)
                                    {
                                        if (item.ep_image == "/img/add.png")
                                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                    }
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_total_rose_loinhuan.php", web.QueryString);
                    }
                });
            }
            catch
            {

            }
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

        private void Select_Thang1(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang1.SelectedIndex > -1)
                month = cbThang1.Text.Split(' ')[1];
            string year;
            if (cbNam1.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_gr = "";
            if (cbGR.SelectedIndex > -1)
            {
                ListGroup gr = (ListGroup)cbGR.SelectedItem;
                id_gr = gr.lgr_id;
            }
            getData2(month, year, id_gr);
        }

        private void Select_Thang2(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang2.SelectedIndex > -1)
                month = cbThang2.Text.Split(' ')[1];
            string year;
            if (cbNam2.SelectedIndex > -1)
                year = cbNam2.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "";
            if (cbNV1.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV1.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData4(month, year, id_user);
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
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemMoiHoaHongLoiNhuan(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
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
            Border b = sender as Border;
            DSNVHoaHongLoiNhuan data = (DSNVHoaHongLoiNhuan)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.PopupChinhSuaNVHHLoiNhuan(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaNV(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVHoaHongLoiNhuan data = (DSNVHoaHongLoiNhuan)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaNVHHLN(Main, data.ro_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHHLoiNhuan data = (DSNhomHHLoiNhuan)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaNVHHLN(Main, data.ro_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSuaNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHHLoiNhuan data = (DSNhomHHLoiNhuan)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaNhomHHLN(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
