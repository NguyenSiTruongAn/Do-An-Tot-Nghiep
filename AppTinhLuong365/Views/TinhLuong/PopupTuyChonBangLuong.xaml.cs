using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppTinhLuong365.Model.APIEntity;
using DatePicker = AppTinhLuong365.Resouce.tool.DatePicker;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupTuyChonBangLuong.xaml
    /// </summary>
    public partial class PopupTuyChonBangLuong : Page, INotifyPropertyChanged
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

        private string name;
        private string dep_name;
        private string ep_id;
        private int month;
        private int year;
        private string start_date;
        private string end_date;

        public PopupTuyChonBangLuong(MainWindow main, string dataName, string dataDepName, string dataEpId, int selectedMonth, int selectedYear, DateTime? start_date, DateTime? end_date)
        {
            this.DataContext = this;
            InitializeComponent();
            name = dataName;
            dep_name = dataDepName;
            ep_id = dataEpId;
            month = selectedMonth;
            year = selectedYear;
            this.start_date = start_date+"";
            this.end_date = end_date+"";
            Main = main;
        }

        MainWindow Main;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.ChiTietLuong(Main, name, dep_name, ep_id, month, year, start_date, end_date));
            this.Visibility = Visibility.Collapsed;
            Main.title.Text = "Bảng lương nhân viên / Chi tiết lương nhân viên";
            Main.sidebar.SelectedIndex = -1;
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, ep_id));
            this.Visibility = Visibility.Collapsed;
            Main.title.Text = "Danh sách nhân viên / Hồ sơ nhân viên";
            Main.sidebar.SelectedIndex = -1;
        }
    }
}
