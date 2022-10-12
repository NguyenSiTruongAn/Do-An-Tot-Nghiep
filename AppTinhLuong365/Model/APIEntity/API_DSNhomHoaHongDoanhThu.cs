using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNhomHoaHongDoanhThu
    {
        public List<DSNhomHoaHongDoanhThu> list_rose_gr { get; set; }
        public string message { get; set; }
    }

    public class ArrUserG
    {
        public string img { get; set; }
        public string display_img
        {
            get
            {
                string result = "https://chamcong.24hpay.vn/upload/employee/" + img;
                if (string.IsNullOrEmpty(img))
                    result = "https://tinhluong.timviec365.vn/img/add.png";
                return result;
            }
        }
        public string name { get; set; }
        public string pr_id_user { get; set; }
        public string pr_id_group { get; set; }
        public string pr_percent { get; set; }
    }

    public class DtThoiDiem
    {
        public int id { get; set; }
        public string dt_rose_id { get; set; }
        public string dt_money { get; set; }
        public string dt_time { get; set; }
    }

    public class DSNhomHoaHongDoanhThu
    {
        public string ro_id_group { get; set; }
        public string ro_id { get; set; }
        public string ro_id_tl { get; set; }
        public string ro_time { get; set; }
        public string display_ro_time 
        {
            get
            {
                string result = "Tháng" + DateTime.Parse(ro_time).ToString("MM/yyyy");
                return result;
            } 
        }
        public string tl_name { get; set; }
        public string ro_note { get; set; }
        public string ro_price { get; set; }
        public string lgr_name { get; set; }
        public string lgr_count { get; set; }
        public string tl_id { get; set; }
        public string tl_phan_tram { get; set; }
        public string tl_money_min { get; set; }
        public string tl_money_max { get; set; }
        public List<DtThoiDiem> dt_thoi_diem { get; set; }
        public List<ArrUserG> arr_user_g { get; set; }
    }

    public class API_DSNhomHoaHongDoanhThu
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNhomHoaHongDoanhThu data { get; set; }
        public object error { get; set; }
    }
}
