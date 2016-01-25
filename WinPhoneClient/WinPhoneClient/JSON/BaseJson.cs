using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class BaseJson : IBaseJsonValue
    {
        private static string ResultKey = "success";
        private static string ErrorKey = "msg";
        private static string DataKey = "data";
        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(ResultKey, JsonValue.CreateBooleanValue(false)),
                new KeyValuePair<string, IJsonValue>(ErrorKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DataKey, null)
            };
        }

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

        public object Data
        {
            get
            {
                if (Json != null && Json.ContainsKey(DataKey))
                {
                    var value = Json[DataKey];
                    if (value != null)
                    {
                        if (IsArray(value))
                            return value.GetArray();
                        return value.GetObject();
                    }
                }
                return null;
            }
        }

        private bool IsArray(IJsonValue value)
        {
            if (value != null)
            {
                try
                {
                    value.GetArray();
                    return true;
                }
                catch (Exception)
                {
                }
            }

            return false;
        }
    }
}
