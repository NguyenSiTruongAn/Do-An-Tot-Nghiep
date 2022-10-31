using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_rose_nv
    {
        public List<Rose> rose { get; set; }
        public string message { get; set; }
    }

    public class API_List_rose_nv
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_rose_nv data { get; set; }
        public object error { get; set; }
    }

    public class Rose
    {
        public int id_ep { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public double sum_rose { get; set; }
        public string rose_1 { get; set; }
        public string display_rose_1 {
            get
            {
                string a = "";
                if (Convert.ToDouble(rose_1) >= 0)
                {
                    double m;
                    if (double.TryParse(rose_1, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(rose_1.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string rose_2 { get; set; }
        public string display_rose_2
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(rose_2) >= 0)
                {
                    double m;
                    if (double.TryParse(rose_2, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(rose_2.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string rose_3 { get; set; }
        public string display_rose_3
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(rose_3) >= 0)
                {
                    double m;
                    if (double.TryParse(rose_3, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(rose_3.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string rose_4 { get; set; }
        public string display_rose_4
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(rose_4) >= 0)
                {
                    double m;
                    if (double.TryParse(rose_4, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(rose_4.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string rose_5 { get; set; }
        public string display_rose_5
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(rose_5) >= 0)
                {
                    double m;
                    if (double.TryParse(rose_5, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(rose_5.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
    }
}
