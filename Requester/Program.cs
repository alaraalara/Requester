using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Requester.Models;
using RestSharp;
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
                Requester requesterLasVegasSummit2 = new Requester(LasVegasSummit2, "localhost");


                var LasVegasSummit3 = acuprox.Logs.Where(data => data.ProcGuid == new Guid("CD81E4D8-C06A-4236-9AB0-64C8ABC13FF2") ||
                                             data.ProcGuid == new Guid("96E9B37A-F91A-4C46-9288-84B8F9483E91") ||
                                             data.ProcGuid == new Guid("9FAF4D97-6D0C-445B-847C-83EFD224EAED"))
                                             .OrderBy(data => data.Dt)
                                             .ToList();
                Requester requesterLasVegasSummit3 = new Requester(LasVegasSummit3, "localhost");
                
                requesterLasVegasSummit2.Execute();

                /*
                var OdataAndAouthMix = acuprox.Logs.Where(data => data.ProcGuid == new Guid("A419AF15-F5AC-43E3-B45D-C3A262B0419D") ||
                                              data.ProcGuid == new Guid("3E10D1FB-1386-495F-B450-D81033AEDA0A") ||
                                              data.ProcGuid == new Guid("59B3C4BA-3BD5-4A29-9817-3387C516B48E") ||
                                              data.ProcGuid == new Guid("907C831E-C5B4-4795-A9BC-F24DCB4A2460") ||
                                              data.ProcGuid == new Guid("F02B30A3-9B81-4189-A07E-62DCA46A157F"))
                                              .ToList();
                */
                

            }

        }

       




    }
}
