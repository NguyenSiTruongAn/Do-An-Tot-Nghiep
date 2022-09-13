using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for NghiPhep.xaml
    /// </summary>
    public partial class NghiPhep : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NghiPhep(MainWindow main)
        {
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
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }
        public MainWindow Main;

        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }

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
            borderes.Visibility = borderes.Visibility == Visibility.Visible ? Visibility.Collapsed: Visibility.Visible;
        }

        private void OpenDes1(object sender, MouseButtonEventArgs e)
        {
            borderes1.Visibility = borderes1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
