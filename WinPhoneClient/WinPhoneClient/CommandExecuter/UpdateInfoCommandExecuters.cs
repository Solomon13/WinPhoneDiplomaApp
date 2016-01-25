using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using WinPhoneClient.JSON;

namespace WinPhoneClient.CommandExecuter
{
    public abstract class UpdateInfoCommandExecuter : IBaseCommandExecuter
    {
        public string UriString { get; }
        public string Token { get; set; }

        public IBaseJsonValue UpdateJson { get; }
        public double DroneId { get; }

        public UpdateInfoCommandExecuter(string requestUri, double droneId, string formatedToken, IBaseJsonValue updateJson)
        {
            if (string.IsNullOrEmpty(requestUri) || double.IsNaN(droneId) || string.IsNullOrEmpty(formatedToken) || updateJson == null)
                throw new ArgumentException();
            UriString = requestUri;
            Token = formatedToken;
            DroneId = droneId;
            UpdateJson = updateJson;
        }
        public async Task<IBaseJsonValue> ExecuteAsync()
        {
            BaseJson baseJson = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri(UriString));
                requestMessage.Headers.Add("Authorization", Token);
                requestMessage.Content = new StringContent(UpdateJson.Json.Stringify(), Encoding.UTF8, "application/json");
                var responce = await httpClient.SendAsync(requestMessage);
                if (responce.IsSuccessStatusCode)
                {
                    baseJson = new BaseJson { Json = JsonObject.Parse(await responce.Content.ReadAsStringAsync()) };
                    //if (!baseJson.Result)
                    //    return baseJson;
                    //droneJson = new DroneJson { Json = baseJson.Data };
                }
            }
            catch (Exception)
            {
                baseJson = null;
            }

            return baseJson;
        }
    }

    public class UpdateDroneInfoCommandExecuter : UpdateInfoCommandExecuter
    {
        public UpdateDroneInfoCommandExecuter(string serverUri, double droneId, string formatedToken, UpdateDroneJson updateJson) 
            : base($"{serverUri}/v1/update/drone/{droneId}", droneId, formatedToken, updateJson)
        {
        }
    }

    public class UpdateSensorCommandExecuter : UpdateInfoCommandExecuter
    {
        public UpdateSensorCommandExecuter(string serverUri, double sensorId, string formatedToken, IBaseJsonValue updateJson) 
            : base($"{serverUri}/v1/update/sensor/{sensorId}", sensorId, formatedToken, updateJson)
        {
        }
    }
}
