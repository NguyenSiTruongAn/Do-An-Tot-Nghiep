﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVHoaHongTien
    {
        public List<DSNVHoaHongTien> list_rose { get; set; }
        public string message { get; set; }
    }

    public class DSNVHoaHongTien
    {
        public string ro_id { get; set; }
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string ro_time { get; set; }
        public string Display_ro_time 
        {
            get
            {
                string result = DateTime.Parse(ro_time).ToString("dd/MM/yyy");
                return result;
            }

        }
        public string ro_note { get; set; }
        public string ro_price { get; set; }
        public string display_ro_price
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(ro_price) >= 0)
                {
                    double m;
                    if (double.TryParse(ro_price, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(ro_price.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
    }

    public class API_DSNVHoaHongTien
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVHoaHongTien data { get; set; }
        public object error { get; set; }
    }
}
