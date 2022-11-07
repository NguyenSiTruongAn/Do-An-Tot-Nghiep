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

namespace AppTinhLuong365.Views.TinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaLuongCoBan.xaml
    /// </summary>
    public partial class PopupChinhSuaLuongCoBan : Page
    {
        public PopupChinhSuaLuongCoBan(MainWindow main, BasicSalary data, string data1)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbInput.Text = data.sb_salary_basic;
            tbInput1.Text = data.sb_salary_bh;
            tbInput2.Text = data.sb_pc_bh;
            dpThang.SelectedDate = DateTime.Parse(data.sb_time_up);
            tbInput3.Text = data.sb_lydo;
            tbInput4.Text = data.sb_quyetdinh;
            this.data = data;
            this.data1 = data1;
        }

        MainWindow Main;
        BasicSalary data;
        string data1;

        private void SuaLuong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateLuong.Text = "Vui lòng nhập đầy đủ";
            }
            if (dpThang.SelectedDate == null)
            {
                allow = false;
                validateTG.Text = "Vui lòng chọn thời gian áp dụng";
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
                    web.QueryString.Add("id_bs", data.sb_id);
                    web.QueryString.Add("salary", tbInput.Text);
                    web.QueryString.Add("salary_bh", tbInput1.Text);
                    web.QueryString.Add("phucapbh", tbInput2.Text);
                    web.QueryString.Add("date_ss", dpThang.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("lydo", tbInput3.Text);
                    web.QueryString.Add("quyetdinh", tbInput4.Text);

                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data1));
                            Main.HomeSelectionPage.Visibility = Visibility.Visible;
                            this.Visibility = Visibility.Collapsed;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_basic_salary.php", web.QueryString);
                }
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
