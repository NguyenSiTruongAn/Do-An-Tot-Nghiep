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
    /// Interaction logic for PopupChinhSuaHoaHongLePhiViTri.xaml
    /// </summary>
    public partial class PopupChinhSuaHoaHongLePhiViTri : Page
    {
        MainWindow Main;
        DSCaiDatHoaHongVT data1 = new DSCaiDatHoaHongVT();
        public PopupChinhSuaHoaHongLePhiViTri(MainWindow main, DSCaiDatHoaHongVT data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            data1 = data;
            tbInput.Text = data.tl_name;
            tbInput1.Text = data.tl_money_min;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateHoaHong.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateHoaHong.Text = "Vui lòng nhập đầy đủ";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_tl", data1.tl_id);
                    }
                    web.QueryString.Add("tl_hoahong", tbInput1.Text);
                    web.QueryString.Add("tl_name", tbInput.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_setting_vt.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
                Main.sidebar.SelectedIndex = 6;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
