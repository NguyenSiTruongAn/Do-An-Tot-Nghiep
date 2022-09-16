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
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = workingArea.Right - 180;
            this.Height = workingArea.Bottom - 100;
            SideBarIndex = 0;
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
                        HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        title.Text = "Quản lý tài khoản nhân sự";
                        break;
                    case 1:
                        HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.HoSoNhanVien(this));
                        this.Title = App.Current.Resources["textNhapLuongCoBanVaCheDo"] as string;
                        title.Text = "Danh sách nhân viên";
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
                        HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.CacKhoanTienKhac(this));
                        this.Title = App.Current.Resources["textCacKhoanTienKhac"] as string;
                        title.Text = App.Current.Resources["textKhoanTienKhac"] as string;
                        break;
                    case 8:
                        HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.BaoHiem(this));
                        this.Title = App.Current.Resources["textBaoHiem"] as string;
                        title.Text = this.Title;
                        break;
                    case 9:
                        SideBarItems[10].Vis = SideBarItems[10].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[11].Vis = SideBarItems[11].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[12].Vis = SideBarItems[12].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
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
                        HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.BaoCaoCongLuong(this));
                        this.Title = App.Current.Resources["textBaoCaoCongLuong"] as string;
                        title.Text = this.Title;
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
                        Process.Start("https://vanthu.timviec365.vn/mau-de-xuat.html");
                        break;
                    case 22:
                        HomeSelectionPage.NavigationService.Navigate(new Views.PhanQuyen.PhanQuyen(this));
                        this.Title = App.Current.Resources["textPhanQuyenTaiKhoan"] as string;
                        title.Text = this.Title;
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
            this.Close();
        }

        private void Maximize(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) IsFull = 1;
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
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://tinhluong.timviec365.vn/huong-dan.html");
        }

        private void TextBlock_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://timviec365.vn/blog/c105/cac-van-de-ve-luong");
        }

        private void TextBlock_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.Home(this));
            this.Title = App.Current.Resources["textTrangChu"] as string;
            title.Text = "Quản lý tài khoản nhân sự";
            SideBarIndex = 0;
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
        }
    }
}
