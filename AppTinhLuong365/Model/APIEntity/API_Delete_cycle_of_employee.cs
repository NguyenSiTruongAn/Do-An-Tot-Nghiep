using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Delete_cycle_of_employee
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }

    public class API_Delete_cycle_of_employee
    {
        public Delete_cycle_of_employee data { get; set; }
        public object error { get; set; }
    }


}
