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
    /// Interaction logic for PopupLuongDaTra.xaml
    /// </summary>
    public partial class PopupLuongDaTra : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;

        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string ep_id;
        private string month;
        private string year;
        private string start_date;
        private string end_date;

        public PopupLuongDaTra(MainWindow main, string ep_id, string month, string year, string start_date, string end_date)
        {
            this.DataContext = this;
            InitializeComponent();
            this.ep_id = ep_id;
            this.month = month;
            this.year = year;
            this.start_date = start_date;
            this.end_date = end_date;
            Main = main;
            getData();
        }

        MainWindow Main;

        private List<ChiTietLuongDaTra> _itemLuong;

        public List<ChiTietLuongDaTra> itemLuong
        {
            get { return _itemLuong; }
            set
            {
                _itemLuong = value;
                OnPropertyChanged();
            }
        }

        // private ListThuongPhat _dulieu;
        //
        // public ListThuongPhat dulieu
        // {
        //     get { return _dulieu; }
        //     set
        //     {
        //         _dulieu = value;
        //         OnPropertyChanged();
        //     }
        // }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                // loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.QueryString.Add("id_user", ep_id);
                string a = "";
                if (start_date != null)
                {
                    DateTime m;
                    if (DateTime.TryParse(start_date, out m)) a = m.ToString("yyyy-MM-dd");
                }
                web.QueryString.Add("start_date", a);
                string b = "";
                if (end_date != null)
                {
                    DateTime m;
                    if (DateTime.TryParse(end_date, out m)) b = m.ToString("yyyy-MM-dd");
                }
                web.QueryString.Add("end_date", b);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Luong_nv api =
                        JsonConvert.DeserializeObject<API_Luong_nv>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        itemLuong = api.data.luong_nv.chi_tiet_luong_da_tra;
                        // dulieu = data[0];
                    }
                    // loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_luong_nv.php",
                    web.QueryString);
            }
        }
    }
}
