﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNhomHHLoiNhuan
    {
        public List<DSNhomHHLoiNhuan> list_rose { get; set; }
        public string message { get; set; }
    }

    public class EpGr
    {
        public string pr_rose { get; set; }
        public string pr_percent { get; set; }
        public string pr_id_user { get; set; }
        public string pr_id_tl { get; set; }
        public string ep_image { get; set; }
        public string display_img
        {
            get
            {
                string result = "https://chamcong.24hpay.vn/upload/employee/" + ep_image;
                if (string.IsNullOrEmpty(ep_image))
                    result = "https://tinhluong.timviec365.vn/img/add.png";
                return result;
            }
        }
        public string ep_name { get; set; }
    }

    public class ArrSl
    {
        public string dt_id { get; set; }
        public int id { get; set; }
        public string dt_rose_id { get; set; }
        public string dt_money { get; set; }
        public string dt_sl { get; set; }
        public string dt_time { get; set; }
    }

    public class DSNhomHHLoiNhuan
    {
        public string ro_id { get; set; }
        public string ro_id_group { get; set; }
        public string lgr_name { get; set; }
        public string c_user { get; set; }
        public string ro_time { get; set; }
        public string Display_ro_time
        {
            get
            {
                string result = "Tháng" + DateTime.Parse(ro_time).ToString("MM/yyyy");
                return result;
            }
        }
        public string tl_name { get; set; }
        public string ro_so_luong { get; set; }
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
        public List<EpGr> ep_gr { get; set; }
        public List<ArrSl> arr_sl { get; set; }
    }

    public class API_DSNhomHHLoiNhuan
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNhomHHLoiNhuan data { get; set; }
        public object error { get; set; }
    }
}
