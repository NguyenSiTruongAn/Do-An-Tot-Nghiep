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
    /// Interaction logic for PopupChinhSuaCSBH.xaml
    /// </summary>
    public partial class PopupChinhSuaCSBH : Page
    {
        MainWindow Main;
        private string id1, name1, note1, name_ct1, ct1, ct_hs1, fs_id1;

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            validateCT.Text = txtValuedateName.Text = "";
            string name = tbInput.Text;
            string note = tbInput1.Text;
            bool allow = true;
            if (string.IsNullOrEmpty(name))
            {
                txtValuedateName.Text = "Vui nhập tên khoản tiền khác";
                allow = false;
            }
            if (string.IsNullOrEmpty(ct1))
            {
                validateCT.Text = "Vui lòng thiết lập công thức";
                allow = false;
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
                    web.QueryString.Add("id_insurrance", id1);
                    web.QueryString.Add("name_recipe", name_ct1);
                    web.QueryString.Add("type_data", ct_hs1);
                    web.QueryString.Add("recipe", ct1);
                    web.QueryString.Add("name", name);
                    web.QueryString.Add("des", note);
                    web.QueryString.Add("fs_ct", fs_id1);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(Main));
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_insurrance.php", web.QueryString);
                }
                //Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(Main));
            }
        }

        public PopupChinhSuaCSBH(MainWindow main, string id, string name, string note, string name_ct, string ct, string ct_hs, string fs_id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            name1 = name;
            note1 = note;
            name_ct1 = name_ct;
            ct1 = ct;
            ct_hs1 = ct_hs;
            tbInput.Text = name;
            tbInput1.Text = note;
            fs_id1 = fs_id;
            id1 = id;
        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void ThietLapCongThuc_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCongThuc(Main, id1, tbInput.Text, tbInput1.Text, name_ct1, ct1, ct_hs1, fs_id1, "2");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 565;
            pop.Height = 481;
        }
    }
}
