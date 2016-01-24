using System;
using Windows.Data.Json;
using WinPhoneClient.Enums;

namespace WinPhoneClient.JSON
{
    public class CommandJson : IBaseJsonValue
    {
        private static string IdKey = "id";
        private static string DescriptionKey = "description";
        private static string LatitudeKey = "latitude";
        private static string LongtitudeKey = "longitude";
        private static string HeightKey = "height";
        private static string DirectionKey = "direction";
        private static string DroneIdKey = "drone_id";
        private static string AddedKey = "added";
        private static string CreatedTimeKey = "created_at";
        private static string UpdatedTimeKey = "updated_at";
        private static string DeletedTimeKey = "deleted_at";
        public JsonObject Json { get; set; } = new JsonObject();

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

        public double Haight
        {
            get
            {
                if (Json != null && Json.ContainsKey(HeightKey))
                    return Json[HeightKey].GetNumber();
                return double.NaN;
            }
            set
            {
                if (Json != null && Json.ContainsKey(HeightKey))
                    Json[HeightKey] = JsonValue.CreateNumberValue(value);
            }
        }

        public Direction Direction
        {
            get
            {
                if (Json != null && Json.ContainsKey(DirectionKey))
                    return (Direction) Enum.Parse(typeof(Direction), Json[DirectionKey].GetString());
                return Direction.N;
            }
            set
            {
                if (Json != null && Json.ContainsKey(DirectionKey))
                    Json[DirectionKey] = JsonValue.CreateStringValue(value.ToString());
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
