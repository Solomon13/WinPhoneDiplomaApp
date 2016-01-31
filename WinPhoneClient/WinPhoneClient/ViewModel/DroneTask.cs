using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight;
using Microsoft.VisualBasic.CompilerServices;
using WinPhoneClient.Enums;
using WinPhoneClient.JSON;
using WinPhoneClient.Model;

namespace WinPhoneClient.ViewModel
{
    public class DroneTask : ObservableObject, IJsonDependent
    {
        public int TaskId { get; }
        public int DroneId { get; set; }
        public string Description { get; set; }
        public int AddedTime { get; set; }

        public List<TaskValueJson> Values { get; set; }

        public Direction CurrentDirection
        {
            get
            {
                if (Values != null && Values.Any())
                    return Values.Last().Direction;

                return Direction.N;
            }
        }

        public BasicGeoposition LastTaskPosition
        {
            get
            {
                if (Values != null && Values.Any())
                {
                    var value = Values.Last();
                    return new BasicGeoposition
                    {
                        Altitude = value.Haight,
                        Latitude = value.Latitude,
                        Longitude = value.Longtitude
                    };
                }

                return new BasicGeoposition();
            }
        }

        public DroneTask(int commadId, int droneId)
        {
            if(commadId < 0 || droneId < 0)
                throw new ArgumentException("id");
            TaskId = commadId;
            DroneId = droneId;
        }
        public void ApplyJson(IBaseJsonValue json)
        {
            if (json is TaskJson)
            {
                var commandJson = json as TaskJson;
                Description = commandJson.Description;
                DroneId = (int)commandJson.DroneId;
                AddedTime = Helpers.Utils.ConvertToUnixTime(commandJson.Added);
            }
        }

        public IBaseJsonValue GetJsonValue()
        {
            var commandJson = new TaskJson();
            commandJson.Json = commandJson.CreateEmptyJsonObject();
            commandJson.CommandId = TaskId;
            commandJson.DroneId = DroneId;

            return commandJson;
        }
    }
}
