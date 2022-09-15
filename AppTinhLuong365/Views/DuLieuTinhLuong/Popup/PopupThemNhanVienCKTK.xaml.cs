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
    /// Interaction logic for PopupThemNhanVienCKTK.xaml
    /// </summary>
    public partial class PopupThemNhanVienCKTK : Page
    {
        MainWindow Main;
        public PopupThemNhanVienCKTK(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc", "dd", "ee", "ff", "gg" };
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
