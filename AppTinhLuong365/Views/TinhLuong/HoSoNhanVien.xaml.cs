using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for HoSoNhanVien.xaml
    /// </summary>
    public partial class HoSoNhanVien : Page, INotifyPropertyChanged
    {
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

        public MainWindow Main;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HoSoNhanVien(MainWindow main, string ep_id)
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

            
            getData(ep_id);
        }

        ItemEmp data1 = new ItemEmp();
        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

        private ChiTietNV _ChiTietNV;

        public ChiTietNV ChiTietNV
        {
            get { return _ChiTietNV; }
            set
            {
                _ChiTietNV = value;
                OnPropertyChanged();
            }
        }

        private void getData(string user_id)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    using (WebClient web = new WebClient())
                    {
                        if (Main.MainType == 0)
                        {
                            web.QueryString.Add("token", Main.CurrentCompany.token);
                            web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                            web.QueryString.Add("id", user_id);
                        }

                        web.UploadValuesCompleted += (s, e) =>
                        {
                            string x = UnicodeEncoding.UTF8.GetString(e.Result);
                            API_ChiTietNV api = JsonConvert.DeserializeObject<API_ChiTietNV>(x);
                            if (api.data != null)
                            {
                                ChiTietNV = api.data.list;
                                if (ChiTietNV != null)
                                    if (ChiTietNV.ep_image == "/img/add.png")
                                        ChiTietNV.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                txtName.Text = ChiTietNV.ep_name;
                                txtNS.Text = DateTime.Parse(ChiTietNV.ep_birth_day).ToString("dd/MM/yyyy");
                                txtDep.Text = ChiTietNV.dep_name;
                                txtAddress.Text = ChiTietNV.ep_address;
                                txtPhoneNumber.Text = ChiTietNV.ep_phone;
                                txtBank.Text = ChiTietNV.ep_phone_tk;
                                txtChucVu.Text = ChiTietNV.position_id;
                                txtBank.Text = ChiTietNV.display_st_bank;
                                txtGt.Text = ChiTietNV.display_ep_gender;
                                txtMa.Text = ChiTietNV.ep_id;
                                txtStartWork.Text = ChiTietNV.display_start_working_time;
                                txtEmail.Text = ChiTietNV.display_ep_email;
                                txtSTK.Text = ChiTietNV.display_st_stk;
                                txtNgayTinhLuong.Text = ChiTietNV.display_st_time;
                                txtGioiThieu.Text = ChiTietNV.ep_description;
                                Ten.Text = ChiTietNV.ep_name;
                                ChucVu.Text = ChiTietNV.dep_name;
                                if (!string.IsNullOrEmpty(ChiTietNV.ep_image))
                                    ChiTietNV.ep_image = "https://chamcong.24hpay.vn/upload/employee/" +
                                                         ChiTietNV.ep_image;
                                else
                                    ChiTietNV.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                image.ImageSource = new BitmapImage(new Uri(ChiTietNV.ep_image));
                            }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/profile_ep.php",
                            web.QueryString);
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
            txtGioiThieuSua.Text = ChiTietNV.ep_description;
        }

        private void OpenDes1(object sender, MouseButtonEventArgs e)
        {
            borderes1.Visibility =
                borderes1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            txtSuaTen.Text = ChiTietNV.ep_name;
            txtep_id.Text = ChiTietNV.ep_id;
            txtSuaPhone.Text = ChiTietNV.ep_phone;
            txtSuaEmail.Text = ChiTietNV.ep_email;
            txtSuaDiaChi.Text = ChiTietNV.ep_address;
            tbInput.Text = ChiTietNV.st_bank;
            tbInput1.Text = ChiTietNV.st_stk;
            cbHonNhan.SelectedIndex = int.Parse(ChiTietNV.ep_married);
            cbNgaySinh.SelectedDate = DateTime.Parse(ChiTietNV.ep_birth_day);
            NgayBatDau.Text = DateTime.Parse(ChiTietNV.start_working_time).ToString("dd/MM/yyyy");
            cbGioiTinh.SelectedIndex = int.Parse(ChiTietNV.ep_gender);
            if (!string.IsNullOrEmpty(ChiTietNV.st_time))
                ChonThang.SelectedDate = DateTime.Parse(ChiTietNV.st_time);
            borderes2.Visibility =
                borderes2.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemLuong(Main, ChiTietNV, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemHopDong(Main, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemPhuThuoc(Main, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.PopupThemDongGop(Main, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ChonPhuThuoc(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            FamilyMember data = (FamilyMember)cb.DataContext;
            foreach (var item in ChiTietNV.family_member)
            {
                if (data.fa_id == item.fa_id)
                    item.fa_status = "1";
            }

            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }

                web.QueryString.Add("id_fa", data.fa_id);
                web.QueryString.Add("aname", data.fa_name);
                web.QueryString.Add("aphone", data.fa_phone);
                web.QueryString.Add("aadd", data.fa_address);
                web.QueryString.Add("ajob", data.fa_job);
                web.QueryString.Add("arela", data.fa_relation);
                web.QueryString.Add("adate", data.fa_birthday);
                web.QueryString.Add("checked_f", data.fa_status);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_ThemMoiPhucLoiPhuCap api =
                        JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(
                            UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_family_member.php",
                    web.QueryString);
            }
        }

        private void HuyPhuThuoc(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            FamilyMember data = (FamilyMember)cb.DataContext;
            foreach (var item in ChiTietNV.family_member)
            {
                if (data.fa_id == item.fa_id)
                    item.fa_status = "0";
            }

            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                }

                web.QueryString.Add("id_fa", data.fa_id);
                web.QueryString.Add("aname", data.fa_name);
                web.QueryString.Add("aphone", data.fa_phone);
                web.QueryString.Add("aadd", data.fa_address);
                web.QueryString.Add("ajob", data.fa_job);
                web.QueryString.Add("arela", data.fa_relation);
                web.QueryString.Add("adate", data.fa_birthday);
                web.QueryString.Add("checked_f", data.fa_status);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_ThemMoiPhucLoiPhuCap api =
                        JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(
                            UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_ep_family_member.php",
                    web.QueryString);
            }
        }

        private void SuaGioiThieu(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.Headers.Add("Authorization", Main.CurrentCompany.token);
                }

                web.QueryString.Add("ep_name", data1.ep_name);
                web.QueryString.Add("ep_phone", data1.ep_phone);
                web.QueryString.Add("ep_address", data1.ep_address);
                web.QueryString.Add("id_ep", data1.ep_id);
                web.QueryString.Add("description", txtGioiThieuSua.Text);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    API_SuaGioiThieu api =
                        JsonConvert.DeserializeObject<API_SuaGioiThieu>(UnicodeEncoding.UTF8.GetString(ee.Result));
                    if (api.data != null)
                    {
                        ChiTietNV.ep_description = txtGioiThieu.Text = txtGioiThieuSua.Text;
                        borderes.Visibility = Visibility.Collapsed;
                        txtGioiThieu.Visibility = Visibility.Visible;
                    }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/update_user_info_employee.php",
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
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }

                    web.QueryString.Add("name", txtSuaTen.Text);
                    web.QueryString.Add("phone", txtSuaPhone.Text);
                    web.QueryString.Add("address", txtSuaDiaChi.Text);
                    web.QueryString.Add("gender", cbGioiTinh.SelectedIndex + "");
                    web.QueryString.Add("date", cbNgaySinh.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    web.QueryString.Add("married", cbHonNhan.SelectedIndex + "");
                    web.QueryString.Add("id_ep", data1.ep_id);
                    if (ChonThang.SelectedDate != null)
                        web.QueryString.Add("fstime", ChonThang.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    else
                        web.QueryString.Add("fstime", "");
                    web.QueryString.Add("fsbank", tbInput.Text);
                    web.QueryString.Add("fsstk", tbInput1.Text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        string y = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_ThemMoiPhucLoiPhuCap api = JsonConvert.DeserializeObject<API_ThemMoiPhucLoiPhuCap>(y);
                        if (api.data != null)
                        {
                            Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(Main, data1.ep_id));
                            Main.title.Text = " Danh sách nhân viên/ Hồ sơ nhân viên";
                            Main.sidebar.SelectedIndex = -1;
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_employee.php",
                        web.QueryString);
                }
            }
        }

        private void SuaLuong(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            BasicSalary data = (BasicSalary)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupChinhSuaLuongCoBan(Main, data, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaLuong(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            BasicSalary data = (BasicSalary)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupThongBaoXoaLCB(Main, data.sb_id, 1, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void SuaHopDong(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ContractWork data = (ContractWork)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupSuaHopDong(Main, data, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaHopDong(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ContractWork data = (ContractWork)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupThongBaoXoaLCB(Main, data.con_id, 2, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaGiaDinh(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            FamilyMember data = (FamilyMember)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupThongBaoXoaLCB(Main, data.fa_id, 3, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void SuaPhuThuoc(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            FamilyMember data = (FamilyMember)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.TinhLuong.Popup.PopupSuaTPGD(Main, data, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XoaDongGop(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            Don data = (Don)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupThongBaoXoaLCB(Main, data.don_id, 4, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void SuaDongGop(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            Don data = (Don)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.TinhLuong.Popup.PopupSuaDongGop(Main, data, data1));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ThayLichLamViec(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.CaiDat.Popup.PopupChinhSuaLichLamViec(Main, ChiTietNV.cycle, data1);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}