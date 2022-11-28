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
    /// Interaction logic for ProFile.xaml
    /// </summary>
    public partial class ProFile : Page, INotifyPropertyChanged
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
        public ProFile(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
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
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        private DataTTNhanVien _TTNV;

        public DataTTNhanVien TTNV
        {
            get { return _TTNV; }
            set
            {
                _TTNV = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    using (WebClient web = new WebClient())
                    {
                        if (Main.MainType == 1)
                        {
                            web.QueryString.Add("token", Main.CurrentEmployee.token);
                            web.QueryString.Add("id_com", Main.CurrentEmployee.com_id);
                            web.QueryString.Add("id", Main.CurrentEmployee.ep_id);
                        }

                        web.UploadValuesCompleted += (s, e) =>
                        {
                            try
                            {
                                string x = UnicodeEncoding.UTF8.GetString(e.Result);
                                API_TTNhanVien api = JsonConvert.DeserializeObject<API_TTNhanVien>(x);
                                if (api.data != null)
                                {
                                    TTNV = api.data;
                                    if (TTNV != null)
                                        if (TTNV.ep_image == "/img/add.png" || string.IsNullOrEmpty(TTNV.ep_image))
                                            TTNV.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                        else TTNV.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + TTNV.ep_image;
                                    txtGioiThieu.Text = TTNV.ep_description;
                                    TTNV = TTNV;
                                    if (TTNV.tax == null)
                                        dataGrid2.Visibility = Visibility.Collapsed;
                                    if (TTNV.insurance == null)
                                        dataGrid3.Visibility = Visibility.Collapsed;
                                    if (TTNV.contract == null)
                                        dataGrid1.Visibility = Visibility.Collapsed;
                                    if (TTNV.salary == null)
                                        dataGrid.Visibility = Visibility.Collapsed;
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/employe/profile_emp.php", web.QueryString);
                    }
                });
            }
            catch
            {
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
        }

        private void OpenDes(object sender, MouseButtonEventArgs e)
        {
            borderes.Visibility = borderes.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            txtGioiThieu.Visibility = txtGioiThieu.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
            txtGioiThieuSua.Text = TTNV.ep_description;
        }

        private void OpenDes1(object sender, MouseButtonEventArgs e)
        {
            borderes1.Visibility =
                borderes1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            txtSuaTen.Text = TTNV.ep_name;
            txtep_id.Text = TTNV.ep_id;
            txtSuaPhone.Text = TTNV.ep_phone;
            txtSuaEmail.Text = TTNV.ep_email;
            txtSuaDiaChi.Text = TTNV.ep_address;
            cbHonNhan.SelectedIndex = int.Parse(TTNV.ep_married);
            cbNgaySinh.SelectedDate = DateTime.Parse(TTNV.display_ep_birth_day);
            NgayBatDau.Text = DateTime.Parse(TTNV.start_working_time).ToString("dd/MM/yyyy");
            cbGioiTinh.SelectedIndex = int.Parse(TTNV.ep_gender);
            borderes2.Visibility =
                borderes2.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SuaGioiThieu(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 1)
                {
                    web.QueryString.Add("token", Main.CurrentEmployee.token);
                    web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                }

                web.QueryString.Add("emp_name", TTNV.ep_name);
                web.QueryString.Add("ep_phone", TTNV.ep_phone);
                web.QueryString.Add("ep_address", TTNV.ep_address);
                web.QueryString.Add("id_emp", Main.CurrentEmployee.ep_id);
                web.QueryString.Add("description", txtGioiThieuSua.Text);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        API_SuaGioiThieu api =
                        JsonConvert.DeserializeObject<API_SuaGioiThieu>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            TTNV.ep_description = txtGioiThieu.Text = txtGioiThieuSua.Text;
                            borderes.Visibility = Visibility.Collapsed;
                            txtGioiThieu.Visibility = Visibility.Visible;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/employe/edit_description_emp.php",
                    web.QueryString);
            }
        }

        private void SuaThongTin(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validateName.Text = validatePhone.Text = validateHonNhan.Text =
                validateNgaySinh.Text = validateGioiTinh.Text = validateDiaChi.Text = "";
            if (string.IsNullOrEmpty(txtSuaTen.Text))
            {
                allow = false;
                validateName.Text = "Vui lòng nhập đầy đủ";
            }

            if (cbHonNhan.SelectedIndex < 1)
            {
                allow = false;
                validateHonNhan.Text = "Vui lòng chọn tình trạng";
            }

            var x = DateTime.Now - cbNgaySinh.SelectedDate;
            if (x.Value.TotalDays < 5475)
            {
                allow = false;
                validateNgaySinh.Text = "Vui lòng chọn ngày sinh phù hợp(>=15 tuổi)";
            }

            if (string.IsNullOrEmpty(txtSuaPhone.Text))
            {
                allow = false;
                validatePhone.Text = "Vui lòng nhập đầy đủ";
            }

            if (cbGioiTinh.SelectedIndex < 1)
            {
                allow = false;
                validateGioiTinh.Text = "Vui lòng chọn giới tính";
            }

            if (string.IsNullOrEmpty(txtSuaDiaChi.Text))
            {
                allow = false;
                validateDiaChi.Text = "Vui lòng nhập thông tin";
            }

            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 1)
                    {
                        web.QueryString.Add("token", Main.CurrentEmployee.token);
                        web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                    }

                    web.QueryString.Add("ep_name", txtSuaTen.Text);
                    web.QueryString.Add("ep_phone", txtSuaPhone.Text);
                    web.QueryString.Add("ep_address", txtSuaDiaChi.Text);
                    web.QueryString.Add("ep_gender", cbGioiTinh.SelectedIndex + "");
                    web.QueryString.Add("birth_day", cbNgaySinh.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("married_id", cbHonNhan.SelectedIndex + "");
                    web.QueryString.Add("id_ep", Main.CurrentEmployee.ep_id);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.ProFile(Main));
                                Main.title.Text = " Hồ sơ cá nhân";
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/employe/edit_emp.php",
                        web.QueryString);
                }
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dg.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.Home(Main));
            Main.SideBarIndexNV = 0;
        }
    }
}
