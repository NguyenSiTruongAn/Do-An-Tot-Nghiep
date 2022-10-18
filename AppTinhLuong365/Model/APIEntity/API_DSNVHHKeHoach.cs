using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDSNVHHKeHoach
    {
        public List<DSNVHHKeHoach> list { get; set; }
        public string message { get; set; }
    }

    public class DSNVHHKeHoach : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string ro_id { get; set; }
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string ep_image { get; set; }
        public string ro_time { get; set; }
        public string display_ro_time
        {
            get
            {
                string result = "Tháng " + DateTime.Parse(ro_time).ToString("MM/yyyy");
                return result;
            }
        }
        public string tl_id { get; set; }
        public string tl_name { get; set; }
        public object tl_hoahong { get; set; }
        private string _ro_kpi_active;
        public string ro_kpi_active
        {
            get { return _ro_kpi_active; }
            set
            {
                _ro_kpi_active = value; OnPropertyChanged();
            }
        }
        public string tl_kpi_yes { get; set; }
        public string display_tl_kpi_yes
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(tl_kpi_yes) >= 0)
                {
                    double m;
                    if (double.TryParse(tl_kpi_yes, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tl_kpi_yes.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string tl_kpi_no { get; set; }
        public string display_tl_kpi_no
        {
            get
            {
                string a = "";
                if (Convert.ToInt64(tl_kpi_no) >= 0)
                {
                    double m;
                    if (double.TryParse(tl_kpi_no, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(tl_kpi_no.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }

                return a;
            }
        }
        public string ro_note { get; set; }
        public string ro_id_tl { get; set; }
    }

    public class API_DSNVHHKeHoach
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVHHKeHoach data { get; set; }
        public object error { get; set; }
    }
}
