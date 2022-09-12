using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for Thue.xaml
    /// </summary>
    public partial class Thue : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Thue(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid1.AutoReponsiveColumn(1);
            dataGrid2.AutoReponsiveColumn(0);
        }

        public List<string> Test { get; set; } = new List<string>() { "aa" ,"bb" ,"cc"  };

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 980)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 980 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
            if (this.ActualWidth > 1630)
            {
                DockPanel.SetDock(dockThueNSCTL, Dock.Right);
                DockPanel.SetDock(dockThueNSDTL, Dock.Right);

            }
            else
            {
                DockPanel.SetDock(dockThueNSCTL, Dock.Bottom);
                DockPanel.SetDock(dockThueNSDTL, Dock.Bottom);
            }
        }

        private void btnTaoMoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThue(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
            Main.PopupSelection.Width = 495;
            Main.PopupSelection.Height = 433;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset-e.Delta);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThietLapThue(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
            Main.PopupSelection.Width = 495;
            Main.PopupSelection.Height = 482;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupTGADThue(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
            Main.PopupSelection.Width = 495;
            Main.PopupSelection.Height = 327;
        }
    }
}
