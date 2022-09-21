using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataTamUng
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ItemTamUng> items { get; set; }
    }
    public class NoiDungTamUng
    {
        public DateTime ngay_tam_ung { get; set; }
        public int sotien_tam_ung { get; set; }
        public string ly_do { get; set; }
    }
    public class ItemTamUng
    {
        public string id_de_xuat { get; set; }
        public string name_dx { get; set; }
        public string type_dx { get; set; }
        public string noi_dung { get; set; }
        public NoiDungTamUng NoiDung
        {
            get
            {
                return JsonConvert.DeserializeObject<NoiDungTamUng>(noi_dung);
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
        public string del_type { get; set; }
    }

    public class API_ListTamUng
    {
        public DataTamUng data { get; set; }
        public object error { get; set; }
    }
}
