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
    /// Interaction logic for PopupChinhSuaPhuCap.xaml
    /// </summary>
    public partial class PopupChinhSuaPhuCap : Page
    {
        private DateTime day1, day_end1;
        private string de,id1;
        public PopupChinhSuaPhuCap(MainWindow main, string id, string name, string salary, string type_tax, string day, string day_end, string note)
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
            textThangAD.SelectedDate = day1;
            DateTime.TryParse(day_end, out day_end1);
            de = day_end;
            if (!string.IsNullOrEmpty(de)) textDenThang.SelectedDate = day_end1;
            tbInput2.Text = note;
            id1 = id;
        }
        MainWindow Main;

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
            if (textThangAD.SelectedDate == null)
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn tháng áp dụng";
            }
            if (string.IsNullOrEmpty(cb_Loai.Text))
            {
                allow = false;
                validateLoai.Text = "Vui lòng chọn loại thu nhập";
            }
            if (allow)
            {
                string x;
                if (string.IsNullOrEmpty(de))
                    x = "";
                else x = textDenThang.SelectedDate.Value.ToString("dd/MM/yyyy");
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                    }
                    web.QueryString.Add("id_welfare", id1);
                    web.QueryString.Add("name", tbInput.Text);
                    web.QueryString.Add("salary", tbInput1.Text);
                    web.QueryString.Add("type", "4");
                    web.QueryString.Add("date", textThangAD.SelectedDate.Value.ToString("dd/MM/yyyy"));
                    web.QueryString.Add("note", tbInput2.Text);
                    web.QueryString.Add("date_end", x);
                    string i;
                    if (cb_Loai.Text == "Thu nhập chịu thuế")
                        i = "1";
                    else i = "0";
                    web.QueryString.Add("type_tax", i);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ChinhSuaPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_ChinhSuaPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                                Main.sidebar.SelectedIndex = 5;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_welfare.php", web.QueryString);
                }
            }
        }

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }
    }
}
