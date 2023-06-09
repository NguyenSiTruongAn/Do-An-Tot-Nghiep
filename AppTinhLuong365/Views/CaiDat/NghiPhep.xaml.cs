﻿using System;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppTinhLuong365.Core;

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for NghiPhep.xaml
    /// </summary>
    public partial class NghiPhep : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NghiPhep(MainWindow main)
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


            // for (var i = DateTime.Now.Year; i <= i + 2; i++)
            // {
            //     ItemList.Add($"Tháng {i - 1}");
            //     ItemList.Add($"Tháng {i}");
            //     ItemList.Add($"Tháng {i + 1}");
            // }

            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid1.AutoReponsiveColumn(0);
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string ep_id = "";
            getData(date);
            getData1(month, year, ep_id);
            getData2();
            getData3();
            getData4(date);
            Year.PlaceHolder = "Năm " + year;
            Month.PlaceHolder = "Tháng " + month;
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

        private void OpenDes(object sender, MouseButtonEventArgs e)
        {
            borderes.Visibility = borderes.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OpenDes1(object sender, MouseButtonEventArgs e)
        {
            borderes1.Visibility =
                borderes1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupNghiPhepCoLuong(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.NghiPhepKhongLuong(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<List> _list;

        public List<List> list
        {
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged();
            }
        }

        private void getData(string date)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("time", date);
                web.QueryString.Add("take", "2");
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_Np_Improperly api =
                        JsonConvert.DeserializeObject<API_List_Np_Improperly>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            list = api.data.list;
                            DateTime aDateTime;
                            foreach (var a in list)
                            {
                                DateTime.TryParse(a.pc_time, out aDateTime);
                                a.pc_time = aDateTime.ToString("dd/MM/yyyy");
                            }
                        }
                    }
                    catch { }
                    //foreach (EpLate item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_np_improperly.php",
                    web.QueryString);
            }
        }

        private List<List> _list1;


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            List data = (List)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.CaiDat.Popup.PopupDanhSachMucPhat(Main, data.pc_shift));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            List data = (List)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.CaiDat.Popup.PopupDanhSachMucPhat1(Main, data.pc_shift));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<ItemNp> _listItemNp;

        public List<ItemNp> listItemNp
        {
            get { return _listItemNp; }
            set
            {
                _listItemNp = value;
                OnPropertyChanged();
            }
        }

        private List<ItemNp> _listItemNp1;

        public List<ItemNp> listItemNp1
        {
            get { return _listItemNp1; }
            set
            {
                _listItemNp1 = value;
                OnPropertyChanged();
            }
        }

        private void getData1(string month, string year, string ep_id)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                //web.QueryString.Add("page", "1");
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.QueryString.Add("id_user", ep_id);
                // if (!string.IsNullOrEmpty(id_nv)) web.QueryString.Add("id_user", id_nv);

                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_Ep_Np api =
                        JsonConvert.DeserializeObject<API_List_Ep_Np>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listItemNp = api.data.list;
                            DateTime aDateTime;
                            foreach (var a in listItemNp)
                            {
                                DateTime.TryParse(a.ngaybatdau_nghi, out aDateTime);
                                a.ngaybatdau_nghi = aDateTime.ToString("dd/MM/yyyy");
                                DateTime.TryParse(a.ngayketthuc_nghi, out aDateTime);
                                a.ngayketthuc_nghi = aDateTime.ToString("dd/MM/yyyy");
                            }
                        }
                        foreach (ItemNp item in listItemNp)
                        {
                            if (item.ep_image == "../img/add.png")
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                            else
                            {
                                item.ep_image = item.ep_image;
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_np.php",
                    web.QueryString);
            }
        }

        private void Thang(object sender, SelectionChangedEventArgs e)
        {
            string month = "", year = "", ep_id = "";
            if (Year.SelectedItem != null)
                year = Year.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (Month.SelectedIndex != -1)
                month = (Month.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            if (Employee.SelectedIndex != -1)
            {
                ListEmployee id1 = (ListEmployee)Employee.SelectedItem;
                string id2 = id1.ep_id;
                if (id2 == "-1")
                    ep_id = "";
                else ep_id = id2;
            }
            else
            {
                ep_id = "";
            }

            getData1(month, year, ep_id);
        }

        private void Search(object sender, SelectionChangedEventArgs e)
        {
            string month = "", year = "", ep_id = "";
            if (Year.SelectedIndex != -1)
                year = Year.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (Month.SelectedIndex != -1)
                month = (Month.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            if (Employee.SelectedIndex != -1)
            {
                ListEmployee id1 = (ListEmployee)Employee.SelectedItem;
                string id2 = id1.ep_id;
                if (id2 == "-1")
                    ep_id = "";
                else ep_id = id2;
            }

            getData1(month, year, ep_id);
        }

        private void NhanVien(object sender, SelectionChangedEventArgs e)
        {
            ListEmployee selected = (ListEmployee)(Employee.SelectedItem);
            if (selected != null)
            {
                listItemNp1 = listItemNp.Where(x => x.ep_id == selected.ep_id).ToList();
            }
            else listItemNp1 = listItemNp;
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
                    try
                    {
                        API_ListEmployee api =
                        JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listEmployee = api.data.data.items;
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php",
                    web.QueryString);
            }
        }

        private List<Item_shift> _listShift;

        public List<Item_shift> listShift
        {
            get { return _listShift; }
            set
            {
                // if (value == null) value = new List<Item_shift>();
                // value.Insert(0, new Item_shift() { shift_id = "-1", shift_name = "Tất cả các ca" });
                _listShift = value;
                OnPropertyChanged();
            }
        }

        private void getData3()
        {
            using (WebClient web = new WebClient())
            {
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_shift api =
                        JsonConvert.DeserializeObject<API_List_shift>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listShift = api.data.items;
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
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_shift.php", web.QueryString);
            }
        }

        public List<List> list1
        {
            get { return _list1; }
            set
            {
                _list1 = value;
                OnPropertyChanged();
            }
        }

        private void getData4(string date)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("time", date);
                web.QueryString.Add("take", "1");
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_Np_Improperly api =
                        JsonConvert.DeserializeObject<API_List_Np_Improperly>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            list1 = api.data.list;
                            DateTime aDateTime;
                            foreach (var a in list1)
                            {
                                DateTime.TryParse(a.pc_time, out aDateTime);
                                a.pc_time = aDateTime.ToString("dd/MM/yyyy");
                            }
                        }
                    }
                    catch { }
                    //foreach (EpLate item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_np_improperly.php",
                    web.QueryString);
            }
        }

        private void SearchBarMonth_OnLoaded(object sender, RoutedEventArgs e)
        {
            int indexMonth = ItemList.ToList().FindIndex(x => x.Contains(DateTime.Now.Month.ToString()));
            if (indexMonth > -1) Month.SelectedIndex = indexMonth;
        }

        private void SearchBarYear_OnLoaded(object sender, RoutedEventArgs e)
        {
            int indexYear = YearList.ToList().FindIndex(x => x.Contains(DateTime.Now.Year.ToString()));
            if (indexYear > -1) Year.SelectedIndex = indexYear;
        }

        private List<string> ca = new List<string>();

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_shift data = (Item_shift)cb.DataContext;
            ca.Add(data.shift_id);
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_shift data = (Item_shift)cb.DataContext;
            ca.Remove(data.shift_id);
        }

        private void ApDung(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (ca.Count <= 0)
            {
                allow = false;
            }

            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }

                    for (int i = 0; i < ca.Count; i++)
                    {
                        string x = ca[i];
                        web.QueryString.Add("shift_id[" + i + "]", ca[i]);
                    }

                    web.QueryString.Add("pn_money", tbInput.Text);
                    if (DatePicker.SelectedDate != null)
                        web.QueryString.Add("pn_time", DatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("pn_type", "2");
                    // web.QueryString.Add("id_group", ID_gr);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string x = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemNhanVienVaoNhom api = JsonConvert.DeserializeObject<API_ThemNhanVienVaoNhom>(x);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiPhep(Main));
                                this.Control.SelectedIndex = 1;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_np.php",
                        web.QueryString);
                }
            }
        }

        private static readonly Regex _regex = new Regex(@"^[0-9]\d*(\.\d{0,2})?$");

        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        private bool IsAllowed(TextBox tb, string text)
        {
            bool isAllowed = true;
            if (tb != null)
            {
                string currentText = tb.Text;
                if (!string.IsNullOrEmpty(tb.SelectedText))
                    currentText = currentText.Remove(tb.CaretIndex, tb.SelectedText.Length);
                isAllowed = IsTextAllowed(currentText.Insert(tb.CaretIndex, text));
            }

            return isAllowed;
        }

        private void Txt_PreviewCurrencyTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsAllowed(sender as TextBox, e.Text);
        }


        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsAllowed(sender as TextBox, text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (ca.Count <= 0)
            {
                allow = false;
            }

            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }

                    for (int i = 0; i < ca.Count; i++)
                    {
                        string x = ca[i];
                        web.QueryString.Add("shift_id[" + i + "]", ca[i]);
                    }

                    web.QueryString.Add("pn_money", tbInput1.Text);
                    if (Picker.SelectedDate != null)
                        web.QueryString.Add("pn_time", Picker.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("pn_type", "1");
                    // web.QueryString.Add("id_group", ID_gr);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string x = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemNhanVienVaoNhom api = JsonConvert.DeserializeObject<API_ThemNhanVienVaoNhom>(x);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiPhep(Main));
                                this.Control.SelectedIndex = 2;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_np.php",
                        web.QueryString);
                }
            }
        }

        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void ListView_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid1.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void NghiPhep_OnLoaded(object sender, RoutedEventArgs e)
        {
            DatePickerDay.SelectedDate = DateTime.Now;
        }

        private void Date()
        {
            string date = "";
            if (DatePickerDay.SelectedDate != null)
                date = DatePickerDay.SelectedDate.Value.ToString("yyyy-MM-dd");
            else
                date = DateTime.Now.ToString("yyyy-MM-dd");
            getData(date);
        }

        private void Select()
        {
            string date = "";
            if (DatePickerNew.SelectedDate != null)
                date = DatePickerNew.SelectedDate.Value.ToString("yyyy-MM-dd");
            else
                date = DateTime.Now.ToString("yyyy-MM-dd");
            getData4(date);
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(Main));
            Main.SideBarIndex = 0;
        }
    }
}