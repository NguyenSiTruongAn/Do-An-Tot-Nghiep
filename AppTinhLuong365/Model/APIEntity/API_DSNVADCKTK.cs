using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVADCKTK
    {
        public List<DSNVADCKTK> list { get; set; }
        public string message { get; set; }
    }

    public class DSNVADCKTK
    {
        public int id { get; set; }
        public string ep_image { get; set; }
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string dep_name { get; set; }
        public string cls_day { get; set; }
        public string displaycl_day
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
        public string displaycl_day_end
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
        public string cls_id { get; set; }
        public string cl_name { get; set; }
    }

    public class API_DSNVADCKTK
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVADCKTK data { get; set; }
        public object error { get; set; }
    }
}
