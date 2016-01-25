using System;
using System.Collections.Generic;
using Windows.Data.Json;
using WinPhoneClient.Enums;

namespace WinPhoneClient.JSON
{
    public class UpdateDroneJson : IBaseJsonValue
    {
        private static string NameKey = "name";
        private static string StatusKey = "status";
        private static string TypeKey = "type";
        private static string AvailableKey = "available";

        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(NameKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(StatusKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(TypeKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(AvailableKey, JsonValue.CreateBooleanValue(false)),
            };
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
                    return (DroneType)Enum.Parse(typeof(DroneType), Json[TypeKey].GetString());
                return DroneType.aircraft;
            }
            set
            {
                if (Json != null && Json.ContainsKey(TypeKey))
                    Json[TypeKey] = JsonValue.CreateStringValue(value.ToString());
            }
        }

        public double Available
        {
            get
            {
                if (Json != null && Json.ContainsKey(AvailableKey))
                    return Json[AvailableKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(AvailableKey))
                    Json[AvailableKey] = JsonValue.CreateNumberValue(value);
            }
        }
    }

}
