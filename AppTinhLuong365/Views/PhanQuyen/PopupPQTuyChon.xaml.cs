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

namespace AppTinhLuong365.Views.PhanQuyen
{
    /// <summary>
    /// Interaction logic for PopupPQTuyChon.xaml
    /// </summary>
    public partial class PopupPQTuyChon : Page
    {
        MainWindow Main;
        private string ep_image;
        private string ep_name;
        private string ep_id;
        private string role_id;
        public PopupPQTuyChon(MainWindow main, string dataEpImage, string dataEpName, string dataEpId, string dataRoleId)
        {
            InitializeComponent();
            this.DataContext = this;
            ep_image = dataEpImage;
            ep_name = dataEpName;
            ep_id = dataEpId;
            role_id = dataRoleId;
            Main = main;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Views.PhanQuyen.PopupPQTK pop = new Views.PhanQuyen.PopupPQTK(Main, ep_image, ep_name, ep_id, role_id);
            pop.Width = 495;
            pop.Height = 320;
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
