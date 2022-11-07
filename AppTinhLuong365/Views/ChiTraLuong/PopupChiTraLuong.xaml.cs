using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.ChiTraLuong
{
    /// <summary>
    /// Interaction logic for PopupChiTraLuong.xaml
    /// </summary>
    public partial class PopupChiTraLuong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChiTraLuong(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            getData();
            ComboBox.SelectedIndex = 0; dteSelectedMonth = new Calendar();
            dteSelectedMonth.Visibility = Visibility.Collapsed;
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            dteSelectedMonth.MouseLeftButtonDown += Select_thang;
            dteSelectedMonth.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged;
            cl = new List<Calendar>();
            cl.Add(dteSelectedMonth);
            cl = cl.ToList();
        }

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }

        int flag = 0;

        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThang != null && !string.IsNullOrEmpty(x))
            {
                textThang.Text = x;
                valuedateDay = dteSelectedMonth.DisplayDate.ToString("yyyy/MM");
                DateTime a = DateTime.Parse(x);
            }
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag += 1;
        }

        private string valuedateDay = "";

        Calendar dteSelectedMonth { get; set; }

        private List<Calendar> _cl;

        public List<Calendar> cl
        {
            get { return _cl; }
            set
            {
                _cl = value; OnPropertyChanged();
            }
        }


        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }

        MainWindow Main;
        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private List<Item_dep> _listDep;

        public List<Item_dep> listDep
        {
            get { return _listDep; }
            set
            {
                if (value == null) value = new List<Item_dep>();
                value.Insert(0, new Item_dep() { dep_id = "0", dep_name = "Toàn bộ nhân viên" });
                _listDep = value;
                OnPropertyChanged();
            }
        }
        private void getData()
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
                        listDep = api.data.list;
                    }
                    //foreach (EpLate item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_dep.php",
                    web.QueryString);
            }
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateStartDate.Text =
                validateEndDate.Text = "";

            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập tên chi trả lương";
            }

            if (StartDate.SelectedDate == null)
            {
                allow = false;
                validateStartDate.Text = "Vui lòng chọn ngày bắt đầu";
            }

            if (EndDate.SelectedDate == null)
            {
                allow = false;
                validateEndDate.Text = "Vui lòng chọn ngày kết thúc";
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
                    web.QueryString.Add("pay_name", tbInput.Text);
                    string date = "";
                    if (textThang.Text != "--------- ----")
                        date = dteSelectedMonth.DisplayDate.ToString("yyyy-MM");
                    web.QueryString.Add("pay_for_time", date);
                    string startDate = "";
                    if (StartDate.SelectedDate != null)
                    {
                        startDate = StartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                    }
                    string endDate = "";
                    if (EndDate.SelectedDate != null)
                    {
                        endDate = EndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                    }

                    web.QueryString.Add("pay_time_start", startDate);
                    web.QueryString.Add("pay_time_end", endDate);
                    string i;
                    if (ComboBoxPay.Text == "Tiền mặt")
                    {
                        i = "1";
                    }
                    else
                    {
                        i = "2";
                    }
                    web.QueryString.Add("pay_unit", i);
                    if (ComboBox.SelectedItem != null)
                    {
                        Item_dep selectedShift = ComboBox.SelectedItem as Item_dep;
                        if (selectedShift != null && selectedShift.dep_id != "-1")
                        {
                            web.QueryString.Add("pay_for", selectedShift.dep_id);
                        }
                    }
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        string a = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_Add_late api =
                            JsonConvert.DeserializeObject<API_Add_late>(
                                UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_pay.php",
                        web.QueryString);
                }

                Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTraLuong(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
