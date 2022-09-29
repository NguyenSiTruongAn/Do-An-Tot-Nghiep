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
    /// Interaction logic for PopupTuyChonDSPL.xaml
    /// </summary>
    public partial class PopupTuyChonDSPL : Page
    {
        private string id1, name1, salary1, note1, type_tax1, day1, day_end1;
        public PopupTuyChonDSPL(MainWindow main, string id, string name, string salary, string note, string type_tax, string day, string day_end)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            name1 = name;
            salary1 = salary;
            type_tax1 = type_tax;
            day1 = day;
            day_end1 = day_end;
            note1 = note;
            id1 = id;
        }

        MainWindow Main;

        private void Close_click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ThemNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemNhanVienVaoPhucLoi(Main, id1, day1, day_end1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChinhSuaThue_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaPhucLoi(Main, id1, name1, salary1, type_tax1, day1, day_end1, note1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void DSNhanVien_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaPhucLoi(Main, id1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
