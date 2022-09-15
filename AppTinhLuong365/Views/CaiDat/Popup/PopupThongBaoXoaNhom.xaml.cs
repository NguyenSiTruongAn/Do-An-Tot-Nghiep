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
    /// Interaction logic for PopupThongBaoXoaNhom.xaml
    /// </summary>
    public partial class PopupThongBaoXoaNhom : Page
    {
        public PopupThongBaoXoaNhom(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        MainWindow Main;

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
