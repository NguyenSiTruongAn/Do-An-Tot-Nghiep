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

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaNVHHLoiNhuan.xaml
    /// </summary>
    public partial class PopupChinhSuaNVHHLoiNhuan : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChinhSuaNVHHLoiNhuan(MainWindow main, DSNVHoaHongLoiNhuan data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            txtName.Text = data.ep_name;
            txtTongTien.Text = data.ro_price;
            listDT = data.arr_rdt;
            data1 = data;
            for (int i = 0; i < listDT.Count; i++)
            {
                listDT[i].id = i;
            }
            foreach (var item in listDT)
            {
                AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
                {
                    FontSize = 15,
                    Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                    SelectedDateChange = DatePicker_SelectedDateChanged,
                    SelectedDate = DateTime.Parse(item.dt_time)
                };
                listTest.Add(new abc() { selectedDate = dp, id = item.id, dt_money = item.dt_money, dt_sl = item.dt_sl, dt_rose_id = item.dt_rose_id, dt_time = item.dt_time });
            }
            listTest = listTest.ToList();
            tbInput1.Text = data.ro_note;
        }
        MainWindow Main;
        private DSNVHoaHongLoiNhuan data1;
        private List<ArrRdt> _listDT;

        public List<ArrRdt> listDT
        {
            get { return _listDT; }
            set
            {
                _listDT = value; OnPropertyChanged();
            }
        }

        private List<DSCaiDatHoaHongLoiNhuan> _listSanPham;

        public List<DSCaiDatHoaHongLoiNhuan> listSanPham
        {
            get { return _listSanPham; }
            set
            {
                _listSanPham = value; OnPropertyChanged();
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
                web.QueryString.Add("type", "3");
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSCaiDatHoaHongLoiNhuan api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongLoiNhuan>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listSanPham = api.data.list;
                            foreach (var item in listSanPham)
                            {
                                if (item.tl_id == data1.ro_id_tl)
                                    cbSanPham.SelectedItem = item;
                            }
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

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppTinhLuong365.Resouce.tool.DatePicker dp = new Resouce.tool.DatePicker
            {
                FontSize = 15,
                Foreground = Application.Current.Resources["#777777"] as SolidColorBrush,
                SelectedDateChange = DatePicker_SelectedDateChanged,
                SelectedDate = DateTime.Now
            };
            listTest.Add(new abc() { id = listTest.Count, dt_money = "", dt_sl="", dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate = dp });
            listTest = listTest.ToList();
        }

        public class abc
        {
            public string dt_rose_id { get; set; }
            public int id { get; set; }
            public string dt_money { get; set; }
            public string dt_sl { get; set; }
            public string dt_time { get; set; }
            public Resouce.tool.DatePicker selectedDate { get; set; }
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void SLChanged(object sender, TextChangedEventArgs e)
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
                            txtTongTien.Text = tong + "";
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

        private void Txt_PreviewCurrencyTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ThemHoaHong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateSL.Text = validateSanPham.Text = validateDT.Text = "";
            if (string.IsNullOrEmpty(txtTongTien.Text))
            {
                allow = false;
                validateDT.Text = "Vui lòng nhập đày đủ";
            }
            if (cbSanPham.SelectedIndex < 0)
            {
                allow = false;
                validateSanPham.Text = "Vui lòng chọn mức hoa hồng";
            }
            DSCaiDatHoaHongLoiNhuan dt = new DSCaiDatHoaHongLoiNhuan();
            dt = (DSCaiDatHoaHongLoiNhuan)cbSanPham.SelectedItem;
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
                    web.QueryString.Add("id_sp", dt.tl_id);
                    web.QueryString.Add("ghichu_u", tbInput1.Text);
                    web.QueryString.Add("chuky", data1.ro_time);
                    for (int i = 0; i < listTest.Count; i++)
                    {
                        if (listTest[i].dt_money != null)
                        {
                            web.QueryString.Add("money_add[" + i + "]", listTest[i].dt_money);
                            web.QueryString.Add("amount[" + i + "]", listTest[i].dt_sl);
                            string time;
                            time = listTest[i].dt_time.Replace("/", " - ");
                            string[] time1 = time.Split(' ');
                            time = "";
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
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongLoiNhuan(Main));
                                Main.sidebar.SelectedIndex = -1;
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_rose_loinhuan.php", web.QueryString);
                }
            }
        }
    }
}
