using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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
using System.Windows.Media;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for Thue.xaml
    /// </summary>
    public partial class Thue : Page, INotifyPropertyChanged
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
        public Thue(MainWindow main)
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
            for (var i = 2021; i <= int.Parse(DateTime.Now.ToString("yyyy")) + 1; i++)
            {
                YearList.Add($"Năm {i}");
            }
            dataGrid1.AutoReponsiveColumn(1);
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbNam.PlaceHolder = cbNam1.PlaceHolder = "Năm " + DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = cbThang1.PlaceHolder = "Tháng " + DateTime.Now.ToString("MM");
            getData(month, year, "", 1);
            getData1(month, year, "", 1);
            getData2();
            getData3("",month, year);
            getData4();
            getData5("",month,year);
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private List<ChinhSachThue> _listCSThue;

        public List<ChinhSachThue> listCSThue
        {
            get { return _listCSThue; }
            set { _listCSThue = value; OnPropertyChanged(); }
        }

        private void getData2()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ChinhSachThue api = JsonConvert.DeserializeObject<API_ChinhSachThue>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listCSThue = api.data.tax_list;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_tax_manager.php", web.QueryString);
            }
        }

        private List<DSNVTheoThoiGian> _listNV;

        public List<DSNVTheoThoiGian> listNV
        {
            get { return _listNV; }
            set
            {
                if (value == null) value = new List<DSNVTheoThoiGian>();
                value.Insert(0, new DSNVTheoThoiGian() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listNV = value;
                cbNV.ItemsSource = _listNV;
                //if (cbNV != null)
                //{
                //    cbNV.Refresh();
                //}
                //OnPropertyChanged();
            }
        }

        private void getData3(string dep_id, string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_dep", dep_id);
                web.QueryString.Add("active", "true");
                web.QueryString.Add("start_date", year + "-" + month + "-01");
                int x = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
                web.QueryString.Add("date", year + "-" + month + "-" + x);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSNVTheoThoiGian api = JsonConvert.DeserializeObject<API_DSNVTheoThoiGian>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNV = api.data.list;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/ep_by_time.php", web.QueryString);
            }
        }

        private List<DSNVTheoThoiGian> _listNV1;
        public List<DSNVTheoThoiGian> listNV1
        {
            get { return _listNV1; }
            set
            {
                if (value == null) value = new List<DSNVTheoThoiGian>();
                value.Insert(0, new DSNVTheoThoiGian() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listNV1 = value;
                cbNV1.ItemsSource = _listNV1;
            }
        }

        private void getData5(string dep_id,string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_dep", dep_id);
                web.QueryString.Add("active", "true");
                web.QueryString.Add("start_date", year + "-" + month + "-01");
                int x = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
                web.QueryString.Add("date", year + "-" + month + "-" + x);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSNVTheoThoiGian api = JsonConvert.DeserializeObject<API_DSNVTheoThoiGian>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNV1 = api.data.list;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/ep_by_time.php", web.QueryString);
            }
        }

        private List<Item_dep> _listItem_dep;

        public List<Item_dep> listItem_dep
        {
            get { return _listItem_dep; }
            set
            {
                if (value == null) value = new List<Item_dep>();
                value.Insert(0, new Item_dep() { dep_id = "-1", dep_name = "Phòng ban (tất cả)" });
                _listItem_dep = value;
                OnPropertyChanged();
            }
        }

        private void getData4()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_dep api =
                        JsonConvert.DeserializeObject<API_List_dep>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listItem_dep = api.data.list;
                    }
                    // foreach (EpLate item in listEpLate)
                    // {
                    //     if (item.ts_image != "/img/add.png")
                    //     {
                    //         item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_dep.php", web.QueryString);
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
                DockPanel.SetDock(dockThueNSCTL, Dock.Right);
                DockPanel.SetDock(dockThueNSDTL, Dock.Right);

            }
            else
            {
                DockPanel.SetDock(dockThueNSCTL, Dock.Bottom);
                DockPanel.SetDock(dockThueNSDTL, Dock.Bottom);
            }
        }

        private void btnTaoMoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupThue(Main, "", "", "", "", "");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 433;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ItemThue data = (ItemThue)b.DataContext;
            var pop = new Views.TinhLuong.PopupThietLapThue(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 482;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ItemUserTax data = (ItemUserTax)b.DataContext;
            var pop = new Views.TinhLuong.PopupTGADThue(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 327;
        }

        private void TuyChonChinhSachThue_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ChinhSachThue data = (ChinhSachThue)b.DataContext;
            var pop = new Views.TinhLuong.PopupTuyChonCSThue(Main, data);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<ItemThue> _listThue;

        public List<ItemThue> listThue
        {
            get { return _listThue; }
            set { _listThue = value; OnPropertyChanged(); }
        }

        private static int totalNVCacNhom;
        private void getData(string month, string year, string ep_id, int page)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("id_month", month);
                web.QueryString.Add("id_year", year);
                web.QueryString.Add("id_user", ep_id);
                web.QueryString.Add("page", page +"");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListEmployeeChuaThietLapThue api = JsonConvert.DeserializeObject<API_ListEmployeeChuaThietLapThue>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listThue = api.data.list;
                        totalNVCacNhom = api.data.total;
                        PageNVCacNhom = ListPageNumber(totalNVCacNhom);
                        loadPage(page, PageNVCacNhom);
                    }
                    foreach (ItemThue item in listThue)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_no_tax.php", web.QueryString);
            }
        }

        private List<ItemUserTax> _listUserTax;

        public List<ItemUserTax> listUserTax
        {
            get { return _listUserTax; }
            set { _listUserTax = value; OnPropertyChanged(); }
        }

        private static int totalNVCacNhom1;
        private void getData1(string month, string year, string ep_id, int page)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("id_user", ep_id);
                web.QueryString.Add("time", year + "-" + month + "-01");
                web.QueryString.Add("page", page+"");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListUserTax api = JsonConvert.DeserializeObject<API_ListUserTax>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listUserTax = api.data.list;
                        totalNVCacNhom1 = api.data.total;
                        PageNVCacNhom1 = ListPageNumber1(totalNVCacNhom1);
                        loadPage1(page, PageNVCacNhom1);
                    }
                    foreach (ItemUserTax item in listUserTax)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }

                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_tax.php", web.QueryString);
            }
        }

        private void ChonPhong(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            getData3(id_phong,month,year);
        }

        private void ChonPhong1(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang1.SelectedIndex > -1)
                month = cbThang1.Text.Split(' ')[1];
            string year;
            if (cbNam1.SelectedIndex > -1)
                year = cbNam1.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong1.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong1.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            getData5(id_phong,month,year);
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
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
            getData1(month, year, id_user, 1);
        }
        private void ThongKeNVBH(object sender, MouseButtonEventArgs e)
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
            getData(month, year, id_user, 1);
        }
        public List<int> ListPageNumber(int total)
        {
            int Pages = total / 10;
            if (total % 10 > 0) Pages++;
            List<int> pnb = new List<int>();
            for (int i = 1; i <= Pages; i++)
            {
                pnb.Add(i);
            }
            return pnb;
        }

        private List<int> _PageNVCacNhom;

        public List<int> PageNVCacNhom
        {
            get { return _PageNVCacNhom; }
            set { _PageNVCacNhom = value; OnPropertyChanged(); }
        }

        private int pagenow;

        private void loadPage(int pagenb, List<int> loaiPage)
        {
            BrushConverter bc = new BrushConverter();
            pagenow = pagenb;
            if (pagenb == 1)
            {
                Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page2.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page3.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage1.Text = "1";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
                PageTruoc.Visibility = Visibility.Collapsed;
                PageDau.Visibility = Visibility.Collapsed;
                Page1.Visibility = Visibility.Visible;
                if (loaiPage.Count > 3)
                {
                    PageCuoi.Visibility = Visibility.Visible;
                    txtpage3.Text = "3";
                    txtpage2.Text = "2";
                    PageTiep.Visibility = Visibility.Visible;
                    Page3.Visibility = Visibility.Visible;
                    Page2.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 2)
                {
                    txtpage3.Text = "3";
                    txtpage2.Text = "2";
                    PageTiep.Visibility = Visibility.Visible;
                    Page3.Visibility = Visibility.Visible;
                    Page2.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 1)
                {
                    Page3.Visibility = Visibility.Collapsed;
                    PageTiep.Visibility = Visibility.Visible;
                    txtpage2.Text = "2";
                    Page2.Visibility = Visibility.Visible;
                    PageCuoi.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PageTiep.Visibility = Visibility.Collapsed;
                    Page3.Visibility = Visibility.Collapsed;
                    PageCuoi.Visibility = Visibility.Collapsed;
                    Page2.Visibility = Visibility.Collapsed;
                }
            }
            else if (pagenb == loaiPage.Count)
            {
                Page3.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page2.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page1.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage3.Text = pagenb + "";
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                Page3.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Collapsed;
                PageCuoi.Visibility = Visibility.Collapsed;
                if (loaiPage.Count > 3)
                {
                    txtpage2.Text = (pagenb - 1) + "";
                    txtpage1.Text = (pagenb - 2) + "";
                    Page2.Visibility = Visibility.Visible;
                    PageDau.Visibility = Visibility.Visible;
                    PageTruoc.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 2)
                {
                    txtpage2.Text = "2";
                    txtpage1.Text = "1";
                    Page2.Visibility = Visibility.Visible;
                    PageTruoc.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                    PageDau.Visibility = Visibility.Collapsed;

                }
                else if (loaiPage.Count > 1)
                {
                    Page1.Visibility = Visibility.Collapsed;
                    txtpage2.Text = "1";
                    Page2.Visibility = Visibility.Visible;
                    PageTruoc.Visibility = Visibility.Visible;
                    PageDau.Visibility = Visibility.Collapsed;
                }
            }
            else if (pagenb == loaiPage.Count - 1)
            {
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page1.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage2.Text = pagenb + "";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                Page2.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Visible;
                PageCuoi.Visibility = Visibility.Collapsed;
                PageTruoc.Visibility = Visibility.Visible;
                Page3.Visibility = Visibility.Visible;
                Page1.Visibility = Visibility.Visible;
                txtpage3.Text = (pagenb + 1) + "";
                txtpage1.Text = (pagenb - 1) + "";
                if (loaiPage.Count > 3) PageDau.Visibility = Visibility.Visible;
                else PageDau.Visibility = Visibility.Collapsed;
            }
            else if (pagenb == 2)
            {
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page1.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage2.Text = "2";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Text = "1";
                txtpage3.Text = "3";
                PageTruoc.Visibility = Visibility.Visible;
                PageDau.Visibility = Visibility.Collapsed;
                Page1.Visibility = Visibility.Visible;
                Page2.Visibility = Visibility.Visible;
                Page3.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Visible;
                if (loaiPage.Count > 3) PageCuoi.Visibility = Visibility.Visible;
                else PageCuoi.Visibility = Visibility.Collapsed;
            }
            else
            {
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page1.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage2.Text = pagenb + "";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Text = (pagenb - 1) + "";
                txtpage3.Text = (pagenb + 1) + "";
                PageTruoc.Visibility = Visibility.Visible;
                PageDau.Visibility = Visibility.Visible;
                PageCuoi.Visibility = Visibility.Visible;
                Page1.Visibility = Visibility.Visible;
                Page2.Visibility = Visibility.Visible;
                Page3.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Visible;
            }
        }
        private void ve_page_1(object sender, MouseButtonEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, 1);

            BrushConverter bc = new BrushConverter();
            Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            Page2.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            Page3.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            txtpage1.Text = "1";
            txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
            txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
            txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
        }

        private void page_truoc_click(object sender, MouseButtonEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, pagenow - 1);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, pagenow + 1);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, PageNVCacNhom.Count);
        }
        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage1.Text);
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage2.Text);
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage3.Text);
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(month, year, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        public List<int> ListPageNumber1(int total)
        {
            int Pages = total / 10;
            if (total % 10 > 0) Pages++;
            List<int> pnb = new List<int>();
            for (int i = 1; i <= Pages; i++)
            {
                pnb.Add(i);
            }
            return pnb;
        }

        private List<int> _PageNVCacNhom1;

        public List<int> PageNVCacNhom1
        {
            get { return _PageNVCacNhom1; }
            set { _PageNVCacNhom1 = value; OnPropertyChanged(); }
        }

        private int pagenow1;

        private void loadPage1(int pagenb, List<int> loaiPage)
        {
            BrushConverter bc = new BrushConverter();
            pagenow1 = pagenb;
            if (pagenb == 1)
            {
                Page11.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page21.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page31.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage11.Text = "1";
                txtpage11.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage21.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage31.Foreground = (Brush)bc.ConvertFrom("#444");
                PageTruoc1.Visibility = Visibility.Collapsed;
                PageDau1.Visibility = Visibility.Collapsed;
                Page11.Visibility = Visibility.Visible;
                if (loaiPage.Count > 3)
                {
                    PageCuoi1.Visibility = Visibility.Visible;
                    txtpage31.Text = "3";
                    txtpage21.Text = "2";
                    PageTiep1.Visibility = Visibility.Visible;
                    Page31.Visibility = Visibility.Visible;
                    Page21.Visibility = Visibility.Visible;
                    Page11.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 2)
                {
                    txtpage31.Text = "3";
                    txtpage21.Text = "2";
                    PageTiep1.Visibility = Visibility.Visible;
                    Page31.Visibility = Visibility.Visible;
                    Page21.Visibility = Visibility.Visible;
                    Page11.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 1)
                {
                    Page31.Visibility = Visibility.Collapsed;
                    PageTiep1.Visibility = Visibility.Visible;
                    txtpage21.Text = "2";
                    Page21.Visibility = Visibility.Visible;
                    PageCuoi1.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PageTiep1.Visibility = Visibility.Collapsed;
                    Page31.Visibility = Visibility.Collapsed;
                    PageCuoi1.Visibility = Visibility.Collapsed;
                    Page21.Visibility = Visibility.Collapsed;
                }
            }
            else if (pagenb == loaiPage.Count)
            {
                Page31.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page21.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page11.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage31.Text = pagenb + "";
                txtpage31.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage21.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage11.Foreground = (Brush)bc.ConvertFrom("#444");
                Page31.Visibility = Visibility.Visible;
                PageTiep1.Visibility = Visibility.Collapsed;
                PageCuoi1.Visibility = Visibility.Collapsed;
                if (loaiPage.Count > 3)
                {
                    txtpage21.Text = (pagenb - 1) + "";
                    txtpage11.Text = (pagenb - 2) + "";
                    Page21.Visibility = Visibility.Visible;
                    PageDau1.Visibility = Visibility.Visible;
                    PageTruoc1.Visibility = Visibility.Visible;
                    Page11.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 2)
                {
                    txtpage21.Text = "2";
                    txtpage11.Text = "1";
                    Page21.Visibility = Visibility.Visible;
                    PageTruoc1.Visibility = Visibility.Visible;
                    Page11.Visibility = Visibility.Visible;
                    PageDau1.Visibility = Visibility.Collapsed;

                }
                else if (loaiPage.Count > 1)
                {
                    Page11.Visibility = Visibility.Collapsed;
                    txtpage21.Text = "1";
                    Page21.Visibility = Visibility.Visible;
                    PageTruoc1.Visibility = Visibility.Visible;
                    PageDau1.Visibility = Visibility.Collapsed;
                }
            }
            else if (pagenb == loaiPage.Count - 1)
            {
                Page21.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page31.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page11.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage21.Text = pagenb + "";
                txtpage21.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage31.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage11.Foreground = (Brush)bc.ConvertFrom("#444");
                Page21.Visibility = Visibility.Visible;
                PageTiep1.Visibility = Visibility.Visible;
                PageCuoi1.Visibility = Visibility.Collapsed;
                PageTruoc1.Visibility = Visibility.Visible;
                Page31.Visibility = Visibility.Visible;
                Page11.Visibility = Visibility.Visible;
                txtpage31.Text = (pagenb + 1) + "";
                txtpage11.Text = (pagenb - 1) + "";
                if (loaiPage.Count > 3) PageDau1.Visibility = Visibility.Visible;
                else PageDau1.Visibility = Visibility.Collapsed;
            }
            else if (pagenb == 2)
            {
                Page21.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page31.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page11.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage21.Text = "2";
                txtpage21.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage31.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage11.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage11.Text = "1";
                txtpage31.Text = "3";
                PageTruoc1.Visibility = Visibility.Visible;
                PageDau1.Visibility = Visibility.Collapsed;
                Page11.Visibility = Visibility.Visible;
                Page21.Visibility = Visibility.Visible;
                Page31.Visibility = Visibility.Visible;
                PageTiep1.Visibility = Visibility.Visible;
                if (loaiPage.Count > 3) PageCuoi1.Visibility = Visibility.Visible;
                else PageCuoi1.Visibility = Visibility.Collapsed;
            }
            else
            {
                Page21.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page31.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page11.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage21.Text = pagenb + "";
                txtpage21.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage31.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage11.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage11.Text = (pagenb - 1) + "";
                txtpage31.Text = (pagenb + 1) + "";
                PageTruoc1.Visibility = Visibility.Visible;
                PageDau1.Visibility = Visibility.Visible;
                PageCuoi1.Visibility = Visibility.Visible;
                Page11.Visibility = Visibility.Visible;
                Page21.Visibility = Visibility.Visible;
                Page31.Visibility = Visibility.Visible;
                PageTiep1.Visibility = Visibility.Visible;
            }
        }
        private void ve_page_11(object sender, MouseButtonEventArgs e)
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
            getData1(month, year, id_user, 1);

            BrushConverter bc = new BrushConverter();
            Page11.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            Page21.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            Page31.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            txtpage11.Text = "1";
            txtpage11.Foreground = (Brush)bc.ConvertFrom("#ffffff");
            txtpage21.Foreground = (Brush)bc.ConvertFrom("#444");
            txtpage31.Foreground = (Brush)bc.ConvertFrom("#444");
        }

        private void page_truoc_click1(object sender, MouseButtonEventArgs e)
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
            getData1(month, year, id_user, pagenow1 - 1);
        }

        private void page_sau_click1(object sender, MouseButtonEventArgs e)
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
            getData1(month, year, id_user, pagenow1 + 1);
        }

        private void page_cuoi_click1(object sender, MouseButtonEventArgs e)
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
            getData1(month, year, id_user, PageNVCacNhom1.Count);
        }
        private void select_page_click11(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage11.Text);
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
            getData1(month, year, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click21(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage21.Text);
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
            getData1(month, year, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click31(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage31.Text);
            string month = DateTime.Now.ToString("MM");
            if (cbThang1.SelectedIndex > -1)
                month = cbThang1.Text.Split(' ')[1];
            string year;
            if (cbNam1.SelectedIndex > -1)
                year = cbNam1.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong1.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong1.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV1.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV1.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(month, year, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void XoaNV(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ItemUserTax data = (ItemUserTax)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.Popup.PopupXoaNVKhoiThue(Main, data.cls_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
