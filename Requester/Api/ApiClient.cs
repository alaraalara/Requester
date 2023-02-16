
using Azure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Requester.Models;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using static System.Formats.Asn1.AsnWriter;

namespace Requester.Api
{
    public class ApiClient
    {
        private AuthType authType;
        private RestClient client;
        private string? token;

        public ApiClient(AuthType authType, string BasePath)
        {
            this.authType = authType;
            client = new RestClient(BasePath);
        }

        public RestResponse Logout(Log log)
        {
            var request = new RestRequest("/22R193/entity/auth/logout", Method.Post);

            request.AddBody(new StringContent(log.Body, Encoding.UTF8, "application/json"));
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }

        public void AddBasicAuthentication(string encoded)
        {
            client.AddDefaultHeader("Authorization", "Basic " + encoded);
            //client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        
        public RestResponse HttpCall(Log log)
        {         
            if (log.QueryString.IsNullOrEmpty()==false)
            {
                log.Path += log.QueryString;
            }
            Enum.TryParse(log.Method, true, out Method method);
            var request = new RestRequest(log.Path, method);
            request.AddHeader("Accept", "application/json");
            if (log.Body.IsNullOrEmpty()==false)
            {
                request.AddParameter("application/json", log.Body, ParameterType.RequestBody);
            }
            return client.Execute(request);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string? AddToken(Dictionary<string, string> credentials)
        {
            var request = new RestRequest("/identity/connect/token", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", credentials["grant_type"]);
            request.AddParameter("client_id", credentials["client_id"]);
            request.AddParameter("client_secret", credentials["client_secret"]);
            request.AddParameter("username", credentials["username"]);
            request.AddParameter("password", credentials["password"]);
            request.AddParameter("scope", "api");
            var response = client.Execute(request);
            if((int) response.StatusCode == 200)
            {
                try
                {
                    token = JObject.Parse(response.Content)["access_token"].ToString();
                    client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
                    return token;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            else
            {
                throw new Exception("can not receive token");
            }
         
        }


    }
    public enum AuthType
    {
        REST, OAUTH, BASIC
    }
}
