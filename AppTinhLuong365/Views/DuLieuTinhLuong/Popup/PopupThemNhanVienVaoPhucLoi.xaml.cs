using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for PopupThemNhanVienVaoPhucLoi.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoPhucLoi : Page
    {
        private DateTime day1, day_end1;
        private string id1;
        public PopupThemNhanVienVaoPhucLoi(MainWindow main, string id, string day, string day_end)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            DateTime.TryParse(day, out day1);
            DateTime.TryParse(day_end, out day_end1);
            id1 = id;
        }

        MainWindow Main;

        private void ThemNhanVienVaoPhucLoi(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {

        }

        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (dteSelectedMonth.DisplayDate == null) dteSelectedMonth1.DisplayDateStart = day1;
            else
                dteSelectedMonth1.DisplayDateStart = dteSelectedMonth.DisplayDate;
            dteSelectedMonth1.DisplayDateEnd = day_end1;
            flag = 1;
        }

        private void dteSelectedMonth1_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (textDenThang != null && !string.IsNullOrEmpty(x))
            {
                textDenThang.Text = x;
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag1 > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        int flag = 0;
        int flag1 = 0;
        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThangAD != null && !string.IsNullOrEmpty(x))
            {
                textThangAD.Text = x;
            }
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag += 1;
        }

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            dteSelectedMonth.DisplayDateStart = day1;
            dteSelectedMonth.DisplayDateEnd = day_end1;
            flag = 1;
        }
    }
}
