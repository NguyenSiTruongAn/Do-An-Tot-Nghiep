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
            getDataTB();
        }

        private void VeBieuDo(Grid gridbd, Grid bd, double d, int type)
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
                    }
                    Line line3 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1.5, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    gridbd.Children.Add(line3);
                }
                else
                    if(d < 2 * Math.PI)
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
                    x = 125 - 125 * (Math.Sin(Math.PI/2));
                    y = 125 + Math.Sqrt(125 * 125 - (125 - x) * (125 - x));
                    z = (250 - y) / ((125 - x) / 0.2);
                    for (double i = 125, j = 250; i > x; i -= 0.2, j -= z)
                    {
                            Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                            gridbd.Children.Add(line1);
                    }
                    Line line3 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1.5, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    gridbd.Children.Add(line3);
                    x = 125 - 125 * (Math.Cos(d - 3*Math.PI / 2));
                    y = Math.Sqrt(125 * 125 - (125- x) * (125 -x));
                    z = (y) / ((x) / 0.2);
                    for (double i = 0, j = 125; i <= x; i += 0.2, j -= z)
                    {
                        if (i >= x - 1)
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
                }
                else if(d == 2 * Math.PI)
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
                    x = 125 - 125 * (Math.Sin(Math.PI / 2));
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
                    }
                    x = 125 - 125 * (Math.Cos(d - 3 * Math.PI / 2));
                    y = Math.Sqrt(125 * 125 - (x) * (x));
                    z = (y) / ((x) / 0.2);
                    for (double i = 0, j = 125; i <= x; i -= 0.2, j -= z)
                    {
                            Line line1 = new Line() { X1 = i, Y1 = j, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#4C5BD4") };
                            gridbd.Children.Add(line1);
                    }
                    Line line2 = new Line() { X1 = 125, Y1 = 0, X2 = 125, Y2 = 125, StrokeThickness = 1, Stroke = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff") };
                    bd.Children.Add(line2);
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
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 800)
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

        private void getDataTB()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("cp", "2");
                }
                if (Main.MainType == 1)
                {
                    web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                    web.QueryString.Add("token", Main.CurrentEmployee.token);
                    web.QueryString.Add("cp", "1");
                    web.QueryString.Add("user_nhan", Main.CurrentEmployee.ep_id);
                }

                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ThongBaoCT api = JsonConvert.DeserializeObject<API_ThongBaoCT>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            Main.listTB = api.data.abc;
                            if (Main.listTB != null)
                                Main.sotb = Main.listTB.Count;
                            if (Main.sotb >= 10)
                            {
                                Main.fontsize = 10;
                                Main.margin = new Thickness(10, -7, 0, 0);
                            }
                            else
                            {
                                Main.fontsize = 14;
                                Main.margin = new Thickness(12.5, -10.5, 0, 0);
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
        }

        private ItemCongLuong _congLuong;

        public ItemCongLuong congLuong
        {
            get { return _congLuong; }
            set
            {
                _congLuong = value;
                OnPropertyChanged();
            }
        }

        double tongluong=0, tongthue=0, tongbh = 0;
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
                    Api_CongLuong api =
                        JsonConvert.DeserializeObject<Api_CongLuong>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        BieuDo.Visibility = BieuDo1.Visibility = stBDCot.Visibility = Visibility.Visible;
                        congLuong = api.data.list;
                        tongbh = double.Parse(congLuong.tong_bh);
                        tongluong = double.Parse(congLuong.tong_luong);
                        tongthue = double.Parse(congLuong.tong_thue);
                        BrushConverter bc = new BrushConverter();
                        grid.Children.Clear();
                        grid1.Children.Clear();
                        double d = congLuong.pt_bh * (Math.PI/50);
                        if (d > 2 * Math.PI)
                            d = 2*Math.PI - ( d - 2 * Math.PI);
                        VeBieuDo(grid,bd1, d,1);
                        double d1 = congLuong.pt_thue * (Math.PI/50);
                        VeBieuDo(grid1, bd2, d1,2);
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
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_cong_luong.php",
                    web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(
                new Views.BaoCaoCongLuong.TongHopLuongNhanVienTheoChuKi(Main, month, year));
        }

        private void Border_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(
                new Views.BaoCaoCongLuong.TongHopThongTinBaoHiem(Main, month, year));
        }

        private void Border_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(
                new Views.BaoCaoCongLuong.TongHopThongTinThue(Main, month, year));
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

        private void HoverBH(object sender, MouseEventArgs e)
        {
            var z = Mouse.GetPosition(Main.PopupSelection);
            bTongbh.Visibility = Visibility.Visible;
            bTongbh.Margin = new Thickness(z.X - 330, z.Y - 90, 0, 0);
            bTongLuong.Visibility = Visibility.Collapsed;
        }

        private void LeaveLuong(object sender, MouseEventArgs e)
        {
            bTongLuong.Visibility = Visibility.Collapsed;
        }
        private void LeaveBH(object sender, MouseEventArgs e)
        {
            bTongbh.Visibility = Visibility.Collapsed;
        }
        private void LeaveT(object sender, MouseEventArgs e)
        {
            bTongT.Visibility = Visibility.Collapsed;
        }

        private void HoverDuoi5tr(object sender, MouseEventArgs e)
        {
            duoi5tr.Visibility = Visibility.Visible;
        }

        private void LeaveDuoi5tr(object sender, MouseEventArgs e)
        {
            duoi5tr.Visibility = Visibility.Collapsed;
        }

        private void Hover10tr(object sender, MouseEventArgs e)
        {
            b10tr.Visibility = Visibility.Visible;
        }

        private void Leave10tr(object sender, MouseEventArgs e)
        {
            b10tr.Visibility = Visibility.Collapsed;
        }
        private void Hover5tr(object sender, MouseEventArgs e)
        {
            b5tr.Visibility = Visibility.Visible;
        }

        private void Leave5tr(object sender, MouseEventArgs e)
        {
            b5tr.Visibility = Visibility.Collapsed;
        }

        private void Hover7tr(object sender, MouseEventArgs e)
        {
            b7tr.Visibility = Visibility.Visible;
        }

        private void Leave7tr(object sender, MouseEventArgs e)
        {
            b7tr.Visibility = Visibility.Collapsed;
        }

        private void HoverTongLuong(object sender, MouseEventArgs e)
        {
            var z = Mouse.GetPosition(Main.PopupSelection);
            bTongLuong.Visibility = Visibility.Visible;
            bTongLuong.Margin = new Thickness(z.X - 330, z.Y - 70, 0, 0);
        }

        private void HoverT(object sender, MouseEventArgs e)
        {
            var z = Mouse.GetPosition(Main.PopupSelection);
            bTongT.Visibility = Visibility.Visible;
            bTongT.Margin = new Thickness(z.X - 330, z.Y - 70, 0, 0);
            bTongLuong.Visibility = Visibility.Collapsed;
        }
    }
}