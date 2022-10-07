using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_ep_holiday
    {
        public List<EpHolidayItem> ep_holiday_list { get; set; }
        public string message { get; set; }
    }

    public class EpHolidayItem
    {
        public string ho_id_user { get; set; }
        public string ho_id_lho { get; set; }
        public string lho_first_time { get; set; }
        public string lho_last_time { get; set; }
        public int lho_status { get; set; }
        public string lho_number { get; set; }
        public string display_pm_monney
        {
            get
            {
                string a = "";
                if (lho_status == 1)
                {
                    int m;
                    if (int.TryParse(lho_number, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else if (lho_status == 2)
                {
                    a = lho_number;
                }

                return a;
            }
        }
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string dep_name { get; set; }
        public string dep_id { get; set; }
        public string time_start { get; set; }
        public string time_end { get; set; }
    }

    public class API_List_ep_holiday
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_ep_holiday data { get; set; }
        public object error { get; set; }
    }
}
