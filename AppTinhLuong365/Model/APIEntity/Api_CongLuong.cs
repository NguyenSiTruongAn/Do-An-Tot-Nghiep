using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CongLuong
    {
        public ItemCongLuong list { get; set; }
        public string message { get; set; }
    }

    public class ItemCongLuong
    {
        public int tong_luong { get; set; }
        public int tong_bh { get; set; }
        public int tong_thue { get; set; }
        public double tong_ltb { get; set; }
        public int so_nv { get; set; }
        public double pt_bh { get; set; }
        public int pt_thue { get; set; }
        public int lnv_15 { get; set; }
        public int lnv_57 { get; set; }
        public int lnv_710 { get; set; }
        public int lnv_10 { get; set; }
        public int luong_da_tra { get; set; }
    }

    public class Api_CongLuong
    {
        public bool result { get; set; }
        public int code { get; set; }
        public CongLuong data { get; set; }
        public object error { get; set; }
    }


}
