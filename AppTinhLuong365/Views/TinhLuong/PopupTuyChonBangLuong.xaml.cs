using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupTuyChonBangLuong.xaml
    /// </summary>
    public partial class PopupTuyChonBangLuong : Page
    {

        private string name;
        private string dep_name;
        private string ep_id;
        private int month;
        private int year;

        public PopupTuyChonBangLuong(MainWindow main, string dataName, string dataDepName, string dataEpId, int selectedMonth, int selectedYear)
        {
            this.DataContext = this;
            InitializeComponent();
            name = dataName;
            dep_name = dataDepName;
            ep_id = dataEpId;
            month = selectedMonth;
            year = selectedYear;
            Main = main;
        }

        MainWindow Main;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.ChiTietLuong(Main, name, dep_name, ep_id, month, year));
            this.Visibility = Visibility.Collapsed;
            Main.title.Text = "Bảng lương nhân viên / Chi tiết lương nhân viên";
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            /*Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main));
            this.Visibility = Visibility.Collapsed;
            Main.title.Text = "Bảng lương nhân viên / Hồ sơ nhân viên";*/
        }
    }
}
