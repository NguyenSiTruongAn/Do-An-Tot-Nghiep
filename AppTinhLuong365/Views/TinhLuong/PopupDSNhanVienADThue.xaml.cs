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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupDSNhanVienADThue.xaml
    /// </summary>
    public partial class PopupDSNhanVienADThue : Page, INotifyPropertyChanged
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
        public PopupDSNhanVienADThue(MainWindow main, string id, string name)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.id = id;
            Name.Text = name;
            getData();
        }

        private string id;

        private List<DSNVADThue> _listDSNV;

        public List<DSNVADThue> listDSNV
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
                        try
                        {
                            API_DSNVADThue api = JsonConvert.DeserializeObject<API_DSNVADThue>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listDSNV = api.data.list;
                                for (int i = 0; i < listDSNV.Count; i++)
                                {
                                    if (listDSNV[i].ep_image == "../img/add.png")
                                        listDSNV[i].ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                }
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_in_tax.php", web.QueryString);
                }
            });
        }

        private void BtnThemNhanVien_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupThemNhanVienVaoThue(Main,"");
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
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

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void XoaNV(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSNVADThue data = (DSNVADThue)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.Popup.PopupXoaThueNV(Main, data.cls_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
