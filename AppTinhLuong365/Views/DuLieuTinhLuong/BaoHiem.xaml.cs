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

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for BaoHiem.xaml
    /// </summary>
    public partial class BaoHiem : Page
    {
        public MainWindow Main;
        public BaoHiem(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
    }
}
