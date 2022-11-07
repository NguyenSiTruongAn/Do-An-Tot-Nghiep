using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Aspose.Cells;
using Microsoft.Win32;
using Border = System.Windows.Controls.Border;

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
            getData(month, year);
            getData1(1, month, year, "", "");
            getData2();
            getData3();
            searchBarYear1.PlaceHolder = "Năm " + year;
            searchBarMonth1.PlaceHolder = "Tháng " + month;
            searchBarYear.PlaceHolder = "Năm " + year;
            searchBarMonth.PlaceHolder = "Tháng " + month;
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
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
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
            if (this.ActualWidth > 900)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 900 && this.ActualWidth > 460)
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

        private static int totalEpLate;

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

        private void getData1(int page, string month, string year, string id_nv, string id_pb)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("time", year + "-" + month);
                if (!string.IsNullOrEmpty(id_nv)) web.QueryString.Add("id_ep", id_nv);
                if (!string.IsNullOrEmpty(id_pb)) web.QueryString.Add("id_dp", id_pb);
                web.QueryString.Add("page", page + "");

                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_Ep_Late api =
                        JsonConvert.DeserializeObject<API_List_Ep_Late>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listEpLate1 = listEpLate = api.data.ep_late;
                        totalEpLate = int.Parse(api.data.count);
                        PageNVDiMuon = ListPageNumber(totalEpLate);
                        loadPage(page, PageNVDiMuon);
                        dataGrid.AutoReponsiveColumn(2);
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

        private void loadPage(int pagenb, List<int> loaiPage)
        {
            BrushConverter bc = new BrushConverter();
            pagenow = pagenb;
            if (pagenb == 1)
            {
                Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage1.Text = "1";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
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
                txtpage3.Text = pagenb + "";
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#ffffff");
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
                txtpage2.Text = pagenb + "";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
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
                txtpage2.Text = "2";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
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
            getData1(1, month, year, id_nv, id_pb);
            BrushConverter bc = new BrushConverter();
            Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            txtpage1.Text = "1";
            txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        }

        private void page_truoc_click(object sender, MouseButtonEventArgs e)
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
            getData1(pagenow - 1, month, year, id_nv, id_pb);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
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
            getData1(pagenow + 1, month, year, id_nv, id_pb);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
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
            getData1(PageNVDiMuon.Count, month, year, id_nv, id_pb);
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

        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage1.Text);
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
            getData1(pagenumber, month, year, id_nv, id_pb);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage2.Text);
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
            getData1(pagenumber, month, year, id_nv, id_pb);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage3.Text);
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
            getData1(pagenumber, month, year, id_nv, id_pb);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
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
            if (searchBarYear1.SelectedItem != null)
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
            else
            {
                id_nv = "";
            }

            if (cbPhongBan.SelectedIndex != -1)
            {
                Item_dep id1 = (Item_dep)cbPhongBan.SelectedItem;
                string id2 = id1.dep_id;
                if (id2 == "-1")
                    id_pb = "";
                else id_pb = id2;
            }
            else
            {
                id_pb = "";
            }

            getData1(1, month, year, id_nv, id_pb);
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
            if (searchBarYear.SelectedItem != null)
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

        private void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void xuatExcel()
        {
            string data = "";
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    string year = "", month = "";
                    if (searchBarYear1.SelectedItem != null)
                        year = searchBarYear1.SelectedItem.ToString().Split(' ')[1];
                    if (searchBarMonth1.SelectedIndex != -1)
                        month = (searchBarMonth1.SelectedIndex + 1) + "";
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("month", month);
                    web.QueryString.Add("year", year);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(e.Result);
                    File.WriteAllText("../../Views/CaiDat/di_muon_ve_som.html", data);
                    string filePath = "";
                    // tạo SaveFileDialog để lưu file excel
                    SaveFileDialog dialog = new SaveFileDialog();

                    // chỉ lọc ra các file có định dạng Excel
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = "di_muon_ve_som_365";
                    // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
                    if (dialog.ShowDialog() == true)
                    {
                        filePath = dialog.FileName;
                        var workbook = new Workbook("../../Views/CaiDat/di_muon_ve_som.html");
                        try
                        {
                            workbook.Save(filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        loading.Visibility = Visibility.Collapsed;
                        //converter.Convert(filePath, convertOptions);
                    }

                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_late.php", web.QueryString);
            }
        }
        private void XuatFileThongKe(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            xuatExcel();
        }
    }
}