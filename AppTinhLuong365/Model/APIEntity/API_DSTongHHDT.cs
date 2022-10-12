using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSTongHHDT
    {
        public List<DSTongHHDT> list { get; set; }
        public string message { get; set; }
    }

    public class DSTongHHDT
    {
        public int ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string time { get; set; }
        public double money { get; set; }
    }

    public class API_DSTongHHDT
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSTongHHDT data { get; set; }
        public object error { get; set; }
    }
}
