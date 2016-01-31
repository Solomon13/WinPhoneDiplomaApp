using System;
using System.Collections.Generic;
using Windows.Data.Json;
using WinPhoneClient.Enums;

namespace WinPhoneClient.JSON
{
    public class TaskJson : IBaseJsonValue
    {
        private static string IdKey = "id";
        private static string DescriptionKey = "description";
        private static string StatusKey = "status";
        private static string DroneIdKey = "drone_id";
        private static string AddedKey = "added";
        private static string CreatedTimeKey = "created_at";
        private static string UpdatedTimeKey = "updated_at";
        private static string DeletedTimeKey = "deleted_at";
        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(IdKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DescriptionKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(StatusKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DroneIdKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(AddedKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(CreatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(UpdatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DeletedTimeKey, JsonValue.CreateStringValue(string.Empty)),
            };
        }

        public double CommandId
        {
            get
            {
                if (Json != null && Json.ContainsKey(IdKey))
                    return Json[IdKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(IdKey))
                    Json[IdKey] = JsonValue.CreateNumberValue(value);
            }
        }

        public string Description
        {
            get
            {
                if (Json != null && Json.ContainsKey(DescriptionKey))
                    return Json[DescriptionKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(DescriptionKey))
                    Json[DescriptionKey] = JsonValue.CreateStringValue(value);
            }
        }

        

        public double DroneId
        {
            get
            {
                if (Json != null && Json.ContainsKey(DroneIdKey))
                    return Json[DroneIdKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(DroneIdKey))
                    Json[DroneIdKey] = JsonValue.CreateNumberValue(value);
            }
        }

        public string Added
        {
            get
            {
                if (Json != null && Json.ContainsKey(AddedKey))
                    return Json[AddedKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(AddedKey))
                    Json[AddedKey] = JsonValue.CreateStringValue(value);
            }
        }

        public string CreatedAt
        {
            get
            {
                if (Json != null && Json.ContainsKey(CreatedTimeKey))
                    return Json[CreatedTimeKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(CreatedTimeKey))
                    Json[CreatedTimeKey] = JsonValue.CreateStringValue(value);
            }
        }

        public string UpdatedAt
        {
            get
            {
                if (Json != null && Json.ContainsKey(UpdatedTimeKey))
                    return Json[UpdatedTimeKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(UpdatedTimeKey))
                    Json[UpdatedTimeKey] = JsonValue.CreateStringValue(value);
            }
        }

        public string DeletedAt
        {
            get
            {
                if (Json != null && Json.ContainsKey(DeletedTimeKey))
                    return Json[DeletedTimeKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(DeletedTimeKey))
                    Json[DeletedTimeKey] = JsonValue.CreateStringValue(value);
            }
        }
    }
}
