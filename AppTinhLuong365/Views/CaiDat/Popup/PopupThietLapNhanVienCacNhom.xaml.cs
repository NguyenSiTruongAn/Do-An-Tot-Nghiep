﻿using System;
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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThietLapNhanVienCacNhom.xaml
    /// </summary>
    public partial class PopupThietLapNhanVienCacNhom : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public PopupThietLapNhanVienCacNhom(MainWindow main, string ep_name, string ep_id, List<LgrName> lgr_name)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Test = lgr_name;
            textName.Text = ep_name;
            textId.Text = ep_id;
            id = ep_id;
            dataGrid1.AutoReponsiveColumn(0);
            getData();
        }
        MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<ListGroup> _listNhom;

        public List<ListGroup> listNhom
        {
            get { return _listNhom; }
            set { _listNhom = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0 && Test.Count>0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id_group", Test[0].gm_id_group);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNhom = api.data.list_group;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }

        public List<LgrName> Test { get; set; } = new List<LgrName>();
        private string id;

        private void XoaNhanVienKhoiNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            LgrName data = (LgrName)b.DataContext;
            bool allow = true;
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    web.QueryString.Add("id_user", id);
                    web.QueryString.Add("id_group", data.gm_id_group);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_TaoNhomLamViec api = JsonConvert.DeserializeObject<API_TaoNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/delete_user_group.php", web.QueryString);
                }
                var pop = new Views.CaiDat.NhomLamViec(Main);
                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                pop.tb.SelectedIndex = 2;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
