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
    /// Interaction logic for HoaHongKeHoach.xaml
    /// </summary>
    public partial class HoaHongKeHoach : Page, INotifyPropertyChanged
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

        public HoaHongKeHoach(MainWindow main)
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
            dataGrid1.AutoReponsiveColumn(5);
            dataGrid2.AutoReponsiveColumn(2);
            dataGrid1.AutoReponsiveColumn(5);
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = cbThang1.PlaceHolder = cbMonthGR.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = cbNam1.PlaceHolder = cbYearGR.PlaceHolder = "Năm " + year;
            getData(month, year, "");
            getData1();
            getData2("", month, year);
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

        private List<DSNVHHKeHoach> _listNVHHKH;

        public List<DSNVHHKeHoach> listNVHHKH
        {
            get { return _listNVHHKH; }
            set { _listNVHHKH = value; OnPropertyChanged(); }
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
                            web.QueryString.Add("month", month);
                            web.QueryString.Add("year", year);
                            web.QueryString.Add("id_user", user_id);
                        }
                        web.UploadValuesCompleted += (s, e) =>
                        {
                            try
                            {
                                string x = UnicodeEncoding.UTF8.GetString(e.Result);
                                API_DSNVHHKeHoach api = JsonConvert.DeserializeObject<API_DSNVHHKeHoach>(x);
                                if (api.data != null)
                                {
                                    listNVHHKH = api.data.list;
                                    if (listNVHHKH != null)
                                        foreach (var item in listNVHHKH)
                                        {
                                            if (item.ep_image == "/img/add.png")
                                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                        }
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_rose_kehoach.php", web.QueryString);
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

        private List<ListGroup> _listNhom;

        public List<ListGroup> listNhom
        {
            get { return _listNhom; }
            set
            {
                if (value == null) value = new List<ListGroup>();
                value.Insert(0, new ListGroup() { lgr_id = "-1", lgr_name = "Tất cả nhóm" });
                _listNhom = value; OnPropertyChanged();
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
                            listNhom = api.data.list_group;
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        private List<DSNhomHHKeHoach> _listNhomKH;

        public List<DSNhomHHKeHoach> listNhomKH
        {
            get { return _listNhomKH; }
            set
            {
                _listNhomKH = value; OnPropertyChanged();
            }
        }

        private void getData2(string id_gr, string month, string year)
        {
            this.Dispatcher.Invoke(() =>
            {
                using (WebClient web = new WebClient())
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_gr", id_gr);
                    web.QueryString.Add("month", month);
                    web.QueryString.Add("year", year);
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        try
                        {
                            API_DSNhomHHKeHoach api = JsonConvert.DeserializeObject<API_DSNhomHHKeHoach>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listNhomKH = api.data.list;
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
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_group_rose_kehoach.php", web.QueryString);
                }
            });

        }

        private List<DSTongHHKeHoach> _listTongHHKH;

        public List<DSTongHHKeHoach> listTongHHKH
        {
            get { return _listTongHHKH; }
            set { _listTongHHKH = value; OnPropertyChanged(); }
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
                                API_DSTongHHKeHoach api = JsonConvert.DeserializeObject<API_DSTongHHKeHoach>(x);
                                if (api.data != null)
                                {
                                    listTongHHKH = api.data.list;
                                    if (listTongHHKH != null)
                                        foreach (var item in listTongHHKH)
                                        {
                                            if (item.ep_image == "/img/add.png")
                                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                        }
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_total_rose_kehoach.php", web.QueryString);
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

            }
            else
            {
                DockPanel.SetDock(dockHoaHongCaNhan, Dock.Bottom);
                DockPanel.SetDock(dockHoaHongNhom, Dock.Bottom);
            }
        }

        private void btnThemMoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemMoiHoaHongKeHoach(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnQuayLai_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
            Main.title.Text = "Hoa hồng";
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVHHKeHoach data = (DSNVHHKeHoach)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaNVHHKH(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
        int dem = 0;
        private void DG_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            DSNVHHKeHoach data = (DSNVHHKeHoach)cb.DataContext;
            dem++;
            if(dem > listNVHHKH.Count)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id_rose", data.ro_id);
                    web.QueryString.Add("id_kh", data.ro_id_tl);
                    web.QueryString.Add("kpi", cb.SelectedIndex + "");
                    web.QueryString.Add("ghichu_u", data.ro_note);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_rose_kehoach.php", web.QueryString);
                }
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
            if (cbMonthGR.SelectedIndex > -1)
                month = cbMonthGR.Text.Split(' ')[1];
            string year;
            if (cbYearGR.SelectedIndex > -1)
                year = cbYearGR.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_gr = "";
            if (cbGR.SelectedIndex > -1)
            {
                ListGroup gr = (ListGroup)cbGR.SelectedItem;
                if (gr.lgr_id != "-1")
                    id_gr = gr.lgr_id;
            }
            getData2(id_gr, month, year);
        }

        private void Select_Thang2(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang1.SelectedIndex > -1)
                month = cbThang1.Text.Split(' ')[1];
            string year;
            if (cbNam1.SelectedIndex > -1)
                year = cbNam1.Text.Split(' ')[1];
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
        int dem1 = 0;
        private void DG_Changed1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            DSNhomHHKeHoach data = (DSNhomHHKeHoach)cb.DataContext;
            dem1++;
            if (dem1 > listNhomKH.Count)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id_rose", data.ro_id);
                    web.QueryString.Add("id_gr", data.ro_id_group);
                    web.QueryString.Add("id_kh", data.ro_id_tl);
                    web.QueryString.Add("kpi", cb.SelectedIndex + "");
                    web.QueryString.Add("chuky", data.ro_time);
                    for(int i =0;i < data.ep_gr.Count; i++)
                    {
                        web.QueryString.Add("id_user[" + i + "]", data.ep_gr[i].pr_id_user);
                        web.QueryString.Add("percent[" + i + "]", data.ep_gr[i].pr_percent);
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_group_rose_kehoach.php", web.QueryString);
                }
            }
        }

        private void XoaNV(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVHHKeHoach data = (DSNVHHKeHoach)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaNVHHKH(Main, data.ro_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHHKeHoach data = (DSNhomHHKeHoach)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaNVHHKH(Main, data.ro_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSuaNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhomHHKeHoach data = (DSNhomHHKeHoach)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaNhomHHKH(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
