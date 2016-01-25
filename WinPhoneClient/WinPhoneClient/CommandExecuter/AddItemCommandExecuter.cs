using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using WinPhoneClient.JSON;

namespace WinPhoneClient.CommandExecuter
{
    public abstract class AddItemCommandExecuter : ICommandExecuter
    {
        public string UriString { get; }
        public string Token { get; set; }
        public IBaseJsonValue ItemToAdd { get; }

        protected AddItemCommandExecuter(string requestUri, string formatedToken, IBaseJsonValue itemToAdd)
        {
            if (string.IsNullOrEmpty(requestUri) || string.IsNullOrEmpty(formatedToken) || itemToAdd == null)
                throw new ArgumentException();
            UriString = requestUri;
            Token = formatedToken;
            ItemToAdd = itemToAdd;
        }

        public async Task<IBaseJsonValue> ExecuteAsync()
        {
            BaseJson baseJson = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(UriString));
                requestMessage.Headers.Add("Authorization", Token);
                requestMessage.Content = new StringContent(ItemToAdd.Json.Stringify(), Encoding.UTF8, "application/json");
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

    public class AddDroneItemCommandExecuter : AddItemCommandExecuter
    {
        public AddDroneItemCommandExecuter(string serverUri, string formatedToken, UpdateDroneJson droneToAdd) 
            : base($"{serverUri}/v1/add/drone", formatedToken, droneToAdd)
        {
        }
    }
}
