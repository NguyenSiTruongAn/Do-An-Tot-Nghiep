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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupThemPhuThuoc.xaml
    /// </summary>
    public partial class PopupThemPhuThuoc : Page
    {
        public PopupThemPhuThuoc(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
