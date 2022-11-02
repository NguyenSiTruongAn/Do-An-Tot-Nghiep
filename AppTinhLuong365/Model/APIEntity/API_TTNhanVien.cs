using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ContractTTNV
    {
        public string con_id { get; set; }
        public string con_name { get; set; }
        public string con_time_up { get; set; }
        public string con_time_end { get; set; }
        public string display_con_time_up
        {
            get
            {
                string result = DateTime.Parse(con_time_up).ToString("dd/MM/yyyy");
                return result;
            }
        }
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
    }

    public class DataTTNhanVien
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public object ep_phone_tk { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string com_id { get; set; }
        public string dep_id { get; set; }
        public string ep_pass { get; set; }
        public object ep_pass_encrypt { get; set; }
        public string ep_address { get; set; }
        public double ep_birth_day { get; set; }
        public string display_ep_birth_day
        {
            get
            {
                string result = CovertDateTime(ep_birth_day).ToString("dd/MM/yyyy");
                return result;
            }
        }
        private DateTime CovertDateTime(double unixTimeStamp)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(unixTimeStamp).ToLocalTime();
            return date;
        }
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
        public string display_ep_married
        {
            get
            {
                string result = "Khác";
                if (ep_married == "1")
                    result = "Độc thân";
                if (ep_married == "2")
                    result = "Đã kết hôn";
                return result;
            }
        }
        public object ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_authentic { get; set; }
        public string role_id { get; set; }
        public string ep_image { get; set; }
        public double create_time { get; set; }
        public string display_create_time
        {
            get
            {
                string result = CovertDateTime(create_time).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public object update_time { get; set; }
        public string start_working_time { get; set; }
        public string display_start_working_time
        {
            get
            {
                string result = "";
                DateTime date = new DateTime();
                if(DateTime.TryParse(start_working_time, out date))
                    result = date.ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string position_id { get; set; }
        public object group_id { get; set; }
        public string ep_description { get; set; }
        public string ep_featured_recognition { get; set; }
        public string ep_status { get; set; }
        public string ep_signature { get; set; }
        public string allow_update_face { get; set; }
        public string version_in_use { get; set; }
        public string from_source { get; set; }
        public string ep_id_tv365 { get; set; }
        public string com_name { get; set; }
        public string dep_name { get; set; }
        public string position_name { get; set; }
        public string st_bank { get; set; }
        public string display_st_bank
        {
            get
            {
                string result = "Chưa cập nhật";
                if (!string.IsNullOrEmpty(st_bank))
                    result = st_bank;
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
                    result = st_stk;
                return result;
            }
        }
        public string src_avt { get; set; }
        public object family { get; set; }
        public object donate { get; set; }
        public List<TaxNV> tax { get; set; }
        public List<InsuranceNV> insurance { get; set; }
        public List<SalaryNV> salary { get; set; }
        public List<ContractTTNV> contract { get; set; }
        public string message { get; set; }
    }

    public class InsuranceNV
    {
        public string cls_day { get; set; }
        public string display_cls_day
        {
            get
            {
                string result = DateTime.Parse(cls_day).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string cls_id { get; set; }
        public string cl_name { get; set; }
    }

    public class API_TTNhanVien
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataTTNhanVien data { get; set; }
        public object error { get; set; }
    }

    public class SalaryNV
    {
        public string sb_id { get; set; }
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
        public string sb_time_up { get; set; }
        public string display_sb_time_up
        {
            get
            {
                string result = DateTime.Parse(sb_time_up).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string sb_location { get; set; }
        public string vi_tri { get; set; }
    }

    public class TaxNV
    {
        public string cls_day { get; set; }
        public string display_cls_day
        {
            get
            {
                string result = DateTime.Parse(cls_day).ToString("dd/MM/yyyy");
                return result;
            }
        }
        public string cls_id { get; set; }
        public string cl_name { get; set; }
    }

    
}
