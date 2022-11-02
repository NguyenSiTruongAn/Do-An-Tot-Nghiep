using AppTinhLuong365.Model.APIEntity;
using Aspose.Cells;
using Microsoft.Win32;
using Newtonsoft.Json;
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

namespace AppTinhLuong365.Views.NhanVien
{
    /// <summary>
    /// Interaction logic for payroll.xaml
    /// </summary>
    public partial class payroll : Page, INotifyPropertyChanged
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

        private string ep_id;
        private string month = DateTime.Now.ToString("MM");
        private string year = DateTime.Now.ToString("yyyy");
        private string start_date = DateTime.Now.ToString("yyyy-MM") + "-01";
        private string end_date = DateTime.Now.ToString("yyyy-MM")  + "-" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        public payroll(MainWindow main)
        {
            ItemList = new ObservableCollection<string>();
            for (var i = 1; i <= 12; i++)
            {
                ItemList.Add($"Tháng {i}");
            }

            YearList = new ObservableCollection<string>();
            var c = DateTime.Now.Year;
            if (c != 0)
            {
                YearList.Add($"Năm {c - 1}");
                YearList.Add($"Năm {c}");
                YearList.Add($"Năm {c + 1}");
            }

            InitializeComponent();
            this.DataContext = this;
            Main = main;
            // string Month = DateTime.Now.ToString("MM");
            // string Year = DateTime.Now.ToString("yyyy");
            // string start_date = DateTime.Now.ToString("yyyy-MM-01");
            // string end_date = DateTime.Now.ToString("yyyy-MM-30");
            int d = int.Parse(month);
            int e = int.Parse(year) + DateTime.Now.Year - 1;
            this.month = d + "";
            this.year = e + "";
            searchBarMonth.SelectedIndex = d - 1;
            searchBarYear.SelectedIndex = e - e + 1;
            DateTime a = (DateTime.Parse(start_date));
            DateTime b = (DateTime.Parse(end_date));
            DatePickerStart.SelectedDate = a;
            DatePickerEnd.SelectedDate = b;
            txtTen.Text = Main.CurrentEmployee.ep_name + "- ID " + Main.CurrentEmployee.ep_id;
            this.ep_id = Main.CurrentEmployee.ep_id;
            getData(d, e, start_date, end_date);
            getData1();
        }

        public ObservableCollection<string> ItemList { get; set; }
        public ObservableCollection<string> YearList { get; set; }

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

        private Item_Luong_nv _itemLuongNv;

        public Item_Luong_nv itemLuongNv
        {
            get { return _itemLuongNv; }
            set
            {
                _itemLuongNv = value;
                OnPropertyChanged();
            }
        }


