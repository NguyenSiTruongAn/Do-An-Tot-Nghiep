using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Count
    {
        public string gm_id_user { get; set; }
    }

    public class Data_user_group
    {
        public List<EpGroup> ep_group { get; set; }
        public List<Count> count { get; set; }
        public string message { get; set; }
    }

    public class EpGroup
    {
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string ep_id { get; set; }
        public string dep_name { get; set; }
        public List<LgrName> lgr_name { get; set; }
    }

    public class LgrName
    {
        public string gm_id_group { get; set; }
        public string lgr_name { get; set; }
    }

    public class List_user_group
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Data_user_group data { get; set; }
        public object error { get; set; }
    }
}
