using Microsoft.IdentityModel.Tokens;
using Requester.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requester
{
    public class Program
    {
        static void Main(string[] args)
        {

            using (var acuprox = new AcuProxContext())
            {
                


                /*
                //Console.WriteLine(requester.CompareRequests(sameRequests));
                var req = acuprox.Logs.Where(data => data.ProcGuid == new Guid("9F922C6D-4436-40C3-A9D1-88ACA8BD1CED") && data.EventType == 1).First();
                var res = acuprox.Logs.Where(data => data.ProcGuid == new Guid("9F922C6D-4436-40C3-A9D1-88ACA8BD1CED") && data.EventType == 2).First();

                var ArrayJsonExample = acuprox.Logs.Where(data => data.ProcGuid == new Guid("7BCDEA6B-A181-4A2D-B546-F295282A358E") && data.EventType == 2).First();
                var JsonObjectDetailArray = acuprox.Logs.Where(data => data.ProcGuid == new Guid("0E7AFD65-23EB-4860-8639-7F825BDC97C0") && data.EventType == 2).First();
                //requester.CompareRequestAndResponse(JsonObjectDetailArray, JsonObjectDetailArray, JsonObjectDetailArray);

                var e1 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("425E57D1-6536-4833-88F2-D16179C3D23C") && data.EventType == 2).First();
                var e2 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("0FB6B3FD-915A-4958-AF15-3E1EC03D6694") && data.EventType == 2).First();

                var response = new Log();
                response.Body = "{\r\n\r\n    \"CustomerID\": {\"value\": \"ABARTENDE\"},\r\n\r\n    \"Description\": {\"value\": \"Test for Webinar\"},\r\n\r\n    \"Details\": \r\n\r\n    [\r\n\r\n        {\r\n\r\n            \"InventoryID\": {\"value\": \"AACOMPUT01\"},\r\n\r\n            \"OrderQty\": {\"value\": 3.000000}\r\n\r\n        },\r\n\r\n        {\r\n\r\n            \"InventoryID\": {\"value\": \"AALEGO500\"},\r\n\r\n            \"OrderQty\": {\"value\": 2.000000}\r\n\r\n        },\r\n\r\n        {\r\n\r\n            \"InventoryID\": {\"value\": \"AAMACHINE1\"},\r\n\r\n            \"OrderQty\": {\"value\": 1.000000}\r\n\r\n        }\r\n\r\n    ],\r\n\r\n    \"OrderType\": {\"value\": \"SO\"}\r\n\r\n}";
                var request = new Log();
                request.Body = "{\r\n\t\"CustomerID\": {\r\n\t\t\"value\": \"ABARTENDE\"\r\n\t},\r\n\t\"Description\": {\r\n\t\t\"value\": \"Test for Webinar\"\r\n\t},\r\n\t\"Details\": [\r\n\t\t{\r\n\t\t\t\"InventoryID\": {\r\n\t\t\t\t\"value\": \"AACOMPUT01\"\r\n\t\t\t},\r\n\t\t\t\"OrderQty\": {\r\n\t\t\t\t\"value\": 4\r\n\t\t\t}\r\n\t\t},\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\t],\r\n\t\"OrderType\": {\r\n\t\t\"value\": \"SO\"\r\n\t}\r\n}";
                var newResponse = new Log();
                newResponse.Body = "{\r\n\r\n    \"CustomerID\": {\"value\": \"ABARTENDE2\"},\r\n\r\n    \"Description\": {\"value\": \"Test for Webinar\"},\r\n\r\n    \"Details\": \r\n\r\n    [\r\n\r\n        {\r\n\r\n            \"InventoryID\": {\"value\": \"BBCOMP\"},\r\n\r\n            \"OrderQty\": {\"value\": 3.000000}\r\n\r\n        },\r\n\r\n        {\r\n\r\n            \"InventoryID\": {\"value\": \"AALEGO500\"},\r\n\r\n            \"OrderQty\": {\"value\": 2.000000}\r\n\r\n        },\r\n\r\n        {\r\n\r\n            \"InventoryID\": {\"value\": \"AAMACHINE1\"},\r\n\r\n            \"OrderQty\": {\"value\": 1.000000}\r\n\r\n        }\r\n\r\n    ],\r\n\r\n    \"OrderType\": {\"value\": \"SO\"}\r\n\r\n}";
                //requester.CompareRequestAndResponse(response, request, newResponse);


                var request2 = new Log();
                request2.Body = "{\r\n    \"EndDate\": {\r\n         \"value\": \"2030-01-01T00:00:00+00:00\"\r\n    },\r\n    \"StartDate\": {\r\n        \"value\": \"2010-01-01T00:00:00+00:00\"\r\n    }\r\n}";
                var response2 = new Log();
                response2.Body = "{\"id\":\"63ddbefc-5d0a-49c1-b677-9c06cd5165e6\",\"rowNumber\":1,\"note\":null,\"EndDate\":{\"value\":\"2030-01-01T00:00:00+00:00\"},\"Page\":{},\"StartDate\":{\"value\":\"2010-01-01T00:00:00+00:00\"},\"Xml\":{},\"custom\":{},\"files\":[]}";
                var NewResponse2 = new Log();
                NewResponse2.Body = "{\"id\":\"65ddbefc-5d0a-49c1-b677-9c06cd5165e6\",\"rowNumber\":1,\"note\":null,\"EndDate\":{\"value\":\"2030-01-01T00:00:00+00:00\"},\"Page\":{},\"StartDate\":{\"value\":\"2010-01-01T00:00:00+00:00\"},\"Xml\":{},\"custom\":{},\"files\":[]}";
                */


                // requester.CompareQueryStringAndResponse(response2, NewResponse2, null);
                var login = acuprox.Logs.Where(data => data.ProcGuid == new Guid("20FAB4AC-C8ED-4A83-BDED-EB4AFD686F59") && data.EventType == 1).First();
                var loginResponse = acuprox.Logs.Where(data => data.ProcGuid == new Guid("20FAB4AC-C8ED-4A83-BDED-EB4AFD686F59") && data.EventType == 2).First();
                
                //GetStockItem
                var request1 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("9660B71F-BAE2-42C6-8A38-1B6326B2618B") && data.EventType == 1).First();
                var response1 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("9660B71F-BAE2-42C6-8A38-1B6326B2618B") && data.EventType == 2).First();

                //PutInventoryQuantity
                var request2 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("5FEF35A5-B5B4-43C4-BFF8-BB8E2A359B90") && data.EventType == 1).First();
                var response2 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("5FEF35A5-B5B4-43C4-BFF8-BB8E2A359B90") && data.EventType == 2).First();

                List<Log> list = new List<Log>() { login, request1, response1, request2, response2 };

                Requester requester = new Requester(list, "localhost");

                //requester.ReExecuteRequests(list);


                ///<example>
                /// Create a SO and then create a shipment for that sales order
                /// TODO: TRY WITH THE ERROR CODE
                /// </example>
                var logina = acuprox.Logs.Where(data => data.ProcGuid == new Guid("6E6ECDEB-EF57-4640-89D8-AC8BB84F3B90") && data.EventType == 1).First();
                //createSO
                var request1a = acuprox.Logs.Where(data => data.ProcGuid == new Guid("12ECE639-8AEB-469D-9EF5-4230FDEC5478") && data.EventType == 1).First();
                var response1a = acuprox.Logs.Where(data => data.ProcGuid == new Guid("12ECE639-8AEB-469D-9EF5-4230FDEC5478") && data.EventType == 2).First();
                //createShipment
                var request2a = acuprox.Logs.Where(data => data.ProcGuid == new Guid("BC5CB547-1075-4267-9169-1AD39D62C05B") && data.EventType == 1).First();
                var response2a = acuprox.Logs.Where(data => data.ProcGuid == new Guid("BC5CB547-1075-4267-9169-1AD39D62C05B") && data.EventType == 2).First();
                List<Log> listA = new List<Log>() { logina, request1a, response1a, request2a, response2a };             
                Requester requesterA = new Requester(listA, "localhost", "5000");
                //requesterA.Execute();

                //requester.GetNewPath();

                var LasVegasSummit = acuprox.Logs.Where(data => data.ProcGuid == new Guid("10FA7469056441F7AC5BDF90F62EEB67") ||
                                              data.ProcGuid == new Guid("E193D248273B48C2989D977D1F24AA31") ||
                                              data.ProcGuid == new Guid("41AFBC40D568431888F06E6F3A461A8E") ||
                                              data.ProcGuid == new Guid("446DDB5C-35E4-40FD-A84A-870A31D8D96A") ||
                                              data.ProcGuid == new Guid("623D343C-B5D0-4D03-B80B-563E3EC872A4") ||
                                              data.ProcGuid == new Guid("3116A223-5286-4254-B0DA-D9AA22D5D486") ||
                                              data.ProcGuid == new Guid("0E201FDD-F1ED-47FF-B053-2508E862077A") ||
                                              data.ProcGuid == new Guid("84431A2F-DD38-4EAF-B908-659782D7CED9") ||
                                              data.ProcGuid == new Guid("A36ED6A8-66DE-4CBF-9EF1-06C3BB200E14") ||
                                              data.ProcGuid == new Guid("43FB4379-C2FA-40AC-AEA1-A41093C12258"))
                                              .OrderBy(data => data.Dt)
                                              .ToList();
                Requester requesterLasVegasSummit1 = new Requester(LasVegasSummit, "localhost", "5000");
                //requesterLasVegasSummit1.Execute();


                var LasVegasSummit2 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("7520AF0516F142579D0FB12DEF0FC9B5") ||
                                              data.ProcGuid == new Guid("A0D36859-F697-423A-A27E-FFBE52B6E730") ||
                                              data.ProcGuid == new Guid("B025B4AA-250E-48C4-9188-BD8B037EB15E") ||
                                              data.ProcGuid == new Guid("78DF19CC-889D-4BA5-9468-C49278995794") ||
                                              data.ProcGuid == new Guid("0F371E1B-5594-4A99-80B7-65356F69BCB2") ||
                                              data.ProcGuid == new Guid("F127F2AF-F359-459C-8BD2-B6BCA6B3F71B") ||
                                              data.ProcGuid == new Guid("4135FD30-3675-4D90-BB64-3B297F12E7C0") ||
                                              data.ProcGuid == new Guid("F5946390-FD14-4EDC-B650-9AB125C28778") ||
                                              data.ProcGuid == new Guid("2FAD7A56-0D36-412C-88FB-D1982D0E66E2") |
                                              data.ProcGuid == new Guid("B311007F-2ECF-4755-AF31-82C704F6A884"))
                                              .ToList();

                var LasVegasSummit3 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("CD81E4D8-C06A-4236-9AB0-64C8ABC13FF2") ||
                                             data.ProcGuid == new Guid("96E9B37A-F91A-4C46-9288-84B8F9483E91") ||
                                             data.ProcGuid == new Guid("9FAF4D97-6D0C-445B-847C-83EFD224EAED"))
                                             .OrderBy(data => data.Dt)
                                             .ToList();
                Requester requesterLasVegasSummit2 = new Requester(LasVegasSummit2, "localhost");
                foreach(var element in LasVegasSummit2)
                {
                    Console.WriteLine(element.Method + " " + element.Path);
                }
                //requesterLasVegasSummit2.Execute();
                requesterLasVegasSummit2.GetElibilityToken("http://localhost/22R193", "passsord", "3210DB15-CD4E-AF21-A060-68F2FE1F22E3@Company", "qlnmLLnLjkG8ZwHyQBgB_Q", "admin", "123");
                //NOTE TO DO: order requests by time not by sesison id
                //add odata
                //finish oauth
                //ask what to do with error responses 

            }

        }

      



    }
}
