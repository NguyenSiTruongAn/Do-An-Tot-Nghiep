using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSCaiDatHoaHongDoanhThu
    {
        public List<DSCaiDatHoaHongDoanhThu> rose_dt { get; set; }
        public string message { get; set; }
    }

    public class API_DSCaiDatHoaHongDoanhThu
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSCaiDatHoaHongDoanhThu data { get; set; }
        public object error { get; set; }
    }

    public class DSCaiDatHoaHongDoanhThu
    {
        public string STT { get; set; }
        public string tl_id { get; set; }
        public string tl_id_com { get; set; }
        public string tl_id_rose { get; set; }
        public string tl_name { get; set; }
        public string tl_money_min { get; set; }
        public string tl_money_max { get; set; }
        public string tl_phan_tram { get; set; }
        public object tl_chiphi { get; set; }
        public object tl_hoahong { get; set; }
        public object tl_kpi_yes { get; set; }
        public string tl_kpi_no { get; set; }
        public string tl_time_created { get; set; }
    }
}
