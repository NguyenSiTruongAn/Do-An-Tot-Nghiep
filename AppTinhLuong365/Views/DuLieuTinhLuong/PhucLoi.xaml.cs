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

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for PhucLoi.xaml
    /// </summary>
    public partial class PhucLoi : Page, INotifyPropertyChanged
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
        public PhucLoi(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        public List<string> Test { get; set; } = new List<string>(){"aaaaa","bb","cc" };
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

            if (this.ActualWidth>615)
            {
                DockPanel.SetDock(borderPhucLoi,Dock.Right);
            }
            else
            {
                DockPanel.SetDock(borderPhucLoi, Dock.Bottom);
            }
            if (this.ActualWidth > 1650)
            {
                DockPanel.SetDock(borderPhuCap, Dock.Right);
            }
            else
            {
                DockPanel.SetDock(borderPhuCap, Dock.Bottom);
            }
            if (this.ActualWidth > 1000)
            {
                DockPanel.SetDock(borderPhuCapTheoCa, Dock.Right);
            }
            else
            {
                DockPanel.SetDock(borderPhuCapTheoCa, Dock.Bottom);
            }
        }

        private void borderPhucLoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupPhucLoi(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void borderPhuCap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemMoiPhuCap(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void borderPhuCapTheoCa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemMoiPhuCapTheoCa(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
