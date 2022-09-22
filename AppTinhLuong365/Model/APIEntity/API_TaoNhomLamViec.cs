using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    public class DataTaoNhomLamViec
    {
        public string message { get; set; }
    }

    public class API_TaoNhomLamViec
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataTaoNhomLamViec data { get; set; }
        public object error { get; set; }
    }
}
