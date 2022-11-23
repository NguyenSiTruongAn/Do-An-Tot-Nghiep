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
    /// Interaction logic for PopupChinhSuaNhomHoaHongDT.xaml
    /// </summary>
    public partial class PopupChinhSuaNhomHoaHongDT : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChinhSuaNhomHoaHongDT(MainWindow main, DSNhomHoaHongDoanhThu data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            data1 = data;
            listNVNhom = data.arr_user_g;
            listDT = data.dt_thoi_diem;
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
                listTest.Add(new abc() { selectedDate = dp, id = item.id, dt_money = item.dt_money, dt_rose_id = item.dt_rose_id, dt_time = item.dt_time });
            }
            listTest = listTest.ToList();
            tbInput7.Text = data.ro_price;
            getData();
        }
        MainWindow Main;
        DSNhomHoaHongDoanhThu data1;

        private List<DtThoiDiem> _listDT;

        public List<DtThoiDiem> listDT
        {
            get { return _listDT; }
            set
            {
                _listDT = value; OnPropertyChanged();
            }
        }

        private List<ArrUserG> _listNVNhom;

        public List<ArrUserG> listNVNhom
        {
            get { return _listNVNhom; }
            set
            {
                _listNVNhom = value; OnPropertyChanged();
            }
        }

        private void tbPTHH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
                    try
                    {
                        API_DSCaiDatHoaHongDoanhThu api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongDoanhThu>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listMucDT = api.data.rose_dt;
                            foreach (var item in listMucDT)
                            {
                                if (item.tl_id == data1.tl_id)
                                    cbMucDT1.SelectedItem = item;
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
                SelectedDate = DateTime.Now
            };
            listTest.Add(new abc() { id = listTest.Count, dt_money = "", dt_time = DateTime.Now.ToString("dd/MM/yyyy"), selectedDate = dp });
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
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
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
                            tbInput7.Text = tong + "";
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

        private void TabControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void LuuHoaHong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateMucDT1.Text = validateMoney.Text = "";
            if (string.IsNullOrEmpty(tbInput7.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhập đày đủ";
            }
            if (cbMucDT1.SelectedIndex < 0)
            {
                allow = false;
                validateMucDT1.Text = "Vui lòng chọn mức hoa hồng";
            }
            DSCaiDatHoaHongDoanhThu dt = new DSCaiDatHoaHongDoanhThu();
            dt = (DSCaiDatHoaHongDoanhThu)cbMucDT1.SelectedItem;
            if (long.Parse(tbInput7.Text) > long.Parse(dt.tl_money_max) || long.Parse(tbInput7.Text) < long.Parse(dt.tl_money_min))
            {
                allow = false;
                validateMucDT1.Text = "Mức doanh thu không phù hợp";
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
                    for(int i=0; i< listNVNhom.Count; i++)
                    {
                        web.QueryString.Add("id_user[" + i + "]", listNVNhom[i].pr_id_user);
                        web.QueryString.Add("dt_percent[" + i + "]", listNVNhom[i].pr_percent);
                    }
                    
                    web.QueryString.Add("id_gr", data1.ro_id_group);
                    web.QueryString.Add("chuky_dt", data1.ro_time);
                    web.QueryString.Add("ghichu_u", data1.ro_note);
                    web.QueryString.Add("id_dt", dt.tl_id);
                    for (int i = 0; i < listTest.Count; i++)
                    {
                        if (listTest[i].dt_money != null)
                        {
                            web.QueryString.Add("money[" + i + "]", listTest[i].dt_money);
                            string time;
                            time = listTest[i].dt_time.Replace("/", " - ");
                            string[] time1 = time.Split(' ');
                            time = "";
                            for (int j = time1.Length - 1; j > -1; j--)
                            {
                                time += time1[j];
                            }
                            web.QueryString.Add("time_g_dt[" + i + "]", time);
                        }
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                var pop = new Views.DuLieuTinhLuong.HoaHongDoanhThu(Main);
                                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                                Main.sidebar.SelectedIndex = -1;
                                pop.tb1.SelectedIndex = 1;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_group_rose_dt.php", web.QueryString);
                }
                
            }
        }

        private void Changed_HoaHong(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            ArrUserG data = (ArrUserG)tb.DataContext;
            foreach(var item in listNVNhom)
            {
                if(data.pr_id_user == item.pr_id_user)
                {
                    item.pr_percent = tb.Text;
                }
            }
        }
    }
}
