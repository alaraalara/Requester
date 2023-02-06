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
    public partial class Requester
    {
        private HttpClient client;
        private string portNumber;
        private string host; 
        private List<Log> ListOfRequests;
        private List<KeyValuePair<string, string>> ResponseBodies;
        private List<KeyValuePair<string, string>> LocationList;

        public Requester(List<Log> ListOfRequests, string Host, string portNumber="") {
            this.client = new HttpClient();
            this.portNumber = ":" + portNumber;
            this.host = Host;
            this.ListOfRequests = ListOfRequests;
            ResponseBodies = new List<KeyValuePair<string, string>>();
            LocationList = new List<KeyValuePair<string, string>>();
        }       

        public void Execute()
        {
            
            if (ListOfRequests.Any(log => log.Path != null && log.Path.ToLower().Contains("login")))
            {
                var login = ListOfRequests.Where(log => log.Path.Contains("login")).First();
                try
                {
                    ReExecuteRequests(client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Logout(login);

                }

            }
            
        }

     
        private void ReExecuteRequests(HttpClient client)
        {
            Log? login = null;
            var query =
                from request in ListOfRequests
                join response in ListOfRequests on request.ProcGuid equals response.ProcGuid
                where request.EventType == 1 && response.EventType == 2
                select new { Request = request, Response = response } ;
            
            if (ListOfRequests.Any(log => log.Path!=null && log.Path.ToLower().Contains("login")))
            {
                login = ListOfRequests.Where(log => log.Path.Contains("login") && log.EventType==1).First();
            }
            if(login != null)
            {
                var loginResponse = HttpCall(login).Result;
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

                            NewPreviousResponse = HttpCall(NewRequest).Result;
                            OldPreviousResponseLog = data.Response;
                            AddLocation(OldPreviousResponseLog.Headers.ToString(), NewPreviousResponse.Headers.ToString());
                            Console.WriteLine(NewRequest.Path);
                            Console.WriteLine(NewPreviousResponse.Content.ReadAsStringAsync().Result);
                            Console.WriteLine(NewPreviousResponse.StatusCode);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                            Logout(login);
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
