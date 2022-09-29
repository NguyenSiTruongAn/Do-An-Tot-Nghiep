using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataPhucLoi
    {
        public List<ListWelfare> list_welfare { get; set; }
        public List<ListAllowance> list_allowance { get; set; }
        public List<ListAllowanceShift> list_allowance_shift { get; set; }
        public string message { get; set; }
    }

    public class ListAllowance
    {
        public string cl_id { get; set; }
        public string cl_name { get; set; }
        public string cl_active { get; set; }
        public string cl_note { get; set; }
        public string cl_salary { get; set; }
        public string cl_day { get; set; }
        public string displaycl_day
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cl_day) && DateTime.TryParse(cl_day, out day))
                {
                    result = day.ToString("dd/MM/yyyy");
                }

                return result;
            }
        }
        public string cl_day_end { get; set; }
        public string displaycl_day_end
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cl_day_end) && DateTime.TryParse(cl_day_end, out day))
                {
                    result = day.ToString("dd/MM/yyyy");
                }

                return result;
            }
        }
        public string cl_type { get; set; }
        public string cl_type_tax { get; set; }
    }

    public class ListAllowanceShift
    {
        public string wf_id { get; set; }
        public string wf_money { get; set; }
        public string wf_time { get; set; }
        public string displaywf_time
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(wf_time) && DateTime.TryParse(wf_time, out day))
                {
                    result = day.ToString("MM/yyyy");
                }

                return result;
            }
        }
        public string wf_time_end { get; set; }
        public string displaywf_time_end
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(wf_time_end) && DateTime.TryParse(wf_time_end, out day))
                {
                    result = day.ToString("MM/yyyy");
                }

                return result;
            }
        }
        public string wf_shift { get; set; }
        public string wf_com { get; set; }
        public string shift_name { get; set; }
    }

    public class ListWelfare
    {
        public string cl_id { get; set; }
        public string cl_name { get; set; }
        public string cl_active { get; set; }
        public string cl_note { get; set; }
        public string cl_salary { get; set; }
        public string cl_day { get; set; }
        public string displaycl_day
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cl_day) && DateTime.TryParse(cl_day, out day))
                {
                    result = day.ToString("MM/yyyy");
                }

                return result;
            }
        }
        public string cl_day_end { get; set; }
        public string displaycl_day_end
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(cl_day_end) && DateTime.TryParse(cl_day_end, out day))
                {
                    result = day.ToString("MM/yyyy");
                }

                return result;
            }
        }
        public string cl_type { get; set; }
        public string cl_type_tax { get; set; }
    }

    public class API_PhucLoi
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataPhucLoi data { get; set; }
        public object error { get; set; }
    }
}
