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

        public async Task<IBaseJsonValue> ExecuteAsync()
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

    public class GetDroneInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetDroneInfoCommandExecuter(string serverUri, double id, string formatedToken) 
            : base($"{serverUri}/v1/get/drone/{id}", formatedToken)
        {
        }
    }

    public class GetAllDronesInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetAllDronesInfoCommandExecuter(string serverUri, string formatedToken)
            : base($"{serverUri}/v1/get/drone/", formatedToken)
        {
        }
    }

    public class GetCommandsInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetCommandsInfoCommandExecuter(string serverUri, double droneId, string formatedToken) 
            : base($"{serverUri}/v1/get/command/{droneId}", formatedToken)
        {
        }
    }

    public class GetRoutesInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetRoutesInfoCommandExecuter(string serverUri, double droneId, string formatedToken) 
            : base($"{serverUri}/v1/get/route/{droneId}", formatedToken)
        {
        }
    }

    public class GetSensorsInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetSensorsInfoCommandExecuter(string serverUri, double droneId, string formatedToken) 
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
            : base($"{serverUri}/v1/values/{sensorId}", formatedToken)
        {
        }
    }

    public class GetAllSensorsCommandExecuter : GetInfoCommandExecuter
    {
        public GetAllSensorsCommandExecuter(string serverUri, string formatedToken) 
            : base($"{serverUri}/v1/sensor/", formatedToken)
        {
        }
    }
}
