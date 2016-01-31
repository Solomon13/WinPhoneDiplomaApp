using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        protected UpdateInfoCommandExecuter(string requestUri, string formatedToken, IBaseJsonValue updateJson)
        {
            if (string.IsNullOrEmpty(requestUri) || string.IsNullOrEmpty(formatedToken) || updateJson == null)
                throw new ArgumentException();
            UriString = requestUri;
            Token = formatedToken;
            UpdateJson = updateJson;
        }
        public virtual async Task<IBaseJsonValue> ExecuteAsync()
        {
            BaseJson baseJson = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri(UriString));
                requestMessage.Headers.Add("Authorization", Token);
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (var key in UpdateJson.Json.Keys)
                {
                    object value;
                    switch (UpdateJson.Json[key].ValueType)
                    {
                        case JsonValueType.String:
                            value = UpdateJson.Json[key].GetString();
                            break;
                        case JsonValueType.Number:
                            value = UpdateJson.Json[key].GetNumber();
                            break;
                        case JsonValueType.Boolean:
                            value = UpdateJson.Json[key].GetBoolean();
                            break;
                        default:
                            value = string.Empty;
                            break;
                    }
                    values.Add(key, value.ToString());
                }
                requestMessage.Content = new FormUrlEncodedContent(values);
                var responce = await httpClient.SendAsync(requestMessage);
                if (responce.IsSuccessStatusCode)
                {
                    baseJson = new BaseJson { Json = JsonObject.Parse(await responce.Content.ReadAsStringAsync()) };
                    if (!baseJson.Result)
                        return null;
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
        public UpdateDroneInfoCommandExecuter(string serverUri, string formatedToken, double droneId, UpdateDroneJson updateJson) 
            : base($"{serverUri}/v1/update/drone/{droneId}", formatedToken, updateJson)
        {
        }
    }

    //public class UpdateSensorCommandExecuter : UpdateInfoCommandExecuter
    //{
    //    public UpdateSensorCommandExecuter(string serverUri, double sensorId, string formatedToken, IBaseJsonValue updateJson) 
    //        : base($"{serverUri}/v1/update/sensor/{sensorId}", sensorId, formatedToken, updateJson)
    //    {
    //    }
    //}
}
