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
    /// Interaction logic for PopupChinhSuaThue.xaml
    /// </summary>
    public partial class PopupChinhSuaThue : Page
    {
        public PopupChinhSuaThue(MainWindow main, string id, string name, string note, string name_ct, string ct, string ct_hs, string fs_id)
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
        MainWindow Main;
        private string id1, name1, note1, name_ct1, ct1, ct_hs1, fs_id1;

        private void LuuThayDoi(object sender, MouseButtonEventArgs e)
        {
            txtValuedate.Text = txtValuedateName.Text = "";
            string name = tbInput.Text;
            string note = tbInput1.Text;
            bool allow = true;
            if (string.IsNullOrEmpty(name))
            {
                txtValuedateName.Text = "Vui nhập tên Thuế";
                allow = false;
            }
            if (string.IsNullOrEmpty(ct1))
            {
                txtValuedate.Text = "Vui lòng thiết lập công thức";
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
                    web.QueryString.Add("name_recipe", name_ct1);
                    web.QueryString.Add("type_data", ct_hs1);
                    web.QueryString.Add("recipe", ct1);
                    web.QueryString.Add("name", name);
                    web.QueryString.Add("des", note);
                    web.QueryString.Add("id_tax", id1);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.Thue(Main));
                            this.Visibility = Visibility.Collapsed;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_tax.php", web.QueryString);
                }
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ThietLapCongThuc_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCongThuc(Main, id1, tbInput.Text, tbInput1.Text, name_ct1, ct1, ct_hs1, fs_id1, "0");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
