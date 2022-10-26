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
    /// Interaction logic for PopupChinhSuaCKTK.xaml
    /// </summary>
    public partial class PopupChinhSuaCKTK : Page
    {
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
                txtValuedateName.Text = "Vui nhập tên khoản tiền khác";
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
                    web.QueryString.Add("fs_name", name_ct1);
                    web.QueryString.Add("fs_data", ct_hs1);
                    web.QueryString.Add("fs_repica", ct1);
                    web.QueryString.Add("cl_name", name);
                    web.QueryString.Add("cl_note", note);
                    web.QueryString.Add("fs_id", id1);
                    web.QueryString.Add("fs_ct", fs_id1);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemCKTK api = JsonConvert.DeserializeObject<API_ThemCKTK>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            int index=Main.pageCacKhoanTienKhac.listCacKhoanTienKhac.FindIndex(x=>x.cl_id== id1);
                            if (index>-1)
                            {
                                Main.pageCacKhoanTienKhac.listCacKhoanTienKhac[index].cl_name = name;
                                Main.pageCacKhoanTienKhac.listCacKhoanTienKhac[index].cl_note = note;
                                Main.pageCacKhoanTienKhac.listCacKhoanTienKhac[index].fs_repica = ct1;
                            }
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_otherMoney.php", web.QueryString);
                }
                //Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }

        public PopupChinhSuaCKTK(MainWindow main, string id, string name, string note, string name_ct, string ct, string ct_hs, string fs_id)
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
            this.Visibility = Visibility.Collapsed;
        }

        private void ThietLapCongThuc_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCongThuc(Main, id1, tbInput.Text, tbInput1.Text, name_ct1, ct1, ct_hs1, fs_id1,"1");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
