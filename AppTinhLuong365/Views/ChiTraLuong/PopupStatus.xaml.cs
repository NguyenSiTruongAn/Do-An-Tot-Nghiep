using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

namespace AppTinhLuong365.Views.ChiTraLuong
{
    /// <summary>
    /// Interaction logic for PopupStatus.xaml
    /// </summary>
    public partial class PopupStatus : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        public PopupStatus(MainWindow main, string id)
        {
            this.DataContext = this;
            InitializeComponent();
            this.id = id;
            Main = main;
        }

        private MainWindow Main;

        private void Close(object sender, MouseEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }


        private void Status(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("status", "3");
                web.QueryString.Add("pid", id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_Delete_cycle_of_employee api =
                        JsonConvert.DeserializeObject<API_Delete_cycle_of_employee>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/update_pay_status.php",
                    web.QueryString);
            }

            Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTietChiTraLuong(Main, id));
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }
        private void Status1(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("status", "2");
                web.QueryString.Add("pid", id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_Delete_cycle_of_employee api =
                        JsonConvert.DeserializeObject<API_Delete_cycle_of_employee>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/update_pay_status.php",
                    web.QueryString);
            }

            Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTietChiTraLuong(Main, id));
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void Status2(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("status", "1");
                web.QueryString.Add("pid", id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_Delete_cycle_of_employee api =
                        JsonConvert.DeserializeObject<API_Delete_cycle_of_employee>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/update_pay_status.php",
                    web.QueryString);
            }

            Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTietChiTraLuong(Main, id));
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }
    }
}
