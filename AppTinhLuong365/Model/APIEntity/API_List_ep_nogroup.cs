using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class API_List_ep_nogroup
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataList_ep_nogroup data { get; set; }
        public object error { get; set; }
    }

    public class DataList_ep_nogroup
    {
        public List<ListEpNoGroup> list { get; set; }
        public string total { get; set; }
        public string message { get; set; }
    }

    public class ListEpNoGroup
    {
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string ep_id { get; set; }
        public string dep_name { get; set; }
    }
}
