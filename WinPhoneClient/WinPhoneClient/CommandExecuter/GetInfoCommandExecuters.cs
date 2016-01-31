using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Data.Json;
using WinPhoneClient.JSON;

namespace WinPhoneClient.CommandExecuter
{
    public abstract class GetInfoCommandExecuter : IBaseCommandExecuter
    {
        public string UriString { get; }
        public string Token { get; set; }

        protected GetInfoCommandExecuter(string requestUri, string formatedToken)
        {
            if(string.IsNullOrEmpty(requestUri) || string.IsNullOrEmpty(formatedToken))
                throw new ArgumentException();
            UriString = requestUri;
            Token = formatedToken;
        }

        public virtual async Task<IBaseJsonValue> ExecuteAsync()
        {
            BaseJson baseJson = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(UriString));
                requestMessage.Headers.Add("Authorization", Token);
                var responce = await httpClient.SendAsync(requestMessage);
                if (responce.IsSuccessStatusCode)
                    baseJson = new BaseJson { Json = JsonObject.Parse(await responce.Content.ReadAsStringAsync()) };
            }
            catch (Exception)
            {
                baseJson = null;
            }

            return baseJson;
        }
    }

    public class GetDroneCommandExecuter : GetInfoCommandExecuter
    {
        public GetDroneCommandExecuter(string serverUri, string formatedToken, double id) 
            : base($"{serverUri}/v1/get/drone/{id}", formatedToken)
        {
        }
    }

    public class GetAllDronesCommandExecuter : GetInfoCommandExecuter
    {
        public GetAllDronesCommandExecuter(string serverUri, string formatedToken)
            : base($"{serverUri}/v1/get/drone/", formatedToken)
        {
        }
    }

    public class GetDroneTasksCommandExecuter : GetInfoCommandExecuter
    {
        public GetDroneTasksCommandExecuter(string serverUri, string formatedToken, int droneId) 
            : base($"{serverUri}/v1/get/task/{droneId}", formatedToken)
        {
        }

        public GetDroneTasksCommandExecuter(string serverUri, string formatedToken, int droneId, int unixStartTime, int unixEndTime)
    : base($"{serverUri}/v1/get/task/{droneId}/{unixStartTime}/{unixEndTime}", formatedToken)
        {
        }
    }

    public class GetTaskValuesCommandExecuter : GetInfoCommandExecuter
    {
        public GetTaskValuesCommandExecuter(string serverUri, string formatedToken, double taskId)
            : base($"{serverUri}/v1/getTaskValues/task/{taskId}", formatedToken)
        {
        }
    }

    public class GetDroneRoutesCommandExecuter : GetInfoCommandExecuter
    {
        public GetDroneRoutesCommandExecuter(string serverUri, string formatedToken, double droneId) 
            : base($"{serverUri}/v1/get/route/{droneId}", formatedToken)
        {
        }

        public GetDroneRoutesCommandExecuter(string serverUri, string formatedToken, double droneId, int unixStartTime, int unixEndTime)
            : base($"{serverUri}/v1/get/route/{droneId}/{unixStartTime}/{unixEndTime}", formatedToken)
        {
        }
    }

    public class GetDroneSensorsCommandExecuter : GetInfoCommandExecuter
    {
        public GetDroneSensorsCommandExecuter(string serverUri, string formatedToken, double droneId) 
            : base($"{serverUri}/v1/get/sensor/{droneId}", formatedToken)
        {
        }
    }

    public class GetAvailableDronesInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetAvailableDronesInfoCommandExecuter(string serverUri, string formatedToken, int bAvailable = 1) 
            : base($"{serverUri}/v1/getAvailable/drones/{bAvailable}", formatedToken)
        {
        }
    }

    public class GetSensorValuesCommandExecuter : GetInfoCommandExecuter
    {
        public GetSensorValuesCommandExecuter(string serverUri, string formatedToken, int sensorId) 
            : base($"{serverUri}/v1/get/sensorValues/{sensorId}", formatedToken)
        {
        }

        public GetSensorValuesCommandExecuter(string serverUri, string formatedToken, int sensorId, int unixStartTime, int unixEndTime)
    : base($"{serverUri}/v1/get/sensorValues/{sensorId}/{unixStartTime}/{unixEndTime}", formatedToken)
        {
        }
    }
}
