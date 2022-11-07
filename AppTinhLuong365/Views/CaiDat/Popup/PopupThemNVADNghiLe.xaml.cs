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
            getData();
        }


        private List<ListNoHoliday> _listEmployee;

        public List<ListNoHoliday> listEmployee
        {
            get { return _listEmployee; }
            set
            {
                _listEmployee = value;
                OnPropertyChanged();
            }
        }

        private List<ListNoHoliday> _listEmployee1;

        public List<ListNoHoliday> listEmployee1
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
                web.QueryString.Add("id_ho", id);
                var z = web.UploadValues(new Uri("https://tinhluong.timviec365.vn/api_app/company/list_ep_no_holiday.php"), web.QueryString);
                API_List_ep_no_holiday api = JsonConvert.DeserializeObject<API_List_ep_no_holiday>(UnicodeEncoding.UTF8.GetString(z));
                if (api.data != null)
                {
                    listEmployee1 = listEmployee = api.data.list_no_holiday;
                    foreach (ListNoHoliday item in listEmployee)
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
            string v = "";
        }
        
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private List<string> nv = new List<string>();

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListNoHoliday data = (ListNoHoliday)cb.DataContext;
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