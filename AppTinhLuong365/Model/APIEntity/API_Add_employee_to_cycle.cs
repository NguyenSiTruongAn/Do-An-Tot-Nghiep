using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Add_employee_to_cycle
    {
        public bool result { get; set; }
        public string message { get; set; }
    }

    public class API_Add_employee_to_cycle
    {
        public Add_employee_to_cycle data { get; set; }
        public object error { get; set; }
    }
}
