using AppTinhLuong365.Model.APIEntity;
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
    /// Interaction logic for PopupChinhSuaNVHoaHongDoanhThu.xaml
    /// </summary>
    public partial class PopupChinhSuaNVHoaHongDoanhThu : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChinhSuaNVHoaHongDoanhThu(MainWindow main, DSNVHoaHongDoanhThu data)
        {
            InitializeComponent();
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            getData();
            txtName.Text = data.ep_name;
            txttongdoanhthu.Text = data.ro_price;
            listDT = data.ro_dt_thoi_diem;
            data1 = data;
            for (int i = 0; i < listDT.Count; i++)
            {
                listDT[i].id = i;
            }
            foreach(var item in listDT)
            {
                AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
                {
                    FontSize = 15,
                    Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                    SelectedDateChange = DatePicker_SelectedDateChanged,
                    SelectedDate = DateTime.Parse(item.dt_time)
                };
                listTest.Add(new abc() { selectedDate = dp, id = item.id, dt_money = item.dt_money, dt_rose_id = item.dt_rose_id, dt_time = item.dt_time });
            }
            listTest = listTest.ToList();
            dteSelectedMonth.DisplayDate = DateTime.Parse(data.ro_time);
            dteSelectedMonth.SelectedDate = DateTime.Parse(data.ro_time);
            textThangAD.Text = DateTime.Parse(data.ro_time).ToString("MM/yyyy");
            tbInput1.Text = data.ro_note;
        }
        MainWindow Main;
        private DSNVHoaHongDoanhThu data1;
        private List<RoDtThoiDiem> _listDT;

        public List<RoDtThoiDiem> listDT
        {
            get { return _listDT; }
            set
            {
                _listDT = value; OnPropertyChanged();
            }
        }

        private List<DSCaiDatHoaHongDoanhThu> _listMucDT;

        public List<DSCaiDatHoaHongDoanhThu> listMucDT
        {
            get { return _listMucDT; }
            set
            {
                _listMucDT = value; OnPropertyChanged();
            }
        }

        private List<abc> _listTest = new List<abc>();

        public List<abc> listTest
        {
            get { return _listTest; }
            set { _listTest = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSCaiDatHoaHongDoanhThu api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongDoanhThu>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listMucDT = api.data.rose_dt;
                        foreach (var item in listMucDT)
                        {
                            if (item.tl_id == data1.tl_id)
                                cbMucDT.SelectedItem = item;
                        }
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_rose_dt.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
            {
                FontSize = 15,
                Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                SelectedDateChange = DatePicker_SelectedDateChanged,
                SelectedDate=DateTime.Now
            };
            listTest.Add(new abc() { id = listTest.Count, dt_money = "", dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate=dp });
            listTest = listTest.ToList();
        }

        public class abc
        {
            public string dt_rose_id { get; set; }
            public int id { get; set; }
            public string dt_money { get; set; }
            public string dt_time { get; set; }
            public Resouce.tool.DatePicker selectedDate { get; set; }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
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

        private void money_textchanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            abc data = (abc)tb.DataContext;
            foreach (var item in listTest)
            {
                if (item.id == data.id)
                    item.dt_money = tb.Text;
            }
            long tong = 0;
            try
            {
                foreach (var item in listTest)
                {
                    if (!string.IsNullOrEmpty(item.dt_money))
                    {
                        long x = long.Parse(item.dt_money);
                        if (tong + x <= long.MaxValue)
                        {
                            tong += x;
                            txttongdoanhthu.Text = tong + "";
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

        private void ThemHoaHong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateChuKy.Text = validateDate.Text = validateMucDT.Text = validateTongDT.Text = "";
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateChuKy.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(txttongdoanhthu.Text))
            {
                allow = false;
                validateTongDT.Text = "Vui lòng nhập đày đủ";
            }
            if (cbMucDT.SelectedIndex < 0)
            {
                allow = false;
                validateMucDT.Text = "Vui lòng chọn mức hoa hồng";
            }
            DSCaiDatHoaHongDoanhThu dt = new DSCaiDatHoaHongDoanhThu();
            dt = (DSCaiDatHoaHongDoanhThu)cbMucDT.SelectedItem;
            if (long.Parse(txttongdoanhthu.Text) > long.Parse(dt.tl_money_max) || long.Parse(txttongdoanhthu.Text) < long.Parse(dt.tl_money_min))
            {
                allow = false;
                validateMucDT.Text = "Mức doanh thu không phù hợp";
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
                    web.QueryString.Add("id_rose", data1.ro_id);
                    DateTime chuky = DateTime.Parse(textThangAD.Text);
                    web.QueryString.Add("chuky_dt", chuky.ToString("yyyy-MM-dd"));
                    
                    web.QueryString.Add("id_dt", dt.tl_id);
                    web.QueryString.Add("ghichu_u", tbInput1.Text);
                    for (int i = 0; i < listTest.Count; i++)
                    {
                        if (listTest[i].dt_money != null)
                        {
                            web.QueryString.Add("money[" + i + "]", listTest[i].dt_money);
                            string time;
                            time = listTest[i].dt_time.Replace("/"," - ");
                            string[] time1 = time.Split(' ');
                            time = "";
                            for(int j = time1.Length-1; j>-1; j--)
                            {
                                time += time1[j];
                            }
                            web.QueryString.Add("time_dt[" + i + "]", time);
                        }
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_rose_dt.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongDoanhThu(Main));
                Main.sidebar.SelectedIndex = -1;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
