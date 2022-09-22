using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Tbl_Salary_Manager
    {
        public List<ItemEmp> list_emp { get; set; }
        public string message { get; set; }
    }

    public class ItemEmp
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_phone_tk { get; set; }
        public string ep_name { get; set; }
        public string ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string ep_gender { get; set; }
        public string ep_married { get; set; }
        public string ep_birth_day { get; set; }
        public object ep_description { get; set; }
        public string create_time { get; set; }
        public string role_id { get; set; }
        public string group_id { get; set; }
        public string start_working_time { get; set; }
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public object update_time { get; set; }
        public string allow_update_face { get; set; }
        public string real_com_id { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public object gr_name { get; set; }
        public object parent_gr { get; set; }
        public object shift_id { get; set; }
        public TblSalary tbl_salary { get; set; }
    }

    public class API_Tbl_Salary_Manager
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Tbl_Salary_Manager data { get; set; }
        public object error { get; set; }
    }

    public class TblSalary
    {
        public string salary_emp { get; set; }
        public string hopdong_emp { get; set; }
    }


}
