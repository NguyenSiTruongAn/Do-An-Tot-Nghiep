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
    /// Interaction logic for PopupDSNhanVienADBaoHiem.xaml
    /// </summary>
    public partial class PopupDSNhanVienADBaoHiem : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupDSNhanVienADBaoHiem(MainWindow main, ListBaoHiem data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = data.cl_id;
            bh = data;
            title.Text = data.cl_name;
            getData();
        }

        private string id;
        private ListBaoHiem bh;

        private List<DSNVGoiBH> _listDSNV;

        public List<DSNVGoiBH> listDSNV
        {
            get { return _listDSNV; }
            set { _listDSNV = value; OnPropertyChanged(); }
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
                        web.QueryString.Add("id_tax", id);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_DSNVGoiBH api = JsonConvert.DeserializeObject<API_DSNVGoiBH>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listDSNV = api.data.ep_insrc;
                            for(int i=0; i<listDSNV.Count; i++)
                            {
                                if (listDSNV[i].ep_image != "")
                                    listDSNV[i].ep_image = "https://chamcong.24hpay.vn/upload/employee/" + listDSNV[i].ep_image;
                                else
                                    listDSNV[i].ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                if (string.IsNullOrEmpty(listDSNV[i].ep_name))
                                {
                                    listDSNV.Remove(listDSNV[i]);
                                    i--;
                                }    
                            }
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_insrc.php", web.QueryString);
                }
            });
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void BtnThemNhanVien_Click(object sender, MouseButtonEventArgs e)
        {

            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienVaoBaoHiem(Main,bh);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollview.ScrollToVerticalOffset(scrollview.VerticalOffset - e.Delta);
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
            if (this.ActualWidth < 925)
                DockPanel.SetDock(btnThemNhanVien, Dock.Bottom);
        }

        private void ChinhSuabaoHiemNhanVien_click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVGoiBH data = (DSNVGoiBH)b.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupChinhSuaBHNV(Main, data);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaNV(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVGoiBH data = (DSNVGoiBH)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupXoaNVKhoiBH(Main, data.cls_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
