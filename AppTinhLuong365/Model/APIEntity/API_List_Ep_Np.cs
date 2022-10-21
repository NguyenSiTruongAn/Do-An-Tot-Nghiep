using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_Ep_Np
    {
        public List<ItemNp> list { get; set; }
        public string message { get; set; }
    }

    public class ItemNp
    {
        public string ep_image { get; set; }
        public string ep_name { get; set; }
        public string ep_id { get; set; }
        public string ngaybatdau_nghi { get; set; }
        public string ngayketthuc_nghi { get; set; }
        public string ca_nghi { get; set; }
        public string CaNghi
        {
            get
            {
                string abc = "";
                if (string.IsNullOrEmpty(ca_nghi))
                {
                    abc = "Cả ngày";
                }
                else
                {
                    abc = ca_nghi;
                }
                return abc;
            }
        }
        public string val_type { get; set; }
        public string TrangThai
        {
            get
            {
                string a = "";
                if (val_type == "0")
                {
                    a = "Đang chờ duyệt";
                }
                else if (val_type == "3")
                {
                    a = "Từ chối";
                }
                else if (val_type == "5")
                {
                    a = "Đã duyệt";
                }
                else
                {
                    a = "";
                }
                return a;
            }
        }
        public object loai_nghi_phep { get; set; }
        public string LoaiNghi
        {
            get
            {
                string b = "";
                if (string.IsNullOrEmpty((string)loai_nghi_phep))
                {
                    b = "Nghỉ phép không lương";
                }
                return b;
            }
        }
    }

    public class API_List_Ep_Np
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_Ep_Np data { get; set; }
        public object error { get; set; }
    }
}
