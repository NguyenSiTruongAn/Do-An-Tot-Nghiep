using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVBH
    {
        public List<DSNVBH> list { get; set; }
        public string message { get; set; }
    }

    public class DSNVBH
    {
        public string cls_id_user { get; set; }
        public string cl_name { get; set; }
        public string cls_id_cl { get; set; }
        public string cls_day { get; set; }
        public string display_cls_day
        {
            get
            {
                string result = DateTime.Parse(cls_day).ToString("MM/yyyy");
                return result;
            }
        }
        public string fs_repica { get; set; }
        public string cl_id_form { get; set; }
        public string cls_id { get; set; }
        public string cls_day_end { get; set; }
        public string display_cls_day_end
        {
            get
            {
                string result = "---";
                if (!string.IsNullOrEmpty(cls_day_end))
                {
                    result = "Tháng " + DateTime.Parse(cls_day_end).ToString("MM/yyyy");
                }
                return result;
            }
        }
    }

    public class API_DSNVBH
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVBH data { get; set; }
        public object error { get; set; }
    }
}
