using AppTinhLuong365.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for HoaHongKeHoach.xaml
    /// </summary>
    public partial class HoaHongKeHoach : Page, INotifyPropertyChanged
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

        public HoaHongKeHoach(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            dataGrid1.AutoReponsiveColumn(1);
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            for (var i = 2022; i <= 2025; i++)
            {
                YearList.Add($"Năm {i}");
            }

            Main = main;
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

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
                DockPanel.SetDock(dockHoaHongCaNhan, Dock.Right);
                DockPanel.SetDock(dockHoaHongNhom, Dock.Right);

            }
            else
            {
                DockPanel.SetDock(dockHoaHongCaNhan, Dock.Bottom);
                DockPanel.SetDock(dockHoaHongNhom, Dock.Bottom);
            }
        }

        private void btnThemMoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.DuLieuTinhLuong.Popup.PopupThemMoiHoaHongLePhiViTri(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            if (pop.tabitem1.IsSelected == true)
                pop.Height = 571;
            else
                pop.Height = 728;
        }

        private void btnQuayLai_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaHoaHongTien(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
