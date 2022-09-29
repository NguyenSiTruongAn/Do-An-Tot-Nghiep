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
    /// Interaction logic for PopupChinhSuaPhucLoi.xaml
    /// </summary>
    public partial class PopupChinhSuaPhucLoi : Page
    {
        private DateTime day1, day_end1;
        private string d_e, id1;
        public PopupChinhSuaPhucLoi(MainWindow main, string id, string name, string salary, string type_tax, string day, string day_end, string note)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbInput.Text = name;
            tbInput1.Text = salary;
            if (type_tax == "0")
                cb_Loai.SelectedIndex = 1;
            else cb_Loai.SelectedIndex = 0;
            DateTime.TryParse(day, out day1);
            textThangAD.Text = day1.ToString("MM/yyyy");
            d_e = day_end;
            if (!string.IsNullOrEmpty(day_end))
            {
                DateTime.TryParse(day_end, out day_end1);
                textDenThang.Text = day_end1.ToString("MM/yyyy");
            }
            tbInput2.Text = note;
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
            validateName.Text = validateMoney.Text = validateDate.Text = validateLoai.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
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
            if (string.IsNullOrEmpty(cb_Loai.Text))
            {
                allow = false;
                validateLoai.Text = "Vui lòng chọn loại thu nhập";
            }
            if (allow)
            {
                string day_end = "";
                if (textDenThang.Text != "--------- ----")
                    day_end = "01/" + textDenThang.Text;
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id_welfare", id1);
                    web.QueryString.Add("name", tbInput.Text);
                    web.QueryString.Add("salary", tbInput1.Text);
                    web.QueryString.Add("type", "3");
                    web.QueryString.Add("date", "01/" + textThangAD.Text);
                    web.QueryString.Add("note", tbInput2.Text);
                    web.QueryString.Add("date_end", day_end);
                    string i;
                    if (cb_Loai.Text == "Thu nhập chịu thuế")
                        i = "1";
                    else i = "0";
                    web.QueryString.Add("type_tax", i);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ChinhSuaPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_ChinhSuaPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_welfare.php", web.QueryString);
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
