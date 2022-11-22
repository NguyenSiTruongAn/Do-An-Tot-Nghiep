using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.TinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThuong.xaml
    /// </summary>
    public partial class PopupThuong : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public PopupThuong(MainWindow main, List<CtThuong> thuong)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            data = thuong;
        }

        MainWindow Main;

        private List<CtThuong> _data;
        public List<CtThuong> data
        {
            get { return _data; }
            set
            {
                _data = value; OnPropertyChanged();
            }
        }
    }
}
