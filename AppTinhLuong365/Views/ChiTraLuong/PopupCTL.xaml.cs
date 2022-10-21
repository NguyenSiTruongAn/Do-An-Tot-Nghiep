using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AppTinhLuong365.Views.ChiTraLuong
{
    /// <summary>
    /// Interaction logic for PopupCTL.xaml
    /// </summary>
    public partial class PopupCTL : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;
        private List<string> nv;
        private int page;

        public PopupCTL(MainWindow main, string id, List<string> nv, int page)
        {
            this.DataContext = this;
            InitializeComponent();
            this.id = id;
            this.nv = nv.ToList();
            this.page = page;
            Main = main;
            getData();
        }

        private MainWindow Main;

        private static readonly Regex _regex = new Regex(@"^[0-9]\d*(\.\d{0,2})?$");

        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        private bool IsAllowed(TextBox tb, string text)
        {
            bool isAllowed = true;
            if (tb != null)
            {
                string currentText = tb.Text;
                if (!string.IsNullOrEmpty(tb.SelectedText))
                    currentText = currentText.Remove(tb.CaretIndex, tb.SelectedText.Length);
                isAllowed = IsTextAllowed(currentText.Insert(tb.CaretIndex, text));
            }

            return isAllowed;
        }

        private void Txt_PreviewCurrencyTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsAllowed(sender as TextBox, e.Text);
        }


        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsAllowed(sender as TextBox, text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
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

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                loading.Visibility = Visibility.Visible;
                web.QueryString.Add("token", Main.CurrentCompany.token);
                web.QueryString.Add("cp", Main.CurrentCompany.com_id);
                web.QueryString.Add("pid", id);
                web.QueryString.Add("page", page + "");
                for (int i = 0; i < nv.Count; i++)
                {
                    string id_ep = "pay_men[" + i + "]";
                    web.QueryString.Add(id_ep, nv[i]);
                }

                web.UploadValuesCompleted += (s, e) =>
                {
                    API_Detail_pay api =
                        JsonConvert.DeserializeObject<API_Detail_pay>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (api.data != null)
                    {
                        listItemDetailPay = api.data.list;
                        listDetailPay = api.data;
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

                    loading.Visibility = Visibility.Collapsed;
                };
                web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/detail_pay.php",
                    web.QueryString);
            }
        }

        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        }

        private void update(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxPc.Text))
            {
                foreach (var item in listItemDetailPay)
                {
                    item.pc_luong = Convert.ToInt32(TextBoxPc.Text);
                    PropertyChanged.Invoke(item, new PropertyChangedEventArgs("pc_luong"));
                    item.money = (item.kq_luong / 100 * item.pc_luong).ToString();
                    PropertyChanged.Invoke(item, new PropertyChangedEventArgs("money"));
                }
            }

            if (!string.IsNullOrEmpty(TextBoxMoney.Text))
            {
                foreach (var item in listItemDetailPay)
                {
                    item.money = TextBoxMoney.Text;
                    if (item.kq_luong != 0)
                    {
                        PropertyChanged.Invoke(item, new PropertyChangedEventArgs("money"));
                        item.pc_luong = (int)(long.Parse(item.money) / item.kq_luong * 100);
                        PropertyChanged.Invoke(item, new PropertyChangedEventArgs("pc_luong"));
                    }
                    else if (item.kq_luong == 0)
                    {
                        PropertyChanged.Invoke(item, new PropertyChangedEventArgs("money"));
                        item.pc_luong = 0;
                        PropertyChanged.Invoke(item, new PropertyChangedEventArgs("pc_luong"));
                    }
                }
            }
        }

        private void Pay(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            foreach (var item in listItemDetailPay)
            {
                if (item.pc_luong > 100)
                {
                    allow = false;
                    // Main.PopupSelection1.NavigationService.Navigate(
                    //     new Views.ChiTraLuong.Validation());
                    // Main.PopupSelection1.Visibility = Visibility.Visible;
                    gridPopup.Children.Add(new Views.ChiTraLuong.Validation());
                }
            }

            if (allow)
            {
                using (WebClient web = new WebClient())
                {
                    web.QueryString.Add("id", id);
                    for (int i = 0; i < nv.Count; i++)
                    {
                        string id_ep = "id_pm[" + i + "]";
                        string salary = "pp_salary[" + i + "]";
                        web.QueryString.Add(id_ep, nv[i]);
                        web.QueryString.Add(salary, listItemDetailPay[i].money);
                    }

                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        API_Add_late api =
                            JsonConvert.DeserializeObject<API_Add_late>(UnicodeEncoding.UTF8.GetString(ee.Result));
                        if (api.data != null)
                        {
                        }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/pay_wage.php",
                        web.QueryString);
                }
            }

            Main.HomeSelectionPage.NavigationService.Navigate(new Views.ChiTraLuong.ChiTietChiTraLuong(Main, id));
            this.Visibility = Visibility.Collapsed;
        }

        private int dem = 0;

        private void Money(object sender, TextChangedEventArgs e)
        {
            if (dem >= listItemDetailPay.Count)
            {
                TextBox t = (TextBox)sender;
                if (t.IsFocused)
                {
                    Item_detail_pay data = (Item_detail_pay)t.DataContext;
                    if (!string.IsNullOrEmpty(t.Text))
                    {
                        for (int i = 0; i < listItemDetailPay.Count; i++)
                        {
                            listItemDetailPay[i].money = t.Text;
                            if (listItemDetailPay[i].ep_id == data.ep_id && listItemDetailPay[i].money != "0")
                            {
                                listItemDetailPay[i].pc_luong = (int)(double.Parse(listItemDetailPay[i].money) /
                                    listItemDetailPay[i].kq_luong * 100);
                                PropertyChanged.Invoke(listItemDetailPay[i], new PropertyChangedEventArgs("pc_luong"));
                            }
                        }
                    }
                }
            }

            dem++;
        }

        private int dem1 = 0;

        private void Percent(object sender, TextChangedEventArgs e)
        {
            if (dem1 >= listItemDetailPay.Count)
            {
                TextBox t = sender as TextBox;
                if (t.IsFocused)
                {
                    Item_detail_pay data = (Item_detail_pay)t.DataContext;
                    if (!string.IsNullOrEmpty(t.Text))
                    {
                        for (int i = 0; i < listItemDetailPay.Count; i++)
                        {
                            listItemDetailPay[i].pc_luong = Convert.ToInt32(t.Text);
                            if (listItemDetailPay[i].ep_id == data.ep_id && listItemDetailPay[i].pc_luong != 0)
                            {
                                listItemDetailPay[i].money =
                                    (listItemDetailPay[i].kq_luong / 100 * listItemDetailPay[i].pc_luong).ToString();
                                PropertyChanged.Invoke(listItemDetailPay[i], new PropertyChangedEventArgs("money"));
                            }
                        }
                    }
                }
            }

            dem1++;
        }
    }
}