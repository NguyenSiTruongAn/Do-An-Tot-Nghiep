using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Aspose.Cells;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace AppTinhLuong365.Views
{
    /// <summary>
    /// Interaction logic for PageNhapLuongCoBanVaCheDo.xaml
    /// </summary>
    public partial class PageNhapLuongCoBanVaCheDo : Page,INotifyPropertyChanged
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
        public PageNhapLuongCoBanVaCheDo(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData(DateTime.Now.ToString("yyyy-MM-dd"),"","",1);
            getData1("", DateTime.Now.ToString("yyyy-MM-dd"));
            getData2();
            selectDate.SelectedDate = DateTime.Now;
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
            if (this.ActualWidth > 1330)
                DockPanel.SetDock(wrapLCBVCD, Dock.Right);
            else
                DockPanel.SetDock(wrapLCBVCD, Dock.Bottom);
        }
        private List<ItemEmp> _listNhanVien;

        public List<ItemEmp> listNhanVien
        {
            get { return _listNhanVien; }
            set { _listNhanVien = value;OnPropertyChanged(); }
        }
        private static int totalNVCacNhom;
        private void getData(string time, string dep, string user, int page)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("page", page+"");
                web.QueryString.Add("time", time);
                web.QueryString.Add("id_dep", dep);
                web.QueryString.Add("id_emp", user);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_Tbl_Salary_Manager api = JsonConvert.DeserializeObject<API_Tbl_Salary_Manager>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNhanVien = api.data.list_emp;
                            totalNVCacNhom = api.data.total_item;
                            PageNVCacNhom = ListPageNumber(totalNVCacNhom);
                            loadPage(page, PageNVCacNhom);
                        }
                        foreach (ItemEmp item in listNhanVien)
                        {
                            if (item.ep_image == "")
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                            else
                                item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_salary_manager.php", web.QueryString);
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://phanmemnhansu.timviec365.vn/bien-dong-nhan-su.html");
        }
        public class abc : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public string name { get; set; }
            private int _hover;
            public int hover
            {
                get => _hover;
                set
                {
                    _hover = value;
                    OnPropertyChanged();
                }
            }

        }
        public List<abc> Test { get; set; } = new List<abc>() { new abc { name = "aa" }, new abc { name = "bb" }, new abc { name = "cc" } };

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

        private void getData1(string dep_id, string date)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_dep", dep_id);
                web.QueryString.Add("active", "true");
                web.QueryString.Add("start_date", date);
                int x = DateTime.DaysInMonth(int.Parse(DateTime.Parse(date).ToString("yyyy")), int.Parse(DateTime.Parse(date).ToString("MM")));
                web.QueryString.Add("date", DateTime.Parse(date).ToString("yyyy") + "-" + DateTime.Parse(date).ToString("MM") + "-" + x);
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

        private void ChonPhong(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("yyyy-MM-dd");
            if (selectDate.SelectedDate != null)
                month = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            getData1(id_phong, month);
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

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.NhapLuongCoBan(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 500;
            pop.Height = 500;
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time,id_phong,id_user,1);
        }
        private void dataGrid1Hover(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Border col = sender as System.Windows.Controls.Border;
            if (col != null)
            {
                try
                {
                    ItemEmp item = (ItemEmp)col.DataContext;
                    int index = listNhanVien.FindIndex(x => x.ep_id == item.ep_id);
                    if (index > -1)
                    {
                        listNhanVien[index].hover = 1;
                    }
                }
                catch { }
            }
            else if ((sender as Grid) != null)
            {
                ItemEmp item = (ItemEmp)(sender as Grid).DataContext;
                int index = listNhanVien.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listNhanVien[index].hover = 1;
                }
            }
        }

        private void dataGrid1Leave(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Border col = sender as System.Windows.Controls.Border;
            if (col != null)
            {
                ItemEmp item = (ItemEmp)col.DataContext;
                int index = listNhanVien.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listNhanVien[index].hover = 0;
                }
            }
            else if ((sender as Grid) != null)
            {
                ItemEmp item = (ItemEmp)(sender as Grid).DataContext;
                int index = listNhanVien.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listNhanVien[index].hover = 0;
                }
            }
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            ItemEmp data = (ItemEmp)dp.DataContext;
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data.ep_id));
            Main.title.Text = " Danh sách nhân viên/ Hồ sơ nhân viên";
            Main.sidebar.SelectedIndex = -1;
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
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
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
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, 1);
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
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, pagenow - 1);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
        {
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, pagenow + 1);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
        {
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, PageNVCacNhom.Count);
        }
        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage1.Text);
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage2.Text);
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            int pagenumber = int.Parse(txtpage3.Text);
            string time = DateTime.Now.ToString("MM");
            if (selectDate.SelectedDate != null)
                time = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
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
            getData(time, id_phong, id_user, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else             
                Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }
  


        private void xuatExcel()
        {
            string year = "", month = "", id_nv = "";
            this.Dispatcher.Invoke(() =>
            {
                if (selectDate.SelectedDate != null)
                    month = selectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                else month = DateTime.Now.ToString("yyyy-MM-dd");
            });
            /*MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Main.CurrentCompany.com_id), "id_comp");
            content.Add(new StringContent(Main.CurrentCompany.token), "token");
            content.Add(new StringContent(month), "m");
            content.Add(new StringContent(year), "y");
            content.Add(new StringContent(id_nv), "uid");
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> response = httpClient.PostAsync("https://tinhluong.timviec365.vn/api_app/company/export_rose.php", content);
            string data = response.Result.Content.ReadAsStringAsync().Result;*/
            string data = "";
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("time", month);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(e.Result);
                    string path = $"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    File.WriteAllText($"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/hoa_hong365.html", data);
                    string filePath = "";
                    // tạo SaveFileDialog để lưu file excel
                    SaveFileDialog dialog = new SaveFileDialog();

                    // chỉ lọc ra các file có định dạng Excel
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = "luong_365";
                    // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
                    if (dialog.ShowDialog() == true)
                    {
                        filePath = dialog.FileName;
                        var workbook = new Workbook($"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/hoa_hong365.html");
                        try
                        {
                            workbook.Save(filePath);
                            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                            if (xlApp == null)
                            {
                                MessageBox.Show("Excel is not properly installed!!");
                                return;
                            }


                            xlApp.DisplayAlerts = false;
                            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                            Excel.Sheets worksheets = xlWorkBook.Worksheets;
                            worksheets[2].Delete();
                            xlWorkBook.Save();
                            xlWorkBook.Close();

                            releaseObject(worksheets);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        //converter.Convert(filePath, convertOptions);
                    }
                    loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_excel_salary_manager.php", web.QueryString);
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Xuatfile(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            xuatExcel();
        }
    }
}
