using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

namespace AppTinhLuong365.Views.Popup
{
    /// <summary>
    /// Interaction logic for PopupSideBar.xaml
    /// </summary>
    public partial class PopupSideBar : Page
    {
        MainWindow Main;
        public PopupSideBar(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                get
                {
                    return _Vis;
                }
                set
                {
                    _Vis = value;
                    OnPropertyChanged();
                }
            }

            public int typePath { get; set; } = 0;
        }
        private List<SideBarItemCom> _SideBarItems = new List<SideBarItemCom>() {
                new SideBarItemCom(){ Icon=App.Current.Resources["iconHome"],Name=App.Current.Resources["textTrangChu"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconNhapLuongCoBanVaCheDo"],Name=App.Current.Resources["textNhapLuongCoBanVaCheDo"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconDuLieuTinhLuong"],Name=App.Current.Resources["textDuLieuTinhLuong"] as string, typePath =1},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textChamCong"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textThuongPhat"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textPhucLoi"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textHoaHong"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textCacKhoanTienKhac"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textBaoHiem"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconTinhLuong"],Name=App.Current.Resources["textSalary"] as string,typePath=1},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textBangLuong"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textTamUng"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textThue"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconChiTraLuong"],Name=App.Current.Resources["textChiTraLuong"] as string, typePath = 1},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconBaoCaoCongLuong"],Name=App.Current.Resources["textBaoCaoCongLuong"] as string, typePath = 1},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconCaiDat"],Name=App.Current.Resources["textCaiDat"] as string, typePath = 1},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textNhomLamViec"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textCaiCaVaLichLamViec"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textDiMuonVeSom"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textNghiPhep"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textNgayNghiLe"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom() { Icon = App.Current.Resources["iconSubMenu"], Name = App.Current.Resources["textBieuMauDeXuat"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconPhanQuyen"],Name=App.Current.Resources["textPhanQuyen"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconChuyenDoiSo"],Name=App.Current.Resources["textDigitalConversion"] as string, typePath =1},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconLogout"],Name=App.Current.Resources["textDangXuat"] as string},
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

        private bool _OpenSubMenu;
        public bool OpenSubMenu
        {
            get { return _OpenSubMenu; }
            set { _OpenSubMenu = value; OnPropertyChanged(); }
        }


        private int _SideBarIndex = 0;
        public int SideBarIndex
        {
            get { return _SideBarIndex; }
            set
            {
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
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(Main));
                        Main.Title = App.Current.Resources["textTrangChu"] as string;
                        Main.title.Text = "Quản lý tài khoản nhân sự";
                        break;
                    case 1:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.PageNhapLuongCoBanVaCheDo(Main));
                        this.Title = App.Current.Resources["textNhapLuongCoBanVaCheDo"] as string;
                        Main.title.Text = "Danh sách nhân viên";
                        break;
                    case 2:
                        SideBarItems[3].Vis = SideBarItems[3].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[4].Vis = SideBarItems[4].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[5].Vis = SideBarItems[5].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[6].Vis = SideBarItems[6].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[7].Vis = SideBarItems[7].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[8].Vis = SideBarItems[8].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        OpenSubMenu = true;
                        break;
                    case 3:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.ChamCong(Main));
                        Main.Title = App.Current.Resources["textChamCongNhanVien"] as string;
                        Main.title.Text = this.Title;
                        break;
                    case 4:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.ThuongPhat(Main));
                        Main.Title = App.Current.Resources["textThuongPhat"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 5:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                        Main.Title = App.Current.Resources["textPhucLoi"] as string;
                        Main.title.Text = App.Current.Resources["textPhucLoiVaPhuCap"] as string;
                        break;
                    case 6:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHong(Main));
                        Main.Title = App.Current.Resources["textHoaHong"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 7:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(Main));
                        Main.Title = App.Current.Resources["textCacKhoanTienKhac"] as string;
                        Main.title.Text = App.Current.Resources["textKhoanTienKhac"] as string;
                        break;
                    case 8:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(Main));
                        Main.Title = App.Current.Resources["textBaoHiem"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 9:
                        SideBarItems[10].Vis = SideBarItems[10].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[11].Vis = SideBarItems[11].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[12].Vis = SideBarItems[12].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        OpenSubMenu = true;
                        break;
                    case 10:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.BangLuong(Main));
                        Main.Title = App.Current.Resources["textBangLuongNhanVien"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 11:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.TamUng(Main));
                        this.Title = App.Current.Resources["textTamUng"] as string;
                        Main.title.Text = this.Title;
                        break;
                    case 12:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.Thue(Main));
                        Main.Title = App.Current.Resources["textThue"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 13:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTraLuong(Main));
                        Main.Title = App.Current.Resources["textChiTraLuong"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 14:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.BaoCaoCongLuong(Main));
                        Main.Title = App.Current.Resources["textBaoCaoCongLuong"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 15:
                        SideBarItems[16].Vis = SideBarItems[16].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[17].Vis = SideBarItems[17].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[18].Vis = SideBarItems[18].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[19].Vis = SideBarItems[19].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[20].Vis = SideBarItems[20].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[21].Vis = SideBarItems[21].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        OpenSubMenu = true;
                        break;
                    case 16:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NhomLamViec(Main));
                        Main.Title = App.Current.Resources["textNhomLamViec"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 17:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.CaiCaVaLichLamViec(Main));
                        Main.Title = App.Current.Resources["textCalendar"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 18:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.DiMuonVeSom(Main));
                        Main.Title = App.Current.Resources["textDiMuonVeSom"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 19:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiPhep(Main));
                        Main.Title = App.Current.Resources["textNghiPhep"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 20:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.NghiLe(Main));
                        Main.Title = App.Current.Resources["textNghiLe"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 21:
                        Process.Start("https://vanthu.timviec365.vn/mau-de-xuat.html");
                        break;
                    case 22:
                        Main.HomeSelectionPage.NavigationService.Navigate(new Views.PhanQuyen.PhanQuyen(Main));
                        Main.Title = App.Current.Resources["textPhanQuyenTaiKhoan"] as string;
                        Main.title.Text = Main.Title;
                        break;
                    case 23:
                        Process.Start("https://quanlychung.timviec365.vn/quan-ly-ung-dung-cong-ty.html");
                        break;
                    default:
                        break;
                }
                var z = new List<int>() { -1, 0, 1, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 16, 17, 18, 19, 20, 21, 22, 23 };
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
    }
}
