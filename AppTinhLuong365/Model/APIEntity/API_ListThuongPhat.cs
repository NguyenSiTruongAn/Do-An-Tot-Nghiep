using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public List<ListThuongPhat> thuong_phat { get; set; }
        public int total { get; set; }
        public string message { get; set; }
    }

    public class API_ListThuongPhat
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Data data { get; set; }
        public object error { get; set; }
    }

    public class DtThuong
    {
        public string pay_id { get; set; }
        public string pay_price { get; set; }
        public string display_pay_price
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(pay_price) >= 0)
                {
                    double m;
                    if (double.TryParse(pay_price, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(pay_price.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string pay_case { get; set; }
        public string pay_day { get; set; }
        public string Display_pay_day
        {
            get
            {
                string result = DateTime.Parse(pay_day).ToString("dd-MM-yyyy");
                return result;
            }
        }
    }

    public class ListThuongPhat
    {
        public string id { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public string thuong { get; set; }
        public string phat { get; set; }
        public List<DtThuong> dt_thuong { get; set; }
        public List<DtThuong> dt_phat { get; set; }
    }

}
