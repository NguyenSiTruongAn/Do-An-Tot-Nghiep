using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for NghiLe.xaml
    /// </summary>
    public partial class NghiLe : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public NghiLe(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            getData1();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupNghiLe(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<HolidayList> _holidayList;

        public List<HolidayList> holidayList
        {
            get { return _holidayList; }
            set { _holidayList = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("year", "2022");    
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_Holiday api = JsonConvert.DeserializeObject<API_List_Holiday>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        holidayList = api.data.holiday_list;
                        DateTime date;
                        foreach (var a in holidayList)
                        {
                            DateTime.TryParse(a.time_start, out date);
                            a.time_start = date.ToString("dd/MM/yyyy");
                            DateTime.TryParse(a.time_end, out date);
                            a.time_end = date.ToString("dd/MM/yyyy");
                        }
                    }
                    //foreach (ItemNp item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_holiday.php", web.QueryString);
            }
        }

        private List<HolidayList> _holidayList1;

        public List<HolidayList> holidayList1
        {
            get { return _holidayList1; }
            set { _holidayList1 = value; OnPropertyChanged(); }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("year", "2021");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_Holiday api = JsonConvert.DeserializeObject<API_List_Holiday>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        holidayList1 = api.data.holiday_list;
                        DateTime date;
                        foreach (var a in holidayList1)
                        {
                            DateTime.TryParse(a.time_start, out date);
                            a.time_start = date.ToString("dd/MM/yyyy");
                            DateTime.TryParse(a.time_end, out date);
                            a.time_end = date.ToString("dd/MM/yyyy");
                        }
                    }
                    //foreach (ItemNp item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_holiday.php", web.QueryString);
            }
        }
        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void TuyChonNghiLe_Click(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            HolidayList data = (HolidayList)t.DataContext;
            var pop = new Views.CaiDat.Popup.PopupTuyChinhNgayNghiLe(Main, data.lho_id, data.lho_name, data.lho_number, data.lho_status, data.time_start, data.time_end);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 208, z.Y + 30, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
