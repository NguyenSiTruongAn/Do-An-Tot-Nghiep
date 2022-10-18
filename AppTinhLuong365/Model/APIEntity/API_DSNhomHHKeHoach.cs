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
    public class DataDSNhomHHKeHoach
    {
        public List<DSNhomHHKeHoach> list { get; set; }
        public string message { get; set; }
    }

    public class EpGrKH
    {
        public string pr_rose { get; set; }
        public string pr_percent { get; set; }
        public string pr_id_user { get; set; }
        public string pr_id_tl { get; set; }
        public string ep_image { get; set; }
        public string display_ep_image
        {
            get
            {
                string result = "https://chamcong.24hpay.vn/upload/employee/" + ep_image;
                if (string.IsNullOrEmpty(ep_image))
                    result = "https://tinhluong.timviec365.vn/img/add.png";
                return result;
            }
        }
        public string ep_name { get; set; }
    }

    public class DSNhomHHKeHoach : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string ro_id { get; set; }
        public string ro_id_group { get; set; }
        public string lgr_name { get; set; }
        public string c_user { get; set; }
        public string tl_id { get; set; }
        public string tl_name { get; set; }
        public string ro_time { get; set; }
        public string display_ro_time
        {
            get
            {
                string result = "Tháng " + DateTime.Parse(ro_time).ToString("MM/yyyy");
                return result;
            }
        }
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
        public string ro_id_tl { get; set; }
        public List<EpGrKH> ep_gr { get; set; }
    }

    public class API_DSNhomHHKeHoach
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNhomHHKeHoach data { get; set; }
        public object error { get; set; }
    }
}
