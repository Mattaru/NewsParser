using HtmlAgilityPack;
using NewsParser.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewsParser.Service
{
    internal static class HTTPRequest
    {
        private static async Task<string> GetRequest(string url)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            HttpClient Client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            Client.DefaultRequestHeaders.Accept.Clear();
            var response = await Client.GetStringAsync(url);
            
            return response;
        }

        private static HtmlDocument GetHTMLDoc(string httpResponse)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResponse);

            return htmlDoc;
        }

        public static SourceModel GetSourceData(string url)
        {
            var response = (SynchronizationContext.Current is null ? GetRequest(url) : Task.Run(() => GetRequest(url))).Result;
            var htmlDoc = GetHTMLDoc(response);
            var News = HTMLParser.SwitchParser(url, htmlDoc);
            var sourceData = new SourceModel(url, News);

            return sourceData;
        }
    }
}
