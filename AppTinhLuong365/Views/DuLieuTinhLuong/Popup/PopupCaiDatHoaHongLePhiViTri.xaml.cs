﻿using AppTinhLuong365.Model.APIEntity;
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
    /// Interaction logic for PopupCaiDatHoaHongLePhiViTri.xaml
    /// </summary>
    public partial class PopupCaiDatHoaHongLePhiViTri : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        public PopupCaiDatHoaHongLePhiViTri(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSCaiDatHoaHongVT> _listDSCaiDatHHVT;

        public List<DSCaiDatHoaHongVT> listDSCaiDatHHVT
        {
            get { return _listDSCaiDatHHVT; }
            set { _listDSCaiDatHHVT = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            this.Dispatcher.Invoke(() =>
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("type", "4");
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        try
                        {
                            API_DSCaiDatHoaHongVT api = JsonConvert.DeserializeObject<API_DSCaiDatHoaHongVT>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listDSCaiDatHHVT = api.data.list;
                                for (int i = 1; i <= listDSCaiDatHHVT.Count; i++)
                                    listDSCaiDatHHVT[i - 1].STT = i + "";
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/setting_rose.php", web.QueryString);
                }
            });
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSCaiDatHoaHongVT data = (DSCaiDatHoaHongVT)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaHoaHongLePhiViTri(Main, data));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnXoaHoaHongTien_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DSCaiDatHoaHongVT data = (DSCaiDatHoaHongVT)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaHoaHongLoiNhuan(Main, data.tl_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnThem_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemHoaHongLePhiViTri(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }
    }
}
