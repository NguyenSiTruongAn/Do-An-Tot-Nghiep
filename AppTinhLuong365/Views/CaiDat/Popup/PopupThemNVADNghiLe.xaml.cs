using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemNVADNghiLe.xaml
    /// </summary>
    public partial class PopupThemNVADNghiLe : Page, INotifyPropertyChanged
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
        MainWindow Main;

        private string id;
        public PopupThemNVADNghiLe(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.id = id;
            getData().ContinueWith((t) =>
            {
                getData1().ContinueWith((t1) =>
                {
                    listEmployee.RemoveAll(x => epHolidayList.Any(item => item.ho_id_user == x.ep_id));
                    this.Dispatcher.Invoke(() =>
                    {
                        dataGrid1.ItemsSource = listEmployee1;
                    });
                });
            });
        }


        private List<ListEmployee> _listEmployee = new List<ListEmployee>();

        public List<ListEmployee> listEmployee
        {
            get { return _listEmployee; }
            set
            {
                _listEmployee = value;
                OnPropertyChanged();
            }
        }

        private List<ListEmployee> _listEmployee1;

        public List<ListEmployee> listEmployee1
        {
            get { return _listEmployee1; }
            set
            {
                _listEmployee1 = value;
                OnPropertyChanged();
            }
        }

        private async Task getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                var z = web.UploadValues(new Uri("https://tinhluong.timviec365.vn/api_app/company/list_emp.php"), web.QueryString);
                API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(z));
                if (api.data != null)
                {
                    listEmployee1 = listEmployee = api.data.data.items;
                    foreach (ListEmployee item in listEmployee)
                    {
                        if (item.ep_image != "")
                        {
                            item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                        }
                        else
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listEmployee1 = listEmployee.Where(x =>
                x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private List<EpHolidayItem> _epHolidayList = new List<EpHolidayItem>();

        public List<EpHolidayItem> epHolidayList
        {
            get { return _epHolidayList; }
            set { _epHolidayList = value; OnPropertyChanged(); }
        }

        private async Task getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("id_ho", id);
                var z = web.UploadValues(new Uri("https://tinhluong.timviec365.vn/api_app/company/list_ep_holiday.php"), "POST", web.QueryString);
                API_List_ep_holiday api = JsonConvert.DeserializeObject<API_List_ep_holiday>(UnicodeEncoding.UTF8.GetString(z));
                if (api.data != null)
                {

                    epHolidayList = api.data.ep_holiday_list;
                    DateTime date;
                    foreach (var a in epHolidayList)
                    {
                        DateTime.TryParse(a.time_start, out date);
                        a.time_start = date.ToString("dd/MM/yyyy");
                        DateTime.TryParse(a.time_end, out date);
                        a.time_end = date.ToString("dd/MM/yyyy");
                    }
                    foreach (EpHolidayItem item in epHolidayList)
                    {
                        if (item.ep_image != "../img/add.png")
                        {
                            item.ep_image = item.ep_image;
                        }
                        else
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                }
            }
        }

        // public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc", "dd", "ee", "ff", "gg" };
        // public List<string> Test1 { get; set; } = new List<string>() { "aa", "bb" };

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private List<string> nv = new List<string>();

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListEmployee data = (ListEmployee)cb.DataContext;
            nv.Add(data.ep_id);
        }

        private string Arr_Id_Ep;

        private void Add(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (nv.Count <= 0)
            {
                allow = false;
            }

            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_com", Main.CurrentCompany.com_id);
                    }

                    int dem = 0;
                    foreach (var item in nv)
                    {
                        if (dem == nv.Count - 1)
                        {
                            Arr_Id_Ep += item.ToString();
                        }
                        else
                        {
                            Arr_Id_Ep += item.ToString() + ",";
                        }

                        dem++;
                    }
                    web.QueryString.Add("id_emp", Arr_Id_Ep);
                    web.QueryString.Add("id_ho", id);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        string a = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_Add_employee_to_cycle api =
                            JsonConvert.DeserializeObject<API_Add_employee_to_cycle>(a);
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_holiday.php",
                        web.QueryString);
                }

                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiLe(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}