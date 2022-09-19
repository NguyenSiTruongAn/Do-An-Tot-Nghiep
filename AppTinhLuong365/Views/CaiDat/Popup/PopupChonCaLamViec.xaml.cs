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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupChonCaLamViec.xaml
    /// </summary>
    public partial class PopupChonCaLamViec : Page
    {
        MainWindow Main;
        public PopupChonCaLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void Btn_QuayLai_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThemLichLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc", "dd", "ee", "ff", "gg" };

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnTiepTuc_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupTaoChuKyLichLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
