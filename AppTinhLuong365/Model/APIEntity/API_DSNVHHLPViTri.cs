using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVHHLPViTri
    {
        public List<DSNVHHLPViTri> list { get; set; }
        public string message { get; set; }
    }

    public class DSNVHHLPViTri
    {
        public string ro_id { get; set; }
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string ro_time { get; set; }
        public string display_ro_time
        {
            get
            {
                string result = "Tháng " + DateTime.Parse(ro_time).ToString("MM/yyyy");
                return result;
            }
        }
        public string tl_id { get; set; }
        public string tl_name { get; set; }
        public string ro_so_luong { get; set; }
        public string ro_note { get; set; }
        public string sum_dt { get; set; }
        public string display_sum_dt
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(sum_dt) >= 0)
                {
                    double m;
                    if (double.TryParse(sum_dt, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(sum_dt.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public List<ArrRVT> arr_rdt { get; set; }
    }

    public class ArrRVT
    {
        public int id { get; set; }
        public string dt_id { get; set; }
        public string dt_rose_id { get; set; }
        public string dt_money { get; set; }
        public string dt_sl { get; set; }
        public string dt_time { get; set; }
    }

    public class API_DSNVHHLPViTri
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVHHLPViTri data { get; set; }
        public object error { get; set; }
    }
}
