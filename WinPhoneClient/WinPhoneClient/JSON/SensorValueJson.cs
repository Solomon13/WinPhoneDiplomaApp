using System.Collections.Generic;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class SensorValueJson : IBaseJsonValue
    {
        private static string IdKey = "id";
        private static string LatitudeKey = "latitude";
        private static string LongtitudeKey = "longitude";
        private static string ValueKey = "value";
        private static string SensorIdKey = "sensor_id";
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
                new KeyValuePair<string, IJsonValue>(LatitudeKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(LongtitudeKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(ValueKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(AddedKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(SensorIdKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(CreatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(UpdatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DeletedTimeKey, JsonValue.CreateStringValue(string.Empty)),
            };
        }

        public double SensorValueId
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

        public double Latitude
        {
            get
            {
                if (Json != null && Json.ContainsKey(LatitudeKey))
                    return Json[LatitudeKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(LatitudeKey))
                    Json[LatitudeKey] = JsonValue.CreateNumberValue(value);
            }
        }

        public double SensorId
        {
            get
            {
                if (Json != null && Json.ContainsKey(SensorIdKey))
                    return Json[SensorIdKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(SensorIdKey))
                    Json[SensorIdKey] = JsonValue.CreateNumberValue(value);
            }
        }

        public double Longtitude
        {
            get
            {
                if (Json != null && Json.ContainsKey(LongtitudeKey))
                    return Json[LongtitudeKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(LongtitudeKey))
                    Json[LongtitudeKey] = JsonValue.CreateNumberValue(value);
            }
        }

        public double Value
        {
            get
            {
                if (Json != null && Json.ContainsKey(ValueKey))
                    return Json[ValueKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(ValueKey))
                    Json[ValueKey] = JsonValue.CreateNumberValue(value);
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
    }
}
