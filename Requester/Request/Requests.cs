using Microsoft.IdentityModel.Tokens;
using Requester.Api;
using Requester.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Requester
{
    public partial class Requests
    {
        private Log login = null; //for testing purposes, delete it later

        private bool Authentication = false;
        private RestResponse? NewPreviousResponse = null;
        private Log? OldPreviousResponse = null;
        private ApiClient client;
        private IEnumerable<RequestResponse> RequestResponsePairs;
        private AuthType AuthType;
        private List<KeyValuePair<string, string>> ResponseBodies;
        private List<KeyValuePair<string, string>> LocationList;

        public Requests(IEnumerable<RequestResponse> RequestResponsePairs, AuthType AuthType, string BasePath) { 
            this.ResponseBodies= new List<KeyValuePair<string, string>>();
            this.LocationList= new List<KeyValuePair<string, string>>();
            this.AuthType = AuthType;
            this.client = new ApiClient(AuthType, BasePath);
            this.RequestResponsePairs = RequestResponsePairs;

        }

        public List<Log> GetRequestsList()
        {
            var list = new List<Log>();
            foreach(var data in RequestResponsePairs)
            {
                list.Add(data.Request);
            }
            return list;
        }
        public void AddLocation(string oldHeader, string newHeader)
        {
            if (oldHeader.Contains("Location") && newHeader.Contains("Location"))
            {
                var indexOld = oldHeader.IndexOf("Location");
                var indexNew = newHeader.IndexOf("Location");
                var oldLocation = oldHeader.Substring(indexOld + "Location:".Length).Split(",")[0];
                var newLocation = newHeader.Substring(indexNew + "Location:".Length).Split(",")[0];
                LocationList.Add(new KeyValuePair<string, string>(oldLocation, newLocation));
            }
        }

        public Log GetRequest(Guid? ProcGuid) {
            if(RequestResponsePairs.Select(d => d.Request.ProcGuid).Contains(ProcGuid) == true)
            {
                return RequestResponsePairs.Where(d => d.Request.ProcGuid == ProcGuid && d.Request.Path != null).First().Request;
            }
            else
            {
                throw new Exception("the request does not exist");
            }
        }

       /* public Log GetResponse(Guid? ProcGuid)
        {
            if (RequestResponsePairs.Select(d => d.Response.ProcGuid).Contains(ProcGuid) == true)
            {
                return RequestResponsePairs.Where(d => d.Response.ProcGuid == ProcGuid && d.Response.Path != null).First().Response;
            }
            else
            {
                throw new Exception("the request does not exist");
            }
        }*/

        private string GetEncodedCredentials(Log log)
        {
            if(log.Headers.IsNullOrEmpty() == false && log.Headers.Contains("Authorization: Basic"))
            {
                var index = log.Headers.IndexOf("Authorization:Basic");
                return log.Headers.Substring(index, "Authorization:Basic".Length + 1).Split(',')[0];
            }
            return String.Empty;
        }

        private Dictionary<string, string> GetOAuthCredentials(Log log)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if(log.Body.IsNullOrEmpty() == false)
            {
                string[] keyValuePairs = log.Body.Split('&');
                foreach (string keyValuePair in keyValuePairs)
                {
                    string[] keyValue = keyValuePair.Split('=');
                    dict.Add(keyValue[0], keyValue[1]);
                }
            }
            return dict;
        }
      
        private void Authenticate(Log log)
        {
            /*if(this.AuthType == AuthType.REST)
            {
                //make a login call, return a response
                client.HttpCall(log);
            }*/
            if (this.AuthType == AuthType.BASIC)
            {
                client.AddBasicAuthentication(GetEncodedCredentials(log));
            }
            if(this.AuthType == AuthType.OAUTH)
            {
                client.AddToken(GetOAuthCredentials(log));
                //return the token and add it to the headers. 
            }
            this.Authentication = true;
            
        }

        public RestResponse GetNewResponse(RequestResponse pair)
        {
            if(pair.Request.Path.IsNullOrEmpty()==false && pair.Request.Path.Contains("login"))//delete later
            {
                login = pair.Request;
            }
            if(Authentication == false)
            {
                Authenticate(pair.Request);
            }
            try
            {
                Log NewRequest = pair.Request;
                if (NewPreviousResponse != null && OldPreviousResponse != null)
                {
                    NewRequest.Path = GetNewPath(NewRequest.Path, OldPreviousResponse.Body, NewPreviousResponse.Content);
                    NewRequest = ChangeRequestBody(OldPreviousResponse.Body, NewPreviousResponse.Content, pair.Request);
                }

                NewPreviousResponse = client.HttpCall(NewRequest);
                OldPreviousResponse = pair.Response;
                AddLocation(OldPreviousResponse.Headers, NewPreviousResponse.Headers.ToString());
                return NewPreviousResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                client.Logout(login);//delete later
            }
            return NewPreviousResponse;



        }


    }
}
