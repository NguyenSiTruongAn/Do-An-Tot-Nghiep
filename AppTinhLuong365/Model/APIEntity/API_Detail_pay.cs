using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Detail_pay
    {
        public string pay_name { get; set; }
        public string pay_status { get; set; }

        public string display_pay_status
        {
            get
            {
                string text = "";
                if (pay_status == "1")
                {
                    text = "Thanh toán toàn bộ";
                }
                else if (pay_status == "2")
                {
                    text = "Thanh toán một phần";
                }
                else
                {
                    text = "Chưa thanh toán";
                }

                return text;
            }
        }

        public int page { get; set; }
        public int total { get; set; }
        public List<Item_detail_pay> list { get; set; }
        public string message { get; set; }
    }

    public class Item_detail_pay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ep_id { get; set; }
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string pay_for_time { get; set; }
        public double kq_luong { get; set; }

        public string display_kq_luong
        {
            get
            {
                string a = "";
                if (kq_luong >= 0)
                {
                    double m;
                    if (double.TryParse(kq_luong.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(kq_luong.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        // public string money { get; set; }
        // public int pc_luong { get; set; }

        private string _money;

        public string money
        {
            get
            {
                long a = 0;
                if (Convert.ToDouble(_money) >= 0)
                {
                    long x = (long)Convert.ToDouble(_money) / 1;
                    a = long.Parse(x + "");
                }

                return _money = a + "";
            }
            set
            {
                _money = value;
                //OnPropertyChanged();
            }
        }

        public string display_money
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(money) >= 0)
                {
                    double m;
                    if (double.TryParse(money, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(money.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }

        public long display_money_long
        {
            get
            {
                long a = 0;
                if (Convert.ToInt64(money) >= 0)
                {
                    a = Convert.ToInt64(money);
                }

                return a;
            }
        }

        private int _pc_luong;

        public int pc_luong
        {
            get { return _pc_luong; }
            set
            {
                _pc_luong = value;
                //OnPropertyChanged();
            }
        }

        public string cn_luong { get; set; }

        public string display_cn_luong
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(cn_luong) >= 0)
                {
                    double m;
                    if (double.TryParse(cn_luong, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(cn_luong.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }

        private Boolean _status;

        public Boolean status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        private bool _validatepc = false;
        public bool validatepc
        {
            get { return _validatepc;}
            set
            {
                _validatepc = value; OnPropertyChanged();
            } }
    }

    public class API_Detail_pay
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Detail_pay data { get; set; }
        public object error { get; set; }
    }
}