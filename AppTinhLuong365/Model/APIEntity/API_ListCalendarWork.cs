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
    public class ListCalendarWork
    {
        public List<AllCalendar> all_calendar { get; set; }
        public List<GeneralCalendar> general_calendar { get; set; }
        public List<PersonalCalendar> personal_calendar { get; set; }
        public string message { get; set; }
    }

    public class AllCalendar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string apply_month { get; set; }

        public string display_apply_month
        {
            get
            {
                string a = DateTime.Parse(apply_month).ToString("MM/yyyy");
                return a;
            }
        }

        public string cy_id { get; set; }
        public string cy_name { get; set; }
        public string num_ep { get; set; }
        public string is_personal { get; set; }
        // public string status { get; set; }
        private Boolean _status;

        public Boolean status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
    }

    public class GeneralCalendar
    {
        public string apply_month { get; set; }
        public string cy_id { get; set; }
        public string cy_name { get; set; }
        public string num_ep { get; set; }
        public string is_personal { get; set; }
    }

    public class PersonalCalendar
    {
        public string apply_month { get; set; }
        public string cy_id { get; set; }
        public string cy_name { get; set; }
        public string num_ep { get; set; }
        public string is_personal { get; set; }
    }

    public class API_ListCalendarWork
    {
        public bool result { get; set; }
        public int code { get; set; }
        public ListCalendarWork data { get; set; }
        public object error { get; set; }
    }
}
