using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_dep
    {
        public List<Item_dep> list { get; set; }
        public string message { get; set; }
    }

    public class Item_dep
    {
        public string dep_id { get; set; }
        public string com_id { get; set; }
        public string dep_name { get; set; }
        public string dep_create_time { get; set; }
        public List<Manager> manager { get; set; }
        public List<object> deputy { get; set; }
        public string total_emp { get; set; }
    }

    public class Manager
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }

    public class API_List_dep
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_dep data { get; set; }
        public object error { get; set; }
    }
}
