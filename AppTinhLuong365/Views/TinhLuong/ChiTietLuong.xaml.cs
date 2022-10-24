using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for ChiTietLuong.xaml
    /// </summary>
    public partial class ChiTietLuong : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string ep_id;

        public ChiTietLuong(MainWindow main, string name, string depName, string epId, int month, int year)
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
            Main = main;
            // string Month = DateTime.Now.ToString("MM");
            // string Year = DateTime.Now.ToString("yyyy");
            string start_date = DateTime.Now.ToString("yyyy-MM-01");
            string end_date = DateTime.Now.ToString("yyyy-MM-30");
            int d = month + 1;
            int e = year + DateTime.Now.Year - 1;
            searchBarMonth.SelectedIndex = d - 1;
            searchBarYear.SelectedIndex = e - e + 1;
            DateTime a = (DateTime.Parse(start_date));
            DateTime b = (DateTime.Parse(end_date));
            DatePickerStart.SelectedDate = a;
            DatePickerEnd.SelectedDate = b;
            Name.Text = name;
            Pb.Text = depName;
            Id.Text = epId;
            this.ep_id = epId;
            Thang.Text = "tháng " + d + "/" + e;
            getData(d, e, start_date, end_date);
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

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

        private Item_Luong_nv _itemLuongNv;

        public Item_Luong_nv itemLuongNv
        {
            get { return _itemLuongNv; }
            set
            {
                _itemLuongNv = value;
                OnPropertyChanged();
            }
        }

        private void getData(int month, int year, string start_date, string end_date)
        {
            using (WebClient web = new WebClient())
            {
                // loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month.ToString());
                web.QueryString.Add("year", year.ToString());
                web.QueryString.Add("id_user", ep_id);
                web.QueryString.Add("start_date", start_date);
                    web.QueryString.Add("end_date", end_date);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Luong_nv api =
                        JsonConvert.DeserializeObject<API_Luong_nv>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        itemLuongNv = api.data.luong_nv;
                    }
                    // loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_luong_nv.php",
                    web.QueryString);
            }
        }

        private void Month(object sender, SelectionChangedEventArgs e)
        {
            string a = DateTime.Now.ToString((searchBarYear.SelectedIndex + DateTime.Now.Year - 1) + "-" +
                                             (searchBarMonth.SelectedIndex + 1) + "-01");
            DateTime b = DateTime.Parse(a);
            DatePickerStart.SelectedDate = b;
            string c = DateTime.Now.ToString((searchBarYear.SelectedIndex + DateTime.Now.Year - 1) + "-" +
                                             (searchBarMonth.SelectedIndex + 1) + "-" +
                                             DateTime.DaysInMonth((searchBarYear.SelectedIndex + 2021),
                                                 (searchBarMonth.SelectedIndex + 1)));
            DateTime d = DateTime.Parse(c);
            DatePickerEnd.SelectedDate = d;
        }
        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", start_date = "", end_date = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");
            
            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            getData(Convert.ToInt32(month), Convert.ToInt32(year), start_date, end_date);
        }

        // [System.ComponentModel.Bindable(true)]
        // public System.Windows.Controls.Primitives.PopupAnimation PopupAnimation { get; set; }
        //
        // private void Open(object sender, MouseEventArgs e)
        // {
        //     myPopup.AllowsTransparency = true;
        //     myTextBlockPopup.PopupAnimation = PopupAnimation.Fade;
        // }
    }
}
