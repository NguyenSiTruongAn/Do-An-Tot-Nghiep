using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for PopupSuaPhuCapTheoCa.xaml
    /// </summary>
    public partial class PopupSuaPhuCapTheoCa : Page
    {
        private DateTime day1, day_end1;
        private string d_e, id1;
        public PopupSuaPhuCapTheoCa(MainWindow main, string id, string salary, string day, string day_end)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbInput1.Text = salary;
            DateTime.TryParse(day, out day1);
            textThangAD.Text = day1.ToString("MM/yyyy");
            d_e = day_end;
            if (!string.IsNullOrEmpty(day_end))
            {
                DateTime.TryParse(day_end, out day_end1);
                textDenThang.Text = day_end1.ToString("MM/yyyy");
            }
            id1 = id;
        }
        MainWindow Main;

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }
        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
        }

        private void dteSelectedMonth1_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0 && string.IsNullOrEmpty(d_e))
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (textDenThang != null && !string.IsNullOrEmpty(x))
            {
                textDenThang.Text = x;
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag1 > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        int flag = 0;
        int flag1 = 0;
        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = day1.ToString("MM/yyyy");
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

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateMoney.Text = validateDate.Text = "";
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhập đầy đủ";
            }
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                    }
                    web.QueryString.Add("id_shift", id1);
                    web.QueryString.Add("salary", tbInput1.Text);
                    web.QueryString.Add("month_start", dteSelectedMonth.DisplayDate.ToString("yyyy-MM-dd"));
                    if (textDenThang.Text != "--------- ----")
                        web.QueryString.Add("month_end", dteSelectedMonth1.DisplayDate.ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ChinhSuaPhuCapTheoCa api = JsonConvert.DeserializeObject<API_ChinhSuaPhuCapTheoCa>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_welfare_shift.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                Main.sidebar.SelectedIndex = 5;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
