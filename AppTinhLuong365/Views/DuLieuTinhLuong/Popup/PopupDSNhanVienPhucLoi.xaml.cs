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
        public PopupDSNhanVienPhucLoi(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
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
                        web.QueryString.Add("id_welf", Main.CurrentCompany.com_id);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_DSNhanVienPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_DSNhanVienPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listDSNV = api.data.list;
                        }
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
            this.Visibility = Visibility.Collapsed;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
