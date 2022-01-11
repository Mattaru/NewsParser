using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.Service
{
    internal class HTTPRequest
    {
        public static void GetRequest(string url)
        {
            var response = CallUrl(url).Result;
            var linkList = ParseHtml(response);
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }

        private static List<string> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var programmerLinks = htmlDoc.DocumentNode.Descendants("a")
                    .Where(node => !node.GetAttributeValue("class", "").Contains("svelte-1pm37ss")).ToList();

            List<string> wikiLink = new List<string>();

            foreach (var link in programmerLinks)
            {
                var res = link.Attributes["href"].Value;

                Console.WriteLine(res);
            }

            return wikiLink;

        }
    }
}
