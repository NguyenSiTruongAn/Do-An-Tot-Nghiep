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
    public class List_Np_Improperly
    {
        public List<List> list { get; set; }
        public string message { get; set; }
    }

    public class List: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string pc_money { get; set; }
        public string pc_time { get; set; }
        public string pc_shift { get; set; }
        public string pc_id { get; set; }
        private int _type1;
        public int type1
        {
            get { return _type1; }
            set { _type1 = value;OnPropertyChanged(); }
        }

    }

    public class API_List_Np_Improperly
    {
        public bool result { get; set; }
        public int code { get; set; }
        public List_Np_Improperly data { get; set; }
        public object error { get; set; }
    }
}
