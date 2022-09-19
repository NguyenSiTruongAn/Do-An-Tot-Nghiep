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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupDSNVLichLamViec.xaml
    /// </summary>
    public partial class PopupDSNVLichLamViec : Page, INotifyPropertyChanged
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
        public PopupDSNVLichLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }


        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

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
    }
}
