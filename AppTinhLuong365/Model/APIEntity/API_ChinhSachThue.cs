using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataChinhSachThue
    {
        public List<ChinhSachThue> tax_list { get; set; }
        public string message { get; set; }
    }

    public class API_ChinhSachThue
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataChinhSachThue data { get; set; }
        public object error { get; set; }
    }

    public class ChinhSachThue
    {
        public string cl_id { get; set; }
        public string cl_name { get; set; }
        public string cl_active { get; set; }
        public string cl_note { get; set; }
        public string fs_repica { get; set; }
        public string fs_name { get; set; }
        public string fs_note { get; set; }
        public string fs_data { get; set; }
        public string fs_type { get; set; }
    }
}
