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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupTuyChinhNgayNghiLe.xaml
    /// </summary>
    public partial class PopupTuyChinhNgayNghiLe : Page
    {
        MainWindow Main;
        private string id;
        private string name;
        private string number;
        private int status;
        private string start_time;
        private string end_time;

        public PopupTuyChinhNgayNghiLe(MainWindow main, string dataLhoId, string dataLhoName, string dataLhoNumber,
            int dataLhoStatus, string dataTimeStart, string dataTimeEnd)
        {
            InitializeComponent();
            this.DataContext = this;
            id = dataLhoId;
            name = dataLhoName;
            number = dataLhoNumber;
            status = dataLhoStatus;
            start_time = dataTimeStart;
            end_time = dataTimeEnd;
            Main = main;
        }

        private void ChinhSua_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupChinhSuaNgayNghiLe(Main, id, name, number, status, start_time, end_time));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnNhanVienApDung_ClickMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupNhanVienADNghiLe(Main);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.MaxWidth = 1054;
        }

        private void Xoa_Click(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupXoaLichNghi(Main, id);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
            pop.Width = 616;
            pop.Height = 252;
        }
    }
}