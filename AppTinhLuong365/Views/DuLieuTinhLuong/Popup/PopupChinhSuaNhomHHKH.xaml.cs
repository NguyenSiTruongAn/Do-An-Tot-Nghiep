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
    /// Interaction logic for PopupChinhSuaNhomHHKH.xaml
    /// </summary>
    public partial class PopupChinhSuaNhomHHKH : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChinhSuaNhomHHKH(MainWindow main, DSNhomHHKeHoach data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            data1 = data;
            listNVNhom = data.ep_gr;
            getData();
            cbKpi.SelectedIndex = int.Parse(data.ro_kpi_active);
        }
        MainWindow Main;
        DSNhomHHKeHoach data1;
        private List<EpGrKH> _listNVNhom;

        public List<EpGrKH> listNVNhom
        {
            get { return _listNVNhom; }
            set
            {
                _listNVNhom = value; OnPropertyChanged();
            }
        }

        private List<DSCaiDatHoaHongKeHoach> _listKeHoach;

        public List<DSCaiDatHoaHongKeHoach> listKeHoach
        {
            get { return _listKeHoach; }
            set
            {
                _listKeHoach = value; OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("type", "5");
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSCaiDatHoaHongKeHoach api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongKeHoach>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listKeHoach = api.data.list;
                            foreach (var item in listKeHoach)
                            {
                                if (item.tl_id == data1.tl_id)
                                    cbKeHoach.SelectedItem = item;
                            }
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/setting_rose.php", web.QueryString);
            }
        }

        private void LuuHoaHong(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateKeHoach.Text = validateNhom.Text = "";
            if (cbKeHoach.SelectedIndex < 0)
            {
                allow = false;
                validateKeHoach.Text = "Vui lòng chọn sản phẩm";
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
                    web.QueryString.Add("id_rose", data1.ro_id);
                    DSCaiDatHoaHongKeHoach sp = new DSCaiDatHoaHongKeHoach();
                    sp = (DSCaiDatHoaHongKeHoach)cbKeHoach.SelectedItem;
                    web.QueryString.Add("id_kh", sp.tl_id);
                    web.QueryString.Add("id_gr", data1.ro_id_group);
                    web.QueryString.Add("kpi", cbKpi.SelectedIndex + "");
                    for (int i = 0; i < listNVNhom.Count; i++)
                    {
                        web.QueryString.Add("id_user[" + i + "]", listNVNhom[i].pr_id_user);
                        web.QueryString.Add("percent[" + i + "]", listNVNhom[i].pr_percent);
                    }
                    web.QueryString.Add("chuky", data1.ro_time);
                    web.QueryString.Add("ghichu_u", data1.ro_time);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            var pop = new Views.DuLieuTinhLuong.HoaHongKeHoach(Main);
                            Main.HomeSelectionPage.NavigationService.Navigate(pop);
                            Main.sidebar.SelectedIndex = -1;
                            pop.tb.SelectedIndex = 1;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_group_rose_kehoach.php", web.QueryString);
                }
                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
            }
        }

        private void Changed_HoaHong(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            EpGrKH data = (EpGrKH)tb.DataContext;
            foreach (var item in listNVNhom)
            {
                if (data.pr_id_user == item.pr_id_user)
                {
                    item.pr_percent = tb.Text;
                }
            }
        }

        private void Txt_PreviewCurrencyTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void TabControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }
    }
}
