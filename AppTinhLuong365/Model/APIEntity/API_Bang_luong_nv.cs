using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Item_Bang_Luong
    {
        public string ep_id { get; set; }
        public string name { get; set; }
        public string ep_image { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string luong_cb { get; set; }

        public string display_luong_cb
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(luong_cb) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_cb, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_cb.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }

        public string pt_hop_dong { get; set; }
        public string cong_chuan { get; set; }
        public int cong_thuc { get; set; }
        public int cong_sau_phat { get; set; }
        public int cong_theo_tien { get; set; }
        public int cong_ghi_nhan { get; set; }
        public int cong_nghi_phep { get; set; }
        public int tong_cong_nhan { get; set; }
        public int luong_thuc { get; set; }
        public string display_luong_thuc
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(luong_thuc) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_thuc.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_thuc.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int luong_sau_phat { get; set; }
        public string display_luong_sau_phat
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(luong_sau_phat) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_sau_phat.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_sau_phat.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string luong_bh { get; set; }
        public string display_luong_bh
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(luong_bh) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_bh, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_bh, out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int ms_phat_tien { get; set; }
        public string display_ms_phat_tien
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(ms_phat_tien) >= 0)
                {
                    double m;
                    if (double.TryParse(ms_phat_tien.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(ms_phat_tien.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int ms_phat_cong { get; set; }
        public double hoa_hong { get; set; }
        public string display_hoa_hong
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(hoa_hong) >= 0)
                {
                    double m;
                    if (double.TryParse(hoa_hong.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(hoa_hong.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int tam_ung { get; set; }
        public string display_tam_ung
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(tam_ung) >= 0)
                {
                    double m;
                    if (double.TryParse(tam_ung.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tam_ung.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int thuong { get; set; }
        public string display_thuong
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(thuong) >= 0)
                {
                    double m;
                    if (double.TryParse(thuong.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(thuong.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public double thuong_le { get; set; }
        public string display_thuong_le
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(thuong_le) >= 0)
                {
                    double m;
                    if (double.TryParse(thuong_le.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(thuong_le.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int phat { get; set; }
        public int phat_nghi { get; set; }
        public string display_phat_nghi
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(phat_nghi) >= 0)
                {
                    double m;
                    if (double.TryParse(phat_nghi.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(phat_nghi.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int phat_nghi_sqd { get; set; }
        public string display_phat_nghi_sqd
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(phat_nghi_sqd) >= 0)
                {
                    double m;
                    if (double.TryParse(phat_nghi_sqd.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(phat_nghi_sqd.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int phuc_loi { get; set; }
        public string display_phuc_loi
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(phuc_loi) >= 0)
                {
                    double m;
                    if (double.TryParse(phuc_loi.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(phuc_loi.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int phu_cap { get; set; }
        public string display_phu_cap
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(phu_cap) >= 0)
                {
                    double m;
                    if (double.TryParse(phu_cap.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(phu_cap.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int phu_cap_ca { get; set; }
        public int bao_hiem { get; set; }
        public string display_bao_hiem
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(bao_hiem) >= 0)
                {
                    double m;
                    if (double.TryParse(bao_hiem.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(bao_hiem.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int khoan_khac { get; set; }
        public string display_khoan_khac
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(khoan_khac) >= 0)
                {
                    double m;
                    if (double.TryParse(khoan_khac.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(khoan_khac.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public double tong_luong { get; set; }
        public string display_tong_luong
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(tong_luong) >= 0)
                {
                    double m;
                    if (double.TryParse(tong_luong.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tong_luong.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public int thue { get; set; }
        public string display_thue
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(thue) >= 0)
                {
                    double m;
                    if (double.TryParse(thue.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(thue.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public double luong_thuc_nhan { get; set; }
        public string display_luong_thuc_nhan
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(luong_thuc_nhan) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_thuc_nhan.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_thuc_nhan.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
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
                    if (double.TryParse(luong_da_tra, out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
    }

    public class Bang_luong_nv
    {
        public List<Item_Bang_Luong> bang_luong { get; set; }
        public int total { get; set; }
        public string message { get; set; }
    }

    public class API_Bang_luong_nv
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Bang_luong_nv data { get; set; }
        public object error { get; set; }
    }
}
