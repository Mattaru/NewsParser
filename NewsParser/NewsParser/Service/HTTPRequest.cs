using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsParser.Service
{
    internal class HTTPRequest
    {
        public static async Task<string> GetRequest(string url)
        {
            HttpClient Client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            Client.DefaultRequestHeaders.Accept.Clear();
            var response = await Client.GetStringAsync(url);

            return response;
        }
    }
}
