using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PopupDSNVLichLamViec.xaml
    /// </summary>
    public partial class PopupDSNVLichLamViec : Page, INotifyPropertyChanged
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
        private string id;
        public PopupDSNVLichLamViec(MainWindow main, string Id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            id = Id;
            getData();
        }


        // public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

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

        private List<Item_emp> _listEmps;
        public List<Item_emp> listEmps
        {
            get { return _listEmps; }
            set { _listEmps = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.QueryString.Add("filter_by[cy_id]", id);
                // web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("month_apply", "2022-10-01");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_emp_in_cycle_by_com_id api = JsonConvert.DeserializeObject<API_List_emp_in_cycle_by_com_id>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listEmps = api.data.items;
                    }
                    foreach (Item_emp item in listEmps)
                    {
                        if (item.ep_image == "")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                        else
                        {
                            item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_employee_cycle.php", web.QueryString);
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            Item_emp data = (Item_emp)p.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThongBaoXoaNhanVien(Main, id, data.ep_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
