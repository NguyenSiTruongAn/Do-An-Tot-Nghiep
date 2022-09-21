using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Contract
    {
        public string con_id { get; set; }
        public string con_name { get; set; }
        public string con_time_up { get; set; }
        public string con_salary_persent { get; set; }
    }

    public class DataEmployee
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
        public int ep_birth_day { get; set; }
        public string ep_gender { get; set; }
        public string ep_married { get; set; }
        public object ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_authentic { get; set; }
        public string role_id { get; set; }
        public string ep_image { get; set; }
        public int create_time { get; set; }
        public object update_time { get; set; }
        public string start_working_time { get; set; }
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
        public string src_avt { get; set; }
        public object family { get; set; }
        public object donate { get; set; }
        public List<Tax> tax { get; set; }
        public List<Insurance> insurance { get; set; }
        public List<Salary> salary { get; set; }
        public List<Contract> contract { get; set; }
        public string message { get; set; }
    }

    public class Insurance
    {
        public string cls_day { get; set; }
        public string cls_id { get; set; }
        public string cl_name { get; set; }
    }

    public class API_UserInfo
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataEmployee data { get; set; }
        public object error { get; set; }
    }

    public class Salary
    {
        public string sb_id { get; set; }
        public string sb_salary_basic { get; set; }
        public string sb_salary_bh { get; set; }
        public string sb_time_up { get; set; }
        public string ten_chucvu { get; set; }
    }

    public class Tax
    {
        public string cls_day { get; set; }
        public string cls_id { get; set; }
        public string cl_name { get; set; }
    }
}
