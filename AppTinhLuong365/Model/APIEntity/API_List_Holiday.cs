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
        public string lho_status { get; set; }
        public string lho_number { get; set; }
        public string lho_time_created { get; set; }
    }

    public class API_List_Holiday
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_Holiday data { get; set; }
        public object error { get; set; }
    }
}
