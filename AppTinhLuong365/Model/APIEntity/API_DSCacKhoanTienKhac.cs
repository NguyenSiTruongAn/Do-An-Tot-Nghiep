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
    public class DataDSCacKhoanTienKhac
    {
        public List<DSCacKhoanTienKhac> other_list { get; set; }
        public string message { get; set; }
    }

    public class DSCacKhoanTienKhac : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string cl_id { get; set; }
        private string _cl_name;
        public string cl_name
        {
            get { return _cl_name; }
            set
            {
                _cl_name = value; OnPropertyChanged();
            }
        }
        public object cl_salary { get; set; }
        public string cl_active { get; set; }
        private string _cl_note;
        public string cl_note
        {
            get { return _cl_note; }
            set
            {
                _cl_note = value; OnPropertyChanged();
            }
        }
        public string cl_type { get; set; }
        public string cl_id_form { get; set; }
        public string cl_com { get; set; }
        private string _fs_repica;
        public string fs_repica
        {
            get { return _fs_repica; }
            set
            {
                _fs_repica = value; OnPropertyChanged();
            }
        }
        public string fs_name { get; set; }
        public string fs_data { get; set; }
    }

    public class API_DSCacKhoanTienKhac
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataDSCacKhoanTienKhac data { get; set; }
        public object error { get; set; }
    }
}
