using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public class UpdateDroneJson : IBaseJsonValue
    {
        private static string NameKey = "name";
        private static string StatusKey = "status";
        private static string TypeKey = "type";
        private static string AvailableKey = "available";

        public JsonObject Json { get; set; } = new JsonObject();

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

        public string Status
        {
            get
            {
                if (Json != null && Json.ContainsKey(StatusKey))
                    return Json[StatusKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(StatusKey))
                    Json[StatusKey] = JsonValue.CreateStringValue(value);
            }
        }

        public string DroneType
        {
            get
            {
                if (Json != null && Json.ContainsKey(TypeKey))
                    return Json[TypeKey].GetString();
                return null;
            }
            set
            {
                if (Json != null && Json.ContainsKey(TypeKey))
                    Json[TypeKey] = JsonValue.CreateStringValue(value);
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
