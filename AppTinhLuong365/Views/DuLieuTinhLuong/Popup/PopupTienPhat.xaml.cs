using AppTinhLuong365.Model.APIEntity;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupTienPhat.xaml
    /// </summary>
    public partial class PopupTienPhat : Page, INotifyPropertyChanged
    {
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupTienPhat(MainWindow main, ListThuongPhat data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.data = data;
        }
        private ListThuongPhat _data;
        public ListThuongPhat data
        {
            get { return _data; }
            set
            {
                _data = value; OnPropertyChanged();
            }
        }

        private void btn_Close(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DtThuong data1 = (DtThuong)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThuongPhat(Main, data, data1.pay_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }

        private void XoaThuongPhat(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            DtThuong data = (DtThuong)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupXoaThuongPhat(Main, data.pay_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
