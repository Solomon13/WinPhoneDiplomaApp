using System;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class BaseJson : IBaseJsonValue
    {
        private static string ResultKey = "success";
        private static string ErrorKey = "msg";
        private static string DataKey = "data";
        public JsonObject Json { get; set; } = new JsonObject();

        public bool Result
        {
            get
            {
                if (Json != null && Json.ContainsKey(ResultKey))
                    return Json[ResultKey].GetBoolean();
                return false;
            }
            set
            {
                if (Json != null && Json.ContainsKey(ResultKey))
                    Json[ResultKey] = JsonValue.CreateBooleanValue(value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                if (Json != null && Json.ContainsKey(ErrorKey))
                    return Json[ErrorKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(ErrorKey))
                    Json[ErrorKey] = JsonValue.CreateStringValue(value);
            }
        }

        public JsonObject Data
        {
            get
            {
                if (Json != null && Json.ContainsKey(DataKey))
                    return Json[DataKey].GetObject();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(DataKey))
                    Json[DataKey] = value;
            }
        }

        public static implicit operator BaseJson(TokenResponceJson v)
        {
            throw new NotImplementedException();
        }
    }
}
