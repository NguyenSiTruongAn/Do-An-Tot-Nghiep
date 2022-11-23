using AppTinhLuong365.Model.APIEntity;
using AppTinhLuong365.Views.DuLieuTinhLuong;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace AppTinhLuong365
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _IsFull = 0;

        public int IsFull
        {
            get { return _IsFull; }
            set
            {
                _IsFull = value;
                var workingArea = System.Windows.SystemParameters.WorkArea;
                switch (IsFull)
                {
                    case 0:
                        this.WindowState = WindowState.Normal;
                        Width = workingArea.Right - 180;
                        Height = workingArea.Bottom - 100;
                        Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                        Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
                        this.ResizeMode = ResizeMode.CanResize;
                        break;
                    case 1:
                        this.WindowState = WindowState.Normal;
                        Left = workingArea.TopLeft.X;
                        Top = workingArea.TopLeft.Y;
                        Width = workingArea.Width;
                        Height = workingArea.Height;
                        this.ResizeMode = ResizeMode.NoResize;
                        break;
                    default:
                        break;
                }

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow(API_Login_Company api)
        {
            InitializeComponent();
            this.DataContext = this;
            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = workingArea.Right - 180;
            this.Height = workingArea.Bottom - 100;
            CurrentCompany = api.data;
            MainType = 0;
            SideBarIndex = 0;
            sidebar.Visibility = Visibility.Visible;
            sidebarNV.Visibility = Visibility.Collapsed;
            tblUserName.Text = CurrentCompany.com_name;
            CurrentImage = "https://chamcong.24hpay.vn/upload/company/logo/" + CurrentCompany.com_logo;
            if (string.IsNullOrEmpty(CurrentCompany.com_logo))
            {
                CurrentImage = "https://quanlychung.timviec365.vn/img/logo_com.png";
            }
            MainType = 0;
            getData();
        }

        public MainWindow(API_Login_Employee api)
        {
            InitializeComponent();
            this.DataContext = this;
            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = workingArea.Right - 180;
            this.Height = workingArea.Bottom - 100;
            CurrentEmployee = api.data;
            MainType = 1;
            SideBarIndexNV = 0;
            sidebar.Visibility = Visibility.Collapsed;
            sidebarNV.Visibility = Visibility.Visible;
            tblUserName.Text = CurrentEmployee.ep_name;
            CurrentImage = "https://chamcong.24hpay.vn/upload/employee/"+CurrentEmployee.ep_image;
            getData();
        }
        private int _sotb = 0;
        public int sotb
        {
            get
            {
                return _sotb;
            }
            set
            {
                _sotb = value;
                OnPropertyChanged();
                if(MainType == 1)
                {
                    if (string.IsNullOrEmpty(CurrentEmployee.ep_image) || CurrentEmployee.ep_image == "/img/add.png" || CurrentEmployee.ep_image == "")
                    {
                        CurrentImage = "https://tinhluong.timviec365.vn/img/add.png";
                    }
                    else
                    {
                        CurrentImage = "https://chamcong.24hpay.vn/upload/employee/" + CurrentEmployee.ep_image;
                    }
                }
                
            }
        }

        public DataLogin_Company CurrentCompany { get; set; }
        public DataLogin_Employee CurrentEmployee { get; set; }
        public int MainType { get; set; }
        private string currentImage;

        public string CurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                OnPropertyChanged();
            }
        }

        public CacKhoanTienKhac pageCacKhoanTienKhac;
        public Action LogOut { get; set; }
        public class SideBarItemCom : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public object Icon { get; set; }
            public string Name { get; set; }
            public bool Child { get; set; }
            public bool HadSubMenu { get; set; } = false;

            private Visibility _Vis = Visibility.Visible;

            public Visibility Vis
            {
                get { return _Vis; }
                set
                {
                    _Vis = value;
                    OnPropertyChanged();
                }
            }

            public int typePath { get; set; } = 0;
        }

        private List<SideBarItemCom> _SideBarItems = new List<SideBarItemCom>()
        {
            new SideBarItemCom()
                { Icon = App.Current.Resources["iconHome"], Name = App.Current.Resources["textTrangChu"] as string },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconNhapLuongCoBanVaCheDo"],
                Name = App.Current.Resources["textNhapLuongCoBanVaCheDo"] as string
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconDuLieuTinhLuong"],
                Name = App.Current.Resources["textDuLieuTinhLuong"] as string, typePath = 1
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textChamCong"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textThuongPhat"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textPhucLoi"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textHoaHong"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"],
                Name = App.Current.Resources["textCacKhoanTienKhac"] as string, Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textBaoHiem"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconTinhLuong"], Name = App.Current.Resources["textSalary"] as string,
                typePath = 1
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textBangLuong"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textTamUng"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textThue"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconChiTraLuong"],
                Name = App.Current.Resources["textChiTraLuong"] as string, typePath = 1
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconBaoCaoCongLuong"],
                Name = App.Current.Resources["textBaoCaoCongLuong"] as string, typePath = 1
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconCaiDat"], Name = App.Current.Resources["textCaiDat"] as string,
                typePath = 1
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textNhomLamViec"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"],
                Name = App.Current.Resources["textCaiCaVaLichLamViec"] as string, Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textDiMuonVeSom"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textNghiPhep"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textNgayNghiLe"] as string,
                Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconSubMenu"],
                Name = App.Current.Resources["textBieuMauDeXuat"] as string, Vis = Visibility.Collapsed
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconPhanQuyen"], Name = App.Current.Resources["textPhanQuyen"] as string
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconChuyenDoiSo"],
                Name = App.Current.Resources["textDigitalConversion"] as string, typePath = 1
            },
        };

        public List<SideBarItemCom> SideBarItems
        {
            get { return _SideBarItems; }
            set
            {
                _SideBarItems = value;
                OnPropertyChanged();
            }
        }

        private List<SideBarItemCom> _SideBarItemsNV = new List<SideBarItemCom>()
        {
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconHome"], Name = App.Current.Resources["textAccManagement"] as string
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconNhapLuongCoBanVaCheDo"],
                Name = App.Current.Resources["textProfile"] as string
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconDuLieuTinhLuong"],
                Name = App.Current.Resources["textSalary"] as string, typePath = 1
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconCalendar"], Name = App.Current.Resources["textChamCong"] as string
            },
            new SideBarItemCom()
                { Icon = App.Current.Resources["iconDeXuat"], Name = App.Current.Resources["textOffer"] as string },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconCalendar"], Name = App.Current.Resources["textCalendar"] as string
            },
            new SideBarItemCom()
            {
                Icon = App.Current.Resources["iconChuyenDoiSo"],
                Name = App.Current.Resources["textDigitalConversion"] as string, typePath = 1
            },
        };

        public List<SideBarItemCom> SideBarItemsNV
        {
            get { return _SideBarItemsNV; }
            set
            {
                _SideBarItemsNV = value;
                OnPropertyChanged();
            }
        }

        private bool _OpenSubMenu;

        public bool OpenSubMenu
        {
            get { return _OpenSubMenu; }
            set
            {
                _OpenSubMenu = value;
                OnPropertyChanged();
            }
        }


        private int _SideBarIndex = 0;

        public int SideBarIndex
        {
            get { return _SideBarIndex; }
            set
            {
                if (this.CurrentCompany != null || this.CurrentEmployee != null)
                    switch (value)
                    {
                        case -1:
                            SideBarItems[3].Vis = Visibility.Collapsed;
                            SideBarItems[4].Vis = Visibility.Collapsed;
                            SideBarItems[5].Vis = Visibility.Collapsed;
                            SideBarItems[6].Vis = Visibility.Collapsed;
                            SideBarItems[7].Vis = Visibility.Collapsed;
                            SideBarItems[8].Vis = Visibility.Collapsed;
                            SideBarItems[10].Vis = Visibility.Collapsed;
                            SideBarItems[11].Vis = Visibility.Collapsed;
                            SideBarItems[12].Vis = Visibility.Collapsed;
                            SideBarItems[16].Vis = Visibility.Collapsed;
                            SideBarItems[17].Vis = Visibility.Collapsed;
                            SideBarItems[18].Vis = Visibility.Collapsed;
                            SideBarItems[19].Vis = Visibility.Collapsed;
                            SideBarItems[20].Vis = Visibility.Collapsed;
                            SideBarItems[21].Vis = Visibility.Collapsed;
                            OpenSubMenu = false;
                            break;
                        case 0:
                            HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(this));
                            this.Title = App.Current.Resources["textTrangChu"] as string;
                            title.Text = "Quản lý tài khoản nhân sự";
                            break;
                        case 1:
                            HomeSelectionPage.NavigationService.Navigate(new Views.PageNhapLuongCoBanVaCheDo(this));
                            this.Title = App.Current.Resources["textNhapLuongCoBanVaCheDo"] as string;
                            title.Text = "Danh sách nhân viên";
                            break;
                        case 2:
                            SideBarItems[3].Vis = SideBarItems[3].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[4].Vis = SideBarItems[4].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[5].Vis = SideBarItems[5].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[6].Vis = SideBarItems[6].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[7].Vis = SideBarItems[7].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[8].Vis = SideBarItems[8].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            OpenSubMenu = true;
                            break;
                        case 3:
                            HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.ChamCong(this));
                            this.Title = App.Current.Resources["textChamCongNhanVien"] as string;
                            title.Text = this.Title;
                            break;
                        case 4:
                            HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.ThuongPhat(this));
                            this.Title = App.Current.Resources["textThuongPhat"] as string;
                            title.Text = this.Title;
                            break;
                        case 5:
                            HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(this));
                            this.Title = App.Current.Resources["textPhucLoi"] as string;
                            title.Text = App.Current.Resources["textPhucLoiVaPhuCap"] as string;
                            break;
                        case 6:
                            HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(this));
                            this.Title = App.Current.Resources["textHoaHong"] as string;
                            title.Text = this.Title;
                            break;
                        case 7:
                            if (pageCacKhoanTienKhac == null)
                            {
                                pageCacKhoanTienKhac = new Views.DuLieuTinhLuong.CacKhoanTienKhac(this);
                            }

                            HomeSelectionPage.NavigationService.Navigate(pageCacKhoanTienKhac);
                            this.Title = App.Current.Resources["textCacKhoanTienKhac"] as string;
                            title.Text = App.Current.Resources["textKhoanTienKhac"] as string;
                            break;
                        case 8:
                            HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(this));
                            this.Title = App.Current.Resources["textBaoHiem"] as string;
                            title.Text = this.Title;
                            break;
                        case 9:
                            SideBarItems[10].Vis = SideBarItems[10].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[11].Vis = SideBarItems[11].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[12].Vis = SideBarItems[12].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            OpenSubMenu = true;
                            break;
                        case 10:
                            HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.BangLuong(this));
                            this.Title = App.Current.Resources["textBangLuongNhanVien"] as string;
                            title.Text = this.Title;
                            break;
                        case 11:
                            HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.TamUng(this));
                            this.Title = App.Current.Resources["textTamUng"] as string;
                            title.Text = this.Title;
                            break;
                        case 12:
                            HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.Thue(this));
                            this.Title = App.Current.Resources["textThue"] as string;
                            title.Text = this.Title;
                            break;
                        case 13:
                            HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTraLuong(this));
                            this.Title = App.Current.Resources["textChiTraLuong"] as string;
                            title.Text = this.Title;
                            break;
                        case 14:
                            HomeSelectionPage.NavigationService.Navigate(
                                new Views.BaoCaoCongLuong.BaoCaoCongLuong(this));
                            this.Title = App.Current.Resources["textBaoCaoCongLuong"] as string;
                            title.Text = this.Title;
                            break;
                        case 15:
                            SideBarItems[16].Vis = SideBarItems[16].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[17].Vis = SideBarItems[17].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[18].Vis = SideBarItems[18].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[19].Vis = SideBarItems[19].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[20].Vis = SideBarItems[20].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            SideBarItems[21].Vis = SideBarItems[21].Vis == Visibility.Visible
                                ? Visibility.Collapsed
                                : Visibility.Visible;
                            OpenSubMenu = true;
                            break;
                        case 16:
                            HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(this));
                            this.Title = App.Current.Resources["textNhomLamViec"] as string;
                            title.Text = this.Title;
                            break;
                        case 17:
                            HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(this));
                            this.Title = App.Current.Resources["textCalendar"] as string;
                            title.Text = this.Title;
                            break;
                        case 18:
                            HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.DiMuonVeSom(this));
                            this.Title = App.Current.Resources["textDiMuonVeSom"] as string;
                            title.Text = this.Title;
                            break;
                        case 19:
                            HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiPhep(this));
                            this.Title = App.Current.Resources["textNghiPhep"] as string;
                            title.Text = this.Title;
                            break;
                        case 20:
                            HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiLe(this));
                            this.Title = App.Current.Resources["textNghiLe"] as string;
                            title.Text = this.Title;
                            break;
                        case 21:
                            Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + CurrentCompany.com_id + "." + CurrentCompany.pass + "&link=https://vanthu.timviec365.vn/mau-de-xuat.html");
                            break;
                        case 22:
                            HomeSelectionPage.NavigationService.Navigate(new Views.PhanQuyen.PhanQuyen(this));
                            this.Title = App.Current.Resources["textPhanQuyenTaiKhoan"] as string;
                            title.Text = this.Title;
                            break;
                        case 23:
                            Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + CurrentCompany.com_id + "." + CurrentCompany.pass + "&link=https://quanlychung.timviec365.vn/quan-ly-ung-dung-cong-ty.html");
                            break;
                        default:
                            break;
                    }

                var z = new List<int>()
                    { -1, 0, 1, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 16, 17, 18, 19, 20, 21, 22, 23 };
                if (z.Contains(value)) _SideBarIndex = value;
                else if (value != 5)
                {
                    var h = sidebar.SelectedIndex;
                    if (h > -1)
                    {
                        SideBarIndex = h;
                    }
                }

                OnPropertyChanged();
            }
        }

        private int _SideBarIndexNV = 0;

        public int SideBarIndexNV
        {
            get { return _SideBarIndexNV; }
            set
            {
                switch (value)
                {
                    case 0:
                        HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.Home(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        title.Text = "Quản lý tài khoản";
                        break;
                    case 1:
                        HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.ProFile(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        title.Text = "Hồ sơ cá nhân";
                        break;
                    case 2:
                        HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.payroll(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        title.Text = "Bảng lương";
                        break;
                    case 3:
                        HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.TimeKeeping(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        title.Text = "Chấm công";
                        break;
                    case 4:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + CurrentEmployee.ep_id + "." + CurrentEmployee.pass + "&link=https://vanthu.timviec365.vn/trang-quan-ly-de-xuat.html");
                        break;
                    case 5:
                        HomeSelectionPage.NavigationService.Navigate(new Views.NhanVien.Calendar(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        title.Text = "Lịch làm việc";
                        break;
                    case 6:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + CurrentEmployee.ep_id + "." + CurrentEmployee.pass + "&link=https://quanlychung.timviec365.vn/quan-ly-ung-dung-nhan-vien.html");
                        break;
                    default:
                        break;
                }

                var z = new List<int>() { 0, 1, 2, 3, 5 };
                if (z.Contains(value)) _SideBarIndexNV = value;
                else if (value != 5)
                {
                    var h = sidebarNV.SelectedIndex;
                    if (h > -1)
                    {
                        SideBarIndexNV = h;
                    }
                }

                OnPropertyChanged();
            }
        }

        private List<ThongBaoCT> _listTB;

        public List<ThongBaoCT> listTB
        {
            get { return _listTB; }
            set { _listTB = value; OnPropertyChanged(); }
        }

        private int _fontsize = 14;
        public int fontsize
        {
            get
            {
                return _fontsize;
            }
            set
            {
                _fontsize = value;
                OnPropertyChanged();
            }
        }

        private Thickness _margin = new Thickness(12.5, -10.5, 0, 0);
        public Thickness margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if(MainType == 0)
                {
                    web.QueryString.Add("id_comp", CurrentCompany.com_id);
                    web.QueryString.Add("token", CurrentCompany.token);
                    web.QueryString.Add("cp", "2");
                }
                if(MainType == 1)
                {
                        web.QueryString.Add("id_comp", CurrentEmployee.com_id);
                        web.QueryString.Add("token", CurrentEmployee.token);
                        web.QueryString.Add("cp", "1");
                        web.QueryString.Add("user_nhan", CurrentEmployee.ep_id);
                }
                
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ThongBaoCT api = JsonConvert.DeserializeObject<API_ThongBaoCT>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listTB = api.data.abc;
                        if (listTB != null)
                            sotb = listTB.Count;
                        if (sotb >= 10)
                        {
                            fontsize = 10;
                            margin = new Thickness(10, -7, 0, 0);
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) this.DragMove();
        }

        private void Minimimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
            Application.Current.Shutdown();
        }

        private void Maximize(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) IsFull = 1;
            if (this.ActualWidth < 1250)
            {
                textTitle.Visibility = Visibility.Collapsed;
                if (this.ActualWidth < 1025)
                {
                    gridTop.Visibility = Visibility.Collapsed;
                    sidebar.Visibility = Visibility.Collapsed;
                    column1.Width = new GridLength(0);
                    gridTop1.Visibility = Visibility.Visible;
                }
                else
                {
                    gridTop.Visibility = Visibility.Visible;
                    sidebar.Visibility = Visibility.Visible;
                    column1.Width = new GridLength(300);
                    gridTop1.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                textTitle.Visibility = Visibility.Visible;
                gridTop.Visibility = Visibility.Visible;
                gridTop1.Visibility = Visibility.Collapsed;
                sidebar.Visibility = Visibility.Visible;
                column1.Width = new GridLength(300);
            }
        }

        private void HomeSelectionPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PopupSelection.Visibility == Visibility.Collapsed)
            {
                PopupSelection.NavigationService.Navigate(null);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(this));
            this.Title = App.Current.Resources["textTrangChu"] as string;
            SideBarIndex = 0;
            sidebar.SelectedIndex = 0;
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if(MainType == 1)
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + CurrentEmployee.ep_id + "." + CurrentEmployee.pass + "&link=https://tinhluong.timviec365.vn/huong-dan.html");
            else
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + CurrentCompany.com_id + "." + CurrentCompany.pass + "&link=https://tinhluong.timviec365.vn/huong-dan.html");

        }

        private void TextBlock_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            if (MainType == 1)
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + CurrentEmployee.ep_id + "." + CurrentEmployee.pass + "&link=https://timviec365.vn/blog/c105/cac-van-de-ve-luong");
            else
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + CurrentCompany.com_id + "." + CurrentCompany.pass + "&link=https://timviec365.vn/blog/c105/cac-van-de-ve-luong");
        }

        private void TextBlock_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(this));
            this.Title = App.Current.Resources["textTrangChu"] as string;
            title.Text = "Quản lý tài khoản nhân sự";
            SideBarIndex = 0;
            sidebar.SelectedIndex = 0;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TrangChu.PopupTaiKhoan(this);
            pop.Width = 200;
            pop.Height = 193;
            pop.HorizontalAlignment = HorizontalAlignment.Right;
            pop.VerticalAlignment = VerticalAlignment.Top;
            pop.Margin = new Thickness(0, 70, 20, 0);
            this.PopupSelection.NavigationService.Navigate(pop);
            this.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ClickPutSidePopup(object sender, MouseButtonEventArgs e)
        {
            this.PopupSelection.NavigationService.Navigate(null);
            this.PopupSelection.Visibility = Visibility.Hidden;
            this.HomeSelectionPage.Focus();
        }

        private void btn_OpenMenu_Click(object sender, MouseButtonEventArgs e)
        {
            this.PopupSelection.NavigationService.Navigate(new Views.Popup.PopupSideBar(this));
            this.PopupSelection.Visibility = Visibility.Visible;
        }

        private void XemThongBao(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TrangChu.popup.PopupThongBao(this);
            this.PopupSelection.NavigationService.Navigate(pop);
            this.PopupSelection.Visibility = Visibility.Visible;
            var z = Mouse.GetPosition(this.PopupSelection);
            pop.Margin = new Thickness(z.X - 448, z.Y + 30, 0, 0);
        }
    }
}