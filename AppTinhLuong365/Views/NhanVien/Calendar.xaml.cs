using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AppTinhLuong365.Views.NhanVien
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Page, INotifyPropertyChanged
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
        public Calendar(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            for (var i = 2021; i <= DateTime.Now.Year + 1; i++)
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
            txtThang.Text = month + "/" + year;
            txtTen.Text = Main.CurrentEmployee.ep_name;
            txtID.Text = Main.CurrentEmployee.ep_id;
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
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
        }
        MainWindow Main;
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
            public List<Lich> ctca { get; set; }
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

        private List<List<Lich>> _listDSNV;

        public List<List<Lich>> listDSNV
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
                        web.QueryString.Add("id_ep", Main.CurrentEmployee.ep_id);
                        web.QueryString.Add("month", month + "");
                        web.QueryString.Add("year", year + "");
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_LichLamViecNV api = JsonConvert.DeserializeObject<API_LichLamViecNV>(UnicodeEncoding.UTF8.GetString(e.Result));
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
                                List<Lich> Chitietca = new List<Lich>();
                                if (listDSNV != null)
                                    Chitietca = listDSNV[i - 1];
                                var d = new lichlamviec() { id = listLich.Count, ngay = i, ctca = Chitietca, status = 1 };
                                DateTime date = DateTime.Parse(year + "-" + month + "-" + i);
                                if (date == DateTime.Now)
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
                            listLich = listLich.ToList();
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/employe/tbl_cycle.php", web.QueryString);
                }
            });
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
            txtThang.Text = month + "/" + year;
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void DexuatLich(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://vanthu.timviec365.vn/tao-de-xuat-lich-lam-viec.html");
        }
    }
}
