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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupNghiLe.xaml
    /// </summary>
    public partial class PopupNghiLe : Page
    {
        public PopupNghiLe(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateStartDate.Text = validateEndDate.Text = "";

            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập tên kỳ nghỉ lễ";
            }

            if (DatePickerStart.SelectedDate == null)
            {
                allow = false;
                validateStartDate.Text = "Vui lòng nhập ngày nghỉ lễ";
            }

            if (DatePickerEnd.SelectedDate == null)
            {
                allow = false;
                validateEndDate.Text = "Vui lòng nhập ngày nghỉ lễ";
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

                    web.QueryString.Add("lho_name", tbInput.Text);
                    web.QueryString.Add("lho_first_time", DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("lho_last_time", DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    string i;
                    if (BoxSalary.Text == "Tiền Lương")
                    {
                        i = "1";
                    }
                    else
                    {
                        i = "2";
                    }
                    web.QueryString.Add("lho_status", i);
                    web.QueryString.Add("lho_number", tbInput1.Text);

                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string a = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_Add_late api =
                                JsonConvert.DeserializeObject<API_Add_late>(
                                    UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiLe(Main));
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch{ }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_holiday.php",
                        web.QueryString);
                }
            }
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