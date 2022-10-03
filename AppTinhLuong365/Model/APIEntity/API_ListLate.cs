using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ListLate
    {
        public List<ItemLate> list_late { get; set; }
        public string message { get; set; }
    }

    public class ItemLate
    {
        public string pm_id { get; set; }
        public string pm_id_com { get; set; }
        public string pm_shift { get; set; }
        public int pm_type { get; set; }

        public string display_pm_type
        {
            get
            {
                string a = "";
                if (pm_type == 1)
                {
                    a = "Phạt đi muộn";
                }
                else if (pm_type == 2)
                {
                    a = "Phạt về sớm";
                }

                return a;
            }
        }

        public string display_phat
        {
            get
            {
                string a = "";
                if (pm_type == 1)
                {
                    a = "Đi muộn " + pm_minute + " phút";
                }
                else if (pm_type == 2)
                {
                    a = "Về sớm " + pm_minute + " phút";
                }

                return a;
            }
        }

        public string pm_minute { get; set; }
        public int pm_type_phat { get; set; }
        public string pm_time_begin { get; set; }

        public string display_pm_time_begin
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(pm_time_begin) && DateTime.TryParse(pm_time_begin, out day))
                {
                    result = "Tháng " + day.ToString("MM/yyyy");
                }

                return result;
            }
        }

        public string pm_time_end { get; set; }

        public string display_pm_time_end
        {
            get
            {
                string result = "";
                DateTime day;
                if (!string.IsNullOrEmpty(pm_time_end) && DateTime.TryParse(pm_time_end, out day))
                {
                    result = "Tháng " + day.ToString("MM/yyyy");
                }

                return result;
            }
        }

        public string pm_monney { get; set; }

        public string display_pm_monney
        {
            get
            {
                string a = "";
                if (pm_type_phat == 1)
                {
                    int m;
                    if(int.TryParse(pm_monney,out m)) a = m.ToString("C0").Replace(@"$","") + " VNĐ/ca";
                    else a = pm_monney + " VNĐ/ca";
                }
                else if (pm_type_phat == 2)
                {
                    a = pm_monney + " công/ca";
                }

                return a;
            }
        }

        public string pm_time_created { get; set; }
        public string shift_name { get; set; }
    }

    public class API_ListLate
    {
        public bool result { get; set; }
        public int code { get; set; }
        public ListLate data { get; set; }
        public object error { get; set; }
    }
}