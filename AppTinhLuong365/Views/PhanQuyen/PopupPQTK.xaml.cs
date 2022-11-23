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

namespace AppTinhLuong365.Views.PhanQuyen
{
    /// <summary>
    /// Interaction logic for PopupPQTK.xaml
    /// </summary>
    public partial class PopupPQTK : Page
    {
        MainWindow Main;
        public string ep_image { get; set; }
        public string id;
        public string role_id;

        public PopupPQTK(MainWindow main, string epImage, string epName, string epId, string roleId)
        {
            InitializeComponent();
            this.DataContext = this;
            ep_image = epImage;
            Name.Text = epName;
            id = Id.Text = epId;
            role_id = roleId;
            if (roleId == "3")
            {
                Role.SelectedIndex = int.Parse((roleId)) - 2;
            }
            else if (roleId == "4")
            {
                Role.SelectedIndex = int.Parse((roleId)) - 4;
            }

            Role.SelectedIndex = int.Parse((roleId)) - 1;
            Main = main;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                if (Role.SelectedIndex == 0)
                {
                    role_id = "1";
                }
                else
                {
                    role_id = "3";
                }
                web.QueryString.Add("role_id", role_id.ToString());
                web.QueryString.Add("id_ep_update", id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        API_ThemNhanVienVaoNhom api =
                        JsonConvert.DeserializeObject<API_ThemNhanVienVaoNhom>(
                            UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.PhanQuyen.PhanQuyen(Main));
                            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                        }
                    }
                    catch { }
                    // foreach (ListEmployee item in listEmployee1)
                    // {
                    //     if (item.ep_image != "")
                    //     {
                    //         item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                    //     }
                    //     else
                    //     {
                    //         item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/update_permission.php",
                    web.QueryString);
            }
        }
    }
}