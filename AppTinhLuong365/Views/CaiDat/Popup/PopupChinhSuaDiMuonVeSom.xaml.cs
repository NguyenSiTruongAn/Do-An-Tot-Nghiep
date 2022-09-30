using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace AppTinhLuong365.Views.CaiDat.Popup
{
    /// <summary>
    /// Interaction logic for PopupChinhSuaDiMuonVeSom.xaml
    /// </summary>
    public partial class PopupChinhSuaDiMuonVeSom : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        public PopupChinhSuaDiMuonVeSom(MainWindow main , string dataPmId, string dataPmShift, string dataShiftName, string dataPmMinute, int dataPmType, int dataPmTypePhat, string dataPmMonney, string dataPmTimeBegin, string dataPmTimeEnd)
        {
            this.DataContext = this;
            InitializeComponent();
            Main = main;
            id = dataPmId;
            BoxPhat.SelectedIndex = dataPmType;
            cbDate.SelectedValue = dataPmShift;
            tbInput.Text = dataPmMinute;
        }

        private MainWindow Main;

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
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
            throw new NotImplementedException();
        }
    }
}