using System;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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


        public CaiCaVaLichLamViec(MainWindow main)
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

            InitializeComponent();
            this.DataContext = this;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            Main = main;
            getData(month, year);
            getData1(month, year);
            getData2(month, year);
            searchBarMonth1.PlaceHolder = "Năm " + year;
            searchBarYear1.PlaceHolder = "Tháng " + month;
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

        private void lv_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThemLichLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupSaoChepLich(Main, searchBarMonth1.SelectedIndex, searchBarYear1.SelectedIndex));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TuyChon_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            GeneralCalendar data = (GeneralCalendar)p.DataContext;
            var pop = new Views.CaiDat.Popup.PopupTuyChonLichLamViec(Main, data.cy_id, data.apply_month, data.cy_name);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<GeneralCalendar> _listGeneralCalendar;

        public List<GeneralCalendar> listGeneralCalendar
        {
            get { return _listGeneralCalendar; }
            set
            {
                _listGeneralCalendar = value;
                OnPropertyChanged();
            }
        }

        private void getData(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListCalendarWork api =
                        JsonConvert.DeserializeObject<API_ListCalendarWork>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listGeneralCalendar = api.data.general_calendar;
                            DateTime aDateTime;
                            foreach (var a in listGeneralCalendar)
                            {
                                DateTime.TryParse(a.apply_month, out aDateTime);
                                a.apply_month = aDateTime.ToString("MM/yyyy");
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_calendarwork.php",
                    web.QueryString);
            }
        }

        private List<PersonalCalendar> _listPersonalCalendar;

        public List<PersonalCalendar> listPersonalCalendar
        {
            get { return _listPersonalCalendar; }
            set
            {
                _listPersonalCalendar = value;
                OnPropertyChanged();
            }
        }

        private void getData1(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListCalendarWork api =
                        JsonConvert.DeserializeObject<API_ListCalendarWork>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data.personal_calendar != null)
                        {
                            listPersonalCalendar = api.data.personal_calendar;
                            DateTime aDateTime;
                            foreach (var a in listPersonalCalendar)
                            {
                                DateTime.TryParse(a.apply_month, out aDateTime);
                                a.apply_month = aDateTime.ToString("MM/yyyy");
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_calendarwork.php",
                    web.QueryString);
            }
        }

        private Item _item;

        public Item item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        private string cw_id;
        private void getData2(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("date", year + "/" + month + "/01");
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_CaiDatCongChuan api =
                        JsonConvert.DeserializeObject<API_CaiDatCongChuan>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            item = api.data.item;
                            tbInput.Text = item.num_days;
                            cw_id = item.cw_id;
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
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/get_config_workday_of_company.php",
                    web.QueryString);
            }
        }

        private void Load(object sender, SelectionChangedEventArgs e)
        {
            string year = "", month = "";
            if (searchBarYear1.SelectedItem != null)
                year = searchBarYear1.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth1.SelectedIndex != -1)
                month = (searchBarMonth1.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            getData(month, year);
            getData1(month, year);
            getData2(month, year);
        }

        private void SearchBarMonth_OnLoaded(object sender, RoutedEventArgs e)
        {
            int indexMonth = ItemList.ToList().FindIndex(x => x.Contains(DateTime.Now.Month.ToString()));
            if (indexMonth > -1) searchBarMonth1.SelectedIndex = indexMonth;
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            int indexYear = YearList.ToList().FindIndex(x => x.Contains(DateTime.Now.Year.ToString()));
            if (indexYear > -1) searchBarYear1.SelectedIndex = indexYear;
        }

        private string id;

        private void TuyChon_Click1(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            PersonalCalendar data = (PersonalCalendar)p.DataContext;
            var pop = new Views.CaiDat.Popup.PopupTuyChonLichLamViec(Main, data.cy_id, data.apply_month, data.cy_name);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                string month="";
                string year = "";
                if (searchBarMonth1.SelectedItem != null)
                {
                    month = (searchBarMonth1.SelectedItem as string).Replace("Tháng ", "");
                }
                if (searchBarYear1.SelectedItem != null)
                {
                    year = (searchBarYear1.SelectedItem as string).Replace("Năm ", "");
                }
                DateTime date=new DateTime(int.Parse(year),int.Parse(month),1);
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("time_cong", date.ToString("yyyy/MM/dd"));
                    web.QueryString.Add("id", cw_id);
                }

                web.QueryString.Add("cong_chuan", tbInput.Text);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_Add_congchuan api =
                        JsonConvert.DeserializeObject<API_Add_congchuan>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_congchuan.php",
                    web.QueryString);
            }

            Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(Main));
            this.Control.SelectedIndex = 2;
            this.tabItem2.IsSelected = false;
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
    }
}