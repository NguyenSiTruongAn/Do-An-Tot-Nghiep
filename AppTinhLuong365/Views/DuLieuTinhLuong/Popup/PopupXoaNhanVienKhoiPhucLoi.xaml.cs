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
    /// Interaction logic for PopupXoaNhanVienKhoiPhucLoi.xaml
    /// </summary>
    public partial class PopupXoaNhanVienKhoiPhucLoi : Page
    {
        private string id_wf1, id_ep1;
        public PopupXoaNhanVienKhoiPhucLoi(MainWindow main, string id_wf, string id_ep)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id_ep1 = id_ep;
            id_wf1 = id_wf;
        }

        MainWindow Main;

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
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }
                web.QueryString.Add("id_ep", id_ep1);
                web.QueryString.Add("id_wf", id_wf1);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        API_XoaNhanVienKhoiPhucLoi api = JsonConvert.DeserializeObject<API_XoaNhanVienKhoiPhucLoi>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                            this.Visibility = Visibility.Collapsed;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_ep_wf.php", web.QueryString);
            }
            
        }
    }
}
