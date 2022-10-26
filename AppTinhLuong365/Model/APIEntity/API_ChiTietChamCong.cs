﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    public class DataChiTietChamCong
    {
        public List<List<ChiTietCa>> list { get; set; }
        public string message { get; set; }
    }

    public class API_ChiTietChamCong
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataChiTietChamCong data { get; set; }
        public object error { get; set; }
    }
    public class ChiTietCa
    {
        public string ts_date { get; set; }
        public string check_in { get; set; }
        public string check_out { get; set; }
        public string shift_id { get; set; }
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string Display_time
        {
            get
            {
                var x = DateTime.Parse(start_time);
                string result = DateTime.Parse(start_time).ToString("HH:mm") + " - " + DateTime.Parse(end_time).ToString("HH:mm");
                return result;
            }
        }
        public string num_to_calculate { get; set; }
        public string status
        {
            get
            {
                string result = "";
                if (string.IsNullOrEmpty(check_in))
                    result = "0";
                else
                {
                    if (string.IsNullOrEmpty(check_out))
                        result = "1";
                    else
                    {
                        if (DateTime.Parse(ts_date + " " + start_time) < DateTime.Parse(check_in) || DateTime.Parse(ts_date + " " + end_time) > DateTime.Parse(check_out))
                        {
                            result = "2";
                        }
                        else
                            result = "3";
                    }
                }
                return result;
            }
        }
    }
}
