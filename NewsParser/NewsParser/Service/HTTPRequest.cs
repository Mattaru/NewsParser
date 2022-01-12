using HtmlAgilityPack;
using NewsParser.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewsParser.Service
{
    internal class HTTPRequest
    {
        public static async Task<string> GetRequest(string url)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear(); 
            var response = client.GetStringAsync(url);

            return await response;
        }
    }
}
