using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVPQ1Nguoi
    {
        public DataDSNVPQ1Nguoi data { get; set; }
        public string message { get; set; }
        public ListEmployee items { get; set; }
    }


    public class API_DSNVPQ1Nguoi
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVPQ1Nguoi data { get; set; }
        public object error { get; set; }
    }
}
