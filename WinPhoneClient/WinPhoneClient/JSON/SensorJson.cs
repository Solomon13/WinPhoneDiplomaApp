using System.Collections.Generic;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class SensorJson : IBaseJsonValue
    {
        private static string IdKey = "id";
        private static string NameKey = "name";
        private static string DroneIdKey = "drone_id";
        private static string CreatedTimeKey = "created_at";
        private static string UpdatedTimeKey = "updated_at";
        private static string DeletedTimeKey = "deleted_at";
        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(IdKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(NameKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DroneIdKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(CreatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(UpdatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DeletedTimeKey, JsonValue.CreateStringValue(string.Empty)),
            };
        }

        public double Id
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

        public string Name
        {
            get
            {
                if (Json != null && Json.ContainsKey(NameKey))
                    return Json[NameKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(NameKey))
                    Json[NameKey] = JsonValue.CreateStringValue(value);
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
