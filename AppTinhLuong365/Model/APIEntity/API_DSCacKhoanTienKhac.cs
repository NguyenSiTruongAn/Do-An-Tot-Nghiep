using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSCacKhoanTienKhac
    {
        public List<DSCacKhoanTienKhac> other_list { get; set; }
        public string message { get; set; }
    }

    public class DSCacKhoanTienKhac
    {
        public string cl_id { get; set; }
        public string cl_name { get; set; }
        public object cl_salary { get; set; }
        public string cl_active { get; set; }
        public string cl_note { get; set; }
        public string cl_type { get; set; }
        public string cl_id_form { get; set; }
        public string cl_com { get; set; }
        public string fs_repica { get; set; }
    }

    public class API_DSCacKhoanTienKhac
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSCacKhoanTienKhac data { get; set; }
        public object error { get; set; }
    }
}
