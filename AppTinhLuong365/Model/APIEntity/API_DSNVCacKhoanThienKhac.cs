using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVCacKhoanThienKhac
    {
        public List<DSNVCacKhoanThienKhac> ep_other_list { get; set; }
        public string message { get; set; }
    }

    public class DSNVCacKhoanThienKhac
    {
        public string cls_id { get; set; }
        public string cls_id_user { get; set; }
        public string cl_name { get; set; }
        public string cls_day { get; set; }
        public string displaycls_day
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cls_day) && DateTime.TryParse(cls_day, out day))
                {
                    result = day.ToString("MM/yyyy");
                }

                return result;
            }
        }
        public string cls_day_end { get; set; }
        public string displaycls_day_end
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cls_day_end) && DateTime.TryParse(cls_day_end, out day))
                {
                    result = day.ToString("MM/yyyy");
                }

                return result;
            }
        }
        public string fs_repica { get; set; }
        public string cls_id_cl { get; set; }
    }

    public class API_DSNVCacKhoanThienKhac
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVCacKhoanThienKhac data { get; set; }
        public object error { get; set; }
    }
}
