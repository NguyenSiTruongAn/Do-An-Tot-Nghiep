using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSTongHHLoiNhuan
    {
        public List<DSTongHHLoiNhuan> list { get; set; }
        public string message { get; set; }
    }

    public class DSTongHHLoiNhuan
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string time { get; set; }
        public string Display_time
        {
            get
            {
                string result = "Tháng " + DateTime.Parse(time).ToString("MM/yyyy");
                return result;
            }
        }
        public string money { get; set; }
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
    }

    public class API_DSTongHHLoiNhuan
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSTongHHLoiNhuan data { get; set; }
        public object error { get; set; }
    }
}
