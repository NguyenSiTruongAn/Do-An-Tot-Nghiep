﻿using System;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppTinhLuong365.Model.APIEntity;
using Aspose.Cells;
using Microsoft.Win32;
using Newtonsoft.Json;
using Border = Aspose.Cells.Border;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppTinhLuong365.Views.TinhLuong
{
    /// <summary>
    /// Interaction logic for ChiTietLuong.xaml
    /// </summary>
    public partial class ChiTietLuong : Page, INotifyPropertyChanged
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
        private string month;
        private string year;
        private string start_date;
        private string end_date;
        private int type;

        public ChiTietLuong(MainWindow main, string name, string depName, string epId, int month, int year, string startDate, string endDate, int type)
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
            this.type = type;
            Main = main;
            // string Month = DateTime.Now.ToString("MM");
            // string Year = DateTime.Now.ToString("yyyy");
            // string start_date = DateTime.Now.ToString("yyyy-MM-01");
            // string end_date = DateTime.Now.ToString("yyyy-MM-30");
            int d = month + 1;
            int e = year + DateTime.Now.Year - 1;
            this.month = d + "";
            this.year = e + "";
            searchBarMonth.SelectedIndex = d - 1;
            searchBarYear.SelectedIndex = e - e + 1;
            start_date = startDate;
            end_date = endDate;
            DateTime a = (DateTime.Parse(start_date));
            DateTime b = (DateTime.Parse(end_date));
            DatePickerStart.SelectedDate = a;
            DatePickerEnd.SelectedDate = b;
            Name.Text = name;
            Pb.Text = depName;
            Id.Text = epId;
            this.ep_id = epId;
            Thang.Text = "tháng " + d + "/" + e;
            getData(d, e, start_date, end_date);
            getDataTB();
            Main.HomeSelectionPage.Focus();
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
            loading.Visibility = Visibility.Visible;
            using (WebClient web = new WebClient())
            {
                // loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("company", Main.CurrentCompany.com_id);
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
                    try
                    {
                        API_Luong_nv api =
                        JsonConvert.DeserializeObject<API_Luong_nv>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            itemLuongNv = api.data.luong_nv;
                        }
                    }
                    catch
                    {
                    }
                    loading.Visibility = Visibility.Collapsed;
                    Main.HomeSelectionPage.Focus();
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_luong_nv.php",
                    web.QueryString);
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
            Thang.Text = "tháng" + month + "/" + year;
            getData(Convert.ToInt32(month), Convert.ToInt32(year), start_date, end_date);
        }

        private void btnTienThuong(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupThuong(Main, itemLuongNv.ct_thuong);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Phat(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupPhat(Main, itemLuongNv.ct_phat);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y + 20, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void HoaHong(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupHoaHong(Main, itemLuongNv.chi_tiet_hoa_hong);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void Luong(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
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
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("m", month);
                    web.QueryString.Add("y", year);
                    web.QueryString.Add("uid", ep_id);
                }

                web.UploadValuesCompleted += (s, e) =>
                {
                    data = UnicodeEncoding.UTF8.GetString(e.Result);
                    string path = $"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    File.WriteAllText($"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/bang_luong_nhan_vien.html", data);
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
                        var workbook = new Workbook($"{Environment.GetEnvironmentVariable("APPDATA")}/TinhLuong/bang_luong_nhan_vien.html");
                        try
                        {
                            workbook.Save(filePath);
                            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                            if (xlApp == null)
                            {
                                MessageBox.Show("Excel is not properly installed!!");
                                return;
                            }


                            xlApp.DisplayAlerts = false;
                            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                            Excel.Sheets worksheets = xlWorkBook.Worksheets;
                            worksheets[2].Delete();
                            xlWorkBook.Save();
                            xlWorkBook.Close();

                            releaseObject(worksheets);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        //converter.Convert(filePath, convertOptions);
                    }
                    loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/export_tbl_luong.php",
                    web.QueryString);
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void XuatFileThongKe(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            xuatExcel();
        }

        private void PhatSQD(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupPhatSQD(Main, itemLuongNv.list_phat_nsqd);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void phatcong(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupPhatCong(Main, itemLuongNv.ct_ms_phat_cong);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void phattien(object sender, MouseButtonEventArgs e)
        {
            var pop = new Views.TinhLuong.Popup.PopupCTPhatDiMuon(Main, itemLuongNv.ct_ms_phat_tien);
            var z = Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X - 615, z.Y, 0, 0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private void TroLai(object sender, MouseButtonEventArgs e)
        {
            if (type == 1)
            {
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.TinhLuong.BangLuong(Main));
                Main.SideBarIndex = 9;
                Main.SideBarIndex = 10;
            }
            if(type == 2)
            {
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.TongHopLuongNhanVienTheoChuKi(Main,DateTime.Now.ToString("MM"),DateTime.Now.ToString("yyyy")));
                Main.SideBarIndex = -1;
            }
            if(type == 3)
            {
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.TongHopThongTinBaoHiem(Main, DateTime.Now.ToString("MM"), DateTime.Now.ToString("yyyy")));
                Main.SideBarIndex = -1;
            }
            if(type == 4)
            {
                Main.HomeSelectionPage.NavigationService.Navigate(new Views.BaoCaoCongLuong.TongHopThongTinThue(Main, DateTime.Now.ToString("MM"), DateTime.Now.ToString("yyyy")));
                Main.SideBarIndex = -1;
            }
        }
    }
}