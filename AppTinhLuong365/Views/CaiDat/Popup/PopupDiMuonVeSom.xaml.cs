using System;
using AppTinhLuong365.Model.APIEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupDiMuonVeSom.xaml
    /// </summary>
    public partial class PopupDiMuonVeSom : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PopupDiMuonVeSom(MainWindow main)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            getData();
        }

        MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
        }

        private List<Item_shift> _listShift;

        public List<Item_shift> listShift
        {
            get { return _listShift; }
            set
            {
                if (value == null) value = new List<Item_shift>();
                value.Insert(0, new Item_shift() { shift_id = "-1", shift_name = "Tất cả các ca" });
                _listShift = value;
                OnPropertyChanged();
            }
        }

        private void getData()
        {
            using (WebClient web = new WebClient())
            {
                web.Headers.Add("Authorization", Main.CurrentCompany.token);
                web.QueryString.Add("id_comp", Main.CurrentCompany.com_id);
                web.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        API_List_shift api =
                        JsonConvert.DeserializeObject<API_List_shift>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.data != null)
                        {
                            listShift = api.data.items;
                        }
                    }
                    catch { }
                    // foreach (EpLate item in listEpLate)
                    // {
                    //     if (item.ts_image != "/img/add.png")
                    //     {
                    //         item.ts_image = "https://chamcong.24hpay.vn/image/time_keeping/" + item.ts_image;
                    //     }
                    // }
                };
                web.UploadValuesTaskAsync("https://chamcong.24hpay.vn/service/list_shift.php", web.QueryString);
            }
        }

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

        private void Select_thang(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth.Visibility = dteSelectedMonth.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
            flag = 1;
        }

        int flag = 0;
        int flag1 = 0;

        private void dteSelectedMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (flag == 0)
                x = "";
            else
                x = dteSelectedMonth.DisplayDate.ToString("MM/yyyy");
            if (textThang != null && !string.IsNullOrEmpty(x))
            {
                textThang.Text = x;
            }

            dteSelectedMonth.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth.DisplayDate != null && flag > 0)
            {
                dteSelectedMonth.Visibility = Visibility.Collapsed;
            }

            flag += 1;
        }

        private void Select_thang_end(object sender, MouseButtonEventArgs e)
        {
            dteSelectedMonth1.Visibility = dteSelectedMonth1.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
            flag1 = 1;
        }

        private void dteSelectedMonth_DisplayModeChanged1(object sender, CalendarModeChangedEventArgs e)
        {
            var x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (flag1 == 0)
                x = "";
            else
                x = dteSelectedMonth1.DisplayDate.ToString("MM/yyyy");
            if (TextThang != null && !string.IsNullOrEmpty(x))
            {
                TextThang.Text = x;
            }

            dteSelectedMonth1.DisplayMode = CalendarMode.Year;
            if (dteSelectedMonth1.DisplayDate != null && flag1 > 0)
            {
                dteSelectedMonth1.Visibility = Visibility.Collapsed;
            }

            flag1 += 1;
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            validatePhat.Text = validateMinute.Text =
                validateTypePhat.Text = validateMoney.Text = validateStartTime.Text = "";
            if (BoxPhat.Text == "Chọn lý do")
            {
                allow = false;
                validatePhat.Text = "Vui lòng chọn phương thức";
            }

            if (string.IsNullOrEmpty(tbInput.Text))
            {
                allow = false;
                validateMinute.Text = "Vui lòng nhập số phút để tính phạt";
            }

            if (BoxTypePhat.Text == "Chọn phương thức")
            {
                allow = false;
                validateTypePhat.Text = "Vui lòng chọn phương thức";
            }

            if (string.IsNullOrEmpty(tbInput1.Text))
            {
                allow = false;
                validateMoney.Text = "Vui lòng nhập công hoặc tiền";
            }

            if (textThang.Text == "--------- ----")
            {
                allow = false;
                validateStartTime.Text = "Vui lòng chọn thời gian áp dụng";
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

                    string i;
                    if (BoxPhat.Text == "Đi muộn")
                    {
                        i = "1";
                    }
                    else if (BoxPhat.Text == "Về sớm")
                    {
                        i = "2";
                    }
                    else i = "0";

                    web.QueryString.Add("pm_type", i);
                    if (cbDate.SelectedItem != null)
                    {
                        Item_shift selectedShift = cbDate.SelectedItem as Item_shift;
                        if (selectedShift != null && selectedShift.shift_id != "-1")
                        {
                            web.QueryString.Add("pm_shift", cbDate.SelectedValue.ToString());
                        }
                        else
                        {
                        }
                    }

                    string y;
                    if (BoxTypePhat.Text == "Phạt tiền")
                    {
                        y = "1";
                    }
                    else if (BoxTypePhat.Text == "Phạt công")
                    {
                        y = "2";
                    }
                    else y = "0";

                    web.QueryString.Add("pm_type_phat", y);
                    web.QueryString.Add("pm_monney", tbInput1.Text);
                    web.QueryString.Add("pm_minute", tbInput.Text);
                    string day_Start = "";
                    if (textThang.Text != "--------- ----")
                        day_Start = dteSelectedMonth.DisplayDate.ToString("yyyy-MM");
                    string day_End = "";
                    if (textThang.Text != "--------- ----")
                        day_End = dteSelectedMonth1.DisplayDate.ToString("yyyy-MM");
                    else
                    {
                        web.QueryString.Add("date_finish", day_End);
                    }

                    web.QueryString.Add("date_start", day_Start);
                    web.UploadValuesCompleted += (s, ee) =>
                    {
                        try
                        {
                            string a = UnicodeEncoding.UTF8.GetString(ee.Result);
                            API_Add_late api =
                                JsonConvert.DeserializeObject<API_Add_late>(
                                    UnicodeEncoding.UTF8.GetString(ee.Result));
                            if (api.data != null)
                            {
                                Main.HomeSelectionPage.NavigationService.Navigate(new Views.CaiDat.DiMuonVeSom(Main));
                                Main.PopupSelection.NavigationService.Navigate(null);Main.PopupSelection.Visibility = Visibility.Hidden;
                            }
                        }
                        catch { }
                    };
                    web.UploadValuesTaskAsync("https://tinhluong.timviec365.vn/api_app/company/add_late.php",
                        web.QueryString);
                }

                
            }
        }
    }
}