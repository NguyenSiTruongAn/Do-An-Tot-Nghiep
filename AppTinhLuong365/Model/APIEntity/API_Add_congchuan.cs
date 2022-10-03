using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    public class Add_congchuan
    {
        public string message { get; set; }
    }

    public class API_Add_congchuan
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Add_congchuan data { get; set; }
        public object error { get; set; }
    }
}
