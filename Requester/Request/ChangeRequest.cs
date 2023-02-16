using JsonDiffPatchDotNet;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Requester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;

namespace Requester
{
    public partial class Requests
    {
       
        /*
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
        */

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
                
                throw new Exception("The structure is not Json");
            }
        }

        private bool IsValidJson(string body)
        {
            try
            {
                JToken.Parse(body);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
        }

        private void AddResponse(string OldPreviousResponse, string NewPreviousResponse)
        {
            if(IsValidJson(OldPreviousResponse) && IsValidJson(NewPreviousResponse))
            {
                ResponseBodies.Add(new KeyValuePair<string, string>(OldPreviousResponse, NewPreviousResponse));
            }
        }

        private Log ChangeRequestBody(string OldPreviousResponse, string NewPreviousResponse, Log Request, int index = -1)
        {
            if (Request.Body.IsNullOrEmpty() || IsValidJson(Request.Body) == false)
            {
                //if the response is null then check the previous previous response!           
                AddResponse(OldPreviousResponse, NewPreviousResponse);
                return Request;
            }
            else if (NewPreviousResponse.IsNullOrEmpty() || OldPreviousResponse.IsNullOrEmpty() || IsValidJson(OldPreviousResponse) == false || IsValidJson(NewPreviousResponse) == false)
            {
                if (index == 0 || ResponseBodies.Count() == 0)
                {
                    AddResponse(OldPreviousResponse, NewPreviousResponse);
                    return Request;
                }
                if (index == -1)
                {
                    index = ResponseBodies.Count()-1;
                    return ChangeRequestBody(ResponseBodies.ElementAt(index).Key, ResponseBodies.ElementAt(index).Value, Request, index);
                }
            }
            else if (index == 0)
            {
                return Request;
            }
            else
            {
                var OldPreviousResponseJson = GetJToken(OldPreviousResponse);
                var RequestJson = GetJToken(Request.Body);
                var NewPreviousResponseJson = GetJToken(NewPreviousResponse);
                var difference = new JsonDiffPatch().Diff(NewPreviousResponseJson, OldPreviousResponseJson);

                if (difference == null)
                {
                    AddResponse(OldPreviousResponse, NewPreviousResponse);
                    return Request;
                }
                else
                {
                    var changed = CompareJsonBody(OldPreviousResponseJson, NewPreviousResponseJson, RequestJson);
                    if (changed == false)
                    {
                        return ChangeRequestBody(ResponseBodies.ElementAt(index-1).Key, ResponseBodies.ElementAt(index-1).Value, Request, index-1);
                    }

                }
                Request.Body = RequestJson.ToString();
                AddResponse(OldPreviousResponse, NewPreviousResponse);
                Console.WriteLine(Request.Body);
            }
            return Request;
        }


        private bool CompareJsonBody(JToken OldPreviousResponseJson, JToken NewPreviousResponseJson, JToken newReqJson)
        {
            bool changed = false;
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
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            return changed;
        }

        public string GetNewPath(string path, string oldResponse, string newResponse, int index = -1)
        {
            if (path.Contains("status"))
            {
                var location = path.Substring(path.IndexOf("status") + "status".Length + 1).Split(",")[0];
                foreach(var data in LocationList)
                {
                    if (data.Key.Contains(location))
                    {
                        return path.Replace(location, data.Value.Substring(1));
                    }
                }
               
            }
            if (oldResponse.IsNullOrEmpty() || newResponse.IsNullOrEmpty() || IsValidJson(oldResponse) == false || IsValidJson(newResponse)==false)
            {
                if (index == 0 || ResponseBodies.Count() == 0)
                {
                    return path;
                }
                if (index == -1)
                {
                    index = ResponseBodies.Count()-1;
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
            foreach (var token in tokens) 
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
            

            var prevResJson = JObject.Parse(PreviousResponse.Body);
            var newResJson = JObject.Parse(NewResponse.Body);

            
            var filterList = new Regex("&");
            Cookies = Cookies.Remove(0, 1);//removes '?'
            foreach (var filter in filterList.Split(Cookies))
            {
                var key = new Regex("=").Split(filter).First();
                var OldValue = new Regex("=").Split(filter.Remove(0, 1)).Last();//removes '='
                List<JToken> tokens = JsonExtensions.FindTokens(JObject.Parse(PreviousResponse.Body), key);
                if (key.Contains("$"))
                {
                    //cases: expand is only applicable for keys and keys stay the same from response to response
                    //filter = eq keyword
                    var regexEq = new Regex("eq");
                    foreach (var de in regexEq.Split(OldValue))
                    {
                        if (de.Contains("%20"))
                        {
                            Console.WriteLine(de.Remove(de.IndexOf("%20"), 3));

                        }
                        Console.WriteLine(de);

                    }

                }

                foreach (var token in tokens)
                {
                    if (token.ToString().Contains(OldValue))
                    {
                        if (token.ToString().Contains("value"))
                        {
                            List<JToken> newValues = JsonExtensions.FindTokens(newResJson.SelectToken(token.Path), "value");
                            foreach (var newValue in newValues)
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


    }
}
