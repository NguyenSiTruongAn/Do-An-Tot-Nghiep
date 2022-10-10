using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSCaiDatHoaHongVT
    {
        public List<DSCaiDatHoaHongVT> list { get; set; }
        public string message { get; set; }
    }

    public class DSCaiDatHoaHongVT
    {
        public string STT { get; set; }
        public string tl_name { get; set; }
        public string tl_money_min { get; set; }
        public string tl_id { get; set; }
    }

    public class API_DSCaiDatHoaHongVT
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSCaiDatHoaHongVT data { get; set; }
        public object error { get; set; }
    }
}
