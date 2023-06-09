﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Aspose.Cells;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for ThuongPhat.xaml
    /// </summary>
    public partial class ThuongPhat : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;

        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }

        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ThuongPhat(MainWindow main)
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
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            getData(month, year, "", "", 1);
            getData1(month,year,"");
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

        private List<ListThuongPhat> _listTP;

        public List<ListThuongPhat> listTP
        {
            get { return _listTP; }
            set
            {
                _listTP = value;
                OnPropertyChanged();
            }
        }

        private static int totalNVCacNhom;

        private void getData(string month, string year, string ep_id, string dep_id, int page)
        {
            txtngay.Text = month + "/" + year;
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_emp", ep_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("page", page + "");
                web.QueryString.Add("year", year);
                web.QueryString.Add("dep_id", dep_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListThuongPhat api =
                        JsonConvert.DeserializeObject<API_ListThuongPhat>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listTP = api.data.thuong_phat;
                            totalNVCacNhom = api.data.total;
                            PageNVCacNhom = ListPageNumber(totalNVCacNhom);
                            loadPage(page, PageNVCacNhom);
                        }

                        foreach (var item in listTP)
                        {
                            if (item.img == "")
                            {
                                item.img = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_payoff_manager.php",
                    web.QueryString);
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
                    try {
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_dep.php",
                    web.QueryString);
            }
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }
        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

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

            if (this.ActualWidth > 2100)
            {
                DockPanel.SetDock(dockThuongPhat, Dock.Right);
            }
            else
                DockPanel.SetDock(dockThuongPhat, Dock.Bottom);
        }

        private void TuyChinhThuongPhat_Click(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            ListThuongPhat data = (ListThuongPhat)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThuongPhat(Main, data, "");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 888;
        }

        private void btnTienThuong(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            ListThuongPhat data = (ListThuongPhat)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTienThuong(Main, data);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
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

            getData(month, year, id_user, id_phong, 1);
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
            set
            {
                _PageNVCacNhom = value;
                OnPropertyChanged();
            }
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

            getData(month, year, id_user, id_phong, 1);

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

            getData(month, year, id_user, id_phong, pagenow - 1);
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

            getData(month, year, id_user, id_phong, pagenow + 1);
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

            getData(month, year, id_user, id_phong, PageNVCacNhom.Count);
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

            getData(month, year, id_user, id_phong, pagenumber);
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

            getData(month, year, id_user, id_phong, pagenumber);
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

            getData(month, year, id_user, id_phong, pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void btnTienPhat(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            ListThuongPhat data = (ListThuongPhat)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTienPhat(Main, data);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
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
                System.Windows.MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void Xuatfile(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            string year = "", month = "", id_nv = "";
            this.Dispatcher.Invoke(() =>
            {
                if (cbNam.SelectedIndex != -1)
                    year = cbNam.SelectedItem.ToString().Split(' ')[1];
                else
                    year = DateTime.Now.ToString("yyyy");
                if (cbThang.SelectedIndex != -1)
                    month = (cbThang.SelectedIndex + 1) + "";
                else month = DateTime.Now.ToString("MM");
                if (cbNV.SelectedIndex != -1)
                {
                    DSNVTheoThoiGian id1 = (DSNVTheoThoiGian)cbNV.SelectedItem;
                    string id2 = id1.ep_id;
                    if (id2 == "-1")
                        id_nv = "";
                    else id_nv = id2;
                }
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
                    web.QueryString.Add("m", month);
                    web.QueryString.Add("y", year);
                    web.QueryString.Add("uid", id_nv);
                }

                web.UploadValuesCompleted += (s, ee) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(ee.Result);
                    string path = $"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    File.WriteAllText($"{ Environment.GetEnvironmentVariable("APPDATA")}/ TinhLuong / thuong_phat_365.html", data);
                    string filePath = "";
                    // tạo SaveFileDialog để lưu file excel
                    Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

                    // chỉ lọc ra các file có định dạng Excel
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = "thuong_phat_365";
                    // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
                    if (dialog.ShowDialog() == true)
                    {
                        filePath = dialog.FileName;
                        var workbook = new Workbook($"{ Environment.GetEnvironmentVariable("APPDATA")}/ TinhLuong / thuong_phat_365.html");
                        try
                        {
                            workbook.Save(filePath);
                            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                            if (xlApp == null)
                            {
                                System.Windows.MessageBox.Show("Excel is not properly installed!!");
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
                            System.Windows.MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }

                        loading.Visibility = Visibility.Collapsed;
                        //converter.Convert(filePath, convertOptions);
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_rose.php",
                    web.QueryString);
            }
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(Main));
            Main.SideBarIndex = 0;
        }
    }
}