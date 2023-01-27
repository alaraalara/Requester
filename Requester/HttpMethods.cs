using Requester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Requester
{
    public class HttpMethods
    {
        HttpClient client;
        string portNumber;

        public HttpMethods(string portNumber)
        {
            this.client = new HttpClient();
            this.portNumber = portNumber;
        }

        

        public async Task<HttpResponseMessage> Logout(Log log)
        {
            string url = "http://localhost" + portNumber + "/22R193/entity/auth/logout";
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
            var url = "http://localhost" + portNumber + log.Path;
            var message = new HttpRequestMessage(new HttpMethod(log.Method), url);

            if (log.Body != null)
            {
                message.Content = new StringContent(log.Body, Encoding.UTF8, "application/json");
            }
            var response = await client.SendAsync(message);
            return response;
        }

    }
}
