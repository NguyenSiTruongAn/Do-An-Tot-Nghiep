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
    /// Interaction logic for PopupChinhSuaHoaHongTien.xaml
    /// </summary>
    public partial class PopupChinhSuaHoaHongTien : Page
    {
        MainWindow Main;
        DSNVHoaHongTien data1 = new DSNVHoaHongTien();
        public PopupChinhSuaHoaHongTien(MainWindow main, DSNVHoaHongTien data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            txtName.Text = data.ep_name;
            time.SelectedDate = DateTime.Parse(data.ro_time);
            tbInput.Text = data.ro_price;
            tbInput1.Text = data.ro_note;
            data1 = data;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if(time.SelectedDate == null)
            {
                allow = false;
                validateTime.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if(string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhạp đầy đủ";
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
                    ListEmployee nv_selected = new ListEmployee();
                    web.QueryString.Add("id_user", data1.ep_id);
                    web.QueryString.Add("time", time.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("money", tbInput.Text);
                    web.QueryString.Add("content", tbInput1.Text);
                    web.QueryString.Add("id_rose", data1.ro_id);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongTien(Main));
                                Main.sidebar.SelectedIndex = -1;
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_rose_tien.php", web.QueryString);
                }
            }
        }
    }
}
