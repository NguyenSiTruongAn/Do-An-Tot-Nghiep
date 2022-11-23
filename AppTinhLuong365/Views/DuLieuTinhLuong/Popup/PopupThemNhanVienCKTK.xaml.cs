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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemNhanVienCKTK.xaml
    /// </summary>
    public partial class PopupThemNhanVienCKTK : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        private string id1;
        public PopupThemNhanVienCKTK(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            id1 = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSNVThemVaoCKTK> _listNV;

        public List<DSNVThemVaoCKTK> listNV
        {
            get { return _listNV; }
            set
            {
                _listNV = value; OnPropertyChanged();
            }
        }

        private List<DSNVThemVaoCKTK> _listNV1;

        public List<DSNVThemVaoCKTK> listNV1
        {
            get { return _listNV1; }
            set { _listNV1 = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSNVThemVaoCKTK api = JsonConvert.DeserializeObject<API_DSNVThemVaoCKTK>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV1 = listNV = api.data.list;
                        }
                        foreach (DSNVThemVaoCKTK item in listNV)
                        {
                            if (item.ep_image == "")
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                            else item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_add_otherMoney.php", web.QueryString);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x => x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSNVThemVaoCKTK data = (DSNVThemVaoCKTK)cb.DataContext;
            data.status = true;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void btnTiepTuc_Click(object sender, MouseButtonEventArgs e)
        {
            List<DSNVThemVaoCKTK> dsnv = new List<DSNVThemVaoCKTK>();
            foreach(var item in listNV)
            {
                if (item.status)
                    dsnv.Add(item);
            }    
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThoiGianADCacKhoanTienKhac(Main, id1, dsnv));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void HuyCHon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSNVThemVaoCKTK data = (DSNVThemVaoCKTK)cb.DataContext;
            data.status = false;
        }
    }
}
