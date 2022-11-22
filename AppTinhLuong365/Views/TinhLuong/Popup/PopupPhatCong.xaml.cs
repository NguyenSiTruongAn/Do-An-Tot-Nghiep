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

namespace AppTinhLuong365.Views.TinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupPhatCong.xaml
    /// </summary>
    public partial class PopupPhatCong : Page, INotifyPropertyChanged
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

        public PopupPhatCong(MainWindow main, List<CtMsPhatCong> phatcong)
        {
            this.DataContext = this;
            InitializeComponent();
            data = phatcong;
            Main = main;
        }
        MainWindow Main;

        private List<CtMsPhatCong> _data;

        public List<CtMsPhatCong> data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }
    }
}
