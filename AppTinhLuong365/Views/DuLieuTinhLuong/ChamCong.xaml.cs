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

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for ChamCong.xaml
    /// </summary>
    public partial class ChamCong : Page, INotifyPropertyChanged
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
        public ChamCong(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            var c = DateTime.Now.Year;
            if (c != null)
            {
                YearList.Add($"Năm {c - 1}");
                YearList.Add($"Năm {c}");
                YearList.Add($"Năm {c + 1}");
            }
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            getData(int.Parse(month),int.Parse(year),"");
            getData1(month,year,"");
            getData2();
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
        private void getData(int month, int year, string ep_id)
        {
            if (!string.IsNullOrEmpty(ep_id))
            {
                this.Dispatcher.Invoke(() =>
                {
                    using (WebClient web = new WebClient())
                    {
                        if (Main.MainType == 0)
                        {
                            web.QueryString.Add("token", Main.CurrentCompany.token);
                            web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                            web.QueryString.Add("id_emp", ep_id);
                            web.QueryString.Add("month", month + "");
                            web.QueryString.Add("year", year + "");
                        }
                        web.UploadValuesCompleted += (s, e) =>
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
                                    if(listDSNV != null)
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
                                Cong = camuon = 0;
                                foreach (var item in listLich)
                                {
                                    if(item!=null && item.ctca != null)
                                    {
                                        foreach (var i in item.ctca)
                                        {
                                            if(!string.IsNullOrEmpty(i.num_to_calculate) && i.check != "Ca chưa hoàn thành")
                                                Cong += double.Parse(i.num_to_calculate);
                                            if (i.status == "2")
                                                camuon++;

                                        }
                                    }    
                                    
                                }
                                listLich = listLich.ToList();
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_timekeeping_manager.php", web.QueryString);
                    }
                });
            }
        }

        private List<DSNVTheoThoiGian> _listNV;

        public List<DSNVTheoThoiGian> listNV
        {
            get { return _listNV; }
            set
            {
                if (value == null) value = new List<DSNVTheoThoiGian>();
                value.Insert(0, new DSNVTheoThoiGian() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listNV = value;
                cbNV.ItemsSource = _listNV;
            }
        }

        private void getData1(string month, string year, string dep_id)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_dep", dep_id);
                web.QueryString.Add("active", "true");
                web.QueryString.Add("start_date", year + "-" + month + "-01");
                int x = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
                web.QueryString.Add("date", year + "-" + month + "-" + x);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_DSNVTheoThoiGian api = JsonConvert.DeserializeObject<API_DSNVTheoThoiGian>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNV = api.data.list;
                    }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/ep_by_time.php", web.QueryString);
            }
        }

        private List<Item_dep> _listItem_dep;

        public List<Item_dep> listItem_dep
        {
            get { return _listItem_dep; }
            set
            {
                if (value == null) value = new List<Item_dep>();
                value.Insert(0, new Item_dep() { dep_id = "-1", dep_name = "Phòng ban (tất cả)" });
                _listItem_dep = value;
                OnPropertyChanged();
            }
        }

        private void getData2()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_dep api =
                        JsonConvert.DeserializeObject<API_List_dep>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listItem_dep = api.data.list;
                    }
                    // foreach (EpLate item in listEpLate)
                    // {
                    //     if (item.ts_image != "/img/add.png")
                    //     {
                    //         item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_dep.php", web.QueryString);
            }
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

        private void ChonPhong(object sender, SelectionChangedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            getData1(month, year,id_phong);
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            if (cbThang.SelectedIndex > -1)
                month = cbThang.Text.Split(' ')[1];
            string year;
            if (cbNam.SelectedIndex > -1)
                year = cbNam.Text.Split(' ')[1];
            else year = DateTime.Now.ToString("yyyy");
            string id_phong = "";
            if (cbPhong.SelectedIndex > -1)
            {
                Item_dep pb = (Item_dep)cbPhong.SelectedItem;
                if (pb.dep_id != "-1")
                    id_phong = pb.dep_id;
            }
            string id_user = "";
            if (cbNV.SelectedIndex > -1)
            {
                DSNVTheoThoiGian nv = (DSNVTheoThoiGian)cbNV.SelectedItem;
                if (nv.ep_id != "-1")
                    id_user = nv.ep_id;
            }
            getData(int.Parse(month), int.Parse(year), id_user);
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }
    }
}
