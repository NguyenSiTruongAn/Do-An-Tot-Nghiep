using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace AppTinhLuong365.Views.TrangChu
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page, INotifyPropertyChanged
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
        public Home(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
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
        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJDaGFtY29uZzM2NS1UaW12aWVjMzY1IiwiaWF0IjoxNjYzNzI1MDYzLCJleHAiOjE2NjM4MTE0NjMsImRhdGEiOnsiaWQiOiIyNjkwIiwibmFtZSI6InBoXHUxZWExbSBtXHUxZWExbmggaFx1MDBmOW5nIiwidHlwZSI6MSwiZW1haWwiOiJ4bHEwMjk5NUBjdW9seS5jb20iLCJwaG9uZV90ayI6bnVsbCwicm9sZSI6IjEiLCJvcyI6MSwiZnJvbSI6ImNjMzY1IiwiY29tX2lkIjoiMTYzNiJ9fQ._PzHqDc8cYCKuq3budRV6DTNN5bwbMWKutM2dN5jEjo");
                web.QueryString.Add("id", "2690");
                web.QueryString.Add("id_com", "1636");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_UserInfo api = JsonConvert.DeserializeObject<API_UserInfo>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                    }

                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/employe/profile_emp.php", web.QueryString);
            }
        }
        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Views.PhanQuyen.PhanQuyen(Main));
            Main.SideBarIndex = 22;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://vanthu.timviec365.vn/mau-de-xuat.html");
        }

        private void DockPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Views.TinhLuong.BangLuong(Main));
            Main.SideBarIndex = 10;
        }

        private void DockPanel_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
