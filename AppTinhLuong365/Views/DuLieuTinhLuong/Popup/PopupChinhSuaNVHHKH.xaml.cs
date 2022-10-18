using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaNVHHKH.xaml
    /// </summary>
    public partial class PopupChinhSuaNVHHKH : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupChinhSuaNVHHKH(MainWindow main, DSNVHHKeHoach data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.data = data;
            getData();
            txtName.Text = data.ep_name;
            cbKpi.SelectedIndex = int.Parse(data.ro_kpi_active);
            tbInput1.Text = data.ro_note;
        }
        MainWindow Main;
        DSNVHHKeHoach data = new DSNVHHKeHoach();

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
                    API_DSCaiDatHoaHongKeHoach api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongKeHoach>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listKeHoach = api.data.list;
                        foreach(var item in listKeHoach)
                        {
                            if(item.tl_id == data.tl_id)
                                cbKeHoach.SelectedItem = item;
                        }
                        
                    }
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
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }
                web.QueryString.Add("id_rose", data.ro_id);
                DSCaiDatHoaHongKeHoach Kh = new DSCaiDatHoaHongKeHoach();
                Kh = (DSCaiDatHoaHongKeHoach)cbKeHoach.SelectedItem;
                web.QueryString.Add("id_kh", Kh.tl_id);
                web.QueryString.Add("kpi", cbKpi.SelectedIndex + "");
                web.QueryString.Add("ghichu_u", tbInput1.Text);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongKeHoach(Main));
                        Main.HomeSelectionPage.Visibility = Visibility.Visible;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_rose_kehoach.php", web.QueryString);
            }
            this.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
