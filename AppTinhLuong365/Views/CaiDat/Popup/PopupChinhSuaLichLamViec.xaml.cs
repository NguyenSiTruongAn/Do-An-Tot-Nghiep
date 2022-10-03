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
    /// Interaction logic for PopupChinhSuaLichLamViec.xaml
    /// </summary>
    public partial class PopupChinhSuaLichLamViec : Page
    {
        MainWindow Main;
        public PopupChinhSuaLichLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbTitle.Text = $"Tháng {DateTime.Now.Month}/{DateTime.Now.Year}";
        }
        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc", "dd", "ee", "ff", "gg" };
        private List<TextBlock> listTextBlock = new List<TextBlock>();
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        
        }

        private void PopupChinhSuaLichLamViec_OnLoaded(object sender, RoutedEventArgs e)
        {
            listTextBlock = new List<TextBlock>() { tbDay1 };
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {

                }
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid g=sender as Grid;
            if (g != null)
            {
                Border bordeIndex = g.Children[0] as Border;
                if (bordeIndex != null)
                {
                    TextBlock tb= bordeIndex.Child as TextBlock;
                    MessageBox.Show(tb.Text);
                }
            }
        }
    }
}
