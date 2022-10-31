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
    /// Interaction logic for PopupChinhSuaCongThuc.xaml
    /// </summary>
    public partial class PopupChinhSuaCongThuc : Page
    {
        public PopupChinhSuaCongThuc(MainWindow main, string id, string name, string note, string name_ct, string ct, string ct_hs, string fs_id, string type)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbInput.Text = name_ct;
            tbInput1.Text = ct;
            name1 = name;
            note1 = note;
            id1 = id;
            fs_id1 = fs_id;
            if (ct_hs == "1")
                CbCongThuc.IsChecked = true;
            else
                CbHangSo.IsChecked = true;
            this.type = type;
        }
        MainWindow Main;
        private string name1, note1, fs_id1, id1, type;
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            tbInput1.Text += tb.Text + " ";
        }

        private void ThemCongThuc(object sender, MouseButtonEventArgs e)
        {
            string tencongthuc = tbInput.Text;
            string congthuc = tbInput1.Text;
            string ct_hs;
            if (CbCongThuc.IsChecked == true)
                ct_hs = "1";
            else ct_hs = "2";
            if (!string.IsNullOrEmpty(congthuc))
            {
                if(type == "1")
                {
                    Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCKTK(Main, id1, name1, note1, tencongthuc, congthuc, ct_hs, fs_id1));
                    Main.PopupSelection.Visibility = Visibility.Visible;
                }
                if(type == "2")
                {
                    Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupChinhSuaCSBH(Main, id1, name1, note1, tencongthuc, congthuc, ct_hs, fs_id1));
                    Main.PopupSelection.Visibility = Visibility.Visible;
                }
                if (type == "0")
                {
                    Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.Popup.PopupChinhSuaThue(Main, id1, name1, note1, tencongthuc, congthuc, ct_hs, fs_id1));
                    Main.PopupSelection.Visibility = Visibility.Visible;
                }
            }
            else valuedate.Text = "Vui lòng nhập công thức";
        }
    }
}
