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
    public class ConnectToServerCommandExecuter : ICommandExecuter
    {
        public TokenRequestJson RequestJson { get; set; }

        public ConnectToServerCommandExecuter(string serverUri, TokenRequestJson requestJson)
        {
            if(string.IsNullOrEmpty(serverUri) || requestJson == null)
                throw new ArgumentException();
            RequestJson = requestJson;
            UriString = $"{serverUri}/v1/get/token";
        }

        public string UriString { get; }

        public async Task<IBaseJsonValue> ExecuteAsync()
        {
            TokenResponceJson responceJson = null;
            try
            {

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responce = await httpClient.PostAsync(new Uri(UriString),
                new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(TokenRequestJson.LoginKey, RequestJson.Login),
                        new KeyValuePair<string, string>(TokenRequestJson.PasswordKey, RequestJson.Password)
                }));

                if (responce.IsSuccessStatusCode)
                {
                    responceJson = new TokenResponceJson { Json = JsonObject.Parse(await responce.Content.ReadAsStringAsync()) };
                }
            }
            catch (Exception)
            {
                responceJson = null;
            }

            return responceJson;
        }
    }
}
