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
    /// Interaction logic for PopupDSNhanVienPhucLoi.xaml
    /// </summary>
    public partial class PopupDSNhanVienPhucLoi : Page, INotifyPropertyChanged
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

        private string id1, day1, day_end1;
        private string _name1;

        public string name1
        {
            get { return _name1; }
            set { _name1 = value; OnPropertyChanged(); }
        }
        public PopupDSNhanVienPhucLoi(MainWindow main, string id, string name, string day, string day_end)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id1 = id;
            name1 = name;
            day1 = day;
            day_end1 = day_end;
            getData();
        }

        private List<DSNhanVienPhucLoi_PhuCap> _listDSNV;

        public List<DSNhanVienPhucLoi_PhuCap> listDSNV
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
                        web.QueryString.Add("id_welf", id1);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        try
                        {
                            API_DSNhanVienPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_DSNhanVienPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listDSNV = api.data.list;
                                foreach (var item in listDSNV)
                                {
                                    if (item.ep_image != "")
                                        item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                                    else
                                        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                }
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/welfare_cate_stf.php", web.QueryString);
                }
            });
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void XoaNhanVienKhoiNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhanVienPhucLoi_PhuCap data = (DSNhanVienPhucLoi_PhuCap)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupXoaNhanVienKhoiPhucLoi(Main, id1, data.ep_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSua(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNhanVienPhucLoi_PhuCap data = (DSNhanVienPhucLoi_PhuCap)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaNhanVienPhucLoi(Main, data.cls_id, data.cls_day, data.cls_day_end, day1, day_end1));
            Main.Visibility = Visibility.Visible;
        }
    }
}
