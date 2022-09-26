using AppTinhLuong365.Core;
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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemNhanVienVaoLichLamViec.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoLichLamViec : Page, INotifyPropertyChanged
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
        private string ID_gr;
        public PopupThemNhanVienVaoLichLamViec(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ID_gr = id;
            getData();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Add_fileNhanVien_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupAddFileNhanVien(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 500;
            pop.Height = 500;
        }

        private List<Item_employee_not_in_cycle> _listNV;

        public List<Item_employee_not_in_cycle> listNV
        {
            get { return _listNV; }
            set { _listNV = value; OnPropertyChanged(); }
        }

        private List<Item_employee_not_in_cycle> _listNV1;

        public List<Item_employee_not_in_cycle> listNV1
        {
            get { return _listNV1; }
            set { _listNV1 = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("group", ID_gr);
                if (Main.MainType == 0)
                {
                    //web.QueryString.Add("length", "20");
                    web.Headers.Add("Authorization", Main.CurrentCompany.token);
                    //web.QueryString.Add("month_apply", "2022-09-01");
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_employee_not_in_cycle api = JsonConvert.DeserializeObject<API_List_employee_not_in_cycle>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNV = listNV1 = api.data.items;
                    }
                    foreach (Item_employee_not_in_cycle item in listNV)
                    {
                        if (item.ep_image == "")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        } else
                        {
                            item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/get_list_employee_not_in_cycle.php?length=20&month_apply=2022-09-01", web.QueryString);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x => x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }
        private List<string> nv = new List<string>();
        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_employee_not_in_cycle data = (Item_employee_not_in_cycle)cb.DataContext;
            nv.Add(data.ep_id);
        }

        private void ThemNhanVienVaoNhom(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (nv.Count <= 0)
            {
                allow = false;
            }
            if (allow)
            {
                foreach (var item in nv)
                {
                    using (WebClient web = new WebClient())
                    {
                        if (Main.MainType == 0)
                        {
                            web.QueryString.Add("token", Main.CurrentCompany.token);
                            web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        }
                        web.QueryString.Add("id_emp", item);
                        web.QueryString.Add("id_group", ID_gr);
                        web.UploadValuesCompleted += (s, ee) =>
                        {
                            API_TaoNhomLamViec api = JsonConvert.DeserializeObject<API_TaoNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_emp_group_work.php", web.QueryString);
                    }
                }
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
