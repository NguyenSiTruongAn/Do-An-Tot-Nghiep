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
    /// Interaction logic for PopupThemNhanVien.xaml
    /// </summary>
    public partial class PopupThemNhanVien : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        MainWindow Main;
        private string ID_gr;
        public PopupThemNhanVien(MainWindow main, string id)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            ID_gr = id;
            getData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSThemMoiNhanVienVaoNhom> _listNV;

        public List<DSThemMoiNhanVienVaoNhom> listNV
        {
            get { return _listNV; }
            set { _listNV = value; OnPropertyChanged(); }
        }

        private List<DSThemMoiNhanVienVaoNhom> _listNV1;

        public List<DSThemMoiNhanVienVaoNhom> listNV1
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
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSThemMoiNhanVienVaoNhom api = JsonConvert.DeserializeObject<API_DSThemMoiNhanVienVaoNhom>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV = listNV1 = api.data.list;
                        }
                        foreach (DSThemMoiNhanVienVaoNhom item in listNV)
                        {
                            if (item.ep_image == "/img/add.png")
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_add_user_gr.php", web.QueryString);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x=>x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }
        private List<string> nv = new List<string>();
        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSThemMoiNhanVienVaoNhom data = (DSThemMoiNhanVienVaoNhom)cb.DataContext;
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
                            try
                            {
                                API_ThemNhanVienVaoNhom api = JsonConvert.DeserializeObject<API_ThemNhanVienVaoNhom>(UnicodeEncoding.UTF8.GetString(ee.Result));
                                if (api.data != null)
                                {
                                    Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                                    this.Visibility = Visibility.Collapsed;
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_emp_group_work.php", web.QueryString);
                    }
                }
                
            }
            
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSThemMoiNhanVienVaoNhom data = (DSThemMoiNhanVienVaoNhom)cb.DataContext;
            nv.Remove(data.ep_id);
        }
    }
}
