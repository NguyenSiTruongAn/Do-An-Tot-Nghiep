using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for CaiCaVaLichLamViec.xaml
    /// </summary>
    public partial class CaiCaVaLichLamViec : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CaiCaVaLichLamViec(MainWindow main, string ID, string Name)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            for (var i = 2022; i <= 2025; i++)
            {
                YearList.Add($"Năm {i}");
            }
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ID = id;
            tbInput.Text = Name;
            getData();
            getData1();
            getData2();
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public MainWindow Main;

        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }

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

        private void lv_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThemLichLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupSaoChepLich(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TuyChon_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            GeneralCalendar data = (GeneralCalendar)p.DataContext;
            var pop = new Views.CaiDat.Popup.PopupTuyChonLichLamViec(Main, data.cy_id);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<GeneralCalendar> _listGeneralCalendar;

        public List<GeneralCalendar> listGeneralCalendar
        {
            get { return _listGeneralCalendar; }
            set { _listGeneralCalendar = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListCalendarWork api = JsonConvert.DeserializeObject<API_ListCalendarWork>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listGeneralCalendar = api.data.general_calendar;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_calendarwork.php", web.QueryString);
            }
        }

        private List<PersonalCalendar> _listPersonalCalendar;

        public List<PersonalCalendar> listPersonalCalendar
        {
            get { return _listPersonalCalendar; }
            set { _listPersonalCalendar = value; OnPropertyChanged(); }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListCalendarWork api = JsonConvert.DeserializeObject<API_ListCalendarWork>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listPersonalCalendar = api.data.personal_calendar;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_calendarwork.php", web.QueryString);
            }
        }

        private Item _item;

        public Item item
        {
            get { return _item; }
            set { _item = value; OnPropertyChanged(); }
        }

        private void getData2()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("date", "2022/09/01");
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_CaiDatCongChuan api = JsonConvert.DeserializeObject<API_CaiDatCongChuan>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        item = api.data.item;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/get_config_workday_of_company.php", web.QueryString);
            }
        }

        private string id;

        private void ChinhSuaNhom(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateDes.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateDes.Text = "Vui lòng nhập đầy đủ";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                    }
                    web.QueryString.Add("id_group", id);
                    web.QueryString.Add("name", tbInput.Text);
                    web.QueryString.Add("des", tbInput1.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_SuaNhomLamViec api = JsonConvert.DeserializeObject<API_SuaNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_group_work.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
