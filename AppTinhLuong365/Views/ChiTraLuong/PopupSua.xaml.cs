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
    /// Interaction logic for PopupSua.xaml
    /// </summary>
    public partial class PopupSua : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        public PopupSua(MainWindow main, string dataPayId, string dataPayName, string dataPayForTime, string dataPayTimeStart, string dataPayTimeEnd, string dataPayUnit)
        {
            this.DataContext = this;
            InitializeComponent();
            id = dataPayId;
            tbInput.Text = dataPayName;
            textThang.Text = dataPayForTime;
            DateTime startDate;
            if (!string.IsNullOrEmpty(dataPayTimeStart) && DateTime.TryParse(dataPayTimeStart, out startDate))
            {
                StartDate.SelectedDate = startDate;
            }
            DateTime endDate = new DateTime();
            if (!string.IsNullOrEmpty(dataPayTimeEnd))
            {
                string time;
                time = dataPayTimeEnd.Replace("/", " - ");
                string[] time1 = time.Split(' ');
                time = "";
                for (int j = time1.Length - 1; j > -1; j--)
                {
                    time += time1[j];
                }
                EndDate.SelectedDate = DateTime.Parse(time);
            }

            if (dataPayUnit == "1")
            {
                ComboBoxPay.SelectedIndex = 0;
            }
            else if (dataPayUnit == "2")
            {
                ComboBoxPay.SelectedIndex = 1;
            }
            Main = main;
            dteSelectedMonth = new Calendar();
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
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("pay_id", id);
                    }
                    web.QueryString.Add("pay_name", tbInput.Text);
                    DateTime date;
                    string c = "";
                    if (textThang.Text != "--------- ----")
                    {
                        DateTime.TryParse(textThang.Text, out date);
                        c = date.ToString("yyyy-MM");
                    } 
                    web.QueryString.Add("pay_for_time", c);
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
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_pay.php",
                        web.QueryString);
                }

                Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTraLuong(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
