using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BasicSalary
    {
        public string sb_salary_basic { get; set; }
        public string display_sb_salary_basic 
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(sb_salary_basic) >= 0)
                {
                    double m;
                    if (double.TryParse(sb_salary_basic, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(sb_salary_basic.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string salary_fluctuations { get; set; }
        
        public string sb_salary_bh { get; set; }
        public string display_salary_bh
        {
            get
            {
                string a = "";
                if (!string.IsNullOrEmpty(sb_salary_bh))
                {
                    if (Convert.ToInt64(sb_salary_bh) >= 0)
                    {
                        double m;
                        if (double.TryParse(sb_salary_bh, out m)) a = m.ToString("C0").Replace(@"$", "");
                        a += " VNĐ";
                    }
                    else
                    {
                        double n;
                        if (double.TryParse(sb_salary_bh.ToString(), out n))
                            a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                        a += " VNĐ";
                    }
                }
                return a;
            }
        }
        public string sb_pc_bh { get; set; }
        public string display_sb_pc_bh
        {
            get
            {
                string a = "";
                if (!string.IsNullOrEmpty(sb_pc_bh))
                {
                    if (Convert.ToInt64(sb_pc_bh) >= 0)
                    {
                        double m;
                        if (double.TryParse(sb_pc_bh, out m)) a = m.ToString("C0").Replace(@"$", "");
                        a += " VNĐ";
                    }
                    else
                    {
                        double n;
                        if (double.TryParse(sb_pc_bh.ToString(), out n))
                            a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                        a += " VNĐ";
                    }
                }
                return a;
            }
        }
        public string sb_time_up { get; set; }
        public string display_sb_time_up
        {
            get
            {
                string result = DateTime.Parse(sb_time_up).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string sb_id { get; set; }
        public string sb_lydo { get; set; }
        public string sb_quyetdinh { get; set; }
    }

    public class ContractWork
    {
        public string con_name { get; set; }
        public string con_time_up { get; set; }
        public string display_con_time_up
        {
            get
            {
                string result = DateTime.Parse(con_time_up).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string con_time_end { get; set; }
        public string display_con_time_end
        {
            get
            {
                string result = "---";
                if (con_time_end != "0000-00-00" && !string.IsNullOrEmpty(con_time_end))
                    result = DateTime.Parse(con_time_end).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string con_salary_persent { get; set; }
        public string con_id { get; set; }
    }

    public class DataChiTietNV
    {
        public ChiTietNV list { get; set; }
        public string message { get; set; }
    }

    public class Don
    {
        public string don_name { get; set; }
        public string don_price { get; set; }
        public string display_don_price
        {
            get
            {
                string a = "";
                if (!string.IsNullOrEmpty(don_price))
                {
                    if (Convert.ToInt64(don_price) >= 0)
                    {
                        double m;
                        if (double.TryParse(don_price, out m)) a = m.ToString("C0").Replace(@"$", "");
                        a += " VNĐ";
                    }
                    else
                    {
                        double n;
                        if (double.TryParse(don_price.ToString(), out n))
                            a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                        a += " VNĐ";
                    }
                }
                return a;
            }
        }
        public string don_time_active { get; set; }
        public string display_don_time_active
        {
            get
            {
                string result = "Từ " + DateTime.Parse(don_time_active).ToString("MM/yyyy");
                return result;
            }
        }
        public string don_time_end { get; set; }
        public string display_don_time_end
        {
            get
            {
                string result = "---";
                if (!string.IsNullOrEmpty(don_time_end))
                {
                    result = "Đến " + DateTime.Parse(don_time_end).ToString("MM/yyyy");
                }
                return result;
            }
        }
        public string don_id { get; set; }
    }

    public class FamilyMember
    {
        public string fa_status { get; set; }
        public bool display_fa_status
        {
            get
            {
                if (fa_status == "1")
                    return true;
                else return false;
            }
        }
        public string fa_id { get; set; }
        public string fa_name { get; set; }
        public string fa_birthday { get; set; }
        public string display_fa_birthday
        {
            get
            {
                string result = DateTime.Parse(fa_birthday).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string fa_phone { get; set; }
        public string fa_relation { get; set; }
        public string fa_job { get; set; }
        public string fa_address { get; set; }
    }

    public class ChiTietNV
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string display_ep_email
        {
            get
            {
                string result = "Chưa cập nhật";
                if (!string.IsNullOrEmpty(ep_email))
                {
                    result = ep_email;
                }
                return result;
            }
        }
        public string ep_phone_tk { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string com_id { get; set; }
        public string dep_id { get; set; }
        public string ep_pass { get; set; }
        public string ep_pass_encrypt { get; set; }
        public string ep_address { get; set; }
        public string ep_birth_day { get; set; }
        public string ep_gender { get; set; }
        public string display_ep_gender
        {
            get
            {
                string result = "";
                if (ep_gender == "1")
                    result = "Nam";
                if (ep_gender == "2")
                    result = "Nữ";
                if (ep_gender == "3")
                    result = "Khác";
                return result;
            }
        }
        public string ep_married { get; set; }
        public string ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_authentic { get; set; }
        public string role_id { get; set; }
        public string ep_image { get; set; }
        public string create_time { get; set; }
        public object update_time { get; set; }
        public string start_working_time { get; set; }
        public string display_start_working_time
        {
            get
            {
                string result = DateTime.Parse(start_working_time).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string position_id { get; set; }
        public string group_id { get; set; }
        public string ep_description { get; set; }
        public object ep_featured_recognition { get; set; }
        public string ep_status { get; set; }
        public string ep_signature { get; set; }
        public string allow_update_face { get; set; }
        public string version_in_use { get; set; }
        public string from_source { get; set; }
        public string ep_id_tv365 { get; set; }
        public string com_name { get; set; }
        public string dep_name { get; set; }
        public string cycle { get; set; }
        public string st_time { get; set; }
        public string display_st_time
        {
            get
            {
                string result = "Từ ngày bắt đầu làm việc";
                if (!string.IsNullOrEmpty(st_time))
                {
                    result = DateTime.Parse(st_time).ToString("HH:mm dd/MM/yyyy");
                }
                return result;
            }
        }
        public string st_bank { get; set; }
        public string display_st_bank
        {
            get
            {
                string result = "Chưa cập nhật";
                if (!string.IsNullOrEmpty(st_bank))
                {
                    result = st_bank;
                }
                return result;
            }
        }
        public string st_stk { get; set; }
        public string display_st_stk
        {
            get
            {
                string result = "Chưa cập nhật";
                if (!string.IsNullOrEmpty(st_stk))
                {
                    result = st_stk;
                }
                return result;
            }
        }
        public List<BasicSalary> basic_salary { get; set; }
        public List<ContractWork> contract_work { get; set; }
        public List<FamilyMember> family_member { get; set; }
        public List<Don> don { get; set; }
    }

    public class API_ChiTietNV
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataChiTietNV data { get; set; }
        public object error { get; set; }
    }
}
