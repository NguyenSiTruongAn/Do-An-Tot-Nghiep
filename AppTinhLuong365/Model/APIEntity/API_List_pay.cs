using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_pay
    {
        public List<Item_pay> list { get; set; }
        public string message { get; set; }
    }

    public class Item_pay
    {
        public string pay_id { get; set; }
        public string pay_com { get; set; }
        public string pay_name { get; set; }
        public string pay_unit { get; set; }

        public string display_pay_unit
        {
            get
            {
                string a = "";
                if (pay_unit == "1")
                {
                    a = "Tiền mặt";
                }
                else
                {
                    a = "Chuyển khoản";
                }
                return a;
            }
        }

        public string pay_for_time { get; set; }
        public string pay_time_start { get; set; }
        public string pay_time_end { get; set; }
        public string pay_status { get; set; }

        public string display_pay_status
        {
            get
            {
                string b = "";
                if (pay_status == "1")
                {
                    b = "Thanh toán toàn bộ";
                }
                else
                {
                    b = "Chưa thanh toán";
                }
                return b;
            }
        }

        public string pay_for { get; set; }
        public string name_pay_for { get; set; }
    }

    public class API_List_pay
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_pay data { get; set; }
        public object error { get; set; }
    }
}
