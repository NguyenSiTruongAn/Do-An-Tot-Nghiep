using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data_user_group
    {
        public List<EpGroup> ep_group { get; set; }
        public string message { get; set; }
    }

    public class EpGroup
    {
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string ep_id { get; set; }
        public string dep_name { get; set; }
        public List<string> lgr_name { get; set; }
    }

    public class List_user_group
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Data_user_group data { get; set; }
        public object error { get; set; }
    }
}
