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

namespace AppTinhLuong365.Views.ChiTraLuong
{
    /// <summary>
    /// Interaction logic for PopupChiTraLuong.xaml
    /// </summary>
    public partial class PopupChiTraLuong : Page
    {
        public PopupChiTraLuong(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
        }

        MainWindow Main;
        private string name1; string note1;
        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ThietLapCongThuc_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.PopupChinhSuaThue(Main, "2", name1, note1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 565;
            pop.Height = 481;
        }
    }
}
