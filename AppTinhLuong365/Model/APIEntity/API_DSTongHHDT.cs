﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSTongHHDT
    {
        public List<DSTongHHDT> list { get; set; }
        public string message { get; set; }
    }

    public class DSTongHHDT
    {
        public int ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string time { get; set; }
        public string money { get; set; }
        public string display_money
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(money) >= 0)
                {
                    double m;
                    if (double.TryParse(money, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(money.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
    }

    public class API_DSTongHHDT
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSTongHHDT data { get; set; }
        public object error { get; set; }
    }
}
