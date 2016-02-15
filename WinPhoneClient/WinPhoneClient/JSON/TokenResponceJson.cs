using System.Collections.Generic;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class TokenResponceJson : IBaseJsonValue
    {
        private static string TokenKey = "token";

        public string Token
        {
            get
            {
                if (Json != null && Json.ContainsKey(TokenKey))
                    return Json[TokenKey].GetString();
                return null;
            }
        }

        public string FormatedToken => $"Bearer {Token}";
        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(TokenKey, JsonValue.CreateStringValue(string.Empty))
            };
        }
    }
}
