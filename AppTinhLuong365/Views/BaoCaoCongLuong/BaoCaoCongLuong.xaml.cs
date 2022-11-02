using System;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AppTinhLuong365.Views.BaoCaoCongLuong
{
    /// <summary>
    /// Interaction logic for BaoCaoCongLuong.xaml
    /// </summary>
    public partial class BaoCaoCongLuong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string month;
        string year;
        public BaoCaoCongLuong(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }

            YearList = new ObservableCollection<string>();
            var c = DateTime.Now.Year;
            YearList.Add($"Năm {c - 1}");
            YearList.Add($"Năm {c}");
            YearList.Add($"Năm {c + 1}"); 
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            this.month = month;
            this.year = year;
            getData(month, year);
            searchBarYear.PlaceHolder = "Năm " + year;
            searchBarMonth.PlaceHolder = "Tháng " + month;
            BieuDo.Visibility = BieuDo1.Visibility = stBDCot.Visibility = Visibility.Hidden;
        }

        private void VeBieuDo(Grid gridbd, double d)
        {
            BrushConverter bc = new BrushConverter();
            if (d == 0)
            {
                Line line2 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                gridbd.Children.Add(line2);
            }
            else
            {
                if (d <= Math.PI / 2)
                {
                    double x = 125 + 125 * (Math.Sin(d));
                    double y = 125 - Math.Sqrt(125 * 125 - (x - 125) * (x - 125));
                    double z = y / ((x - 125) / 0.3);
                    for (double i = 125, j = 0; i < x; i += 0.3, j += z)
                    {
                        if (i > x - 0.7)
                        {
                            Line line = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                            gridbd.Children.Add(line);
                        }
                        else
                        {
                            Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                            gridbd.Children.Add(line1);
                        }
                    }
                    Line line2 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    gridbd.Children.Add(line2);
                }
                else
                if (d <= Math.PI)
                {
                    double x = 125 + 125 * (Math.Sin(Math.PI / 2));
                    double y = 125 - Math.Sqrt(125 * 125 - (x - 125) * (x - 125));
                    double z = y / ((x - 125) / 0.3);
                    for (double i = 125, j = 0; i < x; i += 0.3, j += z)
                    {
                        Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                        gridbd.Children.Add(line1);
                    }
                    x = 125 + 125 * (Math.Cos(d - Math.PI / 2));
                    y = 125 + Math.Sqrt(125 * 125 - (x - 125) * (x - 125));
                    z = (y-125) / ((250 - x) / 0.2);
                    for (double i = 250, j = 125; i > x; i -= 0.2, j += z)
                    {
                        if (i <= x + 1)
                        {
                            Line line = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                            gridbd.Children.Add(line);
                        }
                        else
                        {
                            Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                            gridbd.Children.Add(line1);
                        }
                    }
                    Line line2 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1.5, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    gridbd.Children.Add(line2);
                }
                else
                if (d <= 3 * Math.PI / 2)
                {
                    double x = 125 + 125 * (Math.Sin(Math.PI / 2));
                    double y = 125 - Math.Sqrt(125 * 125 - (x - 125) * (x - 125));
                    double z = y / ((x - 125) / 0.3);
                    for (double i = 125, j = 0; i < x; i += 0.3, j += z)
                    {
                        Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                        gridbd.Children.Add(line1);
                    }
                    x = 125 + 125 * (Math.Cos(Math.PI / 2));
                    y = 125 + Math.Sqrt(125 * 125 - (x - 125) * (x - 125));
                    z = y / ((250 - x) / 0.2);
                    for (double i = 250, j = 125; i > x; i -= 0.2, j += z)
                    {
                        Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                        gridbd.Children.Add(line1);
                    }
                    Line line2 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1.5, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    gridbd.Children.Add(line2);
                    x = 125 - 125 * (Math.Sin(d - Math.PI));
                    y = 125 + Math.Sqrt(125 * 125 - (125 - x) * (125 - x));
                    z = (250 - y) / ((125 - x) / 0.2);
                    for (double i = 125, j = 250; i > x; i -= 0.2, j -= z)
                    {
                        if (i <= x + 1)
                        {
                            Line line = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                            gridbd.Children.Add(line);
                        }
                        else
                        {
                            Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                            gridbd.Children.Add(line1);
                        }
                        if (j < 125)
                        {
                            MessageBox.Show(j + "");
                            break;
                        }
                    }
                    Line line3 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1.5, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    gridbd.Children.Add(line3);
                }
            }
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public MainWindow Main;

        private int _IsSmallSize;

        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
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

        private ItemCongLuong _congLuong;

        public ItemCongLuong congLuong
        {
            get { return _congLuong; }
            set { _congLuong = value; OnPropertyChanged(); }
        }

        private void getData(string month, string year)
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
                web.QueryString.Add("month", month);
                web.QueryString.Add("year", year);
                web.UploadValuesCompleted += (s, e) =>
                {
                    Api_CongLuong api = JsonConvert.DeserializeObject<Api_CongLuong>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        BieuDo.Visibility = BieuDo1.Visibility = stBDCot.Visibility = Visibility.Visible;
                        congLuong = api.data.list;
                        BrushConverter bc = new BrushConverter();
                        grid.Children.Clear();
                        grid1.Children.Clear();
                        //double d = congLuong.pt_bh * (Math.PI/50);
                        VeBieuDo(grid, congLuong.pt_bh * (Math.PI / 50));
                        double d1 = congLuong.pt_thue * (Math.PI/50);
                        VeBieuDo(grid1, d1);
                        int total = congLuong.so_nv;
                        int x = (int)total / 10;
                        nb6.Text = x * 12 + "";
                        nb5.Text = x * 10 + "";
                        nb4.Text = x * 8 + "";
                        nb3.Text = x * 6 + "";
                        nb2.Text = x * 4 + "";
                        nb1.Text = x * 2 + "";
                        bdduoi5tr.Width = ((double)congLuong.lnv_15 / (2 * x)) * bddonvi.ActualWidth;
                        bd5tr.Width = ((double)congLuong.lnv_57 / (2 * x)) * bddonvi.ActualWidth;
                        bd7tr.Width = ((double)congLuong.lnv_710 / (2 * x)) * bddonvi.ActualWidth;
                        bd10tr.Width = ((double)congLuong.lnv_10 / (2 * x)) * bddonvi.ActualWidth;
                    }
                    loading.Visibility = Visibility.Collapsed;
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_cong_luong.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.TongHopLuongNhanVienTheoChuKi(Main, month, year));
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            year = ""; month = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");

            getData(month, year);
        }
    }
}
