using AppTinhLuong365.Model.APIEntity;
using Microsoft.Win32;
using Newtonsoft.Json;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
//using GroupDocs.Conversion.Options.Convert;
using System.Net.Http;

namespace AppTinhLuong365.Views.DuLieuTinhLuong
{
    /// <summary>
    /// Interaction logic for HoaHong.xaml
    /// </summary>
    public partial class HoaHong : Page, INotifyPropertyChanged
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
        public HoaHong(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }
            YearList = new ObservableCollection<string>();
            var c = DateTime.Now.Year;
            if (c != null)
            {
                YearList.Add($"Năm {c - 1}");
                YearList.Add($"Năm {c}");
                YearList.Add($"Năm {c + 1}");
            }
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            cbThang.PlaceHolder = "Tháng " + month;
            cbNam.PlaceHolder = "Năm " + year;
            getData(month, year, "");
            getData1();
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

        private List<ThongKeHoaHong> _listThongKeHH;

        public List<ThongKeHoaHong> listThongKeHH
        {
            get { return _listThongKeHH; }
            set { _listThongKeHH = value; OnPropertyChanged(); }
        }

        private void getData(string month, string year, string user_id)
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
                            web.QueryString.Add("time", year + "-" + month);
                            web.QueryString.Add("id_user", user_id);
                        }
                        web.UploadValuesCompleted += (s, e) =>
                        {
                            try
                            {
                                string x = UnicodeEncoding.UTF8.GetString(e.Result);
                                API_ThongKeHoaHong api = JsonConvert.DeserializeObject<API_ThongKeHoaHong>(x);
                                if (api.data != null)
                                {
                                    listThongKeHH = api.data.rose;
                                    foreach (var item in listThongKeHH)
                                    {
                                        if (item.ep_image == "/img/add.png")
                                            item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                    }
                                }
                            }
                            catch { }
                        };
                        web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_rose_nv.php", web.QueryString);
                    }
                });
            }
            catch
            {

            }

        }

        private List<ListEmployee> _listNV;

        public List<ListEmployee> listNV
        {
            get { return _listNV; }
            set
            {
                if (value == null) value = new List<ListEmployee>();
                value.Insert(0, new ListEmployee() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listNV = value; OnPropertyChanged();
            }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListEmployee api = JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data.data != null)
                        {
                            listNV = api.data.data.items;
                        }
                    }
                    catch { }
                    //foreach (ItemTamUng item in listTamUng)
                    //{
                    //    if (item.ep_image == "/img/add.png")
                    //    {
                    //        item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php", web.QueryString);
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
            if (this.ActualWidth > 1650)
            {
                DockPanel.SetDock(dockHoaHong, Dock.Right);
            }
            else
                DockPanel.SetDock(dockHoaHong, Dock.Bottom);
        }

        private void btn_ThemHoaHong_Click(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongTien(Main));
            Main.title.Text = "Hoa hồng/ Hoa hồng tiền";
            Main.SideBarIndex = -1;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongDoanhThu(Main));
            Main.title.Text = "Hoa hồng/ Hoa hồng doanh thu";
            Main.SideBarIndex = -1;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongLoiNhuan(Main));
            Main.title.Text = "Hoa hồng/ Hoa hồng lợi nhuận";
            Main.SideBarIndex = -1;
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongLePhiViTri(Main));
            Main.title.Text = "Hoa hồng/ Hoa hồng lệ phí vị trí";
            Main.SideBarIndex = -1;
        }

        private void Border_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.HoaHongKeHoach(Main));
            Main.title.Text = "Hoa hồng/ Hoa hồng kế hoạch";
            Main.SideBarIndex = -1;
        }

        private void btnCaiDatHoaHongDoanhThu(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.PopupCaiDatHoaHongDoanhThu(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void BtnCaiDatHoaHongLoiNhuan(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupCaiDatHoaHongLoiNhuan(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnCaiDatHoaHongLePhiViTri(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupCaiDatHoaHongLePhiViTri(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void btnCaiDatHoaHongKeHoach(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupCaiDatHoaHongKeHoach(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Btn_File_excel_Click(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(new Views.DuLieuTinhLuong.Popup.PopupFileExcel_HoaHongTien(Main));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            string year = "", month = "", id_nv = "";
            if (cbNam.SelectedIndex != -1)
                year = cbNam.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (cbThang.SelectedIndex != -1)
                month = (cbThang.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");
            if (cbListNV.SelectedIndex != -1)
            {
                ListEmployee id1 = (ListEmployee)cbListNV.SelectedItem;
                string id2 = id1.ep_id;
                if (id2 == "-1")
                    id_nv = "";
                else id_nv = id2;
            }
            getData(month, year, id_nv);
        }
        private void xuatExcel()
        {
            string year = "", month = "", id_nv = "";
            this.Dispatcher.Invoke(() =>
            {
                if (cbNam.SelectedIndex != -1)
                    year = cbNam.SelectedItem.ToString().Split(' ')[1];
                else
                    year = DateTime.Now.ToString("yyyy");
                if (cbThang.SelectedIndex != -1)
                    month = (cbThang.SelectedIndex + 1) + "";
                else month = DateTime.Now.ToString("MM");
                if (cbListNV.SelectedIndex != -1)
                {
                    ListEmployee id1 = (ListEmployee)cbListNV.SelectedItem;
                    string id2 = id1.ep_id;
                    if (id2 == "-1")
                        id_nv = "";
                    else id_nv = id2;
                }
            });
            /*MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Main.CurrentCompany.com_id), "id_comp");
            content.Add(new StringContent(Main.CurrentCompany.token), "token");
            content.Add(new StringContent(month), "m");
            content.Add(new StringContent(year), "y");
            content.Add(new StringContent(id_nv), "uid");
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> response = httpClient.PostAsync("https://tinhluong.timviec365.vn/api_app/company/export_rose.php", content);
            string data = response.Result.Content.ReadAsStringAsync().Result;*/
            string data ="";
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("m", month);
                    web.QueryString.Add("y", year);
                    web.QueryString.Add("uid", id_nv);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(e.Result);
                    File.WriteAllText("../../Views/DuLieuTinhLuong/hoa_hong365.html", data);
                    string filePath = "";
                    // tạo SaveFileDialog để lưu file excel
                    SaveFileDialog dialog = new SaveFileDialog();

                    // chỉ lọc ra các file có định dạng Excel
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = "hoa_hong_365";
                    // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
                    if (dialog.ShowDialog() == true)
                    {
                        filePath = dialog.FileName;
                        var workbook = new Workbook("../../Views/DuLieuTinhLuong/hoa_hong365.html");
                        try
                        {
                            workbook.Save(filePath);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        loading.Visibility = Visibility.Collapsed;
                        //converter.Convert(filePath, convertOptions);
                    }

                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_rose.php", web.QueryString);
            }
        }
        private void XuatFileThongKe(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            xuatExcel();
        }

        private void dataGrid2_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }
    }
}
