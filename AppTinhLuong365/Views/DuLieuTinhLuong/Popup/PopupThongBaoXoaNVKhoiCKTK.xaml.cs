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
    /// Interaction logic for PopupThongBaoXoaNVKhoiCKTK.xaml
    /// </summary>
    public partial class PopupThongBaoXoaNVKhoiCKTK : Page
    {
        public PopupThongBaoXoaNVKhoiCKTK(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id1 = id;
        }

        MainWindow Main;
        private string id1;

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void TiepTuc(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("cls_id", id1);
                }
                web.UploadValuesCompleted += (s, e1) =>
                {
                    API_XoaPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_XoaPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e1.Result));
                    if (api.data != null)
                    {
                        int index = Main.pageCacKhoanTienKhac.listNVCacKhoanTienKhac.FindIndex(x => x.cls_id == id1);
                        if (index > -1)
                        {
                            Main.pageCacKhoanTienKhac.listNVCacKhoanTienKhac.RemoveAt(index);
                            Main.pageCacKhoanTienKhac.listNVCacKhoanTienKhac = Main.pageCacKhoanTienKhac.listNVCacKhoanTienKhac.ToList();
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_ep_otherMoney.php", web.QueryString);
                /*Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(Main));
                Main.sidebar.SelectedIndex = 7;*/
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
