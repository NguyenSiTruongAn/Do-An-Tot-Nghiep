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
    /// Interaction logic for BaoHiem.xaml
    /// </summary>
    public partial class BaoHiem : Page, INotifyPropertyChanged
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
        public BaoHiem(MainWindow main)
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
            dataGrid1.AutoReponsiveColumn(0);
            dataGrid2.AutoReponsiveColumn(0);
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = cbThang1.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = cbNam1.PlaceHolder = "Năm " + year;
            getData();
            getData1(month,year,"");
            getData2();
            getData3(month, year, "", "", 1);
            getData4(month, year, "", "", 1);
            getData5("", month, year);
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
                    catch
                    {

                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
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
            }
        }

        private void getData1(string month, string year, string dep_id)
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
                    try
                    {
                        API_DSNVTheoThoiGian api = JsonConvert.DeserializeObject<API_DSNVTheoThoiGian>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV = api.data.list;
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

        private void getData2()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_dep api =
                        JsonConvert.DeserializeObject<API_List_dep>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listItem_dep = api.data.list;
                        }
                    }
                    catch { }
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
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        private List<ListBaoHiem> _listBaoHiem;

        public List<ListBaoHiem> listBaoHiem
        {
            get { return _listBaoHiem; }
            set { _listBaoHiem = value; OnPropertyChanged(); }
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
                    try
                    {
                        API_ListBaoHiem api = JsonConvert.DeserializeObject<API_ListBaoHiem>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listBaoHiem = api.data.list;
                            listBaoHiem[0].type = "1";
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_insurrance.php", web.QueryString);
            }
        }

        private List<DSNVChuaBH> _listNVChuaBH;

        public List<DSNVChuaBH> listNVChuaBH
        {
            get { return _listNVChuaBH; }
            set { _listNVChuaBH = value; OnPropertyChanged(); }
        }

        private static int totalNVCacNhom;
        private void getData3(string month, string year, string ep_id, string dep_id, int page)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("dp", dep_id);
                    web.QueryString.Add("id", ep_id);
                    web.QueryString.Add("month", month);
                    web.QueryString.Add("year", year);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSNVChuaBH api = JsonConvert.DeserializeObject<API_DSNVChuaBH>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNVChuaBH = api.data.list;
                            totalNVCacNhom = api.data.total;
                            PageNVCacNhom = ListPageNumber(totalNVCacNhom);
                            loadPage(page, PageNVCacNhom);
                            foreach (var item in listNVChuaBH)
                            {
                                if (item.ep_image == "/img/add.png")
                                    item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_no_insurrance.php", web.QueryString);
            }
        }

        private List<DSNVBH> _listNVBH;

        public List<DSNVBH> listNVBH
        {
            get { return _listNVBH; }
            set { _listNVBH = value; OnPropertyChanged(); }
        }
        private static int totalNVCacNhom1;
        private void getData4(string month, string year, string ep_id, string dep_id, int page)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("page", page+"");
                    web.QueryString.Add("dep_id", dep_id);
                    web.QueryString.Add("ep_id", ep_id);
                    web.QueryString.Add("month", month);
                    web.QueryString.Add("year", year);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSNVBH api = JsonConvert.DeserializeObject<API_DSNVBH>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNVBH = api.data.list;
                            totalNVCacNhom1 = api.data.total;
                            PageNVCacNhom1 = ListPageNumber1(totalNVCacNhom1);
                            loadPage1(page, PageNVCacNhom1);
                            foreach (var item in listNVBH)
                            {
                                if (item.ep_image == "")
                                    item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                else item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_insurrance.php", web.QueryString);
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

        private void getData5(string dep_id, string month, string year)
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
                    try
                    {
                        API_DSNVTheoThoiGian api = JsonConvert.DeserializeObject<API_DSNVTheoThoiGian>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV1 = api.data.list;
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/ep_by_time.php", web.QueryString);
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
            getData1(month, year, id_phong);
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
            getData5(id_phong, month, year);
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
                DSNVTheoThoiGian nv = (DSNVTheoThoiGian)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData3(month, year, id_user, id_phong, 1);
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

        private void NhapTienBaoHiem_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListBaoHiem data = (ListBaoHiem)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienVaoBaoHiem(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 495;
        }

        private void BtnTuyChonBaoHiem_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListBaoHiem data = (ListBaoHiem)b.DataContext; 
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTuyChonBaoHiem(Main, data);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVChuaBH data = (DSNVChuaBH)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThietLapBaoHiem(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void ThongKeNVBH(object sender, MouseButtonEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang1.SelectedIndex > -1)
                month = cbThang1.Text.Split(' ')[1];
            string year;
            if (cbNam1.SelectedIndex > -1)
                year = cbNam1 .Text.Split(' ')[1];
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
                DSNVTheoThoiGian nv = (DSNVTheoThoiGian)cbNV1.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData4(month, year, id_user, id_phong, 1);
        }
        public List<int> ListPageNumber(int total)
        {
            int Pages = total / 20;
            if (total % 20 > 0) Pages++;
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
            getData3(month, year, id_user, id_phong, 1);

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
            getData3(month, year, id_user, id_phong, pagenow - 1);
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
            getData3(month, year, id_user, id_phong, pagenow + 1);
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
            getData3(month, year, id_user, id_phong, PageNVCacNhom.Count);
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
            getData3(month, year, id_user, id_phong, pagenumber);
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
            getData3(month, year, id_user, id_phong, pagenumber);
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
            getData3(month, year, id_user, id_phong, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        public List<int> ListPageNumber1(int total)
        {
            int Pages = total / 20;
            if (total % 20 > 0) Pages++;
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
            getData4(month, year, id_user, id_phong, 1);

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
            getData4(month, year, id_user, id_phong, pagenow1 - 1);
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
            getData4(month, year, id_user, id_phong, pagenow1 + 1);
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
            getData4(month, year, id_user, id_phong, PageNVCacNhom1.Count);
        }
        private void select_page_click11(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage11.Text);
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
            getData4(month, year, id_user, id_phong, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click21(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage21.Text);
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
            getData4(month, year, id_user, id_phong, pagenumber);
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
            getData4(month, year, id_user, id_phong, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void TaoMoiBH(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemMoiBH(Main,"","","","",""));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaNV(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVBH data = (DSNVBH)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupXoaNVKhoiBH(Main, data.cls_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(Main));
            Main.SideBarIndex = 0;
        }
    }
}
