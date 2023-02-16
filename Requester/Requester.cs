using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Requester.Api;
using Requester.Models;
using Requester;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Requester
{
    public partial class Requester
    {
        private string PortNumber;
        private string Host; 
        private IEnumerable<RequestResponse> RequestResponsePairs;
        private Dictionary<string, List<Requests>> GroupBySession; 


        public Requester(IEnumerable<Log> ListOfRequests, string Host, string PortNumber = "") {
            this.PortNumber = ":" + PortNumber;
            this.Host = Host;
            ListOfRequests = ListOfRequests.OrderBy(data => data.Dt);
            RequestResponsePairs = from request in ListOfRequests
                                   join response in ListOfRequests on request.ProcGuid equals response.ProcGuid
                                   where request.EventType == 1 && response.EventType == 2
                                   select new RequestResponse { Request = request, Response = response }; ;
            GroupBySession = new Dictionary<string, List<Requests>>();
        }

        /// <summary>
        /// Groups requests according to their sessionID.
        /// </summary>
        private void OrganizeRequests()
        {
            PutODataBatch();
            PutRestBatch();
            PutOauthBatch();
        }

        private Dictionary<string, List<RequestResponse>> GetGroupBySessionList(IEnumerable<IGrouping<bool, RequestResponse>> sameSessionGroup)
        {
            Dictionary<string, List<RequestResponse>> GroupBySessionList = new Dictionary<string, List<RequestResponse>>();
            foreach (var group in sameSessionGroup)
            {
                foreach (var log in group)
                {

                    var sessionID = GetSessionId(log);
                    if (GroupBySessionList.ContainsKey(sessionID))
                    {
                        GroupBySessionList[sessionID].Add(new RequestResponse { Request = log.Request, Response = log.Response });
                        //GroupBySessionList[sessionID] = GroupBySessionList[sessionID];
                    }
                    else
                    {
                        GroupBySessionList.Add(sessionID, new List<RequestResponse> { new RequestResponse { Request = log.Request, Response = log.Response } });
                    }

                }
            }
            return GroupBySessionList;
        }

        private void PutOauthBatch(IEnumerable<RequestResponse> list = null)
        {
            if (list == null)
            {
                list = RequestResponsePairs;
            }
            var sameSessionGroup = RequestResponsePairs.Where(data => (data.Request.Headers != null && data.Request.Headers.Contains("Authorization:Bearer"))
            || data.Request.Path.Contains("identity/connect/token")).OrderBy(data => data.Request.Dt).GroupBy(data => GetSessionId(data) != string.Empty); ;

            foreach(var kvp in GetGroupBySessionList(sameSessionGroup))
            {
                if(GroupBySession.ContainsKey(kvp.Key))
                {
                    var requestGroup = GroupBySession[kvp.Key];
                    requestGroup.Add(new Requests(kvp.Value, AuthType.OAUTH, "http://" + Host + PortNumber));
                    GroupBySession[kvp.Key] = requestGroup;
                }
                else
                {
                    GroupBySession.Add(kvp.Key, new List<Requests> { new Requests(kvp.Value, AuthType.OAUTH, "http://" + Host + PortNumber)});
                }
            }

        }

        private void PutODataBatch()
        {
            Dictionary<string, List<RequestResponse>> GroupBySessionList = new Dictionary<string, List<RequestResponse>>();
            var pairs = new List<RequestResponse>();
            var odata = RequestResponsePairs.Where(data => data.Request.Path.ToLower().Contains("odata"));
            var sameGroupBasic = odata.GroupBy(data => GetBasicAuthCredentials(data.Request) != string.Empty);
            foreach(var group in sameGroupBasic)
            {
                foreach(var log in group)
                {
                    var sessionID = GetBasicAuthCredentials(log.Request);
                    if (GroupBySessionList.ContainsKey(sessionID))
                    {
                        GroupBySessionList[sessionID].Add(new RequestResponse { Request = log.Request, Response = log.Response });
                       //GroupBySessionList[sessionID] = GroupBySessionList[sessionID];
                    }
                    else
                    {
                        GroupBySessionList.Add(sessionID, new List<RequestResponse> { new RequestResponse { Request = log.Request, Response = log.Response } });
                    }
                }
            }
            foreach (var kvp in GroupBySessionList)
            {
                if (GroupBySession.ContainsKey(kvp.Key))
                {
                    var requestGroup = GroupBySession[kvp.Key];
                    requestGroup.Add(new Requests(kvp.Value, AuthType.BASIC, "http://" + Host + PortNumber));
                    GroupBySession[kvp.Key] = requestGroup;

                }
                GroupBySession.Add(kvp.Key, new List<Requests> { new Requests(kvp.Value, AuthType.BASIC, "http://" + Host + PortNumber) });
            }

            PutOauthBatch(odata);
        }
        
        private void PutRestBatch()
        {
            Dictionary<string, List<RequestResponse>> GroupBySessionList = new Dictionary<string, List<RequestResponse>>();
            var sameSessionGroup = RequestResponsePairs.Where(data => data.Request.Headers != null && data.Request.Headers.Contains("Bearer") ==false
            && data.Request.Headers.Contains("Basic")==false && data.Request.Path.Contains("identity") ==false).GroupBy(data => GetSessionId(data) != string.Empty);
            
            foreach(var kvp in GetGroupBySessionList(sameSessionGroup))
            {
                if(GroupBySession.ContainsKey(kvp.Key))
                {
                    var requestGroup = GroupBySession[kvp.Key];
                    requestGroup.Add(new Requests(kvp.Value, AuthType.REST, "http://" + Host + PortNumber));
                    GroupBySession[kvp.Key] = requestGroup;

                }
                GroupBySession.Add(kvp.Key, new List<Requests> { new Requests(kvp.Value, AuthType.REST, "http://" + Host + PortNumber) });

            }
        }

        private string GetSessionId(RequestResponse Pair)
        {
            if ((Pair.Request != null || Pair.Response != null) && (Pair.Request.Cookies.IsNullOrEmpty() == false || Pair.Response.Cookies.IsNullOrEmpty() == false))
            {
                if (Pair.Request.Cookies.Contains("ASP.NET_SessionId:"))
                {
                    return Pair.Request.Cookies.Substring(Pair.Request.Cookies.IndexOf("ASP.NET_SessionId:")).Split(',')[0];
                }
                else if(Pair.Response.Cookies.Contains("ASP.NET_SessionId:")){
                    return Pair.Response.Cookies.Substring(Pair.Response.Cookies.IndexOf("ASP.NET_SessionId:")).Split(',')[0];
                }
            }
            return String.Empty;
        }
        
        private string GetBasicAuthCredentials(Log log)
        {
            if (log.Headers.IsNullOrEmpty() == false && log.Headers.Contains("Basic"))
            {
                var index = log.Headers.IndexOf("Basic");
                return log.Headers.Substring(index).Split(',')[0];
            }
            return String.Empty;
        }
        private Dictionary<string, string> GetOAuthCredentials(Log log)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (log.Body.IsNullOrEmpty() == false)
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

        public void Execute()
        {
            OrganizeRequests();
            foreach(var data in RequestResponsePairs)
            {
                try
                {
                    string sessionInfo = GetSessionId(data);
                    if (sessionInfo == string.Empty)
                    {
                        sessionInfo = GetBasicAuthCredentials(data.Request);
                    }               
                    var newResponse = GroupBySession[sessionInfo].Where(variable=>variable.GetRequestsList().Contains(data.Request)).First().GetNewResponse(data);
                    Console.WriteLine(newResponse.StatusCode);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }

    }
}
