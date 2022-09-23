using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ListLate
    {
        public List<ItemLate> list_late { get; set; }
        public string message { get; set; }
    }

    public class ItemLate
    {
        public string pm_id { get; set; }
        public string pm_id_com { get; set; }
        public string pm_shift { get; set; }
        public string pm_type { get; set; }
        public string pm_minute { get; set; }
        public string pm_type_phat { get; set; }
        public string pm_time_begin { get; set; }
        public string pm_time_end { get; set; }
        public string pm_monney { get; set; }
        public string pm_time_created { get; set; }
    }

    public class API_ListLate
    {
        public bool result { get; set; }
        public int code { get; set; }
        public ListLate data { get; set; }
        public object error { get; set; }
    }


}
