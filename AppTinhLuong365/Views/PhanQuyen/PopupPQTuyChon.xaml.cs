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

namespace AppTinhLuong365.Views.PhanQuyen
{
    /// <summary>
    /// Interaction logic for PopupPQTuyChon.xaml
    /// </summary>
    public partial class PopupPQTuyChon : Page
    {
        MainWindow Main;
        public PopupPQTuyChon(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Views.PhanQuyen.PopupPQTK pop = new Views.PhanQuyen.PopupPQTK(Main);
            pop.Width = 495;
            pop.Height = 320;
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
