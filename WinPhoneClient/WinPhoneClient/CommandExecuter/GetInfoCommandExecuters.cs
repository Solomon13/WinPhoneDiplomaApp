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
        public double Id { get; }

        protected GetInfoCommandExecuter(string requestUri, double id, string formatedToken)
        {
            if(string.IsNullOrEmpty(requestUri) || double.IsNaN(id) || string.IsNullOrEmpty(formatedToken))
                throw new ArgumentException();
            UriString = requestUri;
            Token = formatedToken;
            Id = id;
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
        public GetDroneInfoCommandExecuter(string serverUri, double id, string formatedToken) : base($"{serverUri}/v1/get/drone/{id}", id, formatedToken)
        {
        }
    }

    public class GetCommandInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetCommandInfoCommandExecuter(string serverUri, double id, string formatedToken) : base($"{serverUri}/v1/get/command/{id}", id, formatedToken)
        {
        }
    }

    public class GetRouteInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetRouteInfoCommandExecuter(string serverUri, double id, string formatedToken) : base($"{serverUri}/v1/get/route/{id}", id, formatedToken)
        {
        }
    }

    public class GetSensorInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetSensorInfoCommandExecuter(string serverUri, double id, string formatedToken) : base($"{serverUri}/v1/get/sensor/{id}", id, formatedToken)
        {
        }
    }

    public class GetValueInfoCommandExecuter : GetInfoCommandExecuter
    {
        public GetValueInfoCommandExecuter(string serverUri, double id, string formatedToken) : base($"{serverUri}/v1/get/value/{id}", id, formatedToken)
        {
        }
    }
}
