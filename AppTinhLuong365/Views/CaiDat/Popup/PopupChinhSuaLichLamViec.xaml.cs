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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaLichLamViec.xaml
    /// </summary>
    public partial class PopupChinhSuaLichLamViec : Page, INotifyPropertyChanged
    {
        MainWindow Main;

        string date;
        public PopupChinhSuaLichLamViec(MainWindow main, string cy_id, ItemEmp data, string date)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.cy_id = cy_id;
            this.data = data;
            this.date = date;
            getData();
            getData1();
        }

        private string cy_id;
        ItemEmp data;
        int month;
        int year;
        int start;

        private List<DSCaLamViec> _listCa;

        public List<DSCaLamViec> listCa
        {
            get { return _listCa; }
            set
            {
                /*if (value == null) value = new List<DSCaLamViec>();
                value.Insert(0,new DSCaLamViec() { shift_id="-1",shift_name="Tất cả các ca" });*/
                _listCa = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_com", Main.CurrentCompany.com_id);
                        web.Headers.Add("Authorization", Main.CurrentCompany.token);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_DSCaLamViec api = JsonConvert.DeserializeObject<API_DSCaLamViec>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listCa = api.data.items;
                        }
                    };
                    web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_shift.php", web.QueryString);
                }
            }
            catch (Exception ex)
            {


            }
        }

        private ChiTietLichLamViec _listLLV;

        public ChiTietLichLamViec listLLV
        {
            get { return _listLLV; }
            set
            {
                _listLLV = value;
                OnPropertyChanged();
            }
        }

        private void getData1()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("cy_id", cy_id);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        API_ChiTietLichLamViec api = JsonConvert.DeserializeObject<API_ChiTietLichLamViec>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listLLV = api.data.calendar;
                            tbTitle.Text = $"Tháng {DateTime.Now.Month}/{DateTime.Now.Year}";
                            tbInput.Text = listLLV.cycle_name;
                            month = int.Parse(DateTime.Parse(listLLV.begin).ToString("MM"));
                            year = int.Parse(DateTime.Parse(listLLV.begin).ToString("yyyy"));
                            start = (int)new DateTime(year, month, 1).DayOfWeek;
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
                                List<DSCaLamViec> dsc = new List<DSCaLamViec>();
                                string[] a;
                                if(listLLV.show.Count == DateTime.DaysInMonth(year, month))
                                {
                                    if (!string.IsNullOrEmpty(listLLV.show[i - 1].shift))
                                    {
                                        a = listLLV.show[i - 1].shift.Split(',');
                                        foreach (var item in listCa)
                                        {
                                            foreach (var x in a)
                                            {
                                                if (x == item.shift_id)
                                                    dsc.Add(item);
                                            }
                                        }
                                    }
                                }    
                                var d = new lichlamviec() { id = listLich.Count, ngay = i, ca = dsc.Count, status = 1, dsca = dsc };
                                listLich.Add(d);
                            }
                            int n = 42 - listLich.Count;
                            for (int i = 1; i <= n; i++)
                            {
                                var d = new lichlamviec() { id = listLich.Count, ngay = i, ca = 0, status = 0 };
                                listLich.Add(d);
                            }
                            listLich = listLich.ToList();
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/detail_cycle.php", web.QueryString);
                }
            }
            catch (Exception ex)
            {


            }
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
            public int _ca;
            public int ca
            {
                get { return _ca; }
                set { _ca = value; OnPropertyChanged(); }
            }
            public int _status;
            public int status
            {
                get { return _status; }
                set { _status = value; OnPropertyChanged(); }
            }
            public List<DSCaLamViec> dsca { get; set; }
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
            chonCa.Visibility = Visibility.Visible;
            txtNgay.Text = data.ngay + "";
            txtThang.Text = DateTime.Now.ToString("MM");
            txtNam.Text = DateTime.Now.ToString("yyyy");
            if(data.dsca != null)
            {
                foreach (var item in listCa)
                {
                    item.ischecked = false;
                    foreach (var i in data.dsca)
                    {
                        if (item.shift_id == i.shift_id)
                            item.ischecked = true;
                    }
                }
            }
        }
        private void abc(object sender, MouseButtonEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null)
            {
                if (cb.IsChecked==true)
                {
                    DSCaLamViec data = (DSCaLamViec)cb.DataContext;
                    foreach (var item in listLich)
                    {
                        if (item.status == 2)
                        {
                            item.ca--;
                            item.dsca.Remove(data);
                        }

                    }
                }
                else
                {
                    DSCaLamViec data = (DSCaLamViec)cb.DataContext;
                    foreach (var item in listLich)
                    {
                        if (item.status == 2)
                        {
                            item.ca++;
                            item.dsca.Add(data);
                        }
                    }
                }
            }
        }

        private void LuuLich(object sender, MouseButtonEventArgs e)
        {
            string cycle = "{";
            for (int i =0;  i< DateTime.DaysInMonth(year, month); i++)
            {
                string a = "[";
                if (listLich[start + i].dsca.Count > 0)
                {
                    for (int j = 0; j < listLich[start + i].dsca.Count; j++)
                    {
                        if (j < listLich[start + i].dsca.Count - 1)
                            a += "\"" + listLich[start + i].dsca[j].shift_id + "\"" + ",";
                        else
                            a += "\"" + listLich[start + i].dsca[j].shift_id + "\"" + "]";
                    }
                }
                else
                    a += "]";
                if (i < DateTime.DaysInMonth(year, month) - 1)
                    cycle += "\"" + year+"-"+month+"-"+(i+1) + "\":" + a + ",";
                else
                    cycle += "\"" + year + "-" + month + "-" + (i + 1) + "\":" + a + "}";
            }
            string cycle1 = "[";
            for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
            {
                string a = "\"shift_id\":\"";
                if (listLich[start + i].dsca.Count > 0)
                {
                    for (int j = 0; j < listLich[start + i].dsca.Count; j++)
                    {
                        if (j < listLich[start + i].dsca.Count - 1)
                            a += listLich[start + i].dsca[j].shift_id + ",";
                        else
                            a += listLich[start + i].dsca[j].shift_id + "\"" + "";
                    }
                }
                else
                {
                    a += "\"";
                }
                if (i < DateTime.DaysInMonth(year, month) - 1)
                    cycle1 += "{\"date\":\"" + year + "-" + month + "-" + (i + 1) + "\"," + a + "},";
                else
                    cycle1 += "{\"date\":\"" + year + "-" + month + "-" + (i + 1) + "\"," + a +"}]";
            }

            bool allow = true;
            validateName.Text = "";
            if(string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }

                    if (data != null)
                    {
                        web.QueryString.Add("name", tbInput.Text);
                        web.QueryString.Add("id_ep", data.ep_id);
                        web.QueryString.Add("cycle", cycle);
                        web.QueryString.Add("month", listLLV.begin);
                        web.UploadValuesCompleted += (s, ee) =>
                        {
                            string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data.ep_id));
                                Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                this.Visibility = Visibility.Collapsed;
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_cycle_pf.php", web.QueryString);
                    }
                    else
                    {
                        web.Headers.Add("Authorization", Main.CurrentCompany.token);
                        web.QueryString.Add("cy_name", tbInput.Text);
                        web.QueryString.Add("detail_cycle", cycle1);
                        string a = DateTime.Parse(date).ToString("yyyy/MM/dd");
                        web.QueryString.Add("date", a+"");
                        web.QueryString.Add("cy_id", cy_id);
                        web.UploadValuesCompleted += (s, ee) =>
                        {
                            string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                                if (api.data != null)
                            {
                                if (data != null)
                                {
                                    Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data.ep_id));
                                    Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(Main));
                                    Main.HomeSelectionPage.Visibility = Visibility.Visible;
                                }
                                this.Visibility = Visibility.Collapsed;
                            }
                        };
                        web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/update_cycle.php", web.QueryString);
                    }
                }
            }
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }
    }
}
