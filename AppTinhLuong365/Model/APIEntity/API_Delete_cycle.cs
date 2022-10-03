using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Delete_cycle
    {
        public bool result { get; set; }
        public string message { get; set; }
        public int id { get; set; }
    }

    public class API_Delete_cycle
    {
        public Delete_cycle data { get; set; }
        public object error { get; set; }
    }
}
