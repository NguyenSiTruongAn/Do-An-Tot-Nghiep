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
    public class DataDSNVThemVaoCKTK
    {
        public List<DSNVThemVaoCKTK> list { get; set; }
        public string message { get; set; }
    }

    public class DSNVThemVaoCKTK: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string ep_id { get; set; }
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
        private Boolean _status;
        public Boolean status
        {
            get { return _status; }
            set
            {
                _status = value; OnPropertyChanged();
            }
        }
    }

    public class API_DSNVThemVaoCKTK
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSNVThemVaoCKTK data { get; set; }
        public object error { get; set; }
    }
}
