using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            TenNV.Text = Main.CurrentEmployee.ep_name;
            getData();
            getData1(int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("yyyy")),Main.CurrentEmployee.ep_id);
            txtThang.Text = DateTime.Now.ToString("MM/yyyy");
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

        private DataQuanLyTaiKhoanNV _listNV;

        public DataQuanLyTaiKhoanNV listNV
        {
            get { return _listNV; }
            set
            {
                _listNV = value; OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_company", Main.CurrentEmployee.com_id);
                web.QueryString.Add("token", Main.CurrentEmployee.token);
                web.QueryString.Add("id_emp", Main.CurrentEmployee.ep_id);
                web.QueryString.Add("month", DateTime.Now.ToString("MM"));
                web.QueryString.Add("year", DateTime.Now.ToString("yyyy"));
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_QuanLyTaiKhoanNV api = JsonConvert.DeserializeObject<API_QuanLyTaiKhoanNV>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNV = api.data;
                    }
                    else if (api.error.message == "Không tìm thấy dữ liệu")
                    {
                        listNV = new DataQuanLyTaiKhoanNV() { sum_day = "0", form_pending = "0", sum_late="0", work_day ="0"};
                    }    
                        
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/employe/manager_emp.php", web.QueryString);
            }
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
        private void getData1(int month, int year, string ep_id)
        {
            if (!string.IsNullOrEmpty(ep_id))
            {
                this.Dispatcher.Invoke(() =>
                {
                    using (WebClient web = new WebClient())
                    {
                        if (Main.MainType == 1)
                        {
                            web.QueryString.Add("token", Main.CurrentEmployee.token);
                            web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
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

                                foreach (var item in listLich)
                                {
                                    if (item != null && item.ctca != null)
                                    {
                                        foreach (var i in item.ctca)
                                        {
                                            if (!string.IsNullOrEmpty(i.num_to_calculate) && i.check != "Ca chưa hoàn thành")
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

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void XemLuong(object sender, MouseButtonEventArgs e)
        {
            Main.SideBarIndexNV = 2;
        }
    }
}
