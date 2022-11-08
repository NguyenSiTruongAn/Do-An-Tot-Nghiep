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

        private List<Item_all_employee_of_company> _listNV;

        public List<Item_all_employee_of_company> listNV
        {
            get { return _listNV; }
            set
            {
                _listNV = value;
                OnPropertyChanged();
            }
        }

        private List<Item_all_employee_of_company> _listNV1;

        public List<Item_all_employee_of_company> listNV1
        {
            get { return _listNV1; }
            set
            {
                _listNV1 = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("filter_by[active]", "true");
                if (Main.MainType == 0)
                {
                    web.Headers.Add("Authorization", Main.CurrentCompany.token);
                    web.QueryString.Add("filter_by[company]", Main.CurrentCompany.com_id);
                    web.QueryString.Add("filter_by[not_in_cycle]", ID_gr);
                }

                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_all_employee_of_company api =
                        JsonConvert.DeserializeObject<API_List_all_employee_of_company>(
                            UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV = listNV1 = api.data.items;
                        }

                        foreach (Item_all_employee_of_company item in listNV)
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
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync(
                    "https://chamcong.24hpay.vn/service/list_all_employee_of_company.php",
                    web.QueryString);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x =>
                x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private List<string> nv = new List<string>();

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_all_employee_of_company data = (Item_all_employee_of_company)cb.DataContext;
            nv.Add(data.ep_id);
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_all_employee_of_company data = (Item_all_employee_of_company)cb.DataContext;
            nv.Remove(data.ep_id);
        }

        private string Arr_Id_Ep;
        private void ThemNhanVienVaoNhom(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (nv.Count <= 0)
            {
                allow = false;
                validatePhat.Text = "Vui lòng chọn nhân viên";
            }

            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.Headers.Add("Authorization", Main.CurrentCompany.token);

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
                    web.QueryString.Add("arr_ep_id", Arr_Id_Ep);
                    web.QueryString.Add("cy_id", ID_gr);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_Add_employee_to_cycle api =
                            JsonConvert.DeserializeObject<API_Add_employee_to_cycle>(
                                UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(Main));
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/add_employee_to_cycle.php",
                        web.QueryString);
                }

                
            }
        }
    }
}