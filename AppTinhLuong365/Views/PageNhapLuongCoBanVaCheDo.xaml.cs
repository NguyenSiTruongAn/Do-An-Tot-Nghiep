using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            getData1();
            getData2();
            selectDate.SelectedDate = DateTime.Now;
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
                ListEmployee nv = (ListEmployee)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(time,id_phong,id_user,1);
        }
        private void dataGrid1Hover(object sender, MouseEventArgs e)
        {
            Border col = sender as Border;
            if (col != null)
            {
                ItemEmp item = (ItemEmp)col.DataContext;
                int index = listNhanVien.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listNhanVien[index].hover = 1;
                }
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
            Border col = sender as Border;
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
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data));
            Main.title.Text = " Danh sách nhân viên/ Hồ sơ nhân viên";
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
            Border b = sender as Border;
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
            Border b = sender as Border;
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
            Border b = sender as Border;
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
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }
    }
}
