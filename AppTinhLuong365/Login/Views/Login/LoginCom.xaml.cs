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
//using System.Windows.Shapes;

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
                if(AppTinhLuong365.Properties.Settings.Default.ComTypePass == "0")
                    txtPass.Password = AppTinhLuong365.Properties.Settings.Default.ComPass;
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
            else if (!regex.IsMatch(txtEmail.Text))
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
                            AppTinhLuong365.Properties.Settings.Default.ComTypePass = TypeLogin.ToString();
                            AppTinhLuong365.Properties.Settings.Default.Save();
                        }
                        
                        if(TypeLogin == 0)
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
            else if (!regex.IsMatch(txtEmail.Text))
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
            else if (!regex.IsMatch(txtEmail.Text))
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
            /*var z = new ForgotPasswordWindow(WinLogin);
            z.Type = 2;

            z.IsFull = WinLogin.IsFull;
            z.Width = WinLogin.ActualWidth;
            z.Height = WinLogin.ActualHeight;
            z.Left = WinLogin.Left;
            z.Top = WinLogin.Top;

            WinLogin.Hide();
            z.Show();*/
            Process.Start("https://quanlychung.timviec365.vn/quen-mat-khau.html?type=2");
        }
        
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public class infoQR
        {
            public infoQR(string qRType, string idQR, string idComputer, string nameComputer, double? latitude, double? longitude, string time)
            {
                QRType = qRType;
                this.idQR = idQR;
                IdComputer = idComputer;
                NameComputer = nameComputer;
                this.latitude = latitude;
                this.longitude = longitude;
                Time = time;
            }

            public string QRType { get; set; }
            public string idQR { get; set; }
            public string IdComputer { get; set; }
            public string NameComputer { get; set; }
            public double? latitude { get; set; }
            public double? longitude { get; set; }
            public string Time { get; set; }

        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private void connectionSocket()
        {
            try
            {
                Socket = new ConnectSocket();
                Socket.WIO.On("QRLogin", response =>
                {
                    try
                    {
                        string QrId = response.GetValue<string>(0);
                        string Email = response.GetValue<string>(1).Replace("+", "");
                        Email = Base64Decode(Email);
                        string Password = response.GetValue<string>(2).Replace("+", "");
                        Password = Base64Decode(Password);

                        if (QrId == IdQR)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                Pass = Password;
                                txtEmail.Text = Email;
                                Properties.Settings.Default.ComEmail = Email;
                                Properties.Settings.Default.ComPass = Password;
                                Properties.Settings.Default.ComTypePass = "1";
                                Properties.Settings.Default.Save();
                                runLogin();
                            });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
