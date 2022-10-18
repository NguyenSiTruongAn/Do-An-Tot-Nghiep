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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaLichLamViec.xaml
    /// </summary>
    public partial class PopupChinhSuaLichLamViec : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        public PopupChinhSuaLichLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            tbTitle.Text = $"Tháng {DateTime.Now.Month}/{DateTime.Now.Year}";
            int month =10;
            int year = 2022;
            var start = (int)new DateTime(year, month, 1).DayOfWeek;
            listLich = new List<lichlamviec>();
            if (month - 1 > 0)
            {
                for (int i = 0; i < start; i++)
                {
                    var x = DateTime.DaysInMonth(year, month - 1);
                    listLich.Add(new lichlamviec() { id = listLich.Count, ngay = x - i, ca = 0, status = 0 });
                }
                listLich.Reverse();
            }
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                var d = new lichlamviec() { id = listLich.Count, ngay = i, ca = 1, status = 1 };
                listLich.Add(d);
            }
            int n = 42 - listLich.Count;
            for (int i =1; i <= n; i++)
            {
                var d = new lichlamviec() { id = listLich.Count, ngay = i, ca = 0, status = 0 };
                listLich.Add(d);
            }
            listLich = listLich.ToList();
        }
    
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public class lichlamviec : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int id;
            public int ngay { get; set; }
            public int ca { get; set; }
            public int _status;
            public int status
            {
                get { return _status; }
                set { _status = value; OnPropertyChanged(); }
            }
        }

        private List<lichlamviec> _listLich;

        public List<lichlamviec> listLich
        {
            get { return _listLich; }
            set { _listLich = value; OnPropertyChanged(); }
        }

        public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc", "dd", "ee", "ff", "gg" };
        private List<TextBlock> listTextBlock = new List<TextBlock>();
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        
        }

        private void PopupChinhSuaLichLamViec_OnLoaded(object sender, RoutedEventArgs e)
        {
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

        private void selectNgay(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            lichlamviec data = (lichlamviec)b.DataContext;
            if(data.status == 1)
            {
                foreach (var item in listLich)
                {
                    if (item.status == 2)
                        item.status = 1;
                    if(item.id == data.id)
                        item.status = 2;
                }
            }
        }
    }
}
