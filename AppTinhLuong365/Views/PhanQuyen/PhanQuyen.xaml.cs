using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;

namespace AppTinhLuong365.Views.PhanQuyen
{
    /// <summary>
    /// Interaction logic for PhanQuyen.xaml
    /// </summary>
    public partial class PhanQuyen : Page, INotifyPropertyChanged
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
        public PhanQuyen(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
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
            if (this.ActualWidth > 550)
                DockPanel.SetDock(borderPhanQuyen, Dock.Right);
            else
                DockPanel.SetDock(borderPhanQuyen, Dock.Bottom);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            ListEmployee data = (ListEmployee)t.DataContext;
            Views.PhanQuyen.PopupPQTuyChon pop=new Views.PhanQuyen.PopupPQTuyChon(Main, data.ep_image, data.ep_name, data.ep_id, data.role_id);
            var z=Mouse.GetPosition(Main.PopupSelection);
            pop.Margin = new Thickness(z.X-250,z.Y+15,0,0);
            Main.PopupSelection.NavigationService.Navigate(pop);
            Main.PopupSelection.Visibility = Visibility.Visible;
        }

        private List<ListEmployee> _listEmployee;

        public List<ListEmployee> listEmployee
        {
            get { return _listEmployee; }
            set
            {
                
                _listEmployee = value;
                OnPropertyChanged();
            }
        }

        private List<ListEmployee> _listEmployee1;

        public List<ListEmployee> listEmployee1
        {
            get { return _listEmployee1; }
            set
            {
                if (value == null) value = new List<ListEmployee>();
                value.Insert(0, new ListEmployee() { ep_id = "-1", ep_name = "Tất cả nhân viên" });
                _listEmployee1 = value;
                OnPropertyChanged();
            }
        }

        private ListEmployee _listEmployee2;

        public ListEmployee listEmployee2
        {
            get { return _listEmployee2; }
            set
            {
                _listEmployee2 = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_ListEmployee api =
                        JsonConvert.DeserializeObject<API_ListEmployee>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listEmployee = listEmployee1 = api.data.data.items;
                            listEmployee.RemoveAt(0);
                            listEmployee = listEmployee.ToList();
                        }
                        foreach (ListEmployee item in listEmployee)
                        {
                            if (item.ep_image != "")
                            {
                                item.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + item.ep_image;
                            }
                            else
                            {
                                item.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php",
                    web.QueryString);
            }
        }

        
        private void getData1(string ep_id)
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("id_ep", ep_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        var a = UnicodeEncoding.UTF8.GetString(e.Result);
                        API_DSNVPQ1Nguoi api =
                            JsonConvert.DeserializeObject<API_DSNVPQ1Nguoi>(a);
                        if (api.data.data != null)
                        {
                            if (!string.IsNullOrEmpty(ep_id) && ep_id != "-1")
                            {
                                listEmployee2 = api.data.data.items;

                                listEmployee = null;
                                listEmployee = new List<ListEmployee>();
                                if (listEmployee2.ep_image != "")
                                {
                                    listEmployee2.ep_image = "https://chamcong.24hpay.vn/upload/employee/" + listEmployee2.ep_image;
                                }
                                else
                                {
                                    listEmployee2.ep_image = "https://tinhluong.timviec365.vn/img/add.png";
                                }
                                listEmployee.Add(listEmployee2);
                                listEmployee = listEmployee.ToList();
                            }
                        }
                    }
                    catch { }
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_emp.php",
                    web.QueryString);
            }
        }


        private void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.ScrollToVerticalOffset(Main.scrollMain.VerticalOffset - e.Delta);
        }

        private void NhanVien(object sender, SelectionChangedEventArgs e)
        {
            string ep_id = "";
            if (Employee.SelectedIndex != -1)
            {
                ListEmployee id1 = (ListEmployee)Employee.SelectedItem;
                string id2 = id1.ep_id;
                if (id2 == "-1")
                    ep_id = "";
                else ep_id = id2;
            }
            if(ep_id != "-1" && !string.IsNullOrEmpty(ep_id))
                getData1(ep_id);
            else
            {
                getData();
            }
        }
    }
}
