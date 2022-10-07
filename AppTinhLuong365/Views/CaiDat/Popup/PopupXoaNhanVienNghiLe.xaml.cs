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
    /// Interaction logic for PopupXoaNhanVienNghiLe.xaml
    /// </summary>
    public partial class PopupXoaNhanVienNghiLe : Page
    {
        public string id;
        public string id_ep;
        public PopupXoaNhanVienNghiLe(MainWindow main, string dataHoIdUser, string id)
        {
            this.DataContext = this;
            InitializeComponent();
            this.id = id;
            id_ep = dataHoIdUser;
            Main = main;
        }

        private MainWindow Main;

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {

                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("id_ho", id);
                web.QueryString.Add("id_u", id_ep);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_Delete_cycle_of_employee api =
                        JsonConvert.DeserializeObject<API_Delete_cycle_of_employee>(
                            UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_ep_holiday.php",
                    web.QueryString);
            }

            var pop = new Views.CaiDat.NghiLe(Main);
            Main.HomeSelectionPage.NavigationService.Navigate(pop);
            // pop.Control.SelectedIndex = 1;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
