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
    /// Interaction logic for PopupThemKhoanTienKhac.xaml
    /// </summary>
    public partial class PopupThemKhoanTienKhac : Page
    {
        MainWindow Main;
        private string name1, name_ct1, ct1, ct_hs1, note1;

        private void ThemKhoanTienKhac(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(ct1))
            {
                allow = false;
                txtValuedate.Text = "Vui lòng thiết lập công thức";
            }
            if (string.IsNullOrEmpty(name1))
            {
                allow = false;
                txtValuedateName.Text = "Vui lòng nhập tên khoản tiền khác";
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
                    web.QueryString.Add("fs_name", name_ct1);
                    web.QueryString.Add("fs_data", ct_hs1);
                    web.QueryString.Add("fs_repica", ct1);
                    web.QueryString.Add("cl_name", name1);
                    web.QueryString.Add("cl_note", note1);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_otherMoney.php", web.QueryString);
                }
                //Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }

        public PopupThemKhoanTienKhac(MainWindow main, string name, string note, string name_ct, string ct, string ct_hs)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbInput.Text = name;
            tbInput1.Text = note;
            ct1 = ct;
            name_ct1 = name_ct;
            ct_hs1 = ct_hs;
            name1 = name;
            note1 = note;
        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ThietLapCongThuc_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            name1 = tbInput.Text;
            note1 = tbInput.Text;
            var pop = new Views.TinhLuong.PopupChinhSuaThue(Main, "1", name1, note1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 565;
            pop.Height = 481;
        }
    }
}
