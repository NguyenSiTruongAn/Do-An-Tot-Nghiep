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
using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupDSNVLichLamViec.xaml
    /// </summary>
    public partial class PopupDSNVLichLamViec : Page, INotifyPropertyChanged
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

        private string id;
        // private string cy_name;

        public PopupDSNVLichLamViec(MainWindow main, string Id, string name)
        {
            InitializeComponent();
            //dataGrid2.AutoReponsiveColumn(3);
            this.DataContext = this;
            Main = main;
            id = Id;
            Block1.Text = name;
            getData(1);
        }


        // public List<string> Test { get; set; } = new List<string>() { "aa", "bb", "cc" };

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private void dataGrid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scr.ScrollToVerticalOffset(scr.VerticalOffset - e.Delta);
        }

        private void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrolPopup.ScrollToVerticalOffset(Main.scrolPopup.VerticalOffset - e.Delta);
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

        private List<Item_emp> _listEmps;

        public List<Item_emp> listEmps
        {
            get { return _listEmps; }
            set
            {
                _listEmps = value;
                OnPropertyChanged();
            }
        }

        private static int totalEp;
        private void getData(int page)
        {
            using (WebClient web = new WebClient())
            {
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.QueryString.Add("filter_by[cy_id]", id);
                web.QueryString.Add("off_set", (page - 1) * 20 + "");
                web.QueryString.Add("length", "20");
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_emp_in_cycle_by_com_id api =
                        JsonConvert.DeserializeObject<API_List_emp_in_cycle_by_com_id>(
                            UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listEmps = api.data.items;
                            // if(listEmps != null && listEmps.Count >0)
                            //     Block1.Text = listEmps[0].cy_name;
                            totalEp = int.Parse(api.data.totalItems);
                            PageEp = ListPageNumber(totalEp);
                            loadPage(page, PageEp);
                            // dataGrid2.AutoReponsiveColumn(1);
                        }

                        foreach (Item_emp item in listEmps)
                        {
                            if (item.ep_image == "")
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                            else
                            {
                                item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_employee_cycle.php",
                    web.QueryString);
            }
        }

        private int pagenow;

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
            getData(PageEp.Count);
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

        private List<int> _PageEp;

        public List<int> PageEp
        {
            get { return _PageEp; }
            set
            {
                _PageEp = value;
                OnPropertyChanged();
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border p = sender as Border;
            Item_emp data = (Item_emp)p.DataContext;
            Main.PopupSelection.NavigationService.Navigate(
                new Views.CaiDat.Popup.PopupThongBaoXoaNhanVien(Main, id, data.ep_id, data.apply_month));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}