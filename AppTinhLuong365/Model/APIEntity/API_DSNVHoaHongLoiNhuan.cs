using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVHoaHongLoiNhuan
    {
        public List<DSNVHoaHongLoiNhuan> list_rose { get; set; }
        public string message { get; set; }
    }

    public class ArrRdt
    {
        public int id { get; set; }
        public string dt_id { get; set; }
        public string dt_rose_id { get; set; }
        public string dt_money { get; set; }
        public string dt_sl { get; set; }
        public string dt_time { get; set; }
    }

    public class DSNVHoaHongLoiNhuan
    {
        public string ro_id { get; set; }
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string tl_name { get; set; }
        public string ro_id_tl { get; set; }
        public string ro_so_luong { get; set; }
        public string ro_price { get; set; }
        public string ro_note { get; set; }
        public string ro_time { get; set; }
        public string Display_ro_time
        {
            get
            {
                string result = "Tháng " + DateTime.Parse(ro_time).ToString("MM/yyyy");
                return result;
            }
        }
        public int money { get; set; }
        public List<ArrRdt> arr_rdt { get; set; }
    }

    public class API_DSNVHoaHongLoiNhuan
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVHoaHongLoiNhuan data { get; set; }
        public object error { get; set; }
    }
}
