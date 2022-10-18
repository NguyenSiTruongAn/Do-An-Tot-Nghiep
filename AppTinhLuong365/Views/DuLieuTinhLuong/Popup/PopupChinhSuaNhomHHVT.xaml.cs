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
    /// Interaction logic for PopupChinhSuaNhomHHVT.xaml
    /// </summary>
    public partial class PopupChinhSuaNhomHHVT : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChinhSuaNhomHHVT(MainWindow main, DSNhomHHLPViTri data)
        {
            InitializeComponent(); this.DataContext = this;
            Main = main;
            data1 = data;
            listNVNhom = data.ep_gr;
            listVT = data.arr_sl;
            for (int i = 0; i < listVT.Count; i++)
            {
                listVT[i].id = i;
            }
            foreach (var item in listVT)
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
            txtTongSanPham1.Text = data.ro_so_luong;
            getData();
        }
        MainWindow Main;
        DSNhomHHLPViTri data1;

        private List<ArrSlVT> _listVT;

        public List<ArrSlVT> listVT
        {
            get { return _listVT; }
            set
            {
                _listVT = value; OnPropertyChanged();
            }
        }

        private List<EpGrVT> _listNVNhom;

        public List<EpGrVT> listNVNhom
        {
            get { return _listNVNhom; }
            set
            {
                _listNVNhom = value; OnPropertyChanged();
            }
        }
        private List<DSCaiDatHoaHongVT> _listSanPham;

        public List<DSCaiDatHoaHongVT> listSanPham
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
                web.QueryString.Add("type", "4");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSCaiDatHoaHongVT api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongVT>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listSanPham = api.data.list;
                        foreach (var item in listSanPham)
                        {
                            if (item.tl_id == data1.ep_gr[0].pr_id_tl)
                                cbSanPham1.SelectedItem = item;
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/setting_rose.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
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
            public string dt_sl { get; set; }
            public Resouce.tool.DatePicker selectedDate { get; set; }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
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
            bool allow = false;
            validateSanPham1.Text = validateNhom.Text = validateTongSP.Text = "";
            foreach(var item in listTest)
            {
                if (!string.IsNullOrEmpty(item.dt_sl))
                    allow = true;
            }
            if(allow == false)
            {
                validateTongSP.Text = "Vui lòng nhập đầy đủ";
            }
            if (cbSanPham1.SelectedIndex < 0)
            {
                allow = false;
                validateSanPham1.Text = "Vui lòng chọn sản phẩm";
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
                    DSCaiDatHoaHongVT sp = new DSCaiDatHoaHongVT();
                    sp = (DSCaiDatHoaHongVT)cbSanPham1.SelectedItem;
                    web.QueryString.Add("id_sp", sp.tl_id);
                    web.QueryString.Add("id_gr", data1.ro_id_group);
                    for (int i = 0; i < listNVNhom.Count; i++)
                    {
                        web.QueryString.Add("id_user[" + i + "]", listNVNhom[i].pr_id_user);
                        web.QueryString.Add("percent[" + i + "]", listNVNhom[i].pr_percent);
                    }

                    web.QueryString.Add("chuky", data1.ro_time);
                    for (int i = 0; i < listTest.Count; i++)
                    {
                        if (listTest[i].dt_money != null)
                        {
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
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            var pop = new Views.DuLieuTinhLuong.HoaHongLePhiViTri(Main);
                            Main.HomeSelectionPage.NavigationService.Navigate(pop);
                            Main.sidebar.SelectedIndex = -1;
                            pop.tb.SelectedIndex = 1;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_group_rose_fees.php", web.QueryString);
                }
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void Changed_HoaHong(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            EpGrVT data = (EpGrVT)tb.DataContext;
            foreach (var item in listNVNhom)
            {
                if (data.pr_id_user == item.pr_id_user)
                {
                    item.pr_percent = tb.Text;
                }
            }
        }

        private void Txt_PreviewCurrencyTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
