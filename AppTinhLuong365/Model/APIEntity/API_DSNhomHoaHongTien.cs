using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNhomHoaHongTien
    {
        public List<DSNhomHoaHongTien> list_rose_gr { get; set; }
        public string message { get; set; }
    }

    public class DSNhomHoaHongTien
    {
        public string ro_id_group { get; set; }
        public string ro_price { get; set; }
        public string ro_time { get; set; }
        public string display_ro_time
        {
            get
            {
                string result = DateTime.Parse(ro_time).ToString("dd/MM/yyy");
                return result;
            }
        }
        public string ro_note { get; set; }
        public string ro_id { get; set; }
        public string lgr_name { get; set; }
        public string count_member_gr { get; set; }
    }

    public class API_DSNhomHoaHongTien
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNhomHoaHongTien data { get; set; }
        public object error { get; set; }
    }
}
