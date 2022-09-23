using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Id
    {
        public string name { get; set; }
        public object luong_cb { get; set; }
        public object cong_chuan { get; set; }
        public int cong_thuc { get; set; }
        public int cong_sau_phat { get; set; }
        public int cong_theo_tien { get; set; }
        public int cong_ghi_nhan { get; set; }
        public int cong_nghi_phep { get; set; }
        public int tong_cong_nhan { get; set; }
        public int luong_thuc { get; set; }
        public int luong_sau_phat { get; set; }
        public object luong_bh { get; set; }
        public int ms_phat_tien { get; set; }
        public int ms_phat_cong { get; set; }
        public int hoa_hong { get; set; }
        public int tam_ung { get; set; }
        public int thuong { get; set; }
        public int thuong_le { get; set; }
        public int phat { get; set; }
        public int phat_nghi { get; set; }
        public int phat_nghi_sqd { get; set; }
        public int phuc_loi { get; set; }
        public int phu_cap { get; set; }
        public int phu_cap_ca { get; set; }
        public int bao_hiem { get; set; }
        public int khoan_khac { get; set; }
        public int tong_luong { get; set; }
        public int thue { get; set; }
        public int luong_thuc_nhan { get; set; }
    }

    public class BangLuong
    {
        public List<Id> id { get; set; }
        public string message { get; set; }
    }

    public class Payroll
    {
        public BangLuong bang_luong { get; set; }
        public string message { get; set; }
    }

    public class API_Payroll
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Payroll data { get; set; }
        public object error { get; set; }
    }

}
