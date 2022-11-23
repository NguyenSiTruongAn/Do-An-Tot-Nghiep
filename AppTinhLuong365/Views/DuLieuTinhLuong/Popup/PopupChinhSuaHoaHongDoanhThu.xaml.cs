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
    /// Interaction logic for PopupChinhSuaHoaHongDoanhThu.xaml
    /// </summary>
    public partial class PopupChinhSuaHoaHongDoanhThu : Page
    {
        MainWindow Main;
        private string id;
        public PopupChinhSuaHoaHongDoanhThu(MainWindow main, DSCaiDatHoaHongDoanhThu data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = data.tl_id;
            tbInput.Text = data.tl_name;
            tbInput1.Text = data.tl_money_min;
            tbInput2.Text = data.tl_money_max;
            tbInput3.Text = data.tl_phan_tram;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            txtValidateName.Text = txtvalidateTienMax.Text = txtValidateTienMin.Text = txtValidateHoaHong.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                txtValidateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                txtValidateTienMin.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput2.Text))
            {
                allow = false;
                txtvalidateTienMax.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(tbInput3.Text))
            {
                allow = false;
                txtValidateHoaHong.Text = "Vui lòng nhập hoa hồng";
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
                    web.QueryString.Add("id_tl", id);
                    web.QueryString.Add("tl_money_min", tbInput1.Text);
                    web.QueryString.Add("tl_money_max", tbInput2.Text);
                    web.QueryString.Add("tl_phan_tram", tbInput3.Text);
                    web.QueryString.Add("tl_name", tbInput.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
                                Main.sidebar.SelectedIndex = 6;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch
                        {

                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_setting_dt.php", web.QueryString);
                }
            }
        }
    }
}
