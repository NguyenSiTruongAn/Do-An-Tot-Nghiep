using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ListCalendarWork
    {
        public List<GeneralCalendar> general_calendar { get; set; }
        public List<PersonalCalendar> personal_calendar { get; set; }
        public string message { get; set; }
    }

    public class GeneralCalendar
    {
        public string apply_month { get; set; }
        public string cy_id { get; set; }
        public string cy_name { get; set; }
        public string num_ep { get; set; }
        public string is_personal { get; set; }
    }

    public class PersonalCalendar
    {
        public string apply_month { get; set; }
        public string cy_id { get; set; }
        public string cy_name { get; set; }
        public string num_ep { get; set; }
        public string is_personal { get; set; }
    }

    public class API_ListCalendarWork
    {
        public bool result { get; set; }
        public int code { get; set; }
        public ListCalendarWork data { get; set; }
        public object error { get; set; }
    }
}
