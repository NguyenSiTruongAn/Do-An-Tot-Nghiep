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
    /// Interaction logic for PopupPhat.xaml
    /// </summary>
    public partial class PopupPhat : Page, INotifyPropertyChanged
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

        public PopupPhat(MainWindow main, string ep_id, string month, string year)
        {
            this.DataContext = this;
            InitializeComponent();
            this.ep_id = ep_id;
            this.month = month;
            this.year = year;
            Main = main;
            getData();
        }

        MainWindow Main;

        private List<ListThuongPhat> _data;

        public List<ListThuongPhat> data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        private ListThuongPhat _dulieu;

        public ListThuongPhat dulieu
        {
            get { return _dulieu; }
            set
            {
                _dulieu = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                // loading.Visibility = Visibility.Visible;
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }
                if (Main.MainType == 1)
                {
                    web.QueryString.Add("token", Main.CurrentEmployee.token);
                    web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                }
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.QueryString.Add("id_emp", ep_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListThuongPhat api =
                        JsonConvert.DeserializeObject<API_ListThuongPhat>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            data = api.data.thuong_phat;
                            if (data.Count > 0)
                                dulieu = data[0];
                        }
                    }
                    catch { }
                    // loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_payoff_manager.php",
                    web.QueryString);
            }
        }
    }
}
