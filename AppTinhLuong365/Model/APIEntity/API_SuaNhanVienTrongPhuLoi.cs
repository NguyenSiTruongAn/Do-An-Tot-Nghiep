using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataSuaNhanVienTrongPhuLoi
    {
        public string message { get; set; }
    }

    public class API_SuaNhanVienTrongPhuLoi
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataSuaNhanVienTrongPhuLoi data { get; set; }
        public object error { get; set; }
    }
}
