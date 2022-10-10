using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSCaiDatHoaHongKeHoach
    {
        public List<DSCaiDatHoaHongKeHoach> list { get; set; }
        public string message { get; set; }
    }

    public class DSCaiDatHoaHongKeHoach
    {
        public string STT { get; set; }
        public string tl_name { get; set; }
        public string tl_kpi_yes { get; set; }
        public string tl_kpi_no { get; set; }
        public string tl_id { get; set; }
    }

    public class API_DSCaiDatHoaHongKeHoach
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSCaiDatHoaHongKeHoach data { get; set; }
        public object error { get; set; }
    }
}
