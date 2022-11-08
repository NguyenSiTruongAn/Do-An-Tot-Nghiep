using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_Holiday
    {
        public List<HolidayList> holiday_list { get; set; }
        public string message { get; set; }
    }

    public class HolidayList
    {
        public string lho_id { get; set; }
        public string lho_name { get; set; }
        public string lho_id_created { get; set; }
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
        public string lho_time_created { get; set; }
        public string time_start { get; set; }
        public string display_time_start
        {
            get
            {
                string result = "";
                result = time_start.Split('-')[0] + "/" + time_start.Split('-')[1] + "/" + time_start.Split('-')[2];
                return result;
            }
        }
        public string _time_start
        {
            get
            {
                string result = "";
                result = time_start.Split('-')[2] + "/" + time_start.Split('-')[1] + "/" + time_start.Split('-')[0];
                return result;
            }
        }
        public string time_end { get; set; }
        public string display_time_end
        {
            get
            {
                string result = "";
                result = time_end.Split('-')[0] + "/" + time_end.Split('-')[1] + "/" + time_end.Split('-')[2];
                return result;
            }
        }public string _time_end
        {
            get
            {
                string result = "";
                result = time_end.Split('-')[2] + "/" + time_end.Split('-')[1] + "/" + time_end.Split('-')[0];
                return result;
            }
        }
    }

    public class API_List_Holiday
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_Holiday data { get; set; }
        public object error { get; set; }
    }
}
