using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CaiDatCongChuan
    {
        public bool result { get; set; }
        public string message { get; set; }
        public Item item { get; set; }
    }

    public class Item
    {
        public string cw_id { get; set; }
        public string com_id { get; set; }
        public string apply_month { get; set; }

        public string display_apply_month
        {
            get
            {
                DateTime a;
                string c = "";
                if (apply_month != null)
                {
                    DateTime.TryParse(apply_month, out a);
                    c = a.ToString("MM/yyyy");
                }
                return c;
            }
        }
        public string num_days { get; set; }
    }

    public class API_CaiDatCongChuan
    {
        public CaiDatCongChuan data { get; set; }
        public object error { get; set; }
    }


}
