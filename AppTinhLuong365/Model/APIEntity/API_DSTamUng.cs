using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSTamUng
    {
        public List<DSTamUng> list { get; set; }
        public string count { get; set; }
        public string message { get; set; }
    }

    public class NDTU
    {
        public DateTime ngay_tam_ung { get; set; }
        public string ngay
        {
            get
            {
                string ngay1 = ngay_tam_ung.ToString("dd/MM/yyyy");
                return ngay1;
            }
        }
        public int sotien_tam_ung { get; set; }
        public string ly_do { get; set; }
    }

    public class DSTamUng
    {
        public string id_de_xuat { get; set; }
        public string name_dx { get; set; }
        public string type_dx { get; set; }
        public string noi_dung { get; set; }
        public NDTU NoiDung
        {
            get
            {
                return JsonConvert.DeserializeObject<NDTU>(noi_dung);
            }
        }
        public string name_user { get; set; }
        public string id_user { get; set; }
        public string com_id { get; set; }
        public string kieu_duyet { get; set; }
        public string id_user_duyet { get; set; }
        public string id_user_theo_doi { get; set; }
        public string file_kem { get; set; }
        public string type_duyet { get; set; }
        public string type_time { get; set; }
        public string time_start_out { get; set; }
        public string time_create { get; set; }
        public string time_tiep_nhan { get; set; }
        public string time_duyet { get; set; }
        public string active { get; set; }
        public string display_active
        {
            get
            {
                string ac;
                if (active.Equals("0"))
                    ac = "Chưa nhận tạm ứng";
                else
                    ac = "Đã nhận tạm ứng";
                return ac;
            }
        }
        public string del_type { get; set; }
        public string img { get; set; }
    }

    public class API_DSTamUng
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSTamUng data { get; set; }
        public object error { get; set; }
    }
}
