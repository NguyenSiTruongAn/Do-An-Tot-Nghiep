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

namespace AppTinhLuong365.Views.NhanVien
{
    /// <summary>
    /// Interaction logic for ProFile.xaml
    /// </summary>
    public partial class ProFile : Page, INotifyPropertyChanged
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
        public ProFile(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
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
        }

        private void OpenDes(object sender, MouseButtonEventArgs e)
        {
            borderes.Visibility = borderes.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OpenDes1(object sender, MouseButtonEventArgs e)
        {
            borderes1.Visibility = borderes1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            borderes2.Visibility = borderes2.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            /*Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemLuong(Main, ));
            Main.PopupSelection.Visibility = Visibility.Visible;*/
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            /*Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemHopDong(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;*/
        }

        private void StackPanel_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            /*Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemPhuThuoc(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;*/
        }

        private void StackPanel_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            /*Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemDongGop(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;*/
        }
    }
}
