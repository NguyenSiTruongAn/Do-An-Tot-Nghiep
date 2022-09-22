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
using AppTinhLuong365.Core;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupThietLapNhanVienCacNhom.xaml
    /// </summary>
    public partial class PopupThietLapNhanVienCacNhom : Page
    {
        public PopupThietLapNhanVienCacNhom(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid1.AutoReponsiveColumn(0);
        }
        MainWindow Main;

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
