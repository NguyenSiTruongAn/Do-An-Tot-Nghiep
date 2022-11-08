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
    /// Interaction logic for PopupThongBaoXoaLichLamViec.xaml
    /// </summary>
    public partial class PopupThongBaoXoaLichLamViec : Page
    {
        MainWindow Main;
        public string ID;
        public PopupThongBaoXoaLichLamViec(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ID = id;
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.Headers.Add("Authorization", Main.CurrentCompany.token);
                }
                web.QueryString.Add("cy_id", ID);
                web.QueryString.Add("id_com", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        API_Delete_cycle api = JsonConvert.DeserializeObject<API_Delete_cycle>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(Main));
                            this.Visibility = Visibility.Collapsed;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/delete_cycle.php", web.QueryString);
            }
            
        }
    }
}
