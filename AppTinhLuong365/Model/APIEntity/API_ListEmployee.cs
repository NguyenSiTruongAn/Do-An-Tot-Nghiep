using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataListEmployee
    {
        public DataListEmployee1 data { get; set; }
        public DataListEmployee2 data1 { get; set; }
        public object error { get; set; }
        public string message { get; set; }
    }

    public class DataListEmployee1
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ListEmployee> items { get; set; }
    }

    public class DataListEmployee2
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public ListEmployee itemslist { get; set; }
    }

    public class ListEmployee
    {
        public string ep_id { get; set; }
        public string ep_id_display
        {
            get
            {
                string result;
                if (ep_id == "-1")
                    result = ep_name;
                else
                    result = " (" + ep_id + ") " + ep_name;
                return result;
            }
        }
        public string ep_id_display1
        {
            get
            {
                string result;
                if (ep_id == "-1")
                    result = ep_name;
                else
                    result = ep_name + "(ID " + ep_id + ") ";
                return result;
            }
        }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_birth_day { get; set; }
        public string ep_married { get; set; }
        public string ep_gender { get; set; }
        public string role_id { get; set; }

        public string display_role_id
        {
            get
            {
                string a = "";
                if (role_id == "1")
                {
                    a = "Owner";
                }
                else if (role_id == "4")
                {
                    a = "Admin";
                }
                else if (role_id == "3")
                {
                    a = "Nhân viên";
                }
                return a;
            }
        }

        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string update_time { get; set; }
        public string allow_update_face { get; set; }
        public string real_com_id { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string com_logo { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string create_time { get; set; }
        public string group_id { get; set; }
        public object shift_id { get; set; }
        public bool status { get; set; }
    }

    public class API_ListEmployee
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataListEmployee data { get; set; }
        public object error { get; set; }
    }

}
