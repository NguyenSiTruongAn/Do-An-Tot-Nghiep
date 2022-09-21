using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataListGroup
    {
        public List<ListGroup> list_group { get; set; }
        public string message { get; set; }
    }

    public class ListGroup
    {
        public string lgr_id { get; set; }
        public string lgr_name { get; set; }
        public string lgr_note { get; set; }
        public string count_member { get; set; }
    }

    public class API_ListGroup
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataListGroup data { get; set; }
        public object error { get; set; }
    }
}
