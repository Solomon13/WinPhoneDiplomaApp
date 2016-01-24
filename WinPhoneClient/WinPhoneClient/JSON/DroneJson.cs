using Windows.Data.Json;

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
        public JsonObject Json { get; set; } = new JsonObject();

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
