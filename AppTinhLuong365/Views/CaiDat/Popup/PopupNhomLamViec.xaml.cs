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
    /// Interaction logic for PopupNhomLamViec.xaml
    /// </summary>
    public partial class PopupNhomLamViec : Page
    {
        public PopupNhomLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void NewGroup(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateDes.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateDes.Text = "Vui lòng nhập đầy đủ";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("name", tbInput.Text);
                    web.QueryString.Add("des", tbInput1.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_TaoNhomLamViec api = JsonConvert.DeserializeObject<API_TaoNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/create_group_work.php", web.QueryString);
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
