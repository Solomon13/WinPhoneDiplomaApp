using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class TokenRequestJson : IBaseJsonValue
    {
        public static string LoginKey = "email";
        public static string PasswordKey = "password";
        public TokenRequestJson(string login, string password)
        {
            if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                throw  new AggregateException();
            Json = new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(LoginKey, JsonValue.CreateStringValue(login)),
                new KeyValuePair<string, IJsonValue>(PasswordKey, JsonValue.CreateStringValue(password))
            };
        }
        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(LoginKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(PasswordKey, JsonValue.CreateStringValue(string.Empty))
            };
        }

        public string Login
        {
            get
            {
                if (Json != null && Json.ContainsKey(LoginKey))
                    return Json[LoginKey].GetString();
                return null;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                    return;
                if (Json != null && Json.ContainsKey(LoginKey))
                    Json[LoginKey] = JsonValue.CreateStringValue(value);
            }
        }

        public string Password
        {
            get
            {
                if (Json != null && Json.ContainsKey(PasswordKey))
                    return Json[PasswordKey].GetString();
                return null;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                    return;
                if (Json != null && Json.ContainsKey(PasswordKey))
                    Json[PasswordKey] = JsonValue.CreateStringValue(value);
            }
        }
    }
}
