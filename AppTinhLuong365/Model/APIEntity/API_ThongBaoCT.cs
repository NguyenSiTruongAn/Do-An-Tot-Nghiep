using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ThongBaoCT
    {
        public string tb_id { get; set; }
        public string tb_gui { get; set; }
        public object tb_nhan { get; set; }
        public string tb_com { get; set; }
        public string tb_nd_tb { get; set; }
        public string tb_link { get; set; }
        public string tb_quyen { get; set; }
        public string tb_loai { get; set; }
        public string tb_active { get; set; }
        public string tb_time_created { get; set; }
        public string display_tb_time_created
        {
            get
            {
                string result;
                long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                long time = epoch - long.Parse(tb_time_created);
                if (time < 60)
                    result = time + " giây trước";
                else
                    if (time < 3600)
                    result = time / 60 + " phút trước";
                else
                    if (time < 86400)
                    result = time / 3600 + " giờ trước";
                else if (time < 2592000)
                    result = time / 86400 + " ngày trước";
                else if (time < 31536000)
                    result = time / 2592000 + " tháng trước";
                else
                    result = time / 31536000 + " năm trước";
                return result;
            }
        }
        public string image { get; set; }
        public string display_ep_image
        {
            get
            {
                string result = "https://tinhluong.timviec365.vn/img/add.png";
                if (!string.IsNullOrEmpty(image))
                {
                    if(image != "../img/add.png")
                    {
                        result = "https://chamcong.24hpay.vn/upload/employee/" + image;
                    }
                }
                return result;
            }
        }
        public string com_name { get; set; }
        public string ep_name { get; set; } = "";
    }

    public class DataThongBaoCT
    {
        [JsonProperty("0")]
        public List<ThongBaoCT> abc { get; set; }
        public string message { get; set; }
    }

    public class API_ThongBaoCT
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataThongBaoCT data { get; set; }
        public object error { get; set; }
    }
}
