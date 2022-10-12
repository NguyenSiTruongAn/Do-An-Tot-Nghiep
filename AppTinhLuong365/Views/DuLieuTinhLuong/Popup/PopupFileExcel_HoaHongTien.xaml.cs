using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using Microsoft.Win32;
using System.Net;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Net.Http;

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupFileExcel_HoaHongTien.xaml
    /// </summary>
    public partial class PopupFileExcel_HoaHongTien : Page
    {
        MainWindow Main;
        public PopupFileExcel_HoaHongTien(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.Visibility = Visibility.Collapsed;
        }

        public async void uploadFile()
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog() == true)
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent("1636"), "id_comp");
                content.Add(new StreamContent(new FileStream(op.FileName, FileMode.Open, FileAccess.Read)), "up_file");
                using (HttpClient client = new HttpClient())
                {
                    var response2 = await client.PostAsync("https://tinhluong.timviec365.vn/api_app/company/add_file_rose.php", content);
                    string z = response2.Content.ReadAsStringAsync().Result;
                    API_AddFile api = JsonConvert.DeserializeObject<API_AddFile>(z);
                }
            }
        }

        private void Add_file_Click(object sender, MouseButtonEventArgs e)
        {
            /*Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog() == true)
            {
                using (WebClient web = new WebClient())
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("up_file", op.FileName);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_AddFile api = JsonConvert.DeserializeObject<API_AddFile>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null && api.data.ToString() == "Thêm thành công")
                        {
                            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_file_rose.php", web.QueryString);
                }
            }*/

            uploadFile();
        }
    }
}
