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
    /// Interaction logic for PopupCaiDatHoaHongLoiNhuan.xaml
    /// </summary>
    public partial class PopupCaiDatHoaHongLoiNhuan : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        public PopupCaiDatHoaHongLoiNhuan(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSCaiDatHoaHongLoiNhuan> _listDSCaiDatHHLN;

        public List<DSCaiDatHoaHongLoiNhuan> listDSCaiDatHHLN
        {
            get { return _listDSCaiDatHHLN; }
            set { _listDSCaiDatHHLN = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            this.Dispatcher.Invoke(() =>
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("type", "3");
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        try
                        {
                            API_DSCaiDatHoaHongLoiNhuan api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongLoiNhuan>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listDSCaiDatHHLN = api.data.list;
                                for (int i = 1; i <= listDSCaiDatHHLN.Count; i++)
                                    listDSCaiDatHHLN[i - 1].STT = i + "";
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/setting_rose.php", web.QueryString);
                }
            });
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSCaiDatHoaHongLoiNhuan data = (DSCaiDatHoaHongLoiNhuan)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaHoaHongLoiNhuan(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnXoaHoaHongTien_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSCaiDatHoaHongLoiNhuan data = (DSCaiDatHoaHongLoiNhuan)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaHoaHongLoiNhuan(Main, data.tl_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnThem_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemHoaHongLoiNhuan(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }
    }
}
