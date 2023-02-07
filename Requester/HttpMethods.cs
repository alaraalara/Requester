
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Auxiliary;
using Newtonsoft.Json;
using Requester.Models;
using RestSharp.Authenticators.OAuth;
using System.Net.Http.Headers;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace Requester
{
    public partial class Requester
    {
        AuthType authType;


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
            if(authType == AuthType.OAUTH)
            {
            }
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

        public async Task<Token> GetElibilityToken(string baseAddress, string grant_type, string client_id, string client_secret, string username, string password)
        {
            Console.WriteLine("a0");
            var form = FormUrlEncodedConverter.ToFormUrlEncoded(new Dictionary<string, string>
                {
                    {"grant_type", "password" },
                    {"client_id", client_id },
                    {"client_secret", client_secret },
                    {"username", username },
                    {"password", password },
                    {"scope", "api api:concurrent_access"},
                    {"scope", "api"}
                });
            Console.WriteLine("b1");
            /*
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("grant_type", grant_type);
            /*
                {
                    {"grant_type", grant_type},
                    {"client_id", client_id},
                    {"client_secret", client_secret},
                    {"username", username},
                    {"password", password},
                    {"scope", "api api:concurrent_access"},
                    {"scope", "api"},

                };*/
            HttpResponseMessage tokenResponse = await client.PostAsync(baseAddress, new StringContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            Token tok = JsonConvert.DeserializeObject<Token>(jsonContent);
            Console.WriteLine(jsonContent);
            return tok;
        }


    }
    public enum AuthType
    {
        REST, OAUTH
    }
}
