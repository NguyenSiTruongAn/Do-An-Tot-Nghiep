using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVGoiBH
    {
        public List<DSNVGoiBH> ep_insrc { get; set; }
        public string message { get; set; }
    }

    public class DSNVGoiBH
    {
        public string cls_id_user { get; set; }
        public string cls_day { get; set; }
        public string display_cls_day
        {
            get
            {
                string result = DateTime.Parse(cls_day).ToString("MM/yyyy");
                return result;
            }
        }
        public string cls_id { get; set; }
        public string cls_salary { get; set; }
        public string cls_id_cl { get; set; }
        public string fs_repica { get; set; }
        public string cls_day_end { get; set; }
        public string display_cls_day_end
        {
            get
            {
                string result = "---";
                if (!string.IsNullOrEmpty(cls_day_end) && cls_day_end != "0000-00-00")
                {
                    result = DateTime.Parse(cls_day_end).ToString("MM/yyyy");
                }
                return result;
            }
        }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
    }

    public class API_DSNVGoiBH
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVGoiBH data { get; set; }
        public object error { get; set; }
    }
}
