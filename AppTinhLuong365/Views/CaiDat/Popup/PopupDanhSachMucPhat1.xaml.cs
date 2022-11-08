using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using List = AppTinhLuong365.Model.APIEntity.List;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupDanhSachMucPhat1.xaml
    /// </summary>
    public partial class PopupDanhSachMucPhat1 : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        public PopupDanhSachMucPhat1(MainWindow main, string dataShiftId)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            id = dataShiftId;
            getData();
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private List<List> _list;

        public List<List> list
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.QueryString.Add("take", "1");
                web.QueryString.Add("shift_id", id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_Np_Improperly api = JsonConvert.DeserializeObject<API_List_Np_Improperly>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            list = api.data.list;
                        }
                    }
                    catch { }
                    //foreach (EpLate item in list)
                    //{
                    //    if (item.ts_image != "/img/add.png")
                    //    {
                    //        item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //    }
                    //}
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/list_mp_shift.php", web.QueryString);
            }
        }

        private void Sua(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            List data = (List)b.DataContext;
            if (data.type1 == 1)
            {
                using (WebClient web = new WebClient())
                {
                    if (Main.MainType == 0)
                    {
                        web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                    }
                    web.QueryString.Add("id", data.pc_id);
                    web.QueryString.Add("pc_money", text);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            API_SuaNhomLamViec api = JsonConvert.DeserializeObject<API_SuaNhomLamViec>(UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                var pop = new Views.CaiDat.NghiPhep(Main);
                                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                                pop.Control.SelectedIndex = 1;
                                this.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch{ }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/edit_np.php", web.QueryString);
                }
                
            }
            else
            {
                foreach (var a in list)
                {
                    if (a == data)
                    {
                        a.type1 = 1;
                    }
                }
            }
        }

        private string text;

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            text = textBox.Text;
        }

        private void Xoa(object sender, MouseButtonEventArgs e)
        {
            Path b = sender as Path;
            List data = (List)b.DataContext;
            Main.PopupSelection.NavigationService.Navigate(new Views.CaiDat.Popup.PopupXoaNghiPhep(Main, data.pc_id));
            Main.PopupSelection.Visibility = Visibility.Visible;
        }
    }
}
