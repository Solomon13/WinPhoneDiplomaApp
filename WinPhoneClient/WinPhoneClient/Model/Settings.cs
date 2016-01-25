using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinPhoneClient.JSON;

namespace WinPhoneClient.Model
{
    public class Settings
    {
        public static string DefaultHost = @"https://api.data-center.in.ua";
        public static string DefaultUserName = @"alexeyzherehi@gmail.com";
        public static string DefaultPassword = @"mQznef";
        public string Host { get; set; } = DefaultHost;
        public string Login { get; set; } = DefaultUserName;
        public string Password { get; set; } = DefaultPassword;
        public TokenResponceJson Token { get; set; }
    }
}
