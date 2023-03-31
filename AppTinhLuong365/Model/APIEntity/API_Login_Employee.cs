using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppTinhLuong365.Model.APIEntity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataLogin_Employee
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string com_id { get; set; }
        public string dep_id { get; set; }
        public string ep_address { get; set; }
        public string position_id { get; set; }
        public string role_id { get; set; }
        public string ep_authentic { get; set; }
        public string ep_image { get; set; }
        public string token { get; set; }
        public string ToMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }

        public string pass { get; set; }
        public string message { get; set; }
    }

    public class API_Login_Employee
    {
        public bool result { get; set; }
        public int code { get; set; }
        public DataLogin_Employee data { get; set; }
        public Error error { get; set; }
    }
}
