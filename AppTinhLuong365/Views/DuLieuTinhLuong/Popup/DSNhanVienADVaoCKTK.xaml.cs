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
    /// Interaction logic for DSNhanVienADVaoCKTK.xaml
    /// </summary>
    public partial class DSNhanVienADVaoCKTK : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        private string id1 = "";
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DSNhanVienADVaoCKTK(MainWindow main, string id, string name)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id1 = id;
            name1 = name;
            getData();
        }

        private string _name1;

        public string name1
        {
            get { return _name1; }
            set { _name1 = value; OnPropertyChanged(); }
        }

        private List<DSNVADCKTK> _listNVCacKhoanTienKhac;

        public List<DSNVADCKTK> listNVCacKhoanTienKhac
        {
            get { return _listNVCacKhoanTienKhac; }
            set { _listNVCacKhoanTienKhac = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("id_om", id1);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_DSNVADCKTK api = JsonConvert.DeserializeObject<API_DSNVADCKTK>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNVCacKhoanTienKhac = api.data.list;
                            foreach (var item in listNVCacKhoanTienKhac)
                            {
                                if (item.ep_image == "../img/add.png")
                                    item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_in_otherMoney.php", web.QueryString);
                }
            }
            catch
            {

            }
        }

        private void BtnThemNhanVien_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienCKTK(Main, id1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
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

        private void XoaNhanVien(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVADCKTK data = (DSNVADCKTK)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaNVKhoiCKTK(Main, data.cls_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
