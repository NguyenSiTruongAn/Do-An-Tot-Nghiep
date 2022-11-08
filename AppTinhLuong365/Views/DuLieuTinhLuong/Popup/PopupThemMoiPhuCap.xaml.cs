using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemMoiPhuCap.xaml
    /// </summary>
    public partial class PopupThemMoiPhuCap : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PopupThemMoiPhuCap(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
        }

        MainWindow Main;

        private int _IsSmallSize;

        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 980)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 980 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ThemMoiPhuCap(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateMoney.Text = validateDate.Text = validateLoai.Text = "";
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhập đầy đủ";
            }
            if (textThangAD.SelectedDate == null)
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn tháng áp dụng";
            }
            if (string.IsNullOrEmpty(cb_Loai.Text))
            {
                allow = false;
                validateLoai.Text = "Vui lòng chọn loại thu nhập";
            }
            if (allow)
            {
                string x;
                if (textDenThang.SelectedDate == null)
                    x = "";
                else x = textDenThang.SelectedDate.Value.ToString("dd/MM/yyyy");
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("name", tbInput.Text);
                    web.QueryString.Add("salary", tbInput1.Text);
                    web.QueryString.Add("type", "4");
                    web.QueryString.Add("date", textThangAD.SelectedDate.Value.ToString("dd/MM/yyyy"));
                    web.QueryString.Add("note", tbInput2.Text);
                    web.QueryString.Add("date_end", x);
                    string i;
                    if (cb_Loai.Text == "Thu nhập chịu thuế")
                        i = "1";
                    else i = "0";
                    web.QueryString.Add("type_tax", i);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                                Main.sidebar.SelectedIndex = 5;
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/create_welfare.php", web.QueryString);
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
