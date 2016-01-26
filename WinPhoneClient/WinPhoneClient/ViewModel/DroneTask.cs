using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using GalaSoft.MvvmLight;
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

        public DroneTask(int commadId, int droneId)
        {
            if(commadId < 0 || droneId < 0)
                throw new ArgumentException("id");
            TaskId = commadId;
            DroneId = droneId;
        }
        public void ApplyJson(IBaseJsonValue json)
        {
            if (json is CommandJson)
            {
                var commandJson = json as CommandJson;
                Description = commandJson.Description;
                DroneId = (int)commandJson.DroneId;
            }
        }

        public IBaseJsonValue GetJsonValue()
        {
            var commandJson = new CommandJson();
            commandJson.Json = commandJson.CreateEmptyJsonObject();
            commandJson.CommandId = TaskId;
            commandJson.DroneId = DroneId;

            return commandJson;
        }
    }
}
