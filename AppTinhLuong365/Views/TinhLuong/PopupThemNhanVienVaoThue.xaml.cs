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
using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupThemNhanVienVaoThue.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoThue : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        private string id1;
        public PopupThemNhanVienVaoThue(MainWindow main, string id)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid1.AutoReponsiveColumn(0); getData();
            id1 = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<ListEmployee> _listNV1;

        public List<ListEmployee> listNV1
        {
            get { return _listNV1; }
            set { _listNV1 = value; OnPropertyChanged(); }
        }

        private List<ListEmployee> _listNV;

        public List<ListEmployee> listNV
        {
            get { return _listNV; }
            set
            {
                _listNV = value; OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data.data != null)
                        {
                            listNV = listNV1 = api.data.data.items;
                            foreach (ListEmployee item in listNV)
                            {
                                if (item.ep_image == "")
                                {
                                    item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                }
                                else
                                    item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php", web.QueryString);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x => x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListEmployee data = (ListEmployee)cb.DataContext;
            data.status = true;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnTiepTuc_Click(object sender, MouseButtonEventArgs e)
        {
            List<ListEmployee> dsnv = new List<ListEmployee>();
            foreach (var item in listNV)
            {
                if (item.status)
                    dsnv.Add(item);
            }
            if (dsnv.Count == 0)
                validateNV.Text = "Vui lòng chọn nhân viên";
            else
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.Popup.PopupThoiGianADTNV(Main, id1, dsnv));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }
        }

        private void HuyCHon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListEmployee data = (ListEmployee)cb.DataContext;
            data.status = false;
        }
    }
}
