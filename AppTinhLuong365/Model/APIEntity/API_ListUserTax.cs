using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ListUserTax
    {
        public List<ItemUserTax> list { get; set; }
        public string message { get; set; }
    }

    public class ItemUserTax
    {
        public string cls_id { get; set; }
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string cls_id_user { get; set; }
        public string dep_name { get; set; }
        public string cl_name { get; set; }
        public string fs_repica { get; set; }
        public string cls_day { get; set; }
        public object cls_day_end { get; set; }
    }

    public class API_ListUserTax
    {
        public bool result { get; set; }
        public int code { get; set; }
        public ListUserTax data { get; set; }
        public object error { get; set; }
    }

}
