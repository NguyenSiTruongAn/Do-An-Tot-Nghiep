using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVHoaHongDoanhThu
    {
        public List<DSNVHoaHongDoanhThu> list_rose { get; set; }
        public string message { get; set; }
    }

    public class DSNVHoaHongDoanhThu
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
        public string ro_note { get; set; }
        public string persent { get; set; }
        public string ro_price { get; set; }
        public List<RoDtThoiDiem> ro_dt_thoi_diem { get; set; }
    }

    public class RoDtThoiDiem
    {
        public string dt_rose_id { get; set; }
        public int id { get; set; }
        public string dt_money { get; set; }
        public string dt_time { get; set; }
    }

    public class API_DSNVHoaHongDoanhThu
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVHoaHongDoanhThu data { get; set; }
        public object error { get; set; }
    }
}
