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
        public string tong_luong { get; set; }
        public string display_tong_luong {
            get
            {
                string a = "";
                if (Convert.ToDouble(tong_luong) >= 0)
                {
                    double m;
                    if (double.TryParse(tong_luong, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tong_luong.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a + " VNĐ";
            }
        }
        public string tong_bh { get; set; }
        public string display_tong_bh {
            get
            {
                string a = "";
                if (Convert.ToInt64(tong_bh) >= 0)
                {
                    double m;
                    if (double.TryParse(tong_bh, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tong_bh.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a + " VNĐ";
            }
        }
        public string tong_thue { get; set; }
        public string display_tong_thue
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(tong_thue) >= 0)
                {
                    double m;
                    if (double.TryParse(tong_thue, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tong_thue.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a + " VNĐ";
            }
        }
        public double tong_ltb { get; set; }
        public int so_nv { get; set; }
        public double pt_bh { get; set; }
        public double pt_thue { get; set; }
        public int lnv_15 { get; set; }
        public int lnv_57 { get; set; }
        public int lnv_710 { get; set; }
        public int lnv_10 { get; set; }
        public string luong_da_tra { get; set; }
        public string display_luong_da_tra
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(luong_da_tra) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_da_tra, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_da_tra.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a + " VNĐ";
            }
        }
    }

    public class Api_CongLuong
    {
        public bool result { get; set; }
        public int code { get; set; }
        public CongLuong data { get; set; }
        public object error { get; set; }
    }


}
