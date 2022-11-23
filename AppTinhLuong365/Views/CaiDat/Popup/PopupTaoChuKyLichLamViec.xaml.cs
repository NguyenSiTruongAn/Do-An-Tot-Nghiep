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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupTaoChuKyLichLamViec.xaml
    /// </summary>
    public partial class PopupTaoChuKyLichLamViec : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        MainWindow Main;
        string ten;
        string thang;
        int select;
        string date;
        int month;
        int year;
        int start;

        public PopupTaoChuKyLichLamViec(MainWindow main, string ten, string thang, int select, string date,
            List<DSCaLamViec> ca)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.ten = ten;
            TextBlockThang.Text = thang;
            this.thang = thang;
            this.select = select;
            this.date = date;
            this.dsca = ca;
            getData();
            getData1();
        }

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

        List<DSCaLamViec> dsca;

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
                        try
                        {
                            API_DSCaLamViec api =
                            JsonConvert.DeserializeObject<API_DSCaLamViec>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listCa = api.data.items;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_shift.php", web.QueryString);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Btn_QuayLai_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(
                new Views.CaiDat.Popup.PopupChonCaLamViec(Main, ten, thang, select, date));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<string> ca = new List<string>();

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_shift data = (Item_shift)cb.DataContext;
            ca.Add(data.shift_id);
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_shift data = (Item_shift)cb.DataContext;
            ca.Remove(data.shift_id);
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
            month = int.Parse(DateTime.Parse(thang).ToString("MM"));
            year = int.Parse(DateTime.Parse(thang).ToString("yyyy"));
            start = (int)new DateTime(year, month, 1).DayOfWeek;
            listLich = new List<lichlamviec>();
            if (month - 1 > 0)
            {
                for (int i = 0; i < start; i++)
                {
                    var x = DateTime.DaysInMonth(year, month - 1);
                    listLich.Add(
                        new lichlamviec() { id = listLich.Count, ngay = x - i, ca = 0, status = 0 });
                }

                listLich.Reverse();
            }

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                List<DSCaLamViec> dsc = new List<DSCaLamViec>();
                var d = new lichlamviec()
                    { id = listLich.Count, ngay = i, ca = dsc.Count, status = 1, dsca = dsc };
                listLich.Add(d);
            }

            int n = 42 - listLich.Count;
            for (int i = 1; i <= n; i++)
            {
                var d = new lichlamviec() { id = listLich.Count, ngay = i, ca = 0, status = 0 };
                listLich.Add(d);
            }

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                List<DSCaLamViec> dsc = new List<DSCaLamViec>();
                int x = (int)new DateTime(year, month, listLich[i + start - 1].ngay).DayOfWeek;
                if (DateTime.Parse(date).Day <= listLich[i + start - 1].ngay)
                {
                    if (select == 0)
                    {
                        if (x >= 1 && x < 6)
                        {
                            dsc = dsca;
                        }
                    }

                    if (select == 1)
                    {
                        if (x >= 1 && x < 7)
                        {
                            dsc = dsca;
                        }
                    }

                    if (select == 2)
                    {
                        dsc = dsca;
                    }
                }

                var d = new lichlamviec()
                    { id = i + start - 1, ngay = i, ca = dsc.Count, status = 1, dsca = dsc };
                listLich[i + start - 1] = d;
            }

            listLich = listLich.ToList();
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
                set
                {
                    _ca = value;
                    OnPropertyChanged();
                }
            }

            public int _status;

            public int status
            {
                get { return _status; }
                set
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }

            public List<DSCaLamViec> dsca { get; set; }
        }

        private List<lichlamviec> _listLich;

        public List<lichlamviec> listLich
        {
            get { return _listLich; }
            set
            {
                _listLich = value;
                OnPropertyChanged();
            }
        }

        private List<TextBlock> listTextBlock = new List<TextBlock>();

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
            if (g != null)
            {
                Border bordeIndex = g.Children[0] as Border;
                if (bordeIndex != null)
                {
                    TextBlock tb = bordeIndex.Child as TextBlock;
                    MessageBox.Show(tb.Text);
                }
            }
        }

        private void selectNgay(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            lichlamviec data = (lichlamviec)b.DataContext;
            if (data.status == 1)
            {
                foreach (var item in listLich)
                {
                    if (item.status == 2)
                        item.status = 1;
                    if (item.id == data.id)
                        item.status = 2;
                }
            }

            chonCa.Visibility = Visibility.Visible;
            txtNgay.Text = data.ngay + "";
            txtThang.Text = DateTime.Now.ToString("MM");
            txtNam.Text = DateTime.Now.ToString("yyyy");
            if (data.dsca != null)
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
                if (cb.IsChecked == true)
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
            // string cycle = "{";
            // for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
            // {
            //     string a = "[";
            //     if (listLich[start + i].dsca.Count > 0)
            //     {
            //         for (int j = 0; j < listLich[start + i].dsca.Count; j++)
            //         {
            //             if (j < listLich[start + i].dsca.Count - 1)
            //                 a += "\"" + listLich[start + i].dsca[j].shift_id + "\"" + ",";
            //             else
            //                 a += "\"" + listLich[start + i].dsca[j].shift_id + "\"" + "]";
            //         }
            //     }
            //     else
            //         a += "]";
            //
            //     if (i < DateTime.DaysInMonth(year, month) - 1)
            //         cycle += "\"" + year + "-" + month + "-" + (i + 1) + "\":" + a + ",";
            //     else
            //         cycle += "\"" + year + "-" + month + "-" + (i + 1) + "\":" + a + "}";
            // }

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
                    cycle1 += "{\"date\":\"" + year + "-" + month + "-" + (i + 1) + "\"," + a + "}]";
            }

            bool allow = true;
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.Headers.Add("Authorization", Main.CurrentCompany.token);
                        web.QueryString.Add("id_com", Main.CurrentCompany.com_id);
                    }

                    web.QueryString.Add("cycle_name", ten);
                    web.QueryString.Add("detail_cycle", cycle1);
                    string a = DateTime.Parse(date).ToString("yyyy-MM") + "-01";
                    web.QueryString.Add("date", a);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(Main));
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/add_cycle.php",
                        web.QueryString);
                    
                }
            }
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
        }
    }
}