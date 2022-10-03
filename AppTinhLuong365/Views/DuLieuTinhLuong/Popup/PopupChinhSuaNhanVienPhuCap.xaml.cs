using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaNhanVienPhuCap.xaml
    /// </summary>
    public partial class PopupChinhSuaNhanVienPhuCap : Page
    {
        private DateTime day1, day_end1, day2, day_end2;
        private string d_e, id1;
        private bool setDayEnd;
        public PopupChinhSuaNhanVienPhuCap(MainWindow main, string id_wf, string date_st, string date_end, string day, string day_end)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            DateTime.TryParse(date_st, out day1);
            textThangAD.SelectedDate = day1;
            d_e = date_end;
            if (!string.IsNullOrEmpty(date_end))
            {
                DateTime.TryParse(date_end, out day_end1);
                textDenThang.SelectedDate = day_end1;
            }
            id1 = id_wf;
            DateTime.TryParse(day, out day2);
            DateTime.TryParse(day_end, out day_end2);
            if (day_end != null)
            {
                DateTime.TryParse(day_end, out day_end2);
                setDayEnd = true;
            }
        }
        MainWindow Main;

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateDate.Text = "";
            if (textThangAD.SelectedDate == null)
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if(textThangAD.SelectedDate != null && textDenThang.SelectedDate != null)
            if(textThangAD.SelectedDate.Value.ToString("yyyy-MM-dd").CompareTo(textDenThang.SelectedDate.Value.ToString("yyyy-MM-dd")) > 0)
            {
                allow = false;
                validateDateEnd.Text = "Ngày bắt đầu áp dụng phải nhỏ hơn ngày kết thúc";
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
                    web.QueryString.Add("id_wf", id1);
                    web.QueryString.Add("date_st", textThangAD.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    if (textDenThang.SelectedDate !=null)
                        web.QueryString.Add("date_end", textDenThang.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_SuaNhanVienTrongPhuLoi api = JsonConvert.DeserializeObject<API_SuaNhanVienTrongPhuLoi>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_wf.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                Main.sidebar.SelectedIndex = 5;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
