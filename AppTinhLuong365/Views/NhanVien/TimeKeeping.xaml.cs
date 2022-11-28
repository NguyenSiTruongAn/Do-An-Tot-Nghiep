using AppTinhLuong365.Core;
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

namespace AppTinhLuong365.Views.NhanVien
{
    /// <summary>
    /// Interaction logic for TimeKeeping.xaml
    /// </summary>
    public partial class TimeKeeping : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        MainWindow Main;
        public TimeKeeping(MainWindow main)
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
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            getData(int.Parse(month), int.Parse(year));
            getDataTB();
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
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        public class lichlamviec : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int id;
            public int ngay { get; set; }
            public List<ChiTietCa> ctca { get; set; }
            public int _status;
            public int status
            {
                get { return _status; }
                set { _status = value; OnPropertyChanged(); }
            }
        }

        private double _Cong = 0;

        public double Cong
        {
            get { return _Cong; }
            set { _Cong = value; OnPropertyChanged(); }
        }

        private int _camuon = 0;

        public int camuon
        {
            get { return _camuon; }
            set { _camuon = value; OnPropertyChanged(); }
        }

        private List<lichlamviec> _listLich;

        public List<lichlamviec> listLich
        {
            get { return _listLich; }
            set { _listLich = value; OnPropertyChanged(); }
        }

        private List<List<ChiTietCa>> _listDSNV;

        public List<List<ChiTietCa>> listDSNV
        {
            get { return _listDSNV; }
            set { _listDSNV = value; OnPropertyChanged(); }
        }
        private int start;
        private void getData(int month, int year)
        {
            this.Dispatcher.Invoke(() =>
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 1)
                    {
                        web.QueryString.Add("token", Main.CurrentEmployee.token);
                        web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                        web.QueryString.Add("id_emp", Main.CurrentEmployee.ep_id);
                        web.QueryString.Add("month", month + "");
                        web.QueryString.Add("year", year + "");
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        try
                        {
                            API_ChiTietChamCong api = JsonConvert.DeserializeObject<API_ChiTietChamCong>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listDSNV = api.data.list;
                                /*                            month = int.Parse(DateTime.Parse(listLLV.begin).ToString("MM"));
                                                            year = int.Parse(DateTime.Parse(listLLV.begin).ToString("yyyy"));*/
                                start = (int)new DateTime(year, month, 1).DayOfWeek;
                                listLich = new List<lichlamviec>();
                                if (month - 1 > 0)
                                {
                                    for (int i = 0; i < start; i++)
                                    {
                                        var x = DateTime.DaysInMonth(year, month - 1);
                                        listLich.Add(new lichlamviec() { id = listLich.Count, status = 0 });
                                    }
                                    listLich.Reverse();
                                }
                                for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
                                {
                                    List<ChiTietCa> Chitietca = new List<ChiTietCa>();
                                    if (listDSNV != null)
                                        Chitietca = listDSNV[i - 1];
                                    var d = new lichlamviec() { id = listLich.Count, ngay = i, ctca = Chitietca, status = 1 };
                                    DateTime date = DateTime.Parse(year + "-" + month + "-" + i);
                                    if (date.ToString("yyyy/MM/dd") == DateTime.Now.ToString("yyyy/MM/dd"))
                                        d.status = 3;
                                    listLich.Add(d);
                                }
                                int n = 42 - listLich.Count;
                                if (n >= 7)
                                    n = n - 7;
                                for (int i = 1; i <= n; i++)
                                {
                                    var d = new lichlamviec() { id = listLich.Count, status = 2 };
                                    listLich.Add(d);
                                }
                                Cong = camuon = 0;
                                foreach (var item in listLich)
                                {
                                    if (item != null && item.ctca != null)
                                    {
                                        foreach (var i in item.ctca)
                                        {
                                            if (!string.IsNullOrEmpty(i.num_to_calculate) && i.check != "Ca chưa hoàn thành" && i.check !="Ca nghỉ")
                                                Cong += double.Parse(i.num_to_calculate);
                                            if (i.status == "2")
                                                camuon++;

                                        }
                                    }
                                }
                                listLich = listLich.ToList();
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_timekeeping_manager.php", web.QueryString);
                }
            });
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
            if (this.ActualWidth > 1750)
            {
                DockPanel.SetDock(dockChamCong, Dock.Right);
            }
            else
                DockPanel.SetDock(dockChamCong, Dock.Bottom);
        }

        private void ChonThang(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            getData(int.Parse(month), int.Parse(year));
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ListView lv = sender as ListView;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta); } else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }
        private void st_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.Home(Main));
            Main.SideBarIndexNV = 0;
        }
    }
}
