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
    /// Interaction logic for PopupThemNhanVienVaoBaoHiem.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoBaoHiem : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        string id1;
        ListBaoHiem bh;
        public PopupThemNhanVienVaoBaoHiem(MainWindow main, ListBaoHiem bh)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            getData1();
            id1 = bh.cl_id;
            this.bh = bh;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private List<ListEmployee> _listNV1;

        public List<ListEmployee> listNV1
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
                        API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV1 = listNV = api.data.data.items;
                        }
                        foreach (ListEmployee item in listNV)
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php", web.QueryString);
            }
        }

        private List<ListGroup> _listGR;

        public List<ListGroup> listGR
        {
            get { return _listGR; }
            set
            {
                _listGR = value; OnPropertyChanged();
            }
        }

        private List<ListGroup> _listGR1;

        public List<ListGroup> listGR1
        {
            get { return _listGR1; }
            set
            {
                _listGR1 = value; OnPropertyChanged();
            }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listGR = listGR1 = api.data.list_group;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x => x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
            listGR1 = listGR.Where(x => x.lgr_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListEmployee data = (ListEmployee)cb.DataContext;
            data.status = true;
        }

        private void HuyChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListEmployee data = (ListEmployee)cb.DataContext;
            data.status = false;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void btn_TiepTuc_Click(object sender, MouseButtonEventArgs e)
        {
            List<ListEmployee> dsnv = new List<ListEmployee>();
            foreach (var item in listNV)
            {
                if (item.status)
                    dsnv.Add(item);
            }
            if (dsnv.Count > 0)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThoiGianADBaoHiem(Main, bh, dsnv));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }
            else
                validateNV.Text = "Vui lòng chọn nhân viên";
        }

        private void btn_TiepTucNhom_Click(object sender, MouseButtonEventArgs e)
        {
            if (gr_selected.Count > 0)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThoiGianADBHNhom(Main, bh, gr_selected));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }
            else
                validateNV.Text = "Vui lòng chọn nhân viên";
        }

        List<ListGroup> gr_selected = new List<ListGroup>();

        private void ChonNhom(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListGroup data = (ListGroup)cb.DataContext;
            gr_selected.Add(data);
        }

        private void HuyChonNhom(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListGroup data = (ListGroup)cb.DataContext;
            gr_selected.Remove(data);
        }
    }
}
