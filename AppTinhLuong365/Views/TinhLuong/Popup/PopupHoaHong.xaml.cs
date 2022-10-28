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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.TinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupHoaHong.xaml
    /// </summary>
    public partial class PopupHoaHong : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string ep_id;
        string month;
        string year;
        public PopupHoaHong(MainWindow main, string epId, string month, string year)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            ep_id = epId;
            this.month = month;
            this.year = year;
            getData();
        }

        MainWindow Main;
        private List<Rose> _itemRose;

        public List<Rose> itemRose
        {
            get { return _itemRose; }
            set
            {
                _itemRose = value;
                OnPropertyChanged();
            }
        }

        private Rose _itemRose1;

        public Rose itemRose1
        {
            get { return _itemRose1; }
            set
            {
                _itemRose1 = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                // loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("time", year + "-" + month);
                web.QueryString.Add("id_user", ep_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_rose_nv api =
                        JsonConvert.DeserializeObject<API_List_rose_nv>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        itemRose = api.data.rose;
                        itemRose1 = itemRose[0];
                    }
                    // loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_rose_nv.php",
                    web.QueryString);
            }
        }
    }
}
