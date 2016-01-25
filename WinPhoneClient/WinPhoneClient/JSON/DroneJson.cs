using System;
using System.Collections.Generic;
using Windows.Data.Json;
using WinPhoneClient.Enums;

namespace WinPhoneClient.JSON
{
    public class DroneJson : IBaseJsonValue
    {
        private static string IdKey = "id";
        private static string NameKey = "name";
        private static string StatusKey = "status";
        private static string TypeKey = "type";
        private static string AvailableKey = "available";
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
                new KeyValuePair<string, IJsonValue>(StatusKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(TypeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(AvailableKey, JsonValue.CreateNumberValue(0)),
                new KeyValuePair<string, IJsonValue>(CreatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(UpdatedTimeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DeletedTimeKey, JsonValue.CreateStringValue(string.Empty))
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

        public DroneStatus Status
        {
            get
            {
                if (Json != null && Json.ContainsKey(StatusKey))
                    return (DroneStatus)Enum.Parse(typeof(DroneStatus), Json[StatusKey].GetString());
                return DroneStatus.inactive;
            }
            set
            {
                if (Json != null && Json.ContainsKey(StatusKey))
                    Json[StatusKey] = JsonValue.CreateStringValue(value.ToString());
            }
        }

        public DroneType DroneType
        {
            get
            {
                if (Json != null && Json.ContainsKey(TypeKey))
                    return (DroneType) Enum.Parse(typeof(DroneType), Json[TypeKey].GetString());
                return DroneType.aircraft;
            }
            set
            {
                if (Json != null && Json.ContainsKey(TypeKey))
                    Json[TypeKey] = JsonValue.CreateStringValue(value.ToString());
            }
        }

        public bool Available
        {
            get
            {
                if (Json != null && Json.ContainsKey(AvailableKey))
                    return Json[AvailableKey].GetNumber() != 0;
                return false;
            }
            set
            {
                if (Json != null && Json.ContainsKey(AvailableKey))
                    Json[AvailableKey] = JsonValue.CreateNumberValue(value ? 1 : 0);
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
