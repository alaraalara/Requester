using Requester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json.Nodes;
using Acumatica.RESTClient.Client;
using Acumatica.Auth.Api;
using Newtonsoft.Json.Linq;
using JsonDiffPatchDotNet;
using Newtonsoft;
using Json.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net.Http.Headers;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Requester
{
    public class Requester
    {
        public List<Log> ListOfRequests;
        string portNumber;
        public Dictionary<string, string> ResponseBodies;

        public Requester(List<Log> ListOfRequests, string portNumber=null) { 
            this.ListOfRequests = ListOfRequests;   
            if(portNumber == null)
            {
                this.portNumber = string.Empty;
            }
            else
            {
                this.portNumber = ":" + portNumber;
            }
            ResponseBodies = new Dictionary<string, string>();  
        }

        //NOTE Might be edge cases where bunch of collection of both rest and oatuh type of different session id requests might be executed!
        private bool IsSameBatchRest(List<Log> ListOfRequests)
        {
            //can be 2 login requests too. It should check for every login requests. This only assume 1 login request and its session id.
            System.Guid? LoginProcGUID = ListOfRequests.Where(r => r.Path!=null && r.Path.ToLower().Contains("login")).First().ProcGuid;
            var cookies = ListOfRequests.Where(r => r.ProcGuid == LoginProcGUID && r.EventType == 2).First().Headers;
            string sessionID = null;
            if (cookies != null && cookies.Contains("ASP.NET_SessionId:"))
            {
                sessionID = cookies.Substring(cookies.IndexOf("ASP.NET_SessionId:")).Split(',')[0];
                Console.WriteLine(sessionID);
            }
            if (ListOfRequests.Where(r => r.Headers != null && r.Headers.IndexOf("ASP.NET_SessionId:") != -1).All(r => r.Headers.Substring(r.Headers.IndexOf("ASP.NET_SessionId:")).Split(',')[0] == sessionID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //asumes that login and request for token has to be included in the batch!
        //TODO: TEST IT!
        private bool IsSameBatchOAuth(List<Log> ListOfRequests)
        {
            System.Guid? tokenRequest = ListOfRequests.Where(r => r.Path != null && r.Path.ToLower().Contains("token")).First().ProcGuid;
            var tokenBody = ListOfRequests.Where(r => r.ProcGuid == tokenRequest && r.EventType == 2).First().Body;
            JToken? token = null;
            if (tokenBody != null)
            {
                token = JObject.Parse(tokenBody)["access_token"];
            }
            if (token != null &&
                ListOfRequests.Where(r => r.Headers != null && r.Headers.IndexOf("Authorization:Bearer ") != -1).All(r => r.Headers.Substring(r.Headers.IndexOf("Authorization:Bearer ")).Split(',')[0] == token.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private JToken? GetJToken(string body)
        {
            dynamic? jsonObj = JsonConvert.DeserializeObject(body);
            if (jsonObj is JObject)
            {
                return JObject.Parse(body);
            }
            else if (jsonObj is JArray)
            {
                return JArray.Parse(body);
            }
            else
            {
                //instead of throwing exception it should just return the old request as there is nothing to compare
                //What about errors?
                throw new Exception("The structure is not Json");
            }
        }

        //TODO: has to check for error responses! WHAT TO DO FOR THEM
        //NOTE TODO: Url might change too! depending on the previous body
        private Log ChangeRequestBody(string OldPreviousResponse, string NewPreviousResponse, Log Request, int index=-1)
        {
            if (Request.Body.IsNullOrEmpty())
            {
                //if the response is null then check the previous previous response!
                return Request;
            }
            else if(NewPreviousResponse.IsNullOrEmpty() || OldPreviousResponse.IsNullOrEmpty()) {
                if(index == 0)
                {
                    return Request;
                }
                if(index == -1)
                {
                    index = ResponseBodies.Count()-1;//last element
                }
                else
                {
                    index = index - 1;
                }
                ChangeRequestBody(ResponseBodies.ElementAt(index).Key, ResponseBodies.ElementAt(index).Value, Request, index);                
            }
            try
            {
                var OldPreviousResponseJson = GetJToken(OldPreviousResponse);
                var RequestJson = GetJToken(Request.Body);
                var NewPreviousResponseJson = GetJToken(NewPreviousResponse);
                var difference = new JsonDiffPatch().Diff(NewPreviousResponseJson, OldPreviousResponseJson);

                if (difference == null)
                {
                    return Request;
                }
                else
                {
                    CompareJsonBody(OldPreviousResponseJson, NewPreviousResponseJson, RequestJson);

                }
                ResponseBodies.Add(OldPreviousResponse, NewPreviousResponse);
                Request.Body = RequestJson.ToString();
                Console.WriteLine(Request.Body);
                return Request;
            }
            catch(Exception e) {
                
                return Request;
            }
            
            
        }

        
        private void CompareJsonBody(JToken OldPreviousResponseJson, JToken NewPreviousResponseJson, JToken newReqJson)
        {
            var tokensResponse = JsonExtensions.FindTokens(OldPreviousResponseJson, "id").Concat(JsonExtensions.FindTokens(OldPreviousResponseJson, "value"));
            foreach (var token in tokensResponse)
            {
                var tokenNew = NewPreviousResponseJson.SelectToken(token.Path);
                if (JToken.DeepEquals(NewPreviousResponseJson.SelectToken(token.Path), token) == false)
                {
                    if (newReqJson.SelectToken(token.Path) != null)
                    {
                        newReqJson.SetByPath(token.Path, NewPreviousResponseJson.SelectToken(token.Path));
                    }
                    else
                    {
                        //the path of the request might be different than that of response (additional fields)
                        //TODO: can there be more than 1 additional field?
                        var tokensRequest = JsonExtensions.FindTokens(newReqJson, "value").Concat(JsonExtensions.FindTokens(newReqJson, "id"));
                        foreach (var token2 in tokensRequest)
                        {
                            var subPath = token2.Path.Replace(token2.Path.Split(".")[0] + ".", "");
                            if (subPath == token.Path)
                            {
                                var valueToSet = NewPreviousResponseJson.SelectToken(token.Path);
                                if (valueToSet != null)
                                {
                                    newReqJson.SetByPath(token2.Path, valueToSet);
                                }
                            }
                        }
                    }
                }

            }
        }
        
        public string GetNewPath(string path, string oldResponse, string newResponse, int index=-1)
        {
            
            if(oldResponse.IsNullOrEmpty() || newResponse.IsNullOrEmpty())
            {
                if(ResponseBodies.Count() == 0)
                {
                    return path;
                }
                if (index == -1)
                {
                    index = ResponseBodies.Count() - 1;//last element
                }
                else
                {
                    index = index - 1;
                }
                return GetNewPath(path, ResponseBodies.ElementAt(index).Key, ResponseBodies.ElementAt(index).Value, index);
            }
            var difference = new JsonDiffPatch().Diff(oldResponse, newResponse);
            if (difference.IsNullOrEmpty())
            {
                return path;
            }  
            //0th position is the old value, and 1st position is the new value
            var tokens = JsonExtensions.FindTokens(JToken.Parse(difference), "value").Concat(JsonExtensions.FindTokens(JToken.Parse(difference), "id"))
                .Concat(JsonExtensions.FindTokens(JToken.Parse(difference), "files:put")).Concat(JsonExtensions.FindTokens(JToken.Parse(difference), "self"));
            var pathList = path.Split('/');
            foreach(var token in tokens) // will the end value be always in token??????????
            {
                
                if (token != null && token.ToArray().Length > 1 && pathList.Contains(token[0].ToString()))
                {

                    path = path.Replace(token[0].ToString(), token[1].ToString());
                }
            }
            return path;

        }

        private string CompareQueryStringAndResponse(Log PreviousResponse, Log NewResponse, string Cookies)
        {
            string a = "?fileID=aa015587-d4e0-47f6-84f5-42fc5f9ff870";
            string b = "?$expand=Address&$top=1000&$skip=0&$custom=&$filter=Type%20eq%20'Contact'";
            string c2 = "?$filter=Balance%20eq%200.0M%20and%20Hold%20ne%20true%20and%20LastModifiedDateTime%20gt%20datetimeoffset'2022-04-06T06%3A29%3A50.218%2B00%3A00'&$top=1000&$skip=0&$custom=&$expand=Details";
            string c = "?CustomerID=ABARTENDE";// no case where ? starts
            string d = "?rowNumber=19";

            var prevResJson = JObject.Parse(PreviousResponse.Body);
            var newResJson = JObject.Parse(NewResponse.Body);

            Cookies = b;
            //note equals edge case in filter (Like string c2)
            //edge case: antyhing that starts with dollar sign (filter, skip, expand and etc...)
            //c = c.Remove(0, 1);
            var filterList = new Regex("&");
            Cookies = Cookies.Remove(0, 1);//removes '?'
            foreach(var filter in filterList.Split(Cookies))
            {
                var key = new Regex("=").Split(filter).First();
                var OldValue = new Regex("=").Split(filter.Remove(0, 1)).Last();//removes '='
                List<JToken> tokens = JsonExtensions.FindTokens(JObject.Parse(PreviousResponse.Body), key);
                if (key.Contains("$"))
                {
                    //cases: expand is only applicable for keys and keys stay the same from response to response
                    //filter = eq keyword
                    var regexEq = new Regex("eq");
                    foreach(var de in regexEq.Split(OldValue))
                    {
                        if (de.Contains("%20"))
                        {
                            Console.WriteLine(de.Remove(de.IndexOf("%20"), 3));

                        }
                        Console.WriteLine(de);

                    }
                   
                }

                foreach (var token in tokens) {
                    if (token.ToString().Contains(OldValue)){
                        if (token.ToString().Contains("value"))
                        {                       
                            List<JToken> newValues = JsonExtensions.FindTokens(newResJson.SelectToken(token.Path), "value");
                            foreach(var newValue in newValues)
                            {
                                Cookies = Cookies.Replace(key + "=" + OldValue, key + "=" + newValue);
                            }

                        }
                        else //id 
                        {
                            Cookies = Cookies.Replace(key + "=" + OldValue, key + "=" + newResJson.SelectToken(token.Path));
                        }                     
                    }
                }
             
            }
            Console.WriteLine(Cookies);
            return Cookies;
        }


        public void Execute()
        {
            
            var client = new HttpMethods(this.portNumber);
            if (ListOfRequests.Any(log => log.Path != null && log.Path.ToLower().Contains("login")))
            {
                //this design looks weird!
                var login = ListOfRequests.Where(log => log.Path.Contains("login")).First();
                try
                {
                    ReExecuteRequests(client);
                }
                catch (Exception e)
                {
                    client.Logout(login);

                }

            }
            
        }

        /// <TODO>
        ///might be couple logins
        ///if no login, order them and do the usual login.(but in this case I would need usernae and password)
        ///
        /// </TODO>
        public void ReExecuteRequests(HttpMethods client)
        {
            Log? login = null;
            var query =
                from request in ListOfRequests
                join response in ListOfRequests on request.ProcGuid equals response.ProcGuid
                where request.EventType == 1 && response.EventType == 2
                select new { Request = request, Response = response } ;
            
            //make helper private method to handle login request and execute oauth and rest from there 
            if (ListOfRequests.Any(log => log.Path!=null && log.Path.ToLower().Contains("login")))
            {
                login = ListOfRequests.Where(log => log.Path.Contains("login") && log.EventType==1).First();
            }
            if(login != null)
            {
                var loginResponse = client.HttpCall(login).Result;
                if (loginResponse != null && (int)loginResponse.StatusCode == 204)
                {
                    HttpResponseMessage? NewPreviousResponse =null;
                    Log? OldPreviousResponseLog = null;

                    foreach (var data in query.OrderBy(log => log.Request.Dt))
                    {
                        try
                        {
                            Log NewRequest = data.Request;

                            if (NewPreviousResponse != null && OldPreviousResponseLog!=null)
                            {
                                //data.response has to be the prev response before data.request
                                NewRequest.Path = GetNewPath(NewRequest.Path, OldPreviousResponseLog.Body, NewPreviousResponse.Content.ReadAsStringAsync().Result);
                            }
                            if(NewPreviousResponse!=null)
                            {
                                NewRequest = ChangeRequestBody(OldPreviousResponseLog.Body, NewPreviousResponse.Content.ReadAsStringAsync().Result, data.Request);
                            }

                            NewPreviousResponse = client.HttpCall(NewRequest).Result;
                            OldPreviousResponseLog = data.Response;
                            Console.WriteLine(NewPreviousResponse.StatusCode);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                            client.Logout(login);
                        }
                       

                    }

                }
                else
                {
                    throw new Exception("can't login");
                }
            }
            else
            {
                throw new Exception("credential info is missing. Can't login");
            }



        }

       

      


    }
}
