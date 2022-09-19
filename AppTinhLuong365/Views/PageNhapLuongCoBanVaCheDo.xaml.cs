using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace AppTinhLuong365.Views
{
    /// <summary>
    /// Interaction logic for PageNhapLuongCoBanVaCheDo.xaml
    /// </summary>
    public partial class PageNhapLuongCoBanVaCheDo : Page,INotifyPropertyChanged
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
        public PageNhapLuongCoBanVaCheDo(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
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
            if (this.ActualWidth > 1330)
                DockPanel.SetDock(wrapLCBVCD, Dock.Right);
            else
                DockPanel.SetDock(wrapLCBVCD, Dock.Bottom);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://phanmemnhansu.timviec365.vn/bien-dong-nhan-su.html");
        }
        public class abc : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public string name { get; set; }
            private int _hover;
            public int hover
            {
                get => _hover;
                set
                {
                    _hover = value;
                    OnPropertyChanged();
                }
            }

        }
        public List<abc> Test { get; set; } = new List<abc>() { new abc { name = "aa" }, new abc { name = "bb" }, new abc { name = "cc" } };

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.NhapLuongCoBan(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 500;
            pop.Height = 500;
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {

        }
        private void dataGrid1Hover(object sender, MouseEventArgs e)
        {
            Border col = sender as Border;
            if (col != null)
            {
                abc item = (abc)col.DataContext;
                int index = Test.FindIndex(x => x.name == item.name);
                if (index > -1)
                {
                    Test[index].hover = 1;
                }
            }
            else if ((sender as Grid) != null)
            {
                abc item = (abc)(sender as Grid).DataContext;
                int index = Test.FindIndex(x => x.name == item.name);
                if (index > -1)
                {
                    Test[index].hover = 1;
                }
            }
        }

        private void dataGrid1Leave(object sender, MouseEventArgs e)
        {
            Border col = sender as Border;
            if (col != null)
            {
                abc item = (abc)col.DataContext;
                int index = Test.FindIndex(x => x.name == item.name);
                if (index > -1)
                {
                    Test[index].hover = 0;
                }
            }
            else if ((sender as Grid) != null)
            {
                abc item = (abc)(sender as Grid).DataContext;
                int index = Test.FindIndex(x => x.name == item.name);
                if (index > -1)
                {
                    Test[index].hover = 0;
                }
            }
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
