using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSTongHoaHongTien
    {
        public List<DSTongHoaHongTien> list { get; set; }
        public string message { get; set; }
    }

    public class DSTongHoaHongTien
    {
        public int ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string time { get; set; }
        public string Display_time
        {
            get
            {
                string result = "Tháng" + DateTime.Now.ToString("MM/yyyy");
                return result;
            }
        }
        public string money { get; set; }
    }

    public class API_DSTongHoaHongTien
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSTongHoaHongTien data { get; set; }
        public object error { get; set; }
    }
}
