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
    /// Interaction logic for PopupThemMoiHoaHongDoanhThu.xaml
    /// </summary>
    public partial class PopupThemMoiHoaHongDoanhThu : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupThemMoiHoaHongDoanhThu(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            listDT = listDT;
            getData();
            getData1();
        }

        MainWindow Main;

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
                    API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data.data != null)
                    {
                        listNV = api.data.data.items;
                    }
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

        private List<DSCaiDatHoaHongDoanhThu> _listMucDT;

        public List<DSCaiDatHoaHongDoanhThu> listMucDT
        {
            get { return _listMucDT; }
            set
            {
                _listMucDT = value; OnPropertyChanged();
            }
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

        public class DoanhThu : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int id { get; set; }
            public string doanhThu { get; set; }
            private DateTime _date { get; set; }
            public DateTime date
            {
                get => _date;
                set
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        private int dem = 0;

        private List<DoanhThu> _listDT;

        public List<DoanhThu> listDT
        {
            get { return _listDT; }
            set
            {
                if (value == null) value = new List<DoanhThu>();
                value.Add(new DoanhThu() { id = dem, doanhThu = "", date = DateTime.Now });
                dem++;
                _listDT = value; OnPropertyChanged();
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
            this.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            listDT = listDT.ToList();
        }

        private void money_textchanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            DoanhThu data = (DoanhThu)tb.DataContext;
            foreach (var item in listDT)
            {
                if (item.id == data.id)
                    item.doanhThu = tb.Text;
            }
            long tong = 0;
            try
            {
                foreach (var item in listDT)
                {
                    if (!string.IsNullOrEmpty(item.doanhThu))
                    {
                        long x = long.Parse(item.doanhThu);
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
            if (cbName.SelectedIndex < 0)
            {
                allow = false;
                validateName.Text = "Vui lòng chọn nhân viên";
            }
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateChuKy.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(txttongdoanhthu.Text))
            {
                allow = false;
                txttongdoanhthu.Text = "Vui lòng nhập đày đủ";
            }
            if (cbMucDT.SelectedIndex < 0)
            {
                allow = false;
                validateMucDT.Text = "Vui lòng chọn mức hoa hồng";
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
                    web.QueryString.Add("select_user", nv_selected.ep_id);
                    web.QueryString.Add("chuky_dt", dteSelectedMonth.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("dt_money", txttongdoanhthu.Text);
                    DSCaiDatHoaHongDoanhThu dt = new DSCaiDatHoaHongDoanhThu();
                    dt = (DSCaiDatHoaHongDoanhThu)cbMucDT.SelectedItem;
                    web.QueryString.Add("name_dt", dt.tl_id);
                    web.QueryString.Add("ghichu_u", tbInput1.Text);
                    for (int i = 0; i < listDT.Count; i++)
                    {
                        if (listDT[i].doanhThu != null)
                        {
                            web.QueryString.Add("money_add[" + i + "]", listDT[i].doanhThu);
                            web.QueryString.Add("time_add[" + i + "]", listDT[i].date.ToString("yyyy-MM-dd"));
                        }
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_rose_dt.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongTien(Main));
                Main.sidebar.SelectedIndex = -1;
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
