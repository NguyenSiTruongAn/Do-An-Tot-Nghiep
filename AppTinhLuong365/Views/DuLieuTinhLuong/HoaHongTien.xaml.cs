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
using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for HoaHongTien.xaml
    /// </summary>
    public partial class HoaHongTien : Page, INotifyPropertyChanged
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
        public HoaHongTien(MainWindow main)
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
            var c = DateTime.Now.Year;
            if (c != null)
            {
                YearList.Add($"Năm {c - 1}");
                YearList.Add($"Năm {c}");
                YearList.Add($"Năm {c + 1}");
            }
            dataGrid1.AutoReponsiveColumn(0);
            dataGrid2.AutoReponsiveColumn(2);
            dataGrid3.AutoReponsiveColumn(0);
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            cbThang1.PlaceHolder = "Tháng " + month;
            cbNam1.PlaceHolder = "Năm " + year;
            cbThang2.PlaceHolder = "Tháng " + month;
            cbNam2.PlaceHolder = "Năm " + year;
            getData(month, year, "");
            getData1();
            getData2(month, year, "");
            getData3();
            getData4(month, year, "");
            getDataTB();
        }

        private void getDataTB()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("cp", "2");
                }
                if (Main.MainType == 1)
                {
                    web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                    web.QueryString.Add("token", Main.CurrentEmployee.token);
                    web.QueryString.Add("cp", "1");
                    web.QueryString.Add("user_nhan", Main.CurrentEmployee.ep_id);
                }

                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ThongBaoCT api = JsonConvert.DeserializeObject<API_ThongBaoCT>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            Main.listTB = api.data.abc;
                            if (Main.listTB != null)
                                Main.sotb = Main.listTB.Count;
                            if (Main.sotb >= 10)
                            {
                                Main.fontsize = 10;
                                Main.margin = new Thickness(10, -7, 0, 0);
                            }
                            else
                            {
                                Main.fontsize = 14;
                                Main.margin = new Thickness(12.5, -10.5, 0, 0);
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        private List<DSNVHoaHongTien> _listNVHHTien;

        public List<DSNVHoaHongTien> listNVHHTien
        {
            get { return _listNVHHTien; }
            set { _listNVHHTien = value; OnPropertyChanged(); }
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
                            try
                            {
                                string x = UnicodeEncoding.UTF8.GetString(e.Result);
                                API_DSNVHoaHongTien api = JsonConvert.DeserializeObject<API_DSNVHoaHongTien>(x);
                                if (api.data != null)
                                {
                                    listNVHHTien = api.data.list_rose;
                                    if (listNVHHTien != null)
                                        foreach (var item in listNVHHTien)
                                        {
                                            if (item.ep_image == "/img/add.png")
                                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                        }
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_rose_tien.php", web.QueryString);
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
                    try
                    {
                        API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data.data != null)
                        {
                            listNV = api.data.data.items;
                        }
                    }
                    catch { }
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

        private List<DSNhomHoaHongTien> _listNhomHHTien;

        public List<DSNhomHoaHongTien> listNhomHHTien
        {
            get { return _listNhomHHTien; }
            set { _listNhomHHTien = value; OnPropertyChanged(); }
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
                            web.QueryString.Add("time", year + "-" + month);
                            web.QueryString.Add("id_group", gr_id);
                        }
                        web.UploadValuesCompleted += (s, e) =>
                        {
                            try
                            {
                                string x = UnicodeEncoding.UTF8.GetString(e.Result);
                                API_DSNhomHoaHongTien api = JsonConvert.DeserializeObject<API_DSNhomHoaHongTien>(x);
                                if (api.data != null)
                                {
                                    listNhomHHTien = api.data.list_rose_gr;
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_group_rose_tien.php", web.QueryString);
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
                    try
                    {
                        API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listGR = api.data.list_group;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        private List<DSTongHoaHongTien> _listTongHHTien;

        public List<DSTongHoaHongTien> listTongHHTien
        {
            get { return _listTongHHTien; }
            set { _listTongHHTien = value; OnPropertyChanged(); }
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
                            try
                            {
                                string x = UnicodeEncoding.UTF8.GetString(e.Result);
                                API_DSTongHoaHongTien api = JsonConvert.DeserializeObject<API_DSTongHoaHongTien>(x);
                                if (api.data != null)
                                {
                                    listTongHHTien = api.data.list;
                                    if (listTongHHTien != null)
                                        foreach (var item in listTongHHTien)
                                        {
                                            if (item.ep_image == "/img/add.png")
                                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                        }
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_total_rose_tien.php", web.QueryString);
                    }
                });
            }
            catch
            {

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
                DockPanel.SetDock(dockTongHoaHong, Dock.Right);

            }
            else
            {
                DockPanel.SetDock(dockHoaHongCaNhan, Dock.Bottom);
                DockPanel.SetDock(dockHoaHongNhom, Dock.Bottom);
                DockPanel.SetDock(dockTongHoaHong, Dock.Bottom);
            }
        }

        private void btnThemMoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemMoiHoaHongTien(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnQuayLai_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVHoaHongTien data = (DSNVHoaHongTien)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaHoaHongTien(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSuaNhomHoaHongTien(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHoaHongTien data = (DSNhomHoaHongTien)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaNhomHoaHongTien(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnXoaHoaHongTien_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVHoaHongTien data = (DSNVHoaHongTien)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaHHT(Main, data.ro_id, "1");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnXoaHoaHongTien1_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHoaHongTien data = (DSNhomHoaHongTien)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaHHT(Main, data.ro_id, "0");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XemGhiCHu(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVHoaHongTien data = (DSNVHoaHongTien)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupMieuTaChiTietHoaHongTien(Main, data.ro_note));
            Main.PopupSelection.Visibility = Visibility.Visible;
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
                year = cbNam1.Text.Split(' ')[1];
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

        private void XemGhiCHu1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHoaHongTien data = (DSNhomHoaHongTien)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupMieuTaChiTietHoaHongTien(Main, data.ro_note));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
