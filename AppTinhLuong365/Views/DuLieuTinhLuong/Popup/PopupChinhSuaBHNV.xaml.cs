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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaBHNV.xaml
    /// </summary>
    public partial class PopupChinhSuaBHNV : Page, INotifyPropertyChanged
    {
        MainWindow Main;
        public PopupChinhSuaBHNV(MainWindow main, DSNVGoiBH bhnv)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main; 
            dteSelectedMonth = new Calendar();
            dteSelectedMonth.Visibility = Visibility.Collapsed;
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            dteSelectedMonth.MouseLeftButtonDown += Select_thang;
            dteSelectedMonth.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged;
            cl = new List<Calendar>();
            cl.Add(dteSelectedMonth);
            cl = cl.ToList();
            dteSelectedMonth1 = new Calendar();
            dteSelectedMonth1.Visibility = Visibility.Collapsed;
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            dteSelectedMonth1.MouseLeftButtonDown += Select_thang1;
            dteSelectedMonth1.DisplayModeChanged += dteSelectedMonth_DisplayModeChanged1;
            cl1 = new List<Calendar>();
            cl1.Add(dteSelectedMonth1);
            cl1 = cl1.ToList();
            textThangAD.Text = bhnv.display_cls_day;
            if(bhnv.display_cls_day_end != "---")
            {
                textThangAD1.Text = bhnv.display_cls_day_end;
            }
            this.bhnv = bhnv;
            txtID.Text = bhnv.cls_id_user;
            txtName.Text = bhnv.ep_name;
        }

        private DSNVGoiBH _bhnv;

        public DSNVGoiBH bhnv
        {
            get { return _bhnv; }
            set { _bhnv = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Calendar dteSelectedMonth { get; set; }
        Calendar dteSelectedMonth1 { get; set; }

        private List<Calendar> _cl;

        public List<Calendar> cl
        {
            get { return _cl; }
            set
            {
                _cl = value; OnPropertyChanged();
            }
        }

        private List<Calendar> _cl1;

        public List<Calendar> cl1
        {
            get { return _cl1; }
            set
            {
                _cl1 = value; OnPropertyChanged();
            }
        }

        int flag = 0;
        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag = 1;
            if (textThangAD1.Text != "--------- ----")
                dteSelectedMonth.DisplayDateEnd = DateTime.Parse(textThangAD1.Text);
        }

        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThangAD != null && !string.IsNullOrEmpty(x))
            {
                textThangAD.Text = x;
            }
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }
            flag += 1;
        }

        int flag1 = 0;
        private void Select_thang1(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            flag1 = 1;
            if (textThangAD.Text != "--------- ----")
                dteSelectedMonth1.DisplayDateStart = DateTime.Parse(textThangAD.Text);
        }

        private void dteSelectedMonth_DisplayModeChanged1(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (textThangAD1 != null && !string.IsNullOrEmpty(x))
            {
                textThangAD1.Text = x;
            }
            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }
            flag1 += 1;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void LuuLai(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateDate.Text = "";
            if (textThangAD.Text == "--------- ----")
            {
                allow = false;
                validateDate.Text = "Vui lòng chọn thời gian áp dụng";
            }
            if (allow)
            {
                string day_end = "";
                if (textThangAD1.Text != "--------- ----")
                    day_end =DateTime.Parse(textThangAD1.Text).ToString("yyyy-MM");
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id_tax", bhnv.cls_id);
                    web.QueryString.Add("date_start", DateTime.Parse(textThangAD.Text).ToString("yyyy-MM"));
                    web.QueryString.Add("date_end", day_end);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ChinhSuaPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_ChinhSuaPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(Main));
                                Main.sidebar.SelectedIndex = 8;
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_emp_insrc.php", web.QueryString);
                }
            }
        }
    }
}
