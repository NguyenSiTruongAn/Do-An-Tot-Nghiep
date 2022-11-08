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
    /// Interaction logic for PopupXoaNVKhoiThue.xaml
    /// </summary>
    public partial class PopupXoaNVKhoiThue : Page
    {
        public PopupXoaNVKhoiThue(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.id = id;
        }

        MainWindow Main;
        private string id;

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
                    web.QueryString.Add("id_list", id);
                }
                web.UploadValuesCompleted += (s, e1) =>
                {
                    try
                    {
                        API_XoaPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_XoaPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e1.Result));
                        if (api.data != null)
                        {
                            var pop = new Views.TinhLuong.Thue(Main);
                            Main.HomeSelectionPage.NavigationService.Navigate(pop);
                            pop.tb.SelectedIndex = 2;
                            this.Visibility = Visibility.Collapsed;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_user_in_tax.php", web.QueryString);
            }
        }
    }
}
