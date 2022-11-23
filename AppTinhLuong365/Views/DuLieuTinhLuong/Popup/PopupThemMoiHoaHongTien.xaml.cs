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
    /// Interaction logic for PopupThemMoiHoaHongTien.xaml
    /// </summary>
    public partial class PopupThemMoiHoaHongTien : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        public PopupThemMoiHoaHongTien(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData1();
            getData3();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private List<ListEmployee> _listNV;

        public List<ListEmployee> listNV
        {
            get { return _listNV; }
            set
            {
                _listNV = value; OnPropertyChanged();
            }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data.data != null)
                        {
                            listNV = api.data.data.items;
                        }
                    }
                    catch { }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php", web.QueryString);
            }
        }

        private List<ListGroup> _listGR;

        public List<ListGroup> listGR
        {
            get { return _listGR; }
            set
            {
                _listGR = value; OnPropertyChanged();
            }
        }

        private void getData3()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listGR = api.data.list_group;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void ThemHoaHong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateTime.Text = validateMoney.Text = "";
            if (cbName.SelectedIndex < 0)
            {
                allow = false;
                validateName.Text = "Vui lòng chọn nhân viên";
            }
            if (time.SelectedDate == null)
            {
                allow = false;
                validateTime.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhập đày đủ";
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
                    ListEmployee nv_selected = new ListEmployee();
                    nv_selected = (ListEmployee)cbName.SelectedItem;
                    web.QueryString.Add("id_user", nv_selected.ep_id);
                    web.QueryString.Add("time", time.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("money", tbInput.Text);
                    web.QueryString.Add("content", tbInput1.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongTien(Main));
                                Main.sidebar.SelectedIndex = -1;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_rose_tien.php", web.QueryString);
                }
                
            }
        }

        ListGroup gr_selected = new ListGroup();

        private void ChonNhom(object sender, RoutedEventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            gr_selected = (ListGroup)rd.DataContext;
        }

        private void ThemHoaHong1(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validateTime.Text = validateMoney.Text = "";
            if (gr_selected.lgr_id == null)
            {
                allow = false;
                validateNameNhom.Text = "Vui lòng chọn nhóm";
            }
            if (TimeNhom.SelectedDate == null)
            {
                allow = false;
                validateTimeNhom.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (string.IsNullOrEmpty(tbInput2.Text))
            {
                allow = false;
                validateMoneyNhom.Text = "Vui lòng nhập đày đủ";
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
                    web.QueryString.Add("id_group", gr_selected.lgr_id);
                    web.QueryString.Add("time", TimeNhom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("money", tbInput2.Text);
                    web.QueryString.Add("content", tbInput3.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                var pop = new Views.DuLieuTinhLuong.HoaHongTien(Main);
                                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                                pop.tb1.SelectedIndex = 1;
                                Main.sidebar.SelectedIndex = -1;
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_gr_rose_tien.php", web.QueryString);
                }
                
            }
        }

        private void tbInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
