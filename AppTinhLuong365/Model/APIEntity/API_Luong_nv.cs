using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Luong_nv
    {
        public Item_Luong_nv luong_nv { get; set; }
        public string message { get; set; }
    }

    public class Item_Luong_nv
    {
        public string luong_cb { get; set; }
        public string hop_dong { get; set; }
        public string display_luong_cb
        {
            get
            {
                if (string.IsNullOrEmpty(luong_cb))
                    return "0";
                else
                {
                    string a = "";
                    if (Convert.ToDouble(luong_cb) >= 0)
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
        }
        public string cong_chuan { get; set; }
        public string display_cong_chuan
        {
            get
            {
                if (string.IsNullOrEmpty(cong_chuan))
                {
                    return "0";
                }

                return cong_chuan;
            }
        }
        public string cong_thuc { get; set; }
        public string cong_sau_phat { get; set; }
        public string cong_theo_tien { get; set; }
        public string display_cong_theo_tien
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(cong_theo_tien) >= 0)
                {
                    double m;
                    if (double.TryParse(cong_theo_tien.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else if (cong_theo_tien == null)
                {
                    a = "0";
                }
                else
                {
                    double n;
                    if (double.TryParse(cong_theo_tien.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string cong_ghi_nhan { get; set; }
        public string cong_nghi_phep { get; set; }
        public string tong_cong_nhan { get; set; }
        public double luong_thuc { get; set; }
        public string display_luong_thuc
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(luong_thuc) >= 0)
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
        public double luong_sau_phat { get; set; }
        public string display_luong_sau_phat
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(luong_sau_phat) >= 0)
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
                if (string.IsNullOrEmpty(luong_bh))
                    return "0";
                else
                {
                    string a = "";
                    if (Convert.ToDouble(luong_bh) >= 0)
                    {
                        double m;
                        if (double.TryParse(luong_bh, out m)) a = m.ToString("C0").Replace(@"$", "");
                    }
                    else
                    {
                        double n;
                        if (double.TryParse(luong_bh.ToString(), out n))
                            a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                    }

                    return a;
                }
            }
        }
        public double ms_phat_tien { get; set; }
        public string display_ms_phat_tien
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(ms_phat_tien) >= 0)
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
        public string ms_phat_cong { get; set; }
        public double hoa_hong { get; set; }
        public string display_hoa_hong
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(hoa_hong) >= 0)
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
        public double tam_ung { get; set; }
        public string display_tam_ung
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(tam_ung) >= 0)
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
        public double thuong { get; set; }
        public string display_thuong
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(thuong) >= 0)
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
                if (Convert.ToDouble(thuong_le) >= 0)
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
        public double phat { get; set; }
        public string display_phat
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(phat) >= 0)
                {
                    double m;
                    if (double.TryParse(phat.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(phat.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public double phat_nghi { get; set; }
        public string display_phat_nghi
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(phat_nghi) >= 0)
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
        public double phat_nghi_sqd { get; set; }
        public string display_phat_nghi_sqd
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(phat_nghi_sqd) >= 0)
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
        public double phuc_loi { get; set; }
        public string display_phuc_loi
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(phuc_loi) >= 0)
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
        public double phu_cap { get; set; }
        public string display_phu_cap
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(phu_cap) >= 0)
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
        public double phu_cap_ca { get; set; }
        public string display_phu_cap_ca
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(phu_cap_ca) >= 0)
                {
                    double m;
                    if (double.TryParse(phu_cap_ca.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(phu_cap_ca.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public double bao_hiem { get; set; }
        public string display_bao_hiem
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(bao_hiem) >= 0)
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
        public double khoan_khac { get; set; }
        public string display_khoan_khac
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(khoan_khac) >= 0)
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
                if (Convert.ToDouble(tong_luong) >= 0)
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
        public string thue { get; set; }
        public string display_thue
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(thue) >= 0)
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
                if (Convert.ToDouble(luong_thuc_nhan) >= 0)
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
        public double luong_da_tra { get; set; }
        public string display_luong_da_tra
        {
            get
            {
                if (string.IsNullOrEmpty(luong_da_tra.ToString()))
                    return "0";
                string a = "";
                if (Convert.ToDouble(luong_da_tra) >= 0)
                {
                    double m;
                    if (double.TryParse(luong_da_tra.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(luong_da_tra.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public List<ChiTietLuongDaTra> chi_tiet_luong_da_tra { get; set; }
        public List<ListPhatNsqd> list_phat_nsqd { get; set; }
    }

    public class ChiTietLuongDaTra
    {
        public string dp_money { get; set; }
        public string display_dp_money
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(dp_money) >= 0)
                {
                    double m;
                    if (double.TryParse(dp_money.ToString(), out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(dp_money.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string dp_time { get; set; }

        public string display_dp_time
        {
            get
            {
                string a = "";
                if (dp_time != null)
                {
                    DateTime m;
                    if (DateTime.TryParse(dp_time, out m)) a = m.ToString("dd/MM/yyyy");
                }
                return a;
            }
        }
        public string pay_name { get; set; }
    }

    public class ListPhatNsqd
    {
        public string ngaynghi { get; set; }
        public string canghi { get; set; }
        public string mucphat { get; set; }
    }

    public class API_Luong_nv
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Luong_nv data { get; set; }
        public object error { get; set; }
    }
}
