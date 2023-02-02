﻿using Microsoft.IdentityModel.Tokens;
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
                var sameRequests = acuprox.Logs.Where(data => data.ProcGuid == new Guid("B4979F54-2D22-4CDA-935B-50A5B3C99062") ||
                                                              data.ProcGuid == new Guid("FB3CF3A6-8CAA-42EB-8E8F-CF33049AC220") ||
                                                              data.ProcGuid == new Guid("EA1B17A0-F7CE-4766-8863-3EA6C36E3211") ||
                                                              data.ProcGuid == new Guid("BC9F84EF-5E68-44EB-8A4C-116109B677FE") ||
                                                              data.ProcGuid == new Guid("BA0CF4CE-62B3-4106-B170-623C43FEF9AB") ||
                                                              data.ProcGuid == new Guid("40744521-9743-481D-9BF6-BB460A86A1B4") ||
                                                              data.ProcGuid == new Guid("3956C49E-8D3E-4975-A763-D371D96D558F"))
                                                              .OrderBy(data => data.Dt)
                                                              .ToList();

                var sameRequests2 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("BEEEC9BF-C918-4E7F-91BF-307F6F8FDDCD") ||
                                              data.ProcGuid == new Guid("3B982BF7-4376-4C4E-A9D4-6D6DBED9A3D5") ||
                                              data.ProcGuid == new Guid("6E6A512C-DEE2-4520-A862-2C6A376EDC0E") ||
                                              data.ProcGuid == new Guid("85F049B5-C041-460A-8B23-7D9B5CCABC52") ||
                                              data.ProcGuid == new Guid("50F6265D-F5DA-4BD2-9F6A-873D550AB6D8") ||
                                              data.ProcGuid == new Guid("9F922C6D-4436-40C3-A9D1-88ACA8BD1CED") ||
                                              data.ProcGuid == new Guid("DBFBC65A-2E7F-404A-9777-B6D9F05C6AED") ||
                                              data.ProcGuid == new Guid("8477B58D-8938-4A88-B469-A58AAF3BDE43") ||
                                              data.ProcGuid == new Guid("425E57D1-6536-4833-88F2-D16179C3D23C") ||
                                              data.ProcGuid == new Guid("0FB6B3FD-915A-4958-AF15-3E1EC03D6694") ||
                                              data.ProcGuid == new Guid("5995D90B-F6A0-4668-A515-904490D1BCC7"))
                                              .OrderBy(data => data.Dt)
                                              .ToList();



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

                Requester requester = new Requester(list);

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
                Requester requesterA = new Requester(listA, "5000");
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
                Requester requesterLasVegasSummit = new Requester(LasVegasSummit);
                requesterLasVegasSummit.Execute();
                string a = "{\"id\":\"2e6f0766-43a2-ed11-8715-28dfebb71e4c\",\"rowNumber\":1,\"note\":{\"value\":\"\"},\"Approved\":{\"value\":true},\"BaseCurrencyID\":{\"value\":\"USD\"},\"BillToAddressOverride\":{\"value\":false},\"BillToContactOverride\":{\"value\":false},\"CashAccount\":{\"value\":\"10200\"},\"ContactID\":{\"value\":\"9977\"},\"ControlTotal\":{\"value\":31655.0000},\"CreatedDate\":{\"value\":\"2023-02-01T10:16:55.41-05:00\"},\"CreditHold\":{\"value\":false},\"CurrencyID\":{\"value\":\"USD\"},\"CurrencyRate\":{\"value\":1.00000000},\"CurrencyRateTypeID\":{},\"CustomerID\":{\"value\":\"ABARTENDE\"},\"CustomerOrder\":{},\"Date\":{\"value\":\"2023-02-01T00:00:00-05:00\"},\"Description\":{\"value\":\"Test for Webinar by Key Fields\"},\"DestinationWarehouseID\":{},\"DisableAutomaticDiscountUpdate\":{\"value\":false},\"EffectiveDate\":{\"value\":\"2013-01-01T00:00:00-05:00\"},\"ExternalOrderOrigin\":{},\"ExternalOrderOriginal\":{},\"ExternalOrderSource\":{},\"ExternalRef\":{},\"ExternalRefundRef\":{},\"Hold\":{\"value\":false},\"IsTaxValid\":{},\"LastModified\":{\"value\":\"2023-02-01T10:18:32.51-05:00\"},\"LocationID\":{\"value\":\"MAIN\"},\"NoteID\":{\"value\":\"2e6f0766-43a2-ed11-8715-28dfebb71e4c\"},\"OrderedQty\":{\"value\":6.000000},\"OrderNbr\":{\"value\":\"SO006805\"},\"OrderTotal\":{\"value\":31655.0000},\"OrderType\":{\"value\":\"SO\"},\"PaymentMethod\":{\"value\":\"CHECK\"},\"PaymentRef\":{},\"PreferredWarehouseID\":{},\"Project\":{\"value\":\"X\"},\"ReciprocalRate\":{\"value\":1.00000000},\"RequestedOn\":{\"value\":\"2023-02-01T00:00:00-05:00\"},\"ShipToAddressOverride\":{\"value\":false},\"ShipToContactOverride\":{\"value\":false},\"ShipVia\":{},\"Status\":{\"value\":\"Open\"},\"TaxTotal\":{\"value\":0.0000},\"VATExemptTotal\":{\"value\":0.0000},\"VATTaxableTotal\":{\"value\":0.0000},\"WillCall\":{\"value\":true},\"custom\":{},\"_links\":{\"self\":\"/22R193/entity/default/22.200.001/SalesOrder/2e6f0766-43a2-ed11-8715-28dfebb71e4c\",\"files:put\":\"/22R193/entity/default/22.200.001/files/PX.Objects.SO.SOOrderEntry/Document/2e6f0766-43a2-ed11-8715-28dfebb71e4c/{filename}\"}}";
                string b = "{\"id\":\"2e6f0766-43a2-ed11-8715-28dfebb71e4c\",\"rowNumber\":1,\"note\":{\"value\":\"\"},\"Approved\":{\"value\":true},\"BaseCurrencyID\":{\"value\":\"USD\"},\"BillToAddressOverride\":{\"value\":false},\"BillToContactOverride\":{\"value\":false},\"CashAccount\":{\"value\":\"10200\"},\"ContactID\":{\"value\":\"9977\"},\"ControlTotal\":{\"value\":31655.0000},\"CreatedDate\":{\"value\":\"2023-02-01T10:16:55.41-05:00\"},\"CreditHold\":{\"value\":false},\"CurrencyID\":{\"value\":\"USD\"},\"CurrencyRate\":{\"value\":1.00000000},\"CurrencyRateTypeID\":{},\"CustomerID\":{\"value\":\"ABARTENDE\"},\"CustomerOrder\":{},\"Date\":{\"value\":\"2023-02-01T00:00:00-05:00\"},\"Description\":{\"value\":\"Test for Webinar by Key Fields\"},\"DestinationWarehouseID\":{},\"DisableAutomaticDiscountUpdate\":{\"value\":false},\"EffectiveDate\":{\"value\":\"2013-01-01T00:00:00-05:00\"},\"ExternalOrderOrigin\":{},\"ExternalOrderOriginal\":{},\"ExternalOrderSource\":{},\"ExternalRef\":{},\"ExternalRefundRef\":{},\"Hold\":{\"value\":false},\"IsTaxValid\":{},\"LastModified\":{\"value\":\"2023-02-01T10:18:32.51-05:00\"},\"LocationID\":{\"value\":\"MAIN\"},\"NoteID\":{\"value\":\"2e6f0766-43a2-ed11-8715-28dfebb71e4c\"},\"OrderedQty\":{\"value\":6.000000},\"OrderNbr\":{\"value\":\"XXXXXXXXXXX\"},\"OrderTotal\":{\"value\":31655.0000},\"OrderType\":{\"value\":\"SO\"},\"PaymentMethod\":{\"value\":\"CHECK\"},\"PaymentRef\":{},\"PreferredWarehouseID\":{},\"Project\":{\"value\":\"X\"},\"ReciprocalRate\":{\"value\":1.00000000},\"RequestedOn\":{\"value\":\"2023-02-01T00:00:00-05:00\"},\"ShipToAddressOverride\":{\"value\":false},\"ShipToContactOverride\":{\"value\":false},\"ShipVia\":{},\"Status\":{\"value\":\"Open\"},\"TaxTotal\":{\"value\":0.0000},\"VATExemptTotal\":{\"value\":0.0000},\"VATTaxableTotal\":{\"value\":0.0000},\"WillCall\":{\"value\":true},\"custom\":{},\"_links\":{\"self\":\"/22R193/entity/default/22.200.001/SalesOrder/2e6f0766-43a2-ed11-8715-28dfebb71e4c\",\"files:put\":\"/22R193/entity/default/22.200.001/files/PX.Objects.SO.SOOrderEntry/Document/2e6f0766-43a2-ed11-8715-28dfebb71e4c/{filename}\"}}";
                //requesterLasVegasSummit.GetNewPath("/22R193/entity/default/22.200.001/SalesOrder/SO/SO006804/Files/Test.jpg", a, b);





            }

        }

      



    }
}
