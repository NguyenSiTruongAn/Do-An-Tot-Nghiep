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
    /// Interaction logic for PopupXoaNghiPhep.xaml
    /// </summary>
    public partial class PopupXoaNghiPhep : Page
    {
        private string Id;

        public PopupXoaNghiPhep(MainWindow main, string id)
        {
            this.DataContext = this;
            InitializeComponent();
            Id = id;
            Main = main;
        }

        private MainWindow Main;

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                
                web.QueryString.Add("id", Id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        API_Delete_cycle_of_employee api =
                        JsonConvert.DeserializeObject<API_Delete_cycle_of_employee>(
                            UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            var pop = new Views.CaiDat.NghiPhep(Main);
                            Main.HomeSelectionPage.NavigationService.Navigate(pop);
                            pop.Control.SelectedIndex = 1;
                            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_np.php",
                    web.QueryString);
            }
        }
    }
}