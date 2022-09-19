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
    /// Interaction logic for PopupThemNhanVienVaoLichLamViec.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoLichLamViec : Page
    {
        MainWindow Main;
        public PopupThemNhanVienVaoLichLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Add_fileNhanVien_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupAddFileNhanVien(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 500;
            pop.Height = 500;
        }
    }
}
