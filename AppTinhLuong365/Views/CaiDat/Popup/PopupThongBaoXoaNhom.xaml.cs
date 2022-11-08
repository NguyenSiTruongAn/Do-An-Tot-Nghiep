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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThongBaoXoaNhom.xaml
    /// </summary>
    public partial class PopupThongBaoXoaNhom : Page
    {
        private string ID;
        public PopupThongBaoXoaNhom(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ID = id;
        }
        MainWindow Main;

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void XoaNhom_Click(object sender, MouseButtonEventArgs e)
        {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                    }
                    web.QueryString.Add("id_group", ID);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_XoaNhomLamViec api = JsonConvert.DeserializeObject<API_XoaNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/remove_group_work.php", web.QueryString);
                }
                
        }
    }
}
