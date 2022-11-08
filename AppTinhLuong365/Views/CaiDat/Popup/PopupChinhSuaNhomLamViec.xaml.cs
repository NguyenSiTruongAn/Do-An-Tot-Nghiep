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
    /// Interaction logic for PopupChinhSuaNhomLamViec.xaml
    /// </summary>
    public partial class PopupChinhSuaNhomLamViec : Page
    {
        private string id;
        public PopupChinhSuaNhomLamViec(MainWindow main, string ID, string Name, string Note)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = ID;
            tbInput.Text = Name;
            tbInput1.Text = Note;

        }

        private void ChinhSuaNhom(object sender, MouseButtonEventArgs e)
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
                    }
                    web.QueryString.Add("id_group", id);
                    web.QueryString.Add("name", tbInput.Text);
                    web.QueryString.Add("des", tbInput1.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_SuaNhomLamViec api = JsonConvert.DeserializeObject<API_SuaNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_group_work.php", web.QueryString);
                }
                
            }
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
