using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_Np_Improperly
    {
        public List<List> list { get; set; }
        public string message { get; set; }
    }

    public class List
    {
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string pc_money { get; set; }
        public string pc_time { get; set; }
        public string pc_shift { get; set; }
        public string pc_id { get; set; }
    }

    public class API_List_Np_Improperly
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_Np_Improperly data { get; set; }
        public object error { get; set; }
    }
}
