using System;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AppTinhLuong365.Core;
using Brush = System.Drawing.Brush;

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for DiMuonVeSom.xaml
    /// </summary>
    public partial class DiMuonVeSom : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DiMuonVeSom(MainWindow main)
        {
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

            InitializeComponent();
            this.DataContext = this;
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            getData(month, year);
            getData1(month, year, "", "");
            getData2();
            getData3();
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public MainWindow Main;

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

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupDiMuonVeSom(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<ItemLate> _listLate;

        public List<ItemLate> listLate
        {
            get { return _listLate; }
            set
            {
                _listLate = value;
                OnPropertyChanged();
            }
        }

        private void getData(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("time", year + "-" + month);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListLate api =
                        JsonConvert.DeserializeObject<API_ListLate>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listLate = api.data.list_late;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_late.php",
                    web.QueryString);
            }
        }

        private List<EpLate> _listEpLate;

        public List<EpLate> listEpLate
        {
            get { return _listEpLate; }
            set
            {
                _listEpLate = value;
                OnPropertyChanged();
            }
        }

        private static int totalNVDiMuon;

        private List<EpLate> _listEpLate1 = new List<EpLate>();
        public List<EpLate> listEpLate1
        {
            get { return _listEpLate1; }
            set
            {
                _listEpLate1 = value;
                OnPropertyChanged();
            }
        }

        private void getData1(string month, string year, string id_nv, string id_pb)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("time", year + "-" + month);
                if (!string.IsNullOrEmpty(id_nv)) web.QueryString.Add("id_ep", id_nv);
                if (!string.IsNullOrEmpty(id_pb)) web.QueryString.Add("id_dp", id_pb);
                // web.QueryString.Add("page", pageNB + "");

                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_Ep_Late api =
                        JsonConvert.DeserializeObject<API_List_Ep_Late>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listEpLate1 = listEpLate = api.data.ep_late;
                        // totalNVDiMuon = int.Parse(api.data.total);
                        // PageNVDiMuon = ListPageNumber(totalNVDiMuon);
                        // loadPage_ChuaNhom(pageNB, PageNVChuaNhom);
                        // dataGrid.AutoReponsiveColumn(0);
                    }

                    foreach (EpLate item in listEpLate)
                    {
                        if (item.ts_image != "/img/add.png")
                        {
                            item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_late.php",
                    web.QueryString);
            }
        }

        private int pagenow;

        // private void loadPage_ChuaNhom(int pagenb, List<int> loaiPage)
        // {
        //     this.Dispatcher.Invoke(() =>
        //     {
        //         BrushConverter bc = new BrushConverter();
        //         pagenow = pagenb;
        //         if (pagenb == 1)
        //         {
        //             Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        //             Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             txtpage1_chua_nhom.Text = "1";
        //             txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        //             txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             PageTruoc_chua_nhom.Visibility = Visibility.Collapsed;
        //             PageDau_chua_nhom.Visibility = Visibility.Collapsed;
        //             Page1_chua_nhom.Visibility = Visibility.Visible;
        //             if (loaiPage.Count > 3)
        //             {
        //                 PageCuoi_chua_nhom.Visibility = Visibility.Visible;
        //                 txtpage3_chua_nhom.Text = "3";
        //                 txtpage2_chua_nhom.Text = "2";
        //                 PageTiep_chua_nhom.Visibility = Visibility.Visible;
        //                 Page3_chua_nhom.Visibility = Visibility.Visible;
        //                 Page2_chua_nhom.Visibility = Visibility.Visible;
        //                 Page1_chua_nhom.Visibility = Visibility.Visible;
        //             }
        //             else if (loaiPage.Count > 2)
        //             {
        //                 txtpage3_chua_nhom.Text = "3";
        //                 txtpage2_chua_nhom.Text = "2";
        //                 PageTiep_chua_nhom.Visibility = Visibility.Visible;
        //                 Page3_chua_nhom.Visibility = Visibility.Visible;
        //                 Page2_chua_nhom.Visibility = Visibility.Visible;
        //                 Page1_chua_nhom.Visibility = Visibility.Visible;
        //             }
        //             else if (loaiPage.Count > 1)
        //             {
        //                 Page3_chua_nhom.Visibility = Visibility.Collapsed;
        //                 PageTiep_chua_nhom.Visibility = Visibility.Visible;
        //                 txtpage2_chua_nhom.Text = "2";
        //                 Page2_chua_nhom.Visibility = Visibility.Visible;
        //                 PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
        //             }
        //             else
        //             {
        //                 PageTiep_chua_nhom.Visibility = Visibility.Collapsed;
        //                 Page3_chua_nhom.Visibility = Visibility.Collapsed;
        //                 PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
        //                 Page2_chua_nhom.Visibility = Visibility.Collapsed;
        //             }
        //         }
        //         else if (pagenb == loaiPage.Count)
        //         {
        //             Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        //             Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             txtpage3_chua_nhom.Text = pagenb + "";
        //             txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        //             txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             Page3_chua_nhom.Visibility = Visibility.Visible;
        //             PageTiep_chua_nhom.Visibility = Visibility.Collapsed;
        //             PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
        //             if (loaiPage.Count > 3)
        //             {
        //                 txtpage2_chua_nhom.Text = (pagenb - 1) + "";
        //                 txtpage1_chua_nhom.Text = (pagenb - 2) + "";
        //                 Page2_chua_nhom.Visibility = Visibility.Visible;
        //                 PageDau_chua_nhom.Visibility = Visibility.Visible;
        //                 PageTruoc_chua_nhom.Visibility = Visibility.Visible;
        //                 Page1_chua_nhom.Visibility = Visibility.Visible;
        //             }
        //             else if (loaiPage.Count > 2)
        //             {
        //                 txtpage2_chua_nhom.Text = "2";
        //                 txtpage1_chua_nhom.Text = "1";
        //                 Page2_chua_nhom.Visibility = Visibility.Visible;
        //                 PageTruoc_chua_nhom.Visibility = Visibility.Visible;
        //                 Page1_chua_nhom.Visibility = Visibility.Visible;
        //                 PageDau_chua_nhom.Visibility = Visibility.Collapsed;
        //
        //             }
        //             else if (loaiPage.Count > 1)
        //             {
        //                 Page1_chua_nhom.Visibility = Visibility.Collapsed;
        //                 txtpage2_chua_nhom.Text = "1";
        //                 Page2_chua_nhom.Visibility = Visibility.Visible;
        //                 PageTruoc_chua_nhom.Visibility = Visibility.Visible;
        //                 PageDau_chua_nhom.Visibility = Visibility.Collapsed;
        //             }
        //         }
        //         else if (pagenb == loaiPage.Count - 1)
        //         {
        //             Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        //             Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             txtpage2_chua_nhom.Text = pagenb + "";
        //             txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        //             txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             Page2_chua_nhom.Visibility = Visibility.Visible;
        //             PageTiep_chua_nhom.Visibility = Visibility.Visible;
        //             PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
        //             PageTruoc_chua_nhom.Visibility = Visibility.Visible;
        //             Page3_chua_nhom.Visibility = Visibility.Visible;
        //             Page1_chua_nhom.Visibility = Visibility.Visible;
        //             txtpage3_chua_nhom.Text = (pagenb + 1) + "";
        //             txtpage1_chua_nhom.Text = (pagenb - 1) + "";
        //             if (loaiPage.Count > 3) PageDau_chua_nhom.Visibility = Visibility.Visible;
        //             else PageDau_chua_nhom.Visibility = Visibility.Collapsed;
        //         }
        //         else if (pagenb == 2)
        //         {
        //             Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        //             Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             txtpage2_chua_nhom.Text = "2";
        //             txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        //             txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage1_chua_nhom.Text = "1";
        //             txtpage3_chua_nhom.Text = "3";
        //             PageTruoc_chua_nhom.Visibility = Visibility.Visible;
        //             PageDau_chua_nhom.Visibility = Visibility.Collapsed;
        //             Page1_chua_nhom.Visibility = Visibility.Visible;
        //             Page2_chua_nhom.Visibility = Visibility.Visible;
        //             Page3_chua_nhom.Visibility = Visibility.Visible;
        //             PageTiep_chua_nhom.Visibility = Visibility.Visible;
        //             if (loaiPage.Count > 3) PageCuoi_chua_nhom.Visibility = Visibility.Visible;
        //             else PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
        //         }
        //         else
        //         {
        //             Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        //             Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        //             txtpage2_chua_nhom.Text = pagenb + "";
        //             txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        //             txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
        //             txtpage1_chua_nhom.Text = (pagenb - 1) + "";
        //             txtpage3_chua_nhom.Text = (pagenb + 1) + "";
        //             PageTruoc_chua_nhom.Visibility = Visibility.Visible;
        //             PageDau_chua_nhom.Visibility = Visibility.Visible;
        //             PageCuoi_chua_nhom.Visibility = Visibility.Visible;
        //             Page1_chua_nhom.Visibility = Visibility.Visible;
        //             Page2_chua_nhom.Visibility = Visibility.Visible;
        //             Page3_chua_nhom.Visibility = Visibility.Visible;
        //             PageTiep_chua_nhom.Visibility = Visibility.Visible;
        //         }
        //     });
        // }
        //
        // private void ve_page_1_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     getData1(1);
        //     BrushConverter bc = new BrushConverter();
        //     Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        //     txtpage1_chua_nhom.Text = "1";
        //     txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        //     loadPage_ChuaNhom(1, PageNVDiMuon);
        // }
        //
        // private void page_truoc_click_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     getData1(pagenow - 1);
        // }
        //
        // private void page_sau_click_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     getData1(pagenow + 1);
        // }
        //
        // private void page_cuoi_click_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     getData1(PageNVDiMuon.Count);
        // }
        //
        // private void select_page_click1_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     Border b = sender as Border;
        //     int pagenumber = int.Parse(txtpage1_chua_nhom.Text);
        //     getData1(pagenumber);
        //     // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        // }
        //
        // private void select_page_click2_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     Border b = sender as Border;
        //     int pagenumber = int.Parse(txtpage2_chua_nhom.Text);
        //     getData1(pagenumber);
        //     // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        // }
        //
        // private void select_page_click3_chua_nhom(object sender, MouseButtonEventArgs e)
        // {
        //     Border b = sender as Border;
        //     int pagenumber = int.Parse(txtpage3_chua_nhom.Text);
        //     getData1(pagenumber);
        //     // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        // }

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

        private List<int> _PageNVDiMuon;

        public List<int> PageNVDiMuon
        {
            get { return _PageNVDiMuon; }
            set { _PageNVDiMuon = value; OnPropertyChanged(); }
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "", id_pb = "";
            if (searchBarYear1.SelectedIndex != -1)
                year = searchBarYear1.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth1.SelectedIndex != -1)
                month = (searchBarMonth1.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            if (cbListNV.SelectedIndex != -1)
            {
                ListEmployee id1 = (ListEmployee)cbListNV.SelectedItem;
                string id2 = id1.ep_id;
                if (id2 == "-1")
                    id_nv = "";
                else id_nv = id2;
            }

            if (cbPhongBan.SelectedIndex != -1)
            {
                Item_dep id1 = (Item_dep)cbPhongBan.SelectedItem;
                string id2 = id1.dep_id;
                if (id2 == "-1")
                    id_pb = "";
                else id_pb = id2;
            }

            getData1(month, year, id_nv, id_pb);
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_dep.php",
                    web.QueryString);
            }
        }

        private List<ListEmployee> _listEmployee;

        public List<ListEmployee> listEmployee
        {
            get { return _listEmployee; }
            set
            {
                if (value == null) value = new List<ListEmployee>();
                value.Insert(0, new ListEmployee() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listEmployee = value;
                OnPropertyChanged();
            }
        }

        private void getData3()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListEmployee api =
                        JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listEmployee = api.data.data.items;
                    }
                    // foreach (EpLate item in listEpLate)
                    // {
                    //     if (item.ts_image != "/img/add.png")
                    //     {
                    //         item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php",
                    web.QueryString);
            }
        }

        private void SearchBarMonth_OnLoaded(object sender, RoutedEventArgs e)
        {
            int indexMonth = ItemList.ToList().FindIndex(x => x.Contains(DateTime.Now.Month.ToString()));
            if (indexMonth > -1) searchBarMonth.SelectedIndex = indexMonth;
            if (indexMonth > -1) searchBarMonth1.SelectedIndex = indexMonth;
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            int indexYear = YearList.ToList().FindIndex(x => x.Contains(DateTime.Now.Year.ToString()));
            if (indexYear > -1) searchBarYear.SelectedIndex = indexYear;
            if (indexYear > -1) searchBarYear1.SelectedIndex = indexYear;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            ItemLate data = (ItemLate)p.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupChinhSuaDiMuonVeSom(Main,
                data.pm_id, data.pm_shift, data.shift_name, data.pm_minute, data.pm_type, data.pm_type_phat,
                data.pm_monney, data.pm_time_begin, data.pm_time_end));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void UIElement_OnMouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            ItemLate data = (ItemLate)p.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupXoaMucPhat(Main, data.pm_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void thongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "";
            if (searchBarYear.SelectedIndex != -1)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            getData(month, year);
        }

        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }
    }
}