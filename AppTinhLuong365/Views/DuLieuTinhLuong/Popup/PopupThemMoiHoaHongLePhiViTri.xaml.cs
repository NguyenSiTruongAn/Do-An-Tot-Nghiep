﻿using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemMoiHoaHongLePhiViTri.xaml
    /// </summary>
    public partial class PopupThemMoiHoaHongLePhiViTri : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupThemMoiHoaHongLePhiViTri(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
            {
                FontSize = 15,
                Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                SelectedDateChange = DatePicker_SelectedDateChanged1,
                SelectedDate = DateTime.Now
            };
            listTest = new List<abc>();
            listTest.Add(new abc() { id = listTest.Count, dt_sl = "", dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate = dp });
            listTest = listTest.ToList();
            AppTinhLuong365.Resouce.tool.DatePicker dp1 = new Resouce.tool.DatePicker
            {
                FontSize = 15,
                Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                SelectedDateChange = DatePicker_SelectedDateChanged,
                SelectedDate = DateTime.Now
            };
            listDT = new List<abc>();
            listDT.Add(new abc() { id = listDT.Count, dt_sl = "", dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate = dp1 });
            listDT = listDT.ToList();
            getData();
            getData1();
            getData3();
            dteSelectedMonth = new Calendar();
            dteSelectedMonth.Visibility = Visibility.Collapsed;
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            dteSelectedMonth.MouseLeftButtonDown += Select_thang;
            dteSelectedMonth.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged;
            cl = new List<Calendar>();
            cl.Add(dteSelectedMonth);
            cl = cl.ToList();
            dteSelectedMonth1 = new Calendar();
            dteSelectedMonth1.Visibility = Visibility.Collapsed;
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            dteSelectedMonth1.MouseLeftButtonDown += Select_thang1;
            dteSelectedMonth1.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged1;
            cl1 = new List<Calendar>();
            cl1.Add(dteSelectedMonth1);
            cl1 = cl1.ToList();
        }
        Calendar dteSelectedMonth { get; set; }
        Calendar dteSelectedMonth1 { get; set; }

        private List<Calendar> _cl;

        public List<Calendar> cl
        {
            get { return _cl; }
            set
            {
                _cl = value; OnPropertyChanged();
            }
        }

        private List<Calendar> _cl1;

        public List<Calendar> cl1
        {
            get { return _cl1; }
            set
            {
                _cl1 = value; OnPropertyChanged();
            }
        }

        MainWindow Main;

        private List<DSCaiDatHoaHongVT> _listSanPham;

        public List<DSCaiDatHoaHongVT> listSanPham
        {
            get { return _listSanPham; }
            set
            {
                _listSanPham = value; OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("type", "4");
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSCaiDatHoaHongVT api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongVT>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listSanPham = api.data.list;
                            cbSanPham.SelectedIndex = 0;
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/setting_rose.php", web.QueryString);
            }
        }

        private List<ListEmployee> _listNV;

        public List<ListEmployee> listNV
        {
            get { return _listNV; }
            set
            {
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

        private List<abc> _listDT = new List<abc>();

        public List<abc> listDT
        {
            get { return _listDT; }
            set
            {
                _listDT = value; OnPropertyChanged();
            }
        }

        private List<abc> _listTest = new List<abc>();

        public List<abc> listTest
        {
            get { return _listTest; }
            set
            {
                _listTest = value; OnPropertyChanged();
            }
        }

        private void Border_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
            {
                FontSize = 15,
                Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                SelectedDateChange = DatePicker_SelectedDateChanged1,
                SelectedDate = DateTime.Now
            };
            listTest.Add(new abc() { id = listTest.Count, dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate = dp });
            listTest = listTest.ToList();
        }

        public class abc
        {
            public string dt_rose_id { get; set; }
            public int id { get; set; }
            public string dt_sl { get; set; }
            public string dt_time { get; set; }
            public object selectedDate { get; set; }
        }

        private List<ListGroup> _listGR;

        public List<ListGroup> listGR
        {
            get { return _listGR; }
            set
            {
                _listGR = value; OnPropertyChanged();
            }
        }

        private void getData3()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listGR = api.data.list_group;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
            {
                FontSize = 15,
                Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                SelectedDateChange = DatePicker_SelectedDateChanged,
                SelectedDate = DateTime.Now
            };
            listDT.Add(new abc() { id = listDT.Count, dt_sl = "", dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate = dp });
            listDT = listDT.ToList();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AppTinhLuong365.Resouce.tool.DatePicker dp = sender as AppTinhLuong365.Resouce.tool.DatePicker;
            ContentControl b = dp.Parent as ContentControl;
            if (b != null)
            {
                abc dt = b.DataContext as abc;
                foreach (var item in listDT)
                {
                    if (item.id == dt.id)
                        item.dt_time = dp.SelectedDate.Value.ToString("dd/MM/yyyy");
                }
            }
        }

        private void DatePicker_SelectedDateChanged1(object sender, SelectionChangedEventArgs e)
        {
            AppTinhLuong365.Resouce.tool.DatePicker dp = sender as AppTinhLuong365.Resouce.tool.DatePicker;
            ContentControl b = dp.Parent as ContentControl;
            if (b != null)
            {
                abc dt = b.DataContext as abc;
                foreach (var item in listTest)
                {
                    if (item.id == dt.id)
                        item.dt_time = dp.SelectedDate.Value.ToString("dd/MM/yyyy");
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void SLChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            abc data = (abc)tb.DataContext;
            foreach (var item in listDT)
            {
                if (item.id == data.id)
                    item.dt_sl = tb.Text;
            }
            long tong = 0;
            try
            {
                foreach (var item in listDT)
                {
                    if (!string.IsNullOrEmpty(item.dt_sl))
                    {
                        long x = long.Parse(item.dt_sl);
                        if (tong + x <= long.MaxValue)
                        {
                            tong += x;
                            txtTongSanPham.Text = tong + "";
                        }
                    }
                }
            }
            catch (Exception)
            {
                tb.Text = tb.Text.Remove(tb.Text.Length - 3);
                tb.SelectionStart = tb.Text.Length;
            }
        }

        private void SLChanged1(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            abc data = (abc)tb.DataContext;
            foreach (var item in listTest)
            {
                if (item.id == data.id)
                    item.dt_sl = tb.Text;
            }
            long tong = 0;
            try
            {
                foreach (var item in listTest)
                {
                    if (!string.IsNullOrEmpty(item.dt_sl))
                    {
                        long x = long.Parse(item.dt_sl);
                        if (tong + x <= long.MaxValue)
                        {
                            tong += x;
                            txtTongSanPham1.Text = tong + "";
                        }
                    }
                }
            }
            catch (Exception)
            {
                tb.Text = tb.Text.Remove(tb.Text.Length - 3);
                tb.SelectionStart = tb.Text.Length;
            }
        }

        int flag = 0;
        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }

        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThangAD != null && !string.IsNullOrEmpty(x))
            {
                textThangAD.Text = x;
            }
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag += 1;
        }

        int flag1 = 0;
        private void Select_thang1(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag1 = 1;
        }

        private void dteSelectedMonth_DisplayModeChanged1(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (txtThangNhom != null && !string.IsNullOrEmpty(x))
            {
                txtThangNhom.Text = x;
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag1 > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        private void StackPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void ThemHoaHong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateDate.Text = validateSL.Text = validateSanPham.Text = "";
            if (cbName.SelectedIndex < 0)
            {
                allow = false;
                validateName.Text = "Vui lòng chọn nhân viên";
            }
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(txtTongSanPham.Text))
            {
                allow = false;
                validateSL.Text = "Vui lòng nhập đày đủ";
            }
            if (cbSanPham.SelectedIndex < 0)
            {
                allow = false;
                validateSanPham.Text = "Vui lòng chọn sản phẩm";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    ListEmployee nv_selected = new ListEmployee();
                    nv_selected = (ListEmployee)cbName.SelectedItem;
                    web.QueryString.Add("id_user", nv_selected.ep_id);
                    DateTime chuky = DateTime.Parse(textThangAD.Text);
                    web.QueryString.Add("chuky", chuky.ToString("yyyy-MM"));
                    DSCaiDatHoaHongVT vt = new DSCaiDatHoaHongVT();
                    vt = (DSCaiDatHoaHongVT)cbSanPham.SelectedItem;
                    web.QueryString.Add("id_sp", vt.tl_id);
                    web.QueryString.Add("ghichu_u", tbInput1.Text);
                    for (int i = 0; i < listDT.Count; i++)
                    {
                        if (listDT[i].dt_sl != null)
                        {
                            web.QueryString.Add("amount[" + i + "]", listDT[i].dt_sl);
                            string time = "";
                            string[] time1 = listDT[i].dt_time.Replace("/", " - ").Split(' ');
                            for (int j = time1.Length - 1; j > -1; j--)
                            {
                                time += time1[j];
                            }
                            web.QueryString.Add("time_add[" + i + "]", time);
                        }
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongLePhiViTri(Main));
                                Main.sidebar.SelectedIndex = -1;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_rose_fees.php", web.QueryString);
                }
                
            }
        }

        ListGroup gr_selected = new ListGroup();
        private void ChonNhom(object sender, RoutedEventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            gr_selected = (ListGroup)rd.DataContext;
        }

        private void ThemHoaHong1(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateNhom.Text = validateChuKy1.Text  = validateSL1.Text = validateSanPham1.Text = "";
            if (gr_selected.lgr_id == null)
            {
                allow = false;
                validateNhom.Text = "Vui lòng chọn nhân viên";
            }
            if (txtThangNhom.Text == "--------- ----")
            {
                allow = false;
                validateChuKy1.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(txtTongSanPham1.Text))
            {
                allow = false;
                validateSL1.Text = "Vui lòng nhập đày đủ";
            }
            if (cbSanPham1.SelectedIndex < 0)
            {
                allow = false;
                validateSanPham1.Text = "Vui lòng chọn mức hoa hồng";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    ListEmployee nv_selected = new ListEmployee();
                    web.QueryString.Add("id_gr", gr_selected.lgr_id);
                    DateTime chuky = DateTime.Parse(txtThangNhom.Text);
                    web.QueryString.Add("chuky", chuky.ToString("yyyy-MM-dd"));
                    DSCaiDatHoaHongVT ln = new DSCaiDatHoaHongVT();
                    ln = (DSCaiDatHoaHongVT)cbSanPham1.SelectedItem;
                    web.QueryString.Add("id_sp", ln.tl_id);
                    web.QueryString.Add("ghichu_u", tbInput3.Text);
                    for (int i = 0; i < listTest.Count; i++)
                    {
                        if (listTest[i].dt_sl != null)
                        {
                            web.QueryString.Add("amount[" + i + "]", listTest[i].dt_sl);
                            string time = "";
                            string[] time1 = listTest[i].dt_time.Replace("/", " - ").Split(' ');
                            for (int j = time1.Length - 1; j > -1; j--)
                            {
                                time += time1[j];
                            }
                            web.QueryString.Add("time_add[" + i + "]", time);
                        }
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongLePhiViTri(Main));
                                Main.sidebar.SelectedIndex = -1;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_group_rose_fees.php", web.QueryString);
                }
            }
        }
    }
}
