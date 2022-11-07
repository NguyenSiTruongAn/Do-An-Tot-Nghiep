using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppTinhLuong365.Views.CaiDat
{
    /// <summary>
    /// Interaction logic for NhomLamViec.xaml
    /// </summary>
    public partial class NhomLamViec : Page, INotifyPropertyChanged
    {
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        MainWindow Main;
        public NhomLamViec(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            getData1(1);
            getData2(1);
            dataGrid.AutoReponsiveColumn(0);
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
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<ListGroup> _listNhom;

        public List<ListGroup> listNhom
        {
            get { return _listNhom; }
            set { _listNhom = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("id_group", "");
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_ListGroup api = JsonConvert.DeserializeObject<API_ListGroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNhom = api.data.list_group;
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_group_manager.php", web.QueryString);
            }
        }
        private List<ListEpNoGroup> _listNVChuaNhom = new List<ListEpNoGroup>();
        public List<ListEpNoGroup> listNVChuaNhom
        {
            get { return _listNVChuaNhom; }
            set
            {
                _listNVChuaNhom = value;
                OnPropertyChanged();
            }
        }

        private static int totalNVCacNhom;
        private static int totalNVChuaNhom;

        private List<ListEpNoGroup> _listNVChuaNhom1 = new List<ListEpNoGroup>();
        public List<ListEpNoGroup> listNVChuaNhom1
        {
            get { return _listNVChuaNhom1; }
            set
            {
                _listNVChuaNhom1 = value;
                OnPropertyChanged();
            }
        }

        private void getData1(int pageNB)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("page", pageNB + "");
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_ep_nogroup api = JsonConvert.DeserializeObject<API_List_ep_nogroup>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNVChuaNhom1 = listNVChuaNhom = api.data.list;
                        totalNVChuaNhom = int.Parse(api.data.total);
                        PageNVChuaNhom = ListPageNumber(totalNVChuaNhom);
                        loadPage_ChuaNhom(pageNB, PageNVChuaNhom);
                        dataGrid.AutoReponsiveColumn(0);

                    }
                    foreach (ListEpNoGroup item in listNVChuaNhom)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/tbl_list_emp_nogroup.php", web.QueryString);
            }
        }

        public List<int> ListPageNumber(int total)
        {
            int Pages = total / 20;
            if (total % 20 > 0) Pages++;
            List<int> pnb = new List<int>();
            for (int i = 1; i <= Pages; i++)
            {
                pnb.Add(i);
            }
            return pnb;
        }

        private List<int> _PageNVChuaNhom;

        public List<int> PageNVChuaNhom
        {
            get { return _PageNVChuaNhom; }
            set { _PageNVChuaNhom = value; OnPropertyChanged(); }
        }

        private List<int> _PageNVCacNhom;

        public List<int> PageNVCacNhom
        {
            get { return _PageNVCacNhom; }
            set { _PageNVCacNhom = value; OnPropertyChanged(); }
        }



        private List<EpGroup> _listNVCacNhom = new List<EpGroup>();
        public List<EpGroup> listNVCacNhom
        {
            get { return _listNVCacNhom; }
            set
            {
                _listNVCacNhom = value;
                OnPropertyChanged();
            }
        }
        private List<EpGroup> _listNVCacNhom1 = new List<EpGroup>();
        public List<EpGroup> listNVCacNhom1
        {
            get { return _listNVCacNhom1; }
            set
            {
                _listNVCacNhom1 = value;
                OnPropertyChanged();
            }
        }

        private void getData2(int page)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("company", Main.CurrentCompany.com_id);
                    web.QueryString.Add("page", page + "");
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    List_user_group api = JsonConvert.DeserializeObject<List_user_group>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listNVCacNhom1 = listNVCacNhom = api.data.ep_group;
                        totalNVCacNhom = api.data.count;
                        PageNVCacNhom = ListPageNumber(totalNVCacNhom);
                        loadPage(page, PageNVCacNhom);
                        dataGrid1.AutoReponsiveColumn(1);
                    }
                    foreach (EpGroup item in listNVCacNhom)
                    {
                        if (item.ep_image == "/img/add.png")
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                        }
                    }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_group.php", web.QueryString);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupNhomLamViec(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListGroup data = (ListGroup)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThemNhanVien(Main, data.lgr_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            ListGroup data = (ListGroup)p.DataContext;
            var pop = new Views.CaiDat.Popup.PopupTuyChonNhomLamViec(Main, data.lgr_id, data.lgr_name, data.lgr_note);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 205, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void BtnThietLapNVChuaNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ListEpNoGroup data = (ListEpNoGroup)b.DataContext;
            if (data != null)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThietLapNhomChoNhanVien(Main, data.ep_name, data.ep_id));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }

        }

        private void tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tb.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }

        private void NVChuaNhom_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListEpNoGroup selected = (ListEpNoGroup)(Search_NVChuaThietLap.SelectedItem);
            if (selected != null)
            {
                listNVChuaNhom1 = listNVChuaNhom.Where(x => x.ep_id == selected.ep_id).ToList();
            }
            else listNVChuaNhom1 = listNVChuaNhom;
        }
        private void Search_DSNVCacNhom(object sender, SelectionChangedEventArgs e)
        {

            EpGroup selected = (EpGroup)(Search_NVCacNhom.SelectedItem);
            if (selected != null)
            {
                listNVCacNhom1 = listNVCacNhom.Where(x => x.ep_id == selected.ep_id).ToList();
            }
            else listNVCacNhom1 = listNVCacNhom;
        }

        private void BtnThietLapNVCacNhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            EpGroup data = (EpGroup)b.DataContext;
            if (data != null)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupThietLapNhanVienCacNhom(Main, data.ep_name, data.ep_id, data.lgr_name));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }

        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage1.Text);
            getData2(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage2.Text);
            getData2(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage3.Text);
            getData2(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click1_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage1_chua_nhom.Text);
            getData1(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage2_chua_nhom.Text);
            getData1(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage3_chua_nhom.Text);
            getData1(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private int pagenow;

        private void loadPage(int pagenb, List<int> loaiPage)
        {
            BrushConverter bc = new BrushConverter();
            pagenow = pagenb;
            if (pagenb == 1)
            {
                Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage1.Text = "1";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                PageTruoc.Visibility = Visibility.Collapsed;
                PageDau.Visibility = Visibility.Collapsed;
                Page1.Visibility = Visibility.Visible;
                if (loaiPage.Count > 3)
                {
                    PageCuoi.Visibility = Visibility.Visible;
                    txtpage3.Text = "3";
                    txtpage2.Text = "2";
                    PageTiep.Visibility = Visibility.Visible;
                    Page3.Visibility = Visibility.Visible;
                    Page2.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 2)
                {
                    txtpage3.Text = "3";
                    txtpage2.Text = "2";
                    PageTiep.Visibility = Visibility.Visible;
                    Page3.Visibility = Visibility.Visible;
                    Page2.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 1)
                {
                    Page3.Visibility = Visibility.Collapsed;
                    PageTiep.Visibility = Visibility.Visible;
                    txtpage2.Text = "2";
                    Page2.Visibility = Visibility.Visible;
                    PageCuoi.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PageTiep.Visibility = Visibility.Collapsed;
                    Page3.Visibility = Visibility.Collapsed;
                    PageCuoi.Visibility = Visibility.Collapsed;
                    Page2.Visibility = Visibility.Collapsed;
                }
            }
            else if (pagenb == loaiPage.Count)
            {
                Page3.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage3.Text = pagenb + "";
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                Page3.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Collapsed;
                PageCuoi.Visibility = Visibility.Collapsed;
                if (loaiPage.Count > 3)
                {
                    txtpage2.Text = (pagenb - 1) + "";
                    txtpage1.Text = (pagenb - 2) + "";
                    Page2.Visibility = Visibility.Visible;
                    PageDau.Visibility = Visibility.Visible;
                    PageTruoc.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                }
                else if (loaiPage.Count > 2)
                {
                    txtpage2.Text = "2";
                    txtpage1.Text = "1";
                    Page2.Visibility = Visibility.Visible;
                    PageTruoc.Visibility = Visibility.Visible;
                    Page1.Visibility = Visibility.Visible;
                    PageDau.Visibility = Visibility.Collapsed;

                }
                else if (loaiPage.Count > 1)
                {
                    Page1.Visibility = Visibility.Collapsed;
                    txtpage2.Text = "1";
                    Page2.Visibility = Visibility.Visible;
                    PageTruoc.Visibility = Visibility.Visible;
                    PageDau.Visibility = Visibility.Collapsed;
                }
            }
            else if (pagenb == loaiPage.Count - 1)
            {
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage2.Text = pagenb + "";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Visible;
                PageCuoi.Visibility = Visibility.Collapsed;
                PageTruoc.Visibility = Visibility.Visible;
                Page3.Visibility = Visibility.Visible;
                Page1.Visibility = Visibility.Visible;
                txtpage3.Text = (pagenb + 1) + "";
                txtpage1.Text = (pagenb - 1) + "";
                if (loaiPage.Count > 3) PageDau.Visibility = Visibility.Visible;
                else PageDau.Visibility = Visibility.Collapsed;
            }
            else if (pagenb == 2)
            {
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage2.Text = "2";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage1.Text = "1";
                txtpage3.Text = "3";
                PageTruoc.Visibility = Visibility.Visible;
                PageDau.Visibility = Visibility.Collapsed;
                Page1.Visibility = Visibility.Visible;
                Page2.Visibility = Visibility.Visible;
                Page3.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Visible;
                if (loaiPage.Count > 3) PageCuoi.Visibility = Visibility.Visible;
                else PageCuoi.Visibility = Visibility.Collapsed;
            }
            else
            {
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                Page1.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                txtpage2.Text = pagenb + "";
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage1.Text = (pagenb - 1) + "";
                txtpage3.Text = (pagenb + 1) + "";
                PageTruoc.Visibility = Visibility.Visible;
                PageDau.Visibility = Visibility.Visible;
                PageCuoi.Visibility = Visibility.Visible;
                Page1.Visibility = Visibility.Visible;
                Page2.Visibility = Visibility.Visible;
                Page3.Visibility = Visibility.Visible;
                PageTiep.Visibility = Visibility.Visible;
            }
        }

        private void loadPage_ChuaNhom(int pagenb, List<int> loaiPage)
        {
            this.Dispatcher.Invoke(() =>
            {
                BrushConverter bc = new BrushConverter();
                pagenow = pagenb;
                if (pagenb == 1)
                {
                    Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                    Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    txtpage1_chua_nhom.Text = "1";
                    txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                    txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    PageTruoc_chua_nhom.Visibility = Visibility.Collapsed;
                    PageDau_chua_nhom.Visibility = Visibility.Collapsed;
                    Page1_chua_nhom.Visibility = Visibility.Visible;
                    if (loaiPage.Count > 3)
                    {
                        PageCuoi_chua_nhom.Visibility = Visibility.Visible;
                        txtpage3_chua_nhom.Text = "3";
                        txtpage2_chua_nhom.Text = "2";
                        PageTiep_chua_nhom.Visibility = Visibility.Visible;
                        Page3_chua_nhom.Visibility = Visibility.Visible;
                        Page2_chua_nhom.Visibility = Visibility.Visible;
                        Page1_chua_nhom.Visibility = Visibility.Visible;
                    }
                    else if (loaiPage.Count > 2)
                    {
                        txtpage3_chua_nhom.Text = "3";
                        txtpage2_chua_nhom.Text = "2";
                        PageTiep_chua_nhom.Visibility = Visibility.Visible;
                        Page3_chua_nhom.Visibility = Visibility.Visible;
                        Page2_chua_nhom.Visibility = Visibility.Visible;
                        Page1_chua_nhom.Visibility = Visibility.Visible;
                    }
                    else if (loaiPage.Count > 1)
                    {
                        Page3_chua_nhom.Visibility = Visibility.Collapsed;
                        PageTiep_chua_nhom.Visibility = Visibility.Visible;
                        txtpage2_chua_nhom.Text = "2";
                        Page2_chua_nhom.Visibility = Visibility.Visible;
                        PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        PageTiep_chua_nhom.Visibility = Visibility.Collapsed;
                        Page3_chua_nhom.Visibility = Visibility.Collapsed;
                        PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
                        Page2_chua_nhom.Visibility = Visibility.Collapsed;
                    }
                }
                else if (pagenb == loaiPage.Count)
                {
                    Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                    Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    txtpage3_chua_nhom.Text = pagenb + "";
                    txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                    txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    Page3_chua_nhom.Visibility = Visibility.Visible;
                    PageTiep_chua_nhom.Visibility = Visibility.Collapsed;
                    PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
                    if (loaiPage.Count > 3)
                    {
                        txtpage2_chua_nhom.Text = (pagenb - 1) + "";
                        txtpage1_chua_nhom.Text = (pagenb - 2) + "";
                        Page2_chua_nhom.Visibility = Visibility.Visible;
                        PageDau_chua_nhom.Visibility = Visibility.Visible;
                        PageTruoc_chua_nhom.Visibility = Visibility.Visible;
                        Page1_chua_nhom.Visibility = Visibility.Visible;
                    }
                    else if (loaiPage.Count > 2)
                    {
                        txtpage2_chua_nhom.Text = "2";
                        txtpage1_chua_nhom.Text = "1";
                        Page2_chua_nhom.Visibility = Visibility.Visible;
                        PageTruoc_chua_nhom.Visibility = Visibility.Visible;
                        Page1_chua_nhom.Visibility = Visibility.Visible;
                        PageDau_chua_nhom.Visibility = Visibility.Collapsed;

                    }
                    else if (loaiPage.Count > 1)
                    {
                        Page1_chua_nhom.Visibility = Visibility.Collapsed;
                        txtpage2_chua_nhom.Text = "1";
                        Page2_chua_nhom.Visibility = Visibility.Visible;
                        PageTruoc_chua_nhom.Visibility = Visibility.Visible;
                        PageDau_chua_nhom.Visibility = Visibility.Collapsed;
                    }
                }
                else if (pagenb == loaiPage.Count - 1)
                {
                    Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                    Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    txtpage2_chua_nhom.Text = pagenb + "";
                    txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                    txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    Page2_chua_nhom.Visibility = Visibility.Visible;
                    PageTiep_chua_nhom.Visibility = Visibility.Visible;
                    PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
                    PageTruoc_chua_nhom.Visibility = Visibility.Visible;
                    Page3_chua_nhom.Visibility = Visibility.Visible;
                    Page1_chua_nhom.Visibility = Visibility.Visible;
                    txtpage3_chua_nhom.Text = (pagenb + 1) + "";
                    txtpage1_chua_nhom.Text = (pagenb - 1) + "";
                    if (loaiPage.Count > 3) PageDau_chua_nhom.Visibility = Visibility.Visible;
                    else PageDau_chua_nhom.Visibility = Visibility.Collapsed;
                }
                else if (pagenb == 2)
                {
                    Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                    Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    txtpage2_chua_nhom.Text = "2";
                    txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                    txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage1_chua_nhom.Text = "1";
                    txtpage3_chua_nhom.Text = "3";
                    PageTruoc_chua_nhom.Visibility = Visibility.Visible;
                    PageDau_chua_nhom.Visibility = Visibility.Collapsed;
                    Page1_chua_nhom.Visibility = Visibility.Visible;
                    Page2_chua_nhom.Visibility = Visibility.Visible;
                    Page3_chua_nhom.Visibility = Visibility.Visible;
                    PageTiep_chua_nhom.Visibility = Visibility.Visible;
                    if (loaiPage.Count > 3) PageCuoi_chua_nhom.Visibility = Visibility.Visible;
                    else PageCuoi_chua_nhom.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Page2_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                    Page3_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    txtpage2_chua_nhom.Text = pagenb + "";
                    txtpage2_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                    txtpage3_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#444");
                    txtpage1_chua_nhom.Text = (pagenb - 1) + "";
                    txtpage3_chua_nhom.Text = (pagenb + 1) + "";
                    PageTruoc_chua_nhom.Visibility = Visibility.Visible;
                    PageDau_chua_nhom.Visibility = Visibility.Visible;
                    PageCuoi_chua_nhom.Visibility = Visibility.Visible;
                    Page1_chua_nhom.Visibility = Visibility.Visible;
                    Page2_chua_nhom.Visibility = Visibility.Visible;
                    Page3_chua_nhom.Visibility = Visibility.Visible;
                    PageTiep_chua_nhom.Visibility = Visibility.Visible;
                }
            });
        }

        private void ve_page_1(object sender, MouseButtonEventArgs e)
        {
            getData2(1);
            BrushConverter bc = new BrushConverter();
            Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            txtpage1.Text = "1";
            txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        }

        private void page_truoc_click(object sender, MouseButtonEventArgs e)
        {
            getData2(pagenow - 1);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
        {
            getData2(pagenow + 1);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
        {
            getData2(PageNVCacNhom.Count);
        }

        private void ve_page_1_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            getData1(1);
            BrushConverter bc = new BrushConverter();
            Page1_chua_nhom.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            txtpage1_chua_nhom.Text = "1";
            txtpage1_chua_nhom.Foreground = (Brush)bc.ConvertFrom("#ffffff");
            loadPage_ChuaNhom(1, PageNVChuaNhom);
        }

        private void page_truoc_click_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            getData1(pagenow - 1);
        }

        private void page_sau_click_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            getData1(pagenow + 1);
        }

        private void page_cuoi_click_chua_nhom(object sender, MouseButtonEventArgs e)
        {
            getData1(PageNVChuaNhom.Count);
        }
    }
}