        private void getData(int month, int year, string start_date, string end_date)
        {
            using (WebClient web = new WebClient())
            {
                // loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentEmployee.token);
                web.QueryString.Add("company", Main.CurrentEmployee.com_id);
                web.QueryString.Add("month", month.ToString());
                web.QueryString.Add("year", year.ToString());
                web.QueryString.Add("id_user", ep_id);
                string a = "";
                if (start_date != null)
                {
                    DateTime m;
                    if (DateTime.TryParse(start_date, out m)) a = m.ToString("yyyy-MM-dd");
                }
                web.QueryString.Add("start_date", a);
                string b = "";
                if (end_date != null)
                {
                    DateTime m;
                    if (DateTime.TryParse(end_date, out m)) b = m.ToString("yyyy-MM-dd");
                }
                web.QueryString.Add("end_date", b);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Luong_nv api =
                        JsonConvert.DeserializeObject<API_Luong_nv>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        itemLuongNv = api.data.luong_nv;
                    }
                    // loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_luong_nv.php",
                    web.QueryString);
            }
        }

        private List<Item_dep> _listItem_dep;

        public List<Item_dep> listItem_dep
        {
            get { return _listItem_dep; }
            set
            {
                _listItem_dep = value;
            }
        }

        private void getData1()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentEmployee.token);
                web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    API_List_dep api =
                        JsonConvert.DeserializeObject<API_List_dep>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listItem_dep = api.data.list;
                        foreach(var item in listItem_dep)
                        {
                            if (item.dep_id == Main.CurrentEmployee.dep_id)
                                txtPhong.Text = item.dep_name;
                        }
                    }
                    // foreach (EpLate item in listEpLate)
                    // {
                    //     if (item.ts_image != "/img/add.png")
                    //     {
                    //         item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_dep.php", web.QueryString);
            }
        }

        private void Month(object sender, SelectionChangedEventArgs e)
        {
            string a = DateTime.Now.ToString((searchBarYear.SelectedIndex + DateTime.Now.Year - 1) + "-" +
                                             (searchBarMonth.SelectedIndex + 1) + "-01");
            DateTime b = DateTime.Parse(a);
            DatePickerStart.SelectedDate = b;
            string c = DateTime.Now.ToString((searchBarYear.SelectedIndex + DateTime.Now.Year - 1) + "-" +
                                             (searchBarMonth.SelectedIndex + 1) + "-" +
                                             DateTime.DaysInMonth((searchBarYear.SelectedIndex + 2021),
                                                 (searchBarMonth.SelectedIndex + 1)));
            DateTime d = DateTime.Parse(c);
            DatePickerEnd.SelectedDate = d;
        }

        private void ThongKe(object sender, MouseButtonEventArgs e)
        {
            year = ""; month = ""; start_date = ""; end_date = "";
            if (searchBarYear.SelectedItem != null)
                year = searchBarYear.SelectedItem.ToString().Split(' ')[1];
            else
                year = DateTime.Now.ToString("yyyy");
            if (searchBarMonth.SelectedIndex != -1)
                month = (searchBarMonth.SelectedIndex + 1) + "";
            else month = DateTime.Now.ToString("MM");

            if (DatePickerStart.SelectedDate != null)
                start_date = DatePickerStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                start_date = DatePickerStart.SelectedDate.Value.ToString(year + "-" + month + "-01");
            }
            else
                start_date = DateTime.Now.ToString("yyyy-MM-01");

            if (DatePickerEnd.SelectedDate != null)
                end_date = DatePickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            else if (searchBarMonth != null && searchBarYear != null)
            {
                end_date = DatePickerEnd.SelectedDate.Value.ToString(year + "-" + month + "-30");
            }
            else
                end_date = DateTime.Now.ToString("yyyy-MM-01");

            getData(Convert.ToInt32(month), Convert.ToInt32(year), start_date, end_date);
        }

        private void btnTienThuong(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupThuong(Main, ep_id, month, year);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Phat(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupPhat(Main, ep_id, month, year);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void HoaHong(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupHoaHong(Main, ep_id, month, year);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Luong(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Border b = sender as System.Windows.Controls.Border;
            var pop = new Views.TinhLuong.Popup.PopupLuongDaTra(Main, ep_id, month, year, start_date, end_date);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void xuatExcel()
        {
            // string year = "", month = "", id_nv = "";
            // this.Dispatcher.Invoke(() =>
            // {
            //     if (cbNam.SelectedIndex != -1)
            //         year = cbNam.SelectedItem.ToString().Split(' ')[1];
            //     else
            //         year = DateTime.Now.ToString("yyyy");
            //     if (cbThang.SelectedIndex != -1)
            //         month = (cbThang.SelectedIndex + 1) + "";
            //     else month = DateTime.Now.ToString("MM");
            //     if (cbListNV.SelectedIndex != -1)
            //     {
            //         ListEmployee id1 = (ListEmployee)cbListNV.SelectedItem;
            //         string id2 = id1.ep_id;
            //         if (id2 == "-1")
            //             id_nv = "";
            //         else id_nv = id2;
            //     }
            // });
            /*MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Main.CurrentCompany.com_id), "id_comp");
            content.Add(new StringContent(Main.CurrentCompany.token), "token");
            content.Add(new StringContent(month), "m");
            content.Add(new StringContent(year), "y");
            content.Add(new StringContent(id_nv), "uid");
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> response = httpClient.PostAsync("https://tinhluong.timviec365.vn/api_app/company/export_rose.php", content);
            string data = response.Result.Content.ReadAsStringAsync().Result;*/
            string data = "";
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("token", Main.CurrentEmployee.token);
                    web.QueryString.Add("id_comp", Main.CurrentEmployee.com_id);
                    web.QueryString.Add("m", month);
                    web.QueryString.Add("y", year);
                    web.QueryString.Add("uid", ep_id);
                }

                web.UploadValuesCompleted += (s, e) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(e.Result);
                    File.WriteAllText("../../Views/DuLieuTinhLuong/bang_luong_nhan_vien.html", data);
                    string filePath = "";
                    // tạo SaveFileDialog để lưu file excel
                    SaveFileDialog dialog = new SaveFileDialog();

                    // chỉ lọc ra các file có định dạng Excel
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = "bang_luong_nhan_vien_365";
                    // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
                    if (dialog.ShowDialog() == true)
                    {
                        filePath = dialog.FileName;
                        var workbook = new Workbook("../../Views/DuLieuTinhLuong/bang_luong_nhan_vien.html");
                        try
                        {
                            workbook.Save(filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        loading.Visibility = Visibility.Collapsed;
                        
                        //converter.Convert(filePath, convertOptions);
                    }
                    else loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_tbl_luong.php",
                    web.QueryString);
            }
        }

        private void XuatFileThongKe(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            xuatExcel();
        }
    }
}
