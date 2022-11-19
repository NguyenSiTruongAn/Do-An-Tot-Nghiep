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

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for PhucLoi.xaml
    /// </summary>
    public partial class PhucLoi : Page, INotifyPropertyChanged
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
        public PhucLoi(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
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

        private List<ListWelfare> _listPhucLoi;

        public List<ListWelfare> listPhucLoi
        {
            get { return _listPhucLoi; }
            set { _listPhucLoi = value; OnPropertyChanged(); }
        }

        private List<ListAllowance> _listPhuCap;

        public List<ListAllowance> listPhuCap
        {
            get { return _listPhuCap; }
            set { _listPhuCap = value; OnPropertyChanged(); }
        }

        private List<ListAllowanceShift> _listPhuCapTheoCa;

        public List<ListAllowanceShift> listPhuCapTheoCa
        {
            get { return _listPhuCapTheoCa; }
            set { _listPhuCapTheoCa = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            this.Dispatcher.Invoke(() =>
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.UploadValuesCompleted += (s, e) =>
                    {
                        try
                        {
                            API_PhucLoi api = JsonConvert.DeserializeObject<API_PhucLoi>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                listPhucLoi = api.data.list_welfare;
                                listPhucLoi.Reverse();
                                listPhuCap = api.data.list_allowance;
                                listPhuCap.Reverse();
                                listPhuCapTheoCa = api.data.list_allowance_shift;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_welfare.php", web.QueryString);
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

            if (this.ActualWidth > 615)
            {
                DockPanel.SetDock(borderPhucLoi, Dock.Right);
            }
            else
            {
                DockPanel.SetDock(borderPhucLoi, Dock.Bottom);
            }
            if (this.ActualWidth > 1200)
            {
                DockPanel.SetDock(borderPhuCap, Dock.Right);
            }
            else
            {
                DockPanel.SetDock(borderPhuCap, Dock.Bottom);
            }
            if (this.ActualWidth > 1000)
            {
                DockPanel.SetDock(borderPhuCapTheoCa, Dock.Right);
            }
            else
            {
                DockPanel.SetDock(borderPhuCapTheoCa, Dock.Bottom);
            }
        }

        private void borderPhucLoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupPhucLoi(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void borderPhuCap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemMoiPhuCap(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void borderPhuCapTheoCa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThemMoiPhuCapTheoCa(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void BtnTuyChonDSPL(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            ListWelfare data = (ListWelfare)p.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTuyChonDSPL(Main, data.cl_id, data.cl_name, data.cl_salary, data.cl_note, data.cl_type_tax, data.cl_day, data.cl_day_end);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListAllowanceShift data = (ListAllowanceShift)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupSuaPhuCapTheoCa(Main, data.wf_id, data.wf_money, data.wf_time, data.wf_time_end));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnXoaPhuCapTheoCa_Click(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListAllowanceShift data = (ListAllowanceShift)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupThongBaoXoaPhuCapTheoCa(Main, data.wf_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid2_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void BtnTuyChonDSPC(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            ListAllowance data = (ListAllowance)p.DataContext;
            var pop = new Views.DuLieuTinhLuong.Popup.PopupTuyChonDSPC(Main, data.cl_id, data.cl_name, data.cl_salary, data.cl_note, data.cl_type_tax, data.cl_day, data.cl_day_end);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}

