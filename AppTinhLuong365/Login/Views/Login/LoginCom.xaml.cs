using AppTinhLuong365.Login.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
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

namespace AppTinhLuong365.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginCom.xaml
    /// </summary>
    public partial class LoginCom : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginCom(LoginWindow login)
        {
            InitializeComponent();
            this.DataContext = this;
            WinLogin = login;
            WinLogin.Title = "Đăng nhập công ty";

            if (!string.IsNullOrEmpty(AppTinhLuong365.Properties.Settings.Default.ComEmail) || !string.IsNullOrEmpty(AppTinhLuong365.Properties.Settings.Default.ComPass))
            {
                ckSave.IsChecked = true;
                txtEmail.Text = AppTinhLuong365.Properties.Settings.Default.ComEmail;
                txtPass.Password = AppTinhLuong365.Properties.Settings.Default.ComPass;
                txtPass.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(txtPass, new object[] { Pass.Length, 0 });
                txtPass.Focus();
            }
            else txtEmail.Focus();
        }
        //
        public LoginWindow WinLogin { get; set; }
        private string _Pass = "";
        public string Pass
        {
            get { return _Pass; }
            set { _Pass = value; OnPropertyChanged(); }
        }
        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex regexSDT = new Regex(@"0(1\d{9}|9\d{8})", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        //
        private void SignIn(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://quanlychung.timviec365.vn/dang-ky-cong-ty.html");
        }
        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            WinLogin.Close();

            WinLogin.WinMain.IsFull = WinLogin.IsFull;
            WinLogin.WinMain.Width = WinLogin.ActualWidth;
            WinLogin.WinMain.Height = WinLogin.ActualHeight;
            WinLogin.WinMain.Left = WinLogin.Left;
            WinLogin.WinMain.Top = WinLogin.Top;

            WinLogin.WinMain.Show();
        }
        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Pass = txtPass.Password;
            tblValidatePass.Text = "";
            if (string.IsNullOrEmpty(Pass))
            {
                tblValidatePass.Text = "Không được để trống";
            }

        }
        private async void runLogin()
        {
            bool allow = true;
            tblValidateEmail.Text = tblValidatePass.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Nhập không đúng định dạng";
            }
            if (string.IsNullOrEmpty(Pass))
            {
                allow = false;
                tblValidatePass.Text = "Không được để trống";
            }
            if (allow)
            {
                if (ckSave.IsChecked == true)
                {
                    AppTinhLuong365.Properties.Settings.Default.ComEmail = txtEmail.Text;
                    AppTinhLuong365.Properties.Settings.Default.Save();
                }
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("email", txtEmail.Text);
                    form.Add("pass", Pass);

                    HttpClient httpClient = new HttpClient();
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    var respon = await httpClient.PostAsync("https://tinhluong.timviec365.vn/api_app/company/login_comp.php", new FormUrlEncodedContent(form));
                    AppTinhLuong365.Model.APIEntity.API_Login_Company api = JsonConvert.DeserializeObject<AppTinhLuong365.Model.APIEntity.API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null)
                    {
                        if (ckSave.IsChecked == true)
                        {
                            AppTinhLuong365.Properties.Settings.Default.ComPass = Pass;
                            AppTinhLuong365.Properties.Settings.Default.Save();
                        }

                        var main = new AppTinhLuong365.MainWindow(api);

                        main.IsFull = WinLogin.IsFull;
                        main.Width = WinLogin.Width;
                        main.Height = WinLogin.Height;
                        main.Left = WinLogin.Left;
                        main.Top = WinLogin.Top;

                        main.LogOut = () =>
                        {
                            main.Close();
                            WinLogin.LogOut(main.IsFull, main);
                        };

                        WinLogin.Hide();
                        main.ShowDialog();
                    }
                    else
                    {
                        var n = new Notify();
                        n.Type = Notify.NotifyType.Error;
                        n.setMessage("Đăng nhập không thành công");
                        WinLogin.ShowPopup(n);
                    }
                }
                catch (Exception ex)
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Error;
                    n.setMessage(ex.Message);
                    WinLogin.ShowPopup(n);
                }
            }
        }
        private void Login(object sender, MouseButtonEventArgs e)
        {
            runLogin();
        }
        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                runLogin();
            }
        }
        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            tblValidateEmail.Text ="";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                tblValidateEmail.Text = "Nhập không đúng định dạng";
            }
        }
        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            tblValidatePass.Text = "";
            if (string.IsNullOrEmpty(Pass))
            {
                tblValidatePass.Text = "Không được để trống";
            }
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            tblValidateEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                tblValidateEmail.Text = "Nhập không đúng định dạng";
            }
        }
        private void ckSave_Unchecked(object sender, RoutedEventArgs e)
        {
            AppTinhLuong365.Properties.Settings.Default.ComEmail ="";
            AppTinhLuong365.Properties.Settings.Default.ComPass = "";
            AppTinhLuong365.Properties.Settings.Default.Save();
        }
        private void ForgotPass(object sender, MouseButtonEventArgs e)
        {
            var z = new ForgotPasswordWindow(WinLogin);
            z.Type = 2;

            z.IsFull = WinLogin.IsFull;
            z.Width = WinLogin.ActualWidth;
            z.Height = WinLogin.ActualHeight;
            z.Left = WinLogin.Left;
            z.Top = WinLogin.Top;

            WinLogin.Hide();
            z.Show();
        }
    }
}
