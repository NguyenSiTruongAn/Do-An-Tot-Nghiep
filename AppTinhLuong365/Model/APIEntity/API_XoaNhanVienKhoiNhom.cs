using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Error
    {
        public string message { get; set; }
    }

    public class API_XoaNhanVienKhoiNhom
    {
        public bool result { get; set; }
        public int code { get; set; }
        public object data { get; set; }
        public Error error { get; set; }
    }
}
