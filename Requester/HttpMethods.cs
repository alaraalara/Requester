
using Acumatica.Auth.Model;
using Newtonsoft.Json;
using Requester.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Requester
{
    public partial class Requester
    {
        


        public async Task<HttpResponseMessage> Logout(Log log)
        {
            string url = "http://" + host + portNumber + "/22R193/entity/auth/logout";
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Content = new StringContent(log.Body, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(message);
            return result;
        }

        /// <TODO>
        /// Implement OAuth way of login : value-> qlnmLLnLjkG8ZwHyQBgB_Q
        /// clientID=3210DB15-CD4E-AF21-A060-68F2FE1F22E3@Company
        /// </TODO>
        /// <param name="log"></param>
        /// 
        public async Task<HttpResponseMessage> HttpCall(Log log)
        {
            var url = "http://" + host + portNumber + log.Path;
            if(log.QueryString!= null)
            {
                url += log.QueryString;
            }
            var message = new HttpRequestMessage(new HttpMethod(log.Method), url);

            if (log.Body != null)
            {
                message.Content = new StringContent(log.Body, Encoding.UTF8, "application/json");
            }
            var response = await client.SendAsync(message);
            return response;
        }

        private static async Task<Token> GetElibilityToken()
        {
            string baseAddress = @"https://blah.blah.blah.com/oauth2/token";

            string grant_type = "client_credentials";
            string client_id = "myId";
            string client_secret = "shhhhhhhhhhhhhhItsSecret";

            var form = new Dictionary<string, string>
                {
                    {"grant_type", grant_type},
                    {"client_id", client_id},
                    {"client_secret", client_secret},
                };

            HttpResponseMessage tokenResponse = await client.PostAsync(baseAddress, new FormUrlEncodedContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            Token tok = JsonConvert.DeserializeObject<Token>(jsonContent);
            return tok;
        }


    }
}
