using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    public class DataThemNhanVienVaoNhom
    {
        public string message { get; set; }
    }

    public class API_ThemNhanVienVaoNhom
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataThemNhanVienVaoNhom data { get; set; }
        public object error { get; set; }
    }
}
