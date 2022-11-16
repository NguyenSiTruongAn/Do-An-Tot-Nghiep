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
            string moth = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + moth;
            cbNam.PlaceHolder = "Năm " + year;
            getData();
            getData1("0", moth, year, 1);
            getData2();
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

        private List<DSCacKhoanTienKhac> _listCacKhoanTienKhac;

        public List<DSCacKhoanTienKhac> listCacKhoanTienKhac
        {
            get { return _listCacKhoanTienKhac; }
            set {
                _listCacKhoanTienKhac = value;
                OnPropertyChanged();
                lv.ItemsSource = null;
                lv.Items.Clear();
            }
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
                        API_DSCacKhoanTienKhac api = JsonConvert.DeserializeObject<API_DSCacKhoanTienKhac>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            if(listCacKhoanTienKhac != null)
                            {
                                listCacKhoanTienKhac.Clear();
                            }
                            listCacKhoanTienKhac = api.data.other_list;
                        }
                    }
                    catch { }
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
        private static int totalNV;
        private void getData1(string id_user, string month, string year, int page)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("page", page + "");
                    web.QueryString.Add("time", year + "-" + month);
                    web.QueryString.Add("id_user", id_user);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSNVCacKhoanThienKhac api = JsonConvert.DeserializeObject<API_DSNVCacKhoanThienKhac>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNVCacKhoanTienKhac = api.data.ep_other_list;
                            totalNV = api.data.count;
                            PageNV = ListPageNumber(totalNV);
                            loadPage(page, PageNV);
                            foreach (var item in listNVCacKhoanTienKhac)
                            {
                                if (item.ep_image == "/img/add.png")
                                    item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_otherMoney.php", web.QueryString);
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

        private void getData2()
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
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemKhoanTienKhac(Main, "", "", "", "", "");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 495;
            pop.Height = 433;
        }

        private void TuyChinhCKTK_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSCacKhoanTienKhac data = (DSCacKhoanTienKhac)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTuyChonCacKhoanTienKhac(Main, data.cl_id, data.cl_name, data.cl_note, data.fs_repica, data.fs_name, data.fs_data, data.cl_id_form);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Select_NV (object sender, SelectionChangedEventArgs e)
        {
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
            string id_user = nv.ep_id;
            getData1(id_user, month, year, 1);
        }
        private void Select_Thang(object sender, SelectionChangedEventArgs e)
        {
            string month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if(nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, 1);
        }
        private void Select_Nam(object sender, SelectionChangedEventArgs e)
        {
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year = cbNam.Text.Split(' ')[1];
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, 1);
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

        private List<int> _PageNV;

        public List<int> PageNV
        {
            get { return _PageNV; }
            set { _PageNV = value; OnPropertyChanged(); }
        }

        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage1.Text);
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage2.Text);
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage3.Text);
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private int pagenow;

        private void loadPage(int pagenb, List<int> loaiPage)
        {
            this.Dispatcher.Invoke(() =>
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
                    PageTiep .Visibility = Visibility.Collapsed;
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
            });
        }

        private void ve_page_1(object sender, MouseButtonEventArgs e)
        {
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user,month,year,1);
            BrushConverter bc = new BrushConverter();
            Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            txtpage1.Text = "1";
            txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        }

        private void page_truoc_click(object sender, MouseButtonEventArgs e)
        {
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year ,pagenow - 1);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
        {
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, pagenow + 1);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
        {
            string month;
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            else month = DateTime.Now.ToString("MM");
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_user = "0";
            if (cbNV.SelectedIndex > -1)
            {
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData1(id_user, month, year, PageNV.Count);
        }

        private void XoaNhanVien(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVCacKhoanThienKhac data = (DSNVCacKhoanThienKhac)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaNVKhoiCKTK(Main, data.cls_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
