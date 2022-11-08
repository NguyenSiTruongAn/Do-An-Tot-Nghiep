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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupChonCaLamViec.xaml
    /// </summary>
    public partial class PopupChonCaLamViec : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        MainWindow Main;
        string ten;
        string thang;
        int select;
        string date;
        public PopupChonCaLamViec(MainWindow main, string ten, string thang, int select, string date)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.ten = ten;
            this.thang = thang;
            this.select = select;
            this.date = date;
            getData();
        }

        private void Btn_QuayLai_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThemLichLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<DSCaLamViec> _listShift;

        public List<DSCaLamViec> listShift
        {
            get { return _listShift; }
            set
            {
                _listShift = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSCaLamViec api =
                        JsonConvert.DeserializeObject<API_DSCaLamViec>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listShift = api.data.items;
                        }
                    }
                    catch { }
                    // foreach (EpLate item in listEpLate)
                    // {
                    //     if (item.ts_image != "/img/add.png")
                    //     {
                    //         item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_shift.php", web.QueryString);
            }
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnTiepTuc_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupTaoChuKyLichLamViec(Main, ten, thang, select, date, ca));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<DSCaLamViec> ca = new List<DSCaLamViec>();

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSCaLamViec data = (DSCaLamViec)cb.DataContext;
            ca.Add(data);
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSCaLamViec data = (DSCaLamViec)cb.DataContext;
            ca.Remove(data);
        }
    }
}
