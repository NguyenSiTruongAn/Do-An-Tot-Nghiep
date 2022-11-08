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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupSaoChepLich.xaml
    /// </summary>
    public partial class PopupSaoChepLich : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string month;
        string year;

        public PopupSaoChepLich(MainWindow main, int month, int year)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            dteSelectedMonth = new Calendar();
            dteSelectedMonth.Visibility = Visibility.Collapsed;
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            dteSelectedMonth.MouseLeftButtonDown += Select_thang;
            dteSelectedMonth.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged;
            this.month = (month + 1) + "";
            this.year = (year + DateTime.Now.Year - 1) + "";
            RunThang.Text = this.month + "/" + this.year;   
            cl = new List<Calendar>();
            cl.Add(dteSelectedMonth);
            cl = cl.ToList();
            getData();
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
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
                DateTime a = DateTime.Parse(x);
            }

            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }

            flag += 1;
        }

        Calendar dteSelectedMonth { get; set; }

        private List<Calendar> _cl;

        public List<Calendar> cl
        {
            get { return _cl; }
            set
            {
                _cl = value;
                OnPropertyChanged();
            }
        }

        private List<AllCalendar> _listCalendar;

        public List<AllCalendar> listCalendar
        {
            get { return _listCalendar; }
            set
            {
                _listCalendar = value;
                OnPropertyChanged();
            }
        }

        private void getData()
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
                            listCalendar = api.data.all_calendar;
                            DateTime aDateTime;
                            foreach (var a in listCalendar)
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

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void Copy(object sender, MouseButtonEventArgs e)
        {
            List<string> nv = new List<string>();
            validateList.Text = "";
            bool allow = true;
            foreach (var item in listCalendar)
            {
                if (item.IsChecked == true)
                    nv.Add(item.cy_id);
            }

            if (!string.IsNullOrEmpty(textThang.Text) && textThang.Text == "--------- ----")
            {
                allow = false;
                validateList.Text = "Vui lòng điền đầy đủ thông tin";
            }

            if (nv.Count <= 0)
            {
                allow = false;
            }


            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    for (int i = 0; i < nv.Count; i++)
                    {
                        string id = "arr_id_llv[" + i + "]";
                        // string salary = "pp_salary[" + i + "]";
                        web.QueryString.Add(id, nv[i]);
                    }

                    web.QueryString.Add("month", DateTime.Parse(textThang.Text).ToString("yyyy-MM-dd"));

                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_Add_late api =
                            JsonConvert.DeserializeObject<API_Add_late>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(
                                    new Views.CaiDat.CaiCaVaLichLamViec(Main));
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/copy_calendarwork.php",
                        web.QueryString);
                }
            }
        }

        private void ChonNhanvien(object sender, MouseButtonEventArgs e)
        {
            Border cb = sender as Border;
            if (chonnv.Text == "Chọn tất cả")
            {
                if (listCalendar != null)
                {
                    foreach (var item in listCalendar)
                    {
                        item.status = true;
                        item.IsChecked = true;
                    }
                }

                chonnv.Text = "Bỏ chọn";
            }

            else
            {
                if (listCalendar != null)
                {
                    foreach (var item in listCalendar)
                    {
                        item.status = false;
                        item.IsChecked = false;
                    }
                }

                chonnv.Text = "Chọn tất cả";
            }
        }

        private void listOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        }
    }
}