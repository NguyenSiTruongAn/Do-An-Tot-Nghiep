using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for PopupChinhSuaNgayNghiLe.xaml
    /// </summary>
    public partial class PopupChinhSuaNgayNghiLe : Page
    {
        private string id;
        private string name;
        private string number;
        private int status;
        private string start_time;
        private string end_time;
        public PopupChinhSuaNgayNghiLe(MainWindow main, string id, string name, string number, int status, string startTime, string endTime)
        {
            this.DataContext = this;
            InitializeComponent();
            this.id = id;
            tbInput.Text = name;
            tbInput1.Text = number;
            BoxSalary.SelectedIndex = status - 1;

            DateTime checkDate;
            if (!string.IsNullOrEmpty(startTime) && DateTime.TryParse(startTime, out checkDate))
            {
                DatePickerStart.SelectedDate = checkDate;
            }

            DateTime checkDate1;
            if (!string.IsNullOrEmpty(endTime) && DateTime.TryParse(endTime, out checkDate1))
            {
                DatePickerEnd.SelectedDate = checkDate1;
            }
            Main = main;
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
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
                        web.QueryString.Add("lho_id", id);
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
                                JsonConvert.DeserializeObject<API_Add_late>(a);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiLe(Main));
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_holiday.php",
                        web.QueryString);
                }

                
            }
        }
    }
}
