using HtmlAgilityPack;
using NewsParser.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.Service
{
    internal class HTTPRequest
    {
        private string _response;
        private HtmlDocument _htmlDoc = new HtmlDocument();

        public HTTPRequest(string url)
        {
            _response = GetRequest(url).Result;
            _htmlDoc.LoadHtml(_response);
        }

        private static async Task<string> GetRequest(string url)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(url);

            return await response;
        }

        public static ObservableCollection<NewsModel> ParseHtml(HtmlDocument htmlDoc)
        {
            var programmerLinks = htmlDoc.DocumentNode.Descendants("a")
                    .Where(node => !node.GetAttributeValue("class", "")
                    .Contains("svelte-1pm37ss"))
                    .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();

            foreach (var news in programmerLinks)
            {
                var newsModel = new NewsModel()
                {
                    Text = news.InnerText,
                    Url = news.Attributes["href"].Value,
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;

        }
    }
}
