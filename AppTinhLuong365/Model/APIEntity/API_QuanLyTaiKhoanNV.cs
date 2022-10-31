using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataQuanLyTaiKhoanNV
    {
        public string sum_day { get; set; }
        public string work_day { get; set; }
        public string sum_late { get; set; }
        public string form_pending { get; set; }
        public string message { get; set; }
    }

    public class API_QuanLyTaiKhoanNV
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataQuanLyTaiKhoanNV data { get; set; }
        public object error { get; set; }
    }
}
