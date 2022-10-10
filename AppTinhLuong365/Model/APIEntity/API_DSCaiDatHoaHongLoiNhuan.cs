using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSCaiDatHoaHongLoiNhuan
    {
        public List<DSCaiDatHoaHongLoiNhuan> list { get; set; }
        public string message { get; set; }
    }

    public class DSCaiDatHoaHongLoiNhuan
    {
        public string STT { get; set; }
        public string tl_name { get; set; }
        public string tl_chiphi { get; set; }
        public string tl_id { get; set; }
    }

    public class API_DSCaiDatHoaHongLoiNhuan
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSCaiDatHoaHongLoiNhuan data { get; set; }
        public object error { get; set; }
    }
}
