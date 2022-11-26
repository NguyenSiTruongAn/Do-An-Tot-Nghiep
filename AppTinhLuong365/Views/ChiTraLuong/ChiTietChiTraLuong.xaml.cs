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
using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.ChiTraLuong
{
    /// <summary>
    /// Interaction logic for ChiTietChiTraLuong.xaml
    /// </summary>
    public partial class ChiTietChiTraLuong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        public ChiTietChiTraLuong(MainWindow main, string pay_id)
        {
            this.DataContext = this;
            InitializeComponent();
            id = pay_id;
            Main = main;
            dataGrid.AutoReponsiveColumn(2);
            getData(1);
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

        public MainWindow Main;

        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
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

        private List<Item_detail_pay> _listItemDetailPay;

        public List<Item_detail_pay> listItemDetailPay
        {
            get { return _listItemDetailPay; }
            set
            {
                _listItemDetailPay = value;
                OnPropertyChanged();
            }
        }

        private Detail_pay _listDetailPay;

        public Detail_pay listDetailPay
        {
            get { return _listDetailPay; }
            set
            {
                _listDetailPay = value;
                OnPropertyChanged();
            }
        }

        private static int totalEpLate;

        private void getData(int page)
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("cp", Main.CurrentCompany.com_id);
                web.QueryString.Add("pid", id);
                web.QueryString.Add("page", page + "");
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Detail_pay api =
                        JsonConvert.DeserializeObject<API_Detail_pay>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listItemDetailPay = api.data.list;
                        listDetailPay = api.data;
                        totalEpLate = api.data.total;
                        PageNV = ListPageNumber(totalEpLate);
                        loadPage(page, PageNV);
                        DateTime aDateTime;
                        foreach (var a in listItemDetailPay)
                        {
                            DateTime.TryParse(a.pay_for_time, out aDateTime);
                            a.pay_for_time = aDateTime.ToString("MM/yyyy");
                        }
                    }
                    foreach (Item_detail_pay item in listItemDetailPay)
                    {
                        if (item.ep_image != "/img/add.png")
                        {
                            item.ep_image = item.ep_image;
                        }
                        else
                        {
                            item.ep_image = "https://tinhluong.timviec365.vn" + item.ep_image;
                        }
                    }
                    foreach (Item_detail_pay itemDetail in ItemDetail)
                    {
                        //itemDetail.PropertyChanged += EntryOnPropertyChanged;
                    }
                    loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/detail_pay.php",
                    web.QueryString);
            }
        }

        private int pagenow;

        private void loadPage(int pagenb, List<int> loaiPage)
        {
            BrushConverter bc = new BrushConverter();
            pagenow = pagenb;
            if (pagenb == 1)
            {
                Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page2.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page3.Background = (Brush)bc.ConvertFrom("#ffffff");
                txtpage1.Text = "1";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
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
                Page1.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page3.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                txtpage3.Text = pagenb + "";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#444");
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
                Page1.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Text = pagenb + "";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
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
                Page1.Background = (Brush)bc.ConvertFrom("#ffffff");
                Page2.Background = (Brush)bc.ConvertFrom("#4C5BD4");
                Page3.Background = (Brush)bc.ConvertFrom("#ffffff");
                txtpage2.Text = "2";
                txtpage1.Foreground = (Brush)bc.ConvertFrom("#444");
                txtpage2.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                txtpage3.Foreground = (Brush)bc.ConvertFrom("#444");
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

        private void ve_page_1(object sender, MouseButtonEventArgs e)
        {
            getData(1);
            BrushConverter bc = new BrushConverter();
            Page1.Background = (Brush)bc.ConvertFrom("#4C5BD4");
            txtpage1.Text = "1";
            txtpage1.Foreground = (Brush)bc.ConvertFrom("#ffffff");
        }

        private void page_truoc_click(object sender, MouseButtonEventArgs e)
        {
            getData(pagenow - 1);
        }

        private void page_sau_click(object sender, MouseButtonEventArgs e)
        {
            getData(pagenow + 1);
        }

        private void page_cuoi_click(object sender, MouseButtonEventArgs e)
        {
            getData(PageNV.Count);
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

        private void select_page_click1(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage1.Text);
            getData(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click2(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage2.Text);
            getData(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private void select_page_click3(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            int pagenumber = int.Parse(txtpage3.Text);
            getData(pagenumber);
            // b.Background = (Brush)bc.ConvertFrom("#4C5BD4");
        }

        private List<int> _PageNV;

        public List<int> PageNV
        {
            get { return _PageNV; }
            set { _PageNV = value; OnPropertyChanged(); }
        }

        private void Pay(object sender, MouseButtonEventArgs e)
        {
            List<string> nv = new List<string>();
            validateList.Text = "";
            foreach (var item in listItemDetailPay)
            {
                if (item.IsChecked == true)
                    nv.Add(item.ep_id);
            }
            bool allow = true;
            if (nv.Count <= 0)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.ChiTraLuong.PopupTb(Main));
                Main.PopupSelection.Visibility = Visibility.Visible;
                allow = false;
            }

            if (allow)
            {
                Main.PopupSelection.NavigationService.Navigate(new Views.ChiTraLuong.PopupCTL(Main, id, nv, pagenow));
                Main.PopupSelection.Visibility = Visibility.Visible;
            }
        }

        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){var scroll = dataGrid.GetFirstChildOfType<ScrollViewer>();scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);}else Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_detail_pay data = (Item_detail_pay)cb.DataContext;
            if (data.ep_id.Equals(""))
            {
                foreach (var item in listItemDetailPay)
                {
                    item.status = true;
                }
            }
            else data.status = true;
        }

        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Item_detail_pay data = (Item_detail_pay)cb.DataContext;
            if (data.ep_id.Equals(""))
            {
                foreach (var item in listItemDetailPay)
                {
                    item.status = false;
                }
            }
            else
            {
                data.status = false;
            }
        }

        private void ChangeStatus(object sender, MouseEventArgs e)
        {
            var pop = new Views.ChiTraLuong.PopupStatus(Main, id);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 980, z.Y - 540, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        public List<Item_detail_pay> ItemDetail
        {
            get => _listItemDetailPay;
            set
            {
                if (Equals(value, _listItemDetailPay)) return;
                _listItemDetailPay = value;
                OnPropertyChanged();
            }
        }

        private void EntryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // Only re-check if the IsChecked property changed
            if (args.PropertyName == nameof(Item_detail_pay.IsChecked))
                RecheckAllSelected();
        }

        private void AllSelectedChanged()
        {
            // Has this change been caused by some other change?
            // return so we don't mess things up
            try
            {
                // this can of course be simplified
                if (AllSelected == true)
                {
                    foreach (Item_detail_pay kommune in ItemDetail)
                        kommune.IsChecked = true;
                }
                else if (AllSelected == false)
                {
                    foreach (Item_detail_pay kommune in ItemDetail)
                        kommune.IsChecked = false;
                }
            }
            finally
            {
                _allSelectedChanging = false;
            }
        }

        private void RecheckAllSelected()
        {
            // Has this change been caused by some other change?
            // return so we don't mess things up
            if (_allSelectedChanging) return;

            try
            {
                _allSelectedChanging = true;

                if (ItemDetail.All(e => e.IsChecked))
                    AllSelected = true;
                else if (ItemDetail.All(e => !e.IsChecked))
                    AllSelected = false;
                else
                    AllSelected = false;
            }
            finally
            {
                _allSelectedChanging = false;
            }
        }

        public bool AllSelected
        {
            get => _allSelected;
            set
            {
                _allSelected = value;
                // Set all other CheckBoxes
                AllSelectedChanged();
                OnPropertyChanged();
            }
        }

        private bool _allSelectedChanging;
        private bool _allSelected=false;

        private bool _SelectedAll;

        public bool SelectedAll
        {
            get { return _SelectedAll; }
            set { _SelectedAll = value;OnPropertyChanged(); }
        }


        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox c=sender as CheckBox;
            if (c!=null)
            {
                foreach (Item_detail_pay kommune in ItemDetail) kommune.IsChecked = c.IsChecked.Value;
            }
        }

        private void itemCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
            {
                bool allow = true;
                foreach (var item in listItemDetailPay)
                {
                    if (item.IsChecked!=c.IsChecked.Value)
                    {
                        allow = false;
                        break;
                    }
                }
                if(allow && c.IsChecked.Value) SelectedAll = true;
                else if (!allow && !c.IsChecked.Value) SelectedAll = false;
            }
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTraLuong(Main));
            Main.SideBarIndex = 13;
        }
    }
}
