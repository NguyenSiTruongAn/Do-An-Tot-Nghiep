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

        }
        private List<SideBarItemCom> _SideBarItems = new List<SideBarItemCom>() {
                new SideBarItemCom(){ Icon=App.Current.Resources["iconHome"],Name=App.Current.Resources["textTrangChu"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["LogoChamCong365"],Name=App.Current.Resources["textChamCong"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconCauHinhChamCong"],Name=App.Current.Resources["textCauHinhChamCong"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["LogoChamCong365"],Name=App.Current.Resources["textCapNhatMat"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconCongCong"],Name=App.Current.Resources["textCongCong"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconQuanLyCongTy"],Name=App.Current.Resources["textQuanLyCongTy"] as string,HadSubMenu=true},
                new SideBarItemCom() { Icon = null, Name = App.Current.Resources["textQuanLyCaLamViec"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = null, Name = App.Current.Resources["textQuanLyCongTyCon"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = null, Name = App.Current.Resources["textCauTrucPB"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom() { Icon = null, Name = App.Current.Resources["textQuyenTruyCap"] as string,Vis=Visibility.Collapsed },
                new SideBarItemCom() { Icon = null, Name = App.Current.Resources["textDiMuon"] as string ,Vis=Visibility.Collapsed},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconQuanLyNhanVien"],Name=App.Current.Resources["textQuanLyNhanVien"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconLichSuDiemDanhNV"],Name=App.Current.Resources["textLichSuDiemDanh"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconLich"],Name=App.Current.Resources["textLichSuLV"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconCongCong"],Name=App.Current.Resources["textTKXuat"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconXuatCong"],Name=App.Current.Resources["textXuatCong"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconPQNgDung"],Name=App.Current.Resources["textPQNguoiDung"] as string},
                new SideBarItemCom(){ Icon=App.Current.Resources["iconChuyenDoiSo"],Name=App.Current.Resources["textChuyenDoiSo"] as string},
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
                    case 0:
                        HomeSelectionPage.NavigationService.Navigate(new Views.TrangChu.TrangChu(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        //SideBarItems[6].Vis = SideBarItems[6].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        //SideBarItems[7].Vis = SideBarItems[7].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        //SideBarItems[8].Vis = SideBarItems[8].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        //SideBarItems[9].Vis = SideBarItems[9].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        //SideBarItems[10].Vis = SideBarItems[10].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        //OpenSubMenu = true;
                        //break;
                    case 6:
                        
                        break;
                    case 7:
                        
                        break;
                    case 8:
                        
                        break;
                    case 9:
                        
                        break;
                    case 10:
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    case 13:
                        //Process.Start("https://tinhluong.timviec365.vn/quan-ly-lich-lam-viec.html");
                        break;
                    case 14:
                        break;
                    case 15:
                        break;
                    case 16:
                        break;
                    case 17:
                        break;
                    default:
                        HomeSelectionPage.NavigationService.Navigate(null);
                        break;
                }
                var z = new List<int>() { 1, 5, 10, 13, 17 };
                if (!z.Contains(value)) _SideBarIndex = value;
                else if (value != 5)
                {
                    var h = sidebar.SelectedIndex;
                    sidebar.UnselectAll();
                    sidebar.SelectedIndex = h;
                }
                OnPropertyChanged("SideBarIndex");
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
    }
}
