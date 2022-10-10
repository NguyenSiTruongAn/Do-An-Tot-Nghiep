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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThongBaoXoaNhanVien.xaml
    /// </summary>
    public partial class PopupThongBaoXoaNhanVien : Page
    {
        public string ID;
        public MainWindow Main;
        public string Ep_ID;
        public string month;

        public PopupThongBaoXoaNhanVien(MainWindow main, string id, string ep_id, string month)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ID = id;
            Ep_ID = ep_id;
            this.month = month;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.Headers.Add("Authorization", Main.CurrentCompany.token);
                }

                web.QueryString.Add("id_com", ID);
                web.QueryString.Add("id_ep", Ep_ID);
                web.QueryString.Add("month", month);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    var a = UnicodeEncoding.UTF8.GetString(ee.Result);
                    API_Delete_cycle_of_employee api =
                        JsonConvert.DeserializeObject<API_Delete_cycle_of_employee>(a);
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/delete_cycle_of_employee.php",
                    web.QueryString);
            }

            Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.Popup.PopupDSNVLichLamViec(Main, ID));
            this.Visibility = Visibility.Collapsed;
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
