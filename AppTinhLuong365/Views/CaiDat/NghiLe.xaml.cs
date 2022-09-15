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

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for NghiLe.xaml
    /// </summary>
    public partial class NghiLe : Page
    {
        public MainWindow Main;
        public NghiLe(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupNghiLe(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TuyChonNghiLe_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupTuyChinhNgayNghiLe(Main);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
