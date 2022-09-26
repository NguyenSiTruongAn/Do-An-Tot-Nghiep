using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_employee_not_in_cycle
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_employee_not_in_cycle> items { get; set; }
    }

    public class Item_employee_not_in_cycle
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string ep_gender { get; set; }
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
    }

    public class API_List_employee_not_in_cycle
    {
        public List_employee_not_in_cycle data { get; set; }
        public object error { get; set; }
    }
}
