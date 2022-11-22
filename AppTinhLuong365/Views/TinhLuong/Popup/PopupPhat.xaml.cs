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
    /// Interaction logic for PopupPhat.xaml
    /// </summary>
    public partial class PopupPhat : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;

        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PopupPhat(MainWindow main, List<CtPhat> phat)
        {
            this.DataContext = this;
            InitializeComponent();
            data = phat;
            Main = main;
        }

        MainWindow Main;

        private List<CtPhat> _data;

        public List<CtPhat> data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        private ListThuongPhat _dulieu;

        public ListThuongPhat dulieu
        {
            get { return _dulieu; }
            set
            {
                _dulieu = value;
                OnPropertyChanged();
            }
        }
    }
}
