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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupCalendar.xaml
    /// </summary>
    public partial class PopupCalendar : Page
    {
        public PopupCalendar(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        MainWindow Main;

        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            /*var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThangAD != null)
            {
                textThangAD.Text = x;
            }*/
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            /*if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag = 1;*/
        }
    }
}
