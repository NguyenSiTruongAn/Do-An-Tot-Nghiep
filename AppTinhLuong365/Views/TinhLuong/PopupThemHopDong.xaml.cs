using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupThemHopDong.xaml
    /// </summary>
    public partial class PopupThemHopDong : Page
    {
        public PopupThemHopDong(MainWindow main, string data)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            this.data = data;
        }

        MainWindow Main;
        string data;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void AddFile(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog()==true)
            {

            }
        }

        private void ThemHopDong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateLuong.Text = validateNgay.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateLuong.Text = "Vui lòng nhập đầy đủ";
            }
            else if(int.Parse(tbInput1.Text) > 100)
            {
                allow = false;
                validateLuong.Text = "Vui lòng không nhập quá 100";
            }
            if (dpNgayHieuLuc.SelectedDate == null)
            {
                allow = false;
                validateNgay.Text = "Vui lòng chọn thời gian áp dụng";
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
                    web.QueryString.Add("id", data);
                    web.QueryString.Add("name_ct", tbInput.Text);
                    web.QueryString.Add("salary", tbInput1.Text);
                    web.QueryString.Add("date_ct", dpNgayHieuLuc.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    if (dpNgayHetHan.SelectedDate != null)
                        web.QueryString.Add("date_ect_end", dpNgayHetHan.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    //web.QueryString.Add("file_ect",);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data));
                                Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_contract_work.php", web.QueryString);
                }
            }
        }

        private void tbInput1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
