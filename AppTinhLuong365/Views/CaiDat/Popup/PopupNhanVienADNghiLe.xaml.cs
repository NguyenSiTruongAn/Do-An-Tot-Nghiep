using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupNhanVienADNghiLe.xaml
    /// </summary>
    public partial class PopupNhanVienADNghiLe : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string id, name;
        public PopupNhanVienADNghiLe(MainWindow main, string id, string name)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.id = id;
            TextBlock.Text = name;
            getData();
        }

        private List<EpHolidayItem> _epHolidayList;

        public List<EpHolidayItem> epHolidayList
        {
            get { return _epHolidayList; }
            set { _epHolidayList = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("id_ho", id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_ep_holiday api = JsonConvert.DeserializeObject<API_List_ep_holiday>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            epHolidayList = api.data.ep_holiday_list;
                            DateTime date;
                            foreach (var a in _epHolidayList)
                            {
                                DateTime.TryParse(a.time_start, out date);
                                a.time_start = date.ToString("dd/MM/yyyy");
                                DateTime.TryParse(a.time_end, out date);
                                a.time_end = date.ToString("dd/MM/yyyy");
                            }
                        }
                        foreach (EpHolidayItem item in epHolidayList)
                        {
                            if (item.ep_image != "../img/add.png")
                            {
                                item.ep_image = item.ep_image;
                            }
                            else
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_ep_holiday.php", web.QueryString);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void BtnThemNhanVien_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupThemNVADNghiLe(Main, id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 495;
        }
        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 980)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 980 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
            if (this.ActualWidth < 925)
                DockPanel.SetDock(btnThemNhanVien, Dock.Bottom);
        }

        private void Xoa(object sender, MouseButtonEventArgs e)
        {
            Border t = sender as Border;
            EpHolidayItem data = (EpHolidayItem)t.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.CaiDat.Popup.PopupXoaNhanVienNghiLe(Main, data.ho_id_user, id));
        }
    }
}
