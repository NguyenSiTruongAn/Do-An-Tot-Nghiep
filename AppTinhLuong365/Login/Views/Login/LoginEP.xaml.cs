﻿using AppTinhLuong365.Login.Entities;
using AppTinhLuong365.Model.ConnectToserver;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

namespace AppTinhLuong365.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginCom.xaml
    /// </summary>
    public partial class LoginEP : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginEP(LoginWindow login)
        {
            InitializeComponent();
            this.DataContext = this;
            WinLogin = login;
            WinLogin.Title = "Đăng nhập nhân viên";

            if (!string.IsNullOrEmpty(AppTinhLuong365.Properties.Settings.Default.EpEmail) || !string.IsNullOrEmpty(AppTinhLuong365.Properties.Settings.Default.EpPass))
            {
                ckSave.IsChecked = true;
                txtEmail.Text = AppTinhLuong365.Properties.Settings.Default.EpEmail;
                TypeLogin = int.Parse(AppTinhLuong365.Properties.Settings.Default.EpTypePass);
                if(TypeLogin == 0)
                    txtPass.Password = AppTinhLuong365.Properties.Settings.Default.EpPass;
                TypeLogin = 1;
                txtPass.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(txtPass, new object[] { Pass.Length, 0 });
                txtPass.Focus();
            }
            else txtEmail.Focus();
            
        }
        //
        private int _TypeLogin = 1;

        public int TypeLogin
        {
            get { return _TypeLogin; }
            set { _TypeLogin = value; OnPropertyChanged(); }
        }

        private string _IdQR;

        public string IdQR
        {
            get { return _IdQR; }
            set { _IdQR = value; OnPropertyChanged("IdQR"); }
        }
        public double? latitude { get; set; }
        public double? longitude { get; set; }

        private string _TypeName;
        public ConnectSocket Socket { get; set; }
        public string TypeName
        {
            get { return _TypeName; }
            set { _TypeName = value; OnPropertyChanged("TypeName"); }
        }
        public LoginWindow WinLogin { get; set; }

        private string _Pass = "";
        public string Pass
        {
            get { return _Pass; }
            set { _Pass = value; OnPropertyChanged(); }
        }

        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex regexSDT = new Regex(@"^((09|03|07|08|05|04)+([0-9]{8})\b)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        //
        private void SignIn(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://quanlychung.timviec365.vn/dang-ky-nhan-vien.html");
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
                    AppTinhLuong365.Properties.Settings.Default.EpEmail = txtEmail.Text;
                    AppTinhLuong365.Properties.Settings.Default.Save();
                }
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("email", txtEmail.Text);
                    form.Add("pass", Pass);
                    HttpClient httpClient = new HttpClient();
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    var respon = await httpClient.PostAsync("https://tinhluong.timviec365.vn/api_app/employe/login_emp.php", new FormUrlEncodedContent(form));
                    AppTinhLuong365.Model.APIEntity.API_Login_Employee api = JsonConvert.DeserializeObject<AppTinhLuong365.Model.APIEntity.API_Login_Employee>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null)
                    {
                        if (ckSave.IsChecked == true)
                        {
                            AppTinhLuong365.Properties.Settings.Default.EpPass = Pass;
                            AppTinhLuong365.Properties.Settings.Default.EpTypePass = TypeLogin.ToString();
                            AppTinhLuong365.Properties.Settings.Default.Save();
                        }
                        if (TypeLogin == 0)
                        {
                            api.data.pass = api.data.ToMD5(Pass);
                        }
                        else
                            api.data.pass = Pass;
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
                        n.setMessage(api.error.message);
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
            if (e.Key == Key.Enter)
            {
                runLogin();
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
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
            AppTinhLuong365.Properties.Settings.Default.EpEmail = "";
            AppTinhLuong365.Properties.Settings.Default.EpPass = "";
            AppTinhLuong365.Properties.Settings.Default.Save();
        }

        private void ForgotPass(object sender, MouseButtonEventArgs e)
        {
            /*var z = new ForgotPasswordWindow(WinLogin);
            z.Type = 1;

            z.IsFull = WinLogin.IsFull;
            z.Width = WinLogin.ActualWidth;
            z.Height = WinLogin.ActualHeight;
            z.Left = WinLogin.Left;
            z.Top = WinLogin.Top;

            WinLogin.Hide();
            z.Show();*/
            Process.Start("https://quanlychung.timviec365.vn/quen-mat-khau.html?type=1");
        }
    }
}
