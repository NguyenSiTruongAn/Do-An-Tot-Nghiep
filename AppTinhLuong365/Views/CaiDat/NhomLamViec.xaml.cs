﻿using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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
using AppTinhLuong365.Core;

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for NhomLamViec.xaml
    /// </summary>
    public partial class NhomLamViec : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        MainWindow Main;
        public NhomLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid.AutoReponsiveColumn(0);
            dataGrid1.AutoReponsiveColumn(1);
            getData();
            getData1();
            getData2();
        }
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
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id_group", "");
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
        private List<ListEpNoGroup> _listNVChuaNhom=new List<ListEpNoGroup>();
        public List<ListEpNoGroup> listNVChuaNhom
        {
            get { return _listNVChuaNhom; }
            set
            {
                _listNVChuaNhom = value;
                OnPropertyChanged();
            }
        }
        private List<ListEpNoGroup> _listNVChuaNhom1 = new List<ListEpNoGroup>();
        public List<ListEpNoGroup> listNVChuaNhom1
        {
            get { return _listNVChuaNhom1; }
            set
            {
                _listNVChuaNhom1 = value;
                OnPropertyChanged();
            }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_ep_nogroup api = JsonConvert.DeserializeObject<API_List_ep_nogroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNVChuaNhom1 = listNVChuaNhom = api.data.list;
                    }
                    foreach(ListEpNoGroup item in listNVChuaNhom)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_list_emp_nogroup.php", web.QueryString);
            }
        }

        private List<EpGroup> _listNVCacNhom = new List<EpGroup>();
        public List<EpGroup> listNVCacNhom
        {
            get { return _listNVCacNhom; }
            set
            {
                _listNVCacNhom = value;
                OnPropertyChanged();
            }
        }
        private List<EpGroup> _listNVCacNhom1 = new List<EpGroup>();
        public List<EpGroup> listNVCacNhom1
        {
            get { return _listNVCacNhom1; }
            set
            {
                _listNVCacNhom1 = value;
                OnPropertyChanged();
            }
        }

        private void getData2()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("company", Main.CurrentCompany.com_id);
                    web.QueryString.Add("page", "1");
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    List_user_group api = JsonConvert.DeserializeObject<List_user_group>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNVCacNhom1 = listNVCacNhom = api.data.ep_group;
                    }
                    foreach (EpGroup item in listNVCacNhom)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_group.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupNhomLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListGroup data = (ListGroup)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThemNhanVien(Main, data.lgr_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Path p = sender as Path;
            ListGroup data = (ListGroup)p.DataContext;
            var pop = new Views.CaiDat.Popup.PopupTuyChonNhomLamViec(Main, data.lgr_id, data.lgr_name, data.lgr_note);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void BtnThietLapNVChuaNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListEpNoGroup data = (ListEpNoGroup)b.DataContext;
            if (data != null)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThietLapNhomChoNhanVien(Main, data.ep_name,data.ep_id));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }
     
        }

        private void tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tb.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }

        private void NVChuaNhom_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListEpNoGroup selected = (ListEpNoGroup)(Search_NVChuaThietLap.SelectedItem);
            if (selected != null)
            {
                listNVChuaNhom1 = listNVChuaNhom.Where(x => x.ep_id == selected.ep_id).ToList();
            }
            else listNVChuaNhom1 = listNVChuaNhom;
        }
        private void Search_DSNVCacNhom(object sender, SelectionChangedEventArgs e)
        {

            EpGroup selected = (EpGroup)(Search_NVCacNhom.SelectedItem);
            if (selected != null)
            {
                listNVCacNhom1 = listNVCacNhom.Where(x => x.ep_id == selected.ep_id).ToList();
            }
            else listNVCacNhom1 = listNVCacNhom;
        }

        private void BtnThietLapNVCacNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            EpGroup data = (EpGroup)b.DataContext;
            if (data != null)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThietLapNhanVienCacNhom(Main, data.ep_name, data.ep_id, data.lgr_name));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }
            
        }
    }
}
