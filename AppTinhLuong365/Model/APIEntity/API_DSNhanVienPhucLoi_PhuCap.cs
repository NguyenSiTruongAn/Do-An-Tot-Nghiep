using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNhanVienPhucLoi_PhuCap
    {
        public List<DSNhanVienPhucLoi_PhuCap> list { get; set; }
        public string message { get; set; }
    }

    public class DSNhanVienPhucLoi_PhuCap
    {
        public string ep_id { get; set; }
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string dep_name { get; set; }
        public string type { get; set; }
        public string name_group { get; set; }
        public string cls_day { get; set; }
        public string displaycls_day
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cls_day) && DateTime.TryParse(cls_day, out day))
                {
                    result = day.ToString("dd/MM/yyyy");
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
                    result = day.ToString("dd/MM/yyyy");
                }

                return result;
            }
        }
    }

    public class API_DSNhanVienPhucLoi_PhuCap
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNhanVienPhucLoi_PhuCap data { get; set; }
        public object error { get; set; }
    }
}
