using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Add_late
    {
        public string message { get; set; }
    }

    public class API_Add_late
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Add_late data { get; set; }
        public object error { get; set; }
    }
}
