using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using Path = System.Windows.Shapes.Path;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for BangLuong.xaml
    /// </summary>
    public partial class BangLuong : Page, INotifyPropertyChanged
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

        string month;
        string year;
        public BangLuong(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }

            SearchList = new ObservableCollection<string>();
            SearchList.Add("Mới nhất");
            SearchList.Add("Cũ nhất");
            SearchList.Add("Phòng ban");
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
            string start_date = DateTime.Now.ToString("yyyy-MM-01");
            string end_date = DateTime.Now.ToString("yyyy-MM-30");
            int d = Int32.Parse(month);
            int e = Int32.Parse(year);
            searchBarMonth.SelectedIndex = d - 1;
            searchBarYear.SelectedIndex = e - e + 1;
            DateTime a = (DateTime.Parse(start_date));
            DateTime b = (DateTime.Parse(end_date));
            DatePickerStart.SelectedDate = a;
            DatePickerEnd.SelectedDate = b;
            SearchBar.SelectedIndex = 0;
            getData(1, month, year, "", "", start_date, end_date, "");
            getData1();
            getData2();
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }
        public ObservableCollection<string> SearchList { get; set; }

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

        public List<abc> Test { get; set; } = new List<abc>()
            { new abc { name = "aa" }, new abc { name = "bb" }, new abc { name = "cc" } };

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

        private void dataGrid1Hover(object sender, MouseEventArgs e)
        {
            Border col = sender as Border;
            if (col != null)
            {
                Item_Bang_Luong item = (Item_Bang_Luong)col.DataContext;
                int index = listBangLuong.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listBangLuong[index].hover = 1;
                }
            }
            else if ((sender as Grid) != null)
            {
                Item_Bang_Luong item = (Item_Bang_Luong)(sender as Grid).DataContext;
                int index = listBangLuong.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listBangLuong[index].hover = 1;
                }
            }
        }

        private void dataGrid1Leave(object sender, MouseEventArgs e)
        {
            Border col = sender as Border;
            if (col != null)
            {
                Item_Bang_Luong item = (Item_Bang_Luong)col.DataContext;
                int index = listBangLuong.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listBangLuong[index].hover = 0;
                }
            }
            else if ((sender as Grid) != null)
            {
                Item_Bang_Luong item = (Item_Bang_Luong)(sender as Grid).DataContext;
                int index = listBangLuong.FindIndex(x => x.ep_id == item.ep_id);
                if (index > -1)
                {
                    listBangLuong[index].hover = 0;
                }
            }
        }

        private void TuyChon(object sender, MouseButtonEventArgs e)
        {
            Path p = sender as Path;
            Item_Bang_Luong data = (Item_Bang_Luong)p.DataContext;
            var pop = new Views.TinhLuong.PopupTuyChonBangLuong(Main, data.name, data.dep_name, data.ep_id, searchBarMonth.SelectedIndex, searchBarYear.SelectedIndex, DatePickerStart.SelectedDate, DatePickerEnd.SelectedDate);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 95, z.Y + 15, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<Item_Bang_Luong> _listBangLuong;

        public List<Item_Bang_Luong> listBangLuong
        {
            get { return _listBangLuong; }
            set
            {
                _listBangLuong = value;
                OnPropertyChanged();
            }
        }

        private void getData(int page, string month, string year, string id_nv, string id_pb, string start_date,
            string end_date, string order)
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                if (!string.IsNullOrEmpty(id_nv)) web.QueryString.Add("id_user", id_nv);
                if (!string.IsNullOrEmpty(id_pb)) web.QueryString.Add("id_dp", id_pb);
                web.QueryString.Add("page", page + "");
                web.QueryString.Add("start_date", start_date);
                web.QueryString.Add("end_date", end_date);
                web.QueryString.Add("order", order);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Bang_luong_nv api =
                        JsonConvert.DeserializeObject<API_Bang_luong_nv>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listBangLuong = api.data.bang_luong;
                        totalEp = api.data.total;
                        PageNV = ListPageNumber(totalEp);
                        loadPage(page, PageNV);
                        // dataGrid.AutoReponsiveColumn(0);
                    }

                    foreach (Item_Bang_Luong item in listBangLuong)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                        else
                        {
                            item.ep_image = item.ep_image;
                        }
                    }

                    loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_bang_luong_nv.php",
                    web.QueryString);
            }
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(1, month, year, id_nv, id_pb, start_date, end_date, order);
        }

        private static int totalEp;

        private int pagenow;

        private void loadPage(int pagenb, List<int> loaiPage)
        {
            BrushConverter bc = new BrushConverter();
            pagenow = pagenb;
            if (pagenb == 1)
            {
                Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page2.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page3.Background = (Brush)bc.ConvertFrom("#ffffff");
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
                Page1.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page3.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage3.Text = pagenb + "";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
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
                Page1.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Text = pagenb + "";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
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
                Page1.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Text = "2";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
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
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(1, month, year, id_nv, id_pb, start_date, end_date, order);
            BrushConverter bc = new BrushConverter();
            Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            txtpage1.Text = "1";
            txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        }

        private void page_truoc_click(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(pagenow - 1, month, year, id_nv, id_pb, start_date, end_date, order);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(pagenow + 1, month, year, id_nv, id_pb, start_date, end_date, order);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(PageNV.Count, month, year, id_nv, id_pb, start_date, end_date, order);
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

        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage1.Text);
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(pagenumber, month, year, id_nv, id_pb, start_date, end_date, order);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage2.Text);
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(pagenumber, month, year, id_nv, id_pb, start_date, end_date, order);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage3.Text);
            string year = "", month = "", id_nv = "", id_pb = "", start_date = "", end_date = "", order = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
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

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            if (SearchBar.SelectedIndex != null)
            {
                order = (SearchBar.SelectedIndex + 1) + "";
            }
            else
            {
                order = 1 + "";
            }

            getData(pagenumber, month, year, id_nv, id_pb, start_date, end_date, order);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private List<int> _PageNV;

        public List<int> PageNV
        {
            get { return _PageNV; }
            set
            {
                _PageNV = value;
                OnPropertyChanged();
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

        private void getData1()
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

        private void getData2()
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

        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Month(object sender, SelectionChangedEventArgs e)
        {
            string a = DateTime.Now.ToString((searchBarYear.SelectedIndex + DateTime.Now.Year -1) + "-" +
                                             (searchBarMonth.SelectedIndex + 1) + "-01");
            DateTime b = DateTime.Parse(a);
            DatePickerStart.SelectedDate = b;
            string c = DateTime.Now.ToString((searchBarYear.SelectedIndex + DateTime.Now.Year - 1) + "-" +
                                             (searchBarMonth.SelectedIndex + 1) + "-" +
                                             DateTime.DaysInMonth((searchBarYear.SelectedIndex + 2021),
                                                 (searchBarMonth.SelectedIndex + 1)));
            DateTime d = DateTime.Parse(c);
            DatePickerEnd.SelectedDate = d;
        }

        private void xuatExcel()
        {
            string data = "";
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    var select = cbListNV.SelectedItem as ListEmployee;
                    string uid = null;
                    string dp = null;
                    if (select != null)
                    {
                        uid = select.ep_id;
                    }
                    var select1 = cbPhongBan.SelectedItem as Item_dep;
                    if (select1 != null)
                    {
                        dp = select1.dep_id;
                    }
                    string order;
                    if (SearchBar.SelectedIndex != null)
                    {
                        order = (SearchBar.SelectedIndex + 1) + "";
                    }
                    else
                    {
                        order = 1 + "";
                    }
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    string year = "", month = "";
                    if (searchBarYear.SelectedItem != null)
                        year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
                    if (searchBarMonth.SelectedIndex != -1)
                        month = (searchBarMonth.SelectedIndex + 1) + "";
                    web.QueryString.Add("m", month);
                    web.QueryString.Add("y", year);
                    web.QueryString.Add("uid", uid);
                    web.QueryString.Add("dp", dp);
                    web.QueryString.Add("order", order);
                    // web.QueryString.Add("uid", id_nv);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(e.Result);
                    File.WriteAllText("../../Views/TinhLuong/bang_luong.html", data);
                    string filePath = "";
                    // tạo SaveFileDialog để lưu file excel
                    SaveFileDialog dialog = new SaveFileDialog();

                    // chỉ lọc ra các file có định dạng Excel
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = "bang_luong_365";
                    // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
                    if (dialog.ShowDialog() == true)
                    {
                        filePath = dialog.FileName;
                        var workbook = new Workbook("../../Views/TinhLuong/bang_luong.html");
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_tbl_luong.php", web.QueryString);
            }
        }
        private void XuatFileThongKe(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            xuatExcel();
        }
    }
}