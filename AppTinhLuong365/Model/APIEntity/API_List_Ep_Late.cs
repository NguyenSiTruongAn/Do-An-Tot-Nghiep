using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_Ep_Late
    {
        public List<EpLate> ep_late { get; set; }
        public string message { get; set; }
    }

    public class EpLate
    {
        public string sheet_id { get; set; }
        public string ep_id { get; set; }
        public string ts_image { get; set; }
        public string ts_date { get; set; }
        public string check_in { get; set; }
        public string check_out { get; set; }
        public string ts_location_name { get; set; }
        public string shift_id { get; set; }
        public string shift_name { get; set; }
        public string dep_name { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string ts_error { get; set; }
        public string is_success { get; set; }
        public string ep_name { get; set; }
        public string ep_gender { get; set; }
        public int early { get; set; }
        public int early_second { get; set; }

        public int early_Second
        {
            get
            {
                int a = early_second - early * 60;
                return a;
            }
        }

        public int late_Second
        {
            get
            {
                int a = late_second - late * 60;
                return a;
            }
        }

        public string kq_phat { get; set; }
        public int late { get; set; }
        public int late_second { get; set; }
    }

    public class API_List_Ep_Late
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_Ep_Late data { get; set; }
        public object error { get; set; }
    }
}