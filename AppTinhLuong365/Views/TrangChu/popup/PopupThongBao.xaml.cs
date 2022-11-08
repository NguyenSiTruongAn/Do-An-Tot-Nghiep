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

namespace AppTinhLuong365.Views.TrangChu.popup
{
    /// <summary>
    /// Interaction logic for PopupThongBao.xaml
    /// </summary>
    public partial class PopupThongBao : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PopupThongBao(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
        }
        MainWindow Main;

        private List<ThongBaoCT> _listTB;

        public List<ThongBaoCT> listTB
        {
            get { return _listTB; }
            set { _listTB = value; OnPropertyChanged(); }
        }

        private void getData()
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
                        string x = UnicodeEncoding.UTF8.GetString(e.Result);
                        API_ThongBaoCT api = JsonConvert.DeserializeObject<API_ThongBaoCT>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listTB = api.data.abc;
                            foreach (var item in listTB)
                            {
                                if (!string.IsNullOrEmpty(item.ep_name))
                                    item.com_name = item.ep_name;
                            }
                            if (Main.MainType == 0)
                            {
                                foreach (var item in listTB)
                                {
                                    if (string.IsNullOrEmpty(item.ep_name))
                                        item.com_name = "";
                                }
                            }
                            if (listTB != null)
                            {
                                Main.sotb = listTB.Count;
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
                            else
                                Main.sotb = 0;
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/api_notify.php", web.QueryString);
            }
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollview.ScrollToVerticalOffset(scrollview.VerticalOffset - e.Delta);
        }

        private void ChuyenManTB(object sender, MouseButtonEventArgs e)
        {
            if(Main.MainType == 0)
            {
                Main.sidebar.SelectedIndex = 2;
                Main.sidebar.SelectedIndex = 4;
                Main.PopupSelection.Visibility = Visibility.Collapsed;
            }
            if(Main.MainType == 1)
            {
                Main.sidebarNV.SelectedIndex = 2;
                Main.PopupSelection.Visibility = Visibility.Collapsed;
            }
        }

        private void XoaTatCaThongBao(object sender, MouseButtonEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                if (Main.MainType == 0)
                {
                    web.QueryString.Add("id", Main.CurrentCompany.com_id);
                    web.QueryString.Add("token", Main.CurrentCompany.token);
                    web.QueryString.Add("quyen", "2");
                }
                if (Main.MainType == 1)
                {
                    web.QueryString.Add("token", Main.CurrentEmployee.token);
                    web.QueryString.Add("quyen", "1");
                    web.QueryString.Add("id", Main.CurrentEmployee.ep_id);
                }

                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        string x = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_XoaNhomLamViec api = JsonConvert.DeserializeObject<API_XoaNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            getData();
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/del_all_notify.php", web.QueryString);
            }
        }

        private void XoaThongBao(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            ThongBaoCT data = (ThongBaoCT)b.DataContext;
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("id_tb", data.tb_id);
                web.UploadValuesCompleted += (s, ee) =>
                {
                    try
                    {
                        string x = UnicodeEncoding.UTF8.GetString(ee.Result);
                        API_XoaNhomLamViec api = JsonConvert.DeserializeObject<API_XoaNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                            getData();
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/del_one_notify.php", web.QueryString);
            }
        }
    }
}
