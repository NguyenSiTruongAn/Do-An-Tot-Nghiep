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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaThue.xaml
    /// </summary>
    public partial class PopupChinhSuaThue : Page
    {
        MainWindow Main;
        private string s1, name1, note1;
        public PopupChinhSuaThue(MainWindow main, string s, string name, string note)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            s1 = s;
            name1 = name;
            note1 = note;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBlock text = sender as TextBlock;
            tbInput1.Text += text.Text + " ";
        }

        private void ThemCongThuc(object sender, MouseButtonEventArgs e)
        {
            string name = tbInput.Text;
            string cbCT;
            if (CbCongThuc.IsChecked == true)
                cbCT = "1";
            else cbCT = "2";
            string congthuc = tbInput1.Text;
            if (!string.IsNullOrEmpty(congthuc))
            {
                if (s1 == "1")
                {
                    var pop = new Views.DuLieuTinhLuong.Popup.PopupThemKhoanTienKhac(Main, name1, note1, name, congthuc, cbCT);
                    Main.PopupSelection.NavigationService.Navigate(pop);
                    Main.PopupSelection.Visibility = Visibility.Visible;
                    pop.Width = 565;
                    pop.Height = 481;
                }
                if (s1 == "2")
                {
                    var pop = new Views.DuLieuTinhLuong.Popup.PopupThemMoiBH(Main, name1, note1, name, congthuc, cbCT);
                    Main.PopupSelection.NavigationService.Navigate(pop);
                    Main.PopupSelection.Visibility = Visibility.Visible;
                    pop.Width = 565;
                    pop.Height = 481;
                }
                if (s1 == "0")
                {
                    var pop = new Views.TinhLuong.PopupThue(Main, name1, note1, name, congthuc, cbCT);
                    Main.PopupSelection.NavigationService.Navigate(pop);
                    Main.PopupSelection.Visibility = Visibility.Visible;
                    pop.Width = 565;
                    pop.Height = 481;
                }
            }
            else valuedate.Text = "Vui lòng nhập công thức";
        }
    }
}
