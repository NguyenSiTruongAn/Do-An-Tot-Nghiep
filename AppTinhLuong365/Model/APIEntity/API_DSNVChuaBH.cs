using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVChuaBH
    {
        public List<DSNVChuaBH> list { get; set; }
        public string message { get; set; }
    }

    public class DSNVChuaBH
    {
        public string ep_name { get; set; }
        public string ep_id { get; set; }
        public string dep_name { get; set; }
        public string ep_image { get; set; }
    }

    public class API_DSNVChuaBH
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVChuaBH data { get; set; }
        public object error { get; set; }
    }
}
