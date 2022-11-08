using AppTinhLuong365.Core;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
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

namespace AppTinhLuong365.Views.DuLieuTinhLuong.Popup
{
    /// <summary>
    /// Interaction logic for PopupThemNhanVienVaoPhuCap.xaml
    /// </summary>
    public partial class PopupThemNhanVienVaoPhuCap : Page, INotifyPropertyChanged
    {
        private DateTime day1, day_end1;
        private string id1;
        bool setDayEnd = false;
        public PopupThemNhanVienVaoPhuCap(MainWindow main, string id, string day, string day_end)
        {
            InitializeComponent(); this.DataContext = this;
            Main = main;
            DateTime.TryParse(day, out day1);
            DateTime.TryParse(day_end, out day_end1);
            id1 = id;
            if (day_end != null)
            {
                DateTime.TryParse(day_end, out day_end1);
                setDayEnd = true;
            }
            getData();
        }

        MainWindow Main;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DSNVThemMoiVaoPhucLoi_PhuCap> _listNV;

        public List<DSNVThemMoiVaoPhucLoi_PhuCap> listNV
        {
            get { return _listNV; }
            set
            {
                if (value == null) value = new List<DSNVThemMoiVaoPhucLoi_PhuCap>();
                value.Insert(0, new DSNVThemMoiVaoPhucLoi_PhuCap() { ep_id = "", ep_name = "Tất cả nhân viên", ep_image = "", dep_name = "" });
                _listNV = value; OnPropertyChanged();
            }
        }

        private List<DSNVThemMoiVaoPhucLoi_PhuCap> _listNV1;

        public List<DSNVThemMoiVaoPhucLoi_PhuCap> listNV1
        {
            get { return _listNV1; }
            set { _listNV1 = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("id_wf", id1);
                }
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_DSNVThemMoiVaoPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_DSNVThemMoiVaoPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listNV1 = listNV = api.data.list;
                        }
                        foreach (DSNVThemMoiVaoPhucLoi_PhuCap item in listNV)
                        {
                            if (item.ep_image == "")
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                            else item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_user_other.php", web.QueryString);
            }
        }

        private void ThemNhanVienVaoPhucLoi(object sender, MouseButtonEventArgs e)
        {
            List<string> nv = new List<string>();
            validateDate.Text = validateList.Text = "";
            foreach (var item in listNV1)
            {
                if (item.status == true)
                    nv.Add(item.ep_id);
            }
            bool allow = true;
            if (nv.Count <= 0)
            {
                validateList.Text = "Vui lòng chọn nhân viên";
                allow = false;
            }

            if(textThangAD.SelectedDate == null)
            {
                validateDate.Text = "Vui lòng chọn ngày bắt đầu áp dụng";
            }

            if (textThangAD.SelectedDate != null && textDenThang.SelectedDate != null)
            if (textThangAD.SelectedDate.Value.ToString("yyyy/MM/dd").CompareTo(textDenThang.SelectedDate.Value.ToString("yyyy/MM/dd")) > 0)
            {
                validateDateEnd.Text = "Tháng kết thúc không được nhỏ hơn tháng bắt đầu";
                allow = false;
            }
            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("token", Main.CurrentCompany.token);
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    int dem = 0;
                    foreach (var item in nv)
                    {
                        string idname = "id[" + dem + "]";
                        web.QueryString.Add(idname, item);
                        dem++;
                    }
                    web.QueryString.Add("id_welf", id1);
                    web.QueryString.Add("wf_time", textThangAD.SelectedDate.Value.ToString("yyyy/MM/dd"));
                    if(textDenThang.SelectedDate != null)
                        web.QueryString.Add("wf_time_end", textDenThang.SelectedDate.Value.ToString("yyyy/MM/dd"));
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_ThemNhanVienVaoPhucLoi_PhuCap api = JsonConvert.DeserializeObject<API_ThemNhanVienVaoPhucLoi_PhuCap>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.DuLieuTinhLuong.PhucLoi(Main));
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_ep_welfare.php", web.QueryString);
                }
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            listNV1 = listNV.Where(x => x.ep_name.ToLower().RemoveUnicode().Contains(tbInput.Text.ToLower().RemoveUnicode())).ToList();
        }

        private void ChonNhanvien(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSNVThemMoiVaoPhucLoi_PhuCap data = (DSNVThemMoiVaoPhucLoi_PhuCap)cb.DataContext;
            if (data.ep_id.Equals(""))
            {
                foreach (var item in listNV1)
                {
                    item.status = true;
                }
            }
            else data.status = true;
        }
        private void HuyChon(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DSNVThemMoiVaoPhucLoi_PhuCap data = (DSNVThemMoiVaoPhucLoi_PhuCap)cb.DataContext;
            if (data.ep_id.Equals(""))
            {
                foreach (var item in listNV1)
                {
                    item.status = false;
                }
            }
            else
            {
                data.status = false;
            }
        }
    }
}
