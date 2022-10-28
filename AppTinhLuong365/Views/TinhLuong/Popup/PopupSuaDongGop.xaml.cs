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

namespace AppTinhLuong365.Views.TinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupSuaDongGop.xaml
    /// </summary>
    public partial class PopupSuaDongGop : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupSuaDongGop(MainWindow main, Don data, ItemEmp data1)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.data = data;
            this.data1 = data1;
            tbInput.Text = data.don_name;
            tbInput1.Text = data.don_price;
            dteSelectedMonth = new Calendar();
            dteSelectedMonth.Visibility = Visibility.Collapsed;
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            dteSelectedMonth.MouseLeftButtonDown += Select_thang;
            dteSelectedMonth.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged;
            cl = new List<Calendar>();
            cl.Add(dteSelectedMonth);
            cl = cl.ToList();
            dteSelectedMonth1 = new Calendar();
            dteSelectedMonth1.Visibility = Visibility.Collapsed;
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            dteSelectedMonth1.MouseLeftButtonDown += Select_thang_end;
            dteSelectedMonth1.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged1;
            cl1 = new List<Calendar>();
            cl1.Add(dteSelectedMonth1);
            cl1 = cl1.ToList();
            textThangAD.Text = DateTime.Parse(data.don_time_active).ToString("MM/yyyy");
            textThangEnd.Text = DateTime.Parse(data.don_time_end).ToString("MM/yyyy");
        }
        Calendar dteSelectedMonth { get; set; }
        Calendar dteSelectedMonth1 { get; set; }

        private List<Calendar> _cl;

        public List<Calendar> cl
        {
            get { return _cl; }
            set
            {
                _cl = value; OnPropertyChanged();
            }
        }

        private List<Calendar> _cl1;

        public List<Calendar> cl1
        {
            get { return _cl1; }
            set
            {
                _cl1 = value; OnPropertyChanged();
            }
        }
        MainWindow Main;
        Don data;
        ItemEmp data1;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        int flag = 0;
        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
            dteSelectedMonth.DisplayDateEnd = DateTime.Parse(textThangEnd.Text);
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

        int flag1 = 0;
        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag1 = 1;
            dteSelectedMonth1.DisplayDateStart = DateTime.Parse(textThangAD.Text);
        }

        private void dteSelectedMonth_DisplayModeChanged1(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (textThangEnd != null && !string.IsNullOrEmpty(x))
            {
                textThangEnd.Text = x;
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        private void SuaDongGop(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateTien.Text = validateDate.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateTien.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(textThangAD.Text))
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (DateTime.Parse(textThangAD.Text) > DateTime.Parse(textThangEnd.Text))
            {
                allow = false;
                validateTimeEnd.Text = "Vui lòng chọn tháng kết thúc lớn hơn hoặc bằng tháng bắt đầu";
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
                    web.QueryString.Add("id_don", data.don_id);
                    web.QueryString.Add("bname", tbInput.Text);
                    web.QueryString.Add("bmoney", tbInput1.Text);
                    web.QueryString.Add("btime", DateTime.Parse(textThangAD.Text).ToString("yyyy-MM-dd"));
                    web.QueryString.Add("bend", DateTime.Parse(textThangEnd.Text).ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data1.ep_id));
                            Main.HomeSelectionPage.Visibility = Visibility.Visible;
                            this.Visibility = Visibility.Collapsed;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_don.php", web.QueryString);
                }
            }
        }

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
