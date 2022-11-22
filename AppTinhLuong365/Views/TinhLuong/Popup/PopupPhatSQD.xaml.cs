using AppTinhLuong365.Model.APIEntity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for PopupPhatSQD.xaml
    /// </summary>
    public partial class PopupPhatSQD : Page
    {
        public PopupPhatSQD(MainWindow main, List<ListPhatNsqd> nsqd)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ListPhatSQD = nsqd;
        }
        MainWindow Main;

        private List<ListPhatNsqd> _ListPhatSQD;

        public List<ListPhatNsqd> ListPhatSQD
        {
            get { return _ListPhatSQD; }
            set { _ListPhatSQD = value; }
        }

        private void dataGrid2_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }
    }
}
