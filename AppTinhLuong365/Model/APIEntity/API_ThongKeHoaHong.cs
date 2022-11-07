using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataThongKeHoaHong
    {
        //public SumRose sum_rose { get; set; }
        //public Rose1 rose_1 { get; set; }
        //public Rose2 rose_2 { get; set; }
        //public Rose3 rose_3 { get; set; }
        //public Rose4 rose_4 { get; set; }
        //public Rose5 rose_5 { get; set; }
        public List<ThongKeHoaHong> rose { get; set; }
        public string message { get; set; }
    }

    public class API_ThongKeHoaHong
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataThongKeHoaHong data { get; set; }
        public object error { get; set; }
    }

    public class ThongKeHoaHong
    {
        public string id_ep { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string sum_rose { get; set; }
        public string display_sum_rose
        {
            get
            {
                string a = "";
                if (Convert.ToDouble(sum_rose) >= 0)
                {
                    double m;
                    if (double.TryParse(sum_rose, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(sum_rose.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string rose_1 { get; set; }
        public string display_rose_1
        {
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

    public class Rose1
    {
        public int _2690 { get; set; }
    }

    public class Rose2
    {
        public int _2690 { get; set; }
        public int _15145 { get; set; }
        public int _15011 { get; set; }
        public int _3748 { get; set; }
        public int _6943 { get; set; }
        public int _7370 { get; set; }
    }

    public class Rose3
    {
        public int _2690 { get; set; }
        public double _3166 { get; set; }
        public double _7370 { get; set; }
    }

    public class Rose4
    {
        public int _14998 { get; set; }
        public int _3166 { get; set; }
        public int _7370 { get; set; }
        public int _7442 { get; set; }
    }

    public class Rose5
    {
        public int _15145 { get; set; }
        public int _14957 { get; set; }
        public double _7370 { get; set; }
        public double _7442 { get; set; }
    }

    public class SumRose
    {
        public int _2690 { get; set; }
        public int _14998 { get; set; }
        public int _15145 { get; set; }
        public int _14957 { get; set; }
        public int _15011 { get; set; }
        public int _3748 { get; set; }
        public int _6943 { get; set; }
        public double _7370 { get; set; }
        public double _3166 { get; set; }
        public double _7442 { get; set; }
    }
}
