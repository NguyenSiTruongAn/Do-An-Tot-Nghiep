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
    /// Interaction logic for PopupCTPhatDiMuon.xaml
    /// </summary>
    public partial class PopupCTPhatDiMuon : Page, INotifyPropertyChanged
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

        public PopupCTPhatDiMuon(MainWindow main, List<CtMsPhatTien> phattien)
        {
            this.DataContext = this;
            InitializeComponent();
            data = phattien;
            Main = main;
        }
        MainWindow Main;

        private List<CtMsPhatTien> _data;

        public List<CtMsPhatTien> data
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
