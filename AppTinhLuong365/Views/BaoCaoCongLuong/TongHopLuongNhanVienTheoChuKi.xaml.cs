using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
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

namespace AppTinhLuong365.Views.BaoCaoCongLuong
{
    /// <summary>
    /// Interaction logic for TongHopLuongNhanVienTheoChuKi.xaml
    /// </summary>
    public partial class TongHopLuongNhanVienTheoChuKi : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TongHopLuongNhanVienTheoChuKi(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            for (var i = 2022; i <= 2025; i++)
            {
                YearList.Add($"Năm {i}");
            }
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
        }
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public class abc : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public string name { get; set; }
            private int _hover;
            public int hover
            {
                get => _hover;
                set
                {
                    _hover = value;
                    OnPropertyChanged();
                }
            }

        }

        public List<abc> Test { get; set; } = new List<abc>() { new abc { name = "aa" }, new abc { name = "bb" }, new abc { name = "cc" } };

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 980)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 980 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }
        private void dataGrid1Hover(object sender, MouseEventArgs e)
        {
            //Border col = sender as Border;
            //if (col != null)
            //{
            //    abc item = (abc)col.DataContext;
            //    int index = Test.FindIndex(x => x.name == item.name);
            //    if (index > -1)
            //    {
            //        Test[index].hover = 1;
            //    }
            //}
            //else if ((sender as Grid) != null)
            //{
            //    abc item = (abc)(sender as Grid).DataContext;
            //    int index = Test.FindIndex(x => x.name == item.name);
            //    if (index > -1)
            //    {
            //        Test[index].hover = 1;
            //    }
            //}
        }

        private void dataGrid1Leave(object sender, MouseEventArgs e)
        {
            //Border col = sender as Border;
            //if (col != null)
            //{
            //    abc item = (abc)col.DataContext;
            //    int index = Test.FindIndex(x => x.name == item.name);
            //    if (index > -1)
            //    {
            //        Test[index].hover = 0;
            //    }
            //}
            //else if ((sender as Grid) != null)
            //{
            //    abc item = (abc)(sender as Grid).DataContext;
            //    int index = Test.FindIndex(x => x.name == item.name);
            //    if (index > -1)
            //    {
            //        Test[index].hover = 0;
            //    }
            //}
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // var pop = new Views.TinhLuong.PopupTuyChonBangLuong(Main);
            // var z = Mouse.GetPosition(Main.PopupSelection);
            // pop.Margin = new Thickness(z.X - 95, z.Y + 15, 0, 0);
            // Main.PopupSelection.NavigationService.Navigate(pop);
            // Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private BangLuong _bangLuong;

        public BangLuong bangLuong
        {
            get { return _bangLuong; }
            set { _bangLuong = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("month", "9");
                web.QueryString.Add("year", "2022");
                web.QueryString.Add("page", "1");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Payroll api = JsonConvert.DeserializeObject<API_Payroll>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        bangLuong = api.data.bang_luong;
                    }
                    loading.Visibility = Visibility.Visible;
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_bang_luong_nv.php", web.QueryString);
            }
        }
    }
}
