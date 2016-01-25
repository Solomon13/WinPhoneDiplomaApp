using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class UpdateSensorJson : IBaseJsonValue
    {
        private static string NameKey = "name";
        private static string DroneIdKey = "drone_id";
        public JsonObject Json { get; set; }
        public JsonObject CreateEmptyJsonObject()
        {
            return new JsonObject
            {
                new KeyValuePair<string, IJsonValue>(NameKey, JsonValue.CreateStringValue(string.Empty)),
                new KeyValuePair<string, IJsonValue>(DroneIdKey, JsonValue.CreateNumberValue(0))
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
    }
}
