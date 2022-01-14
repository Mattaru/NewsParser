using NewsParser.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NewsParser.Service
{
    internal static class HTMLRequest
    {
        public static async Task<string> GetRequest(string url)
        {
            HttpClient Client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            Client.DefaultRequestHeaders.Accept.Clear();
            var response = await Client.GetStringAsync(url);

            return response;
        }

        public static void GetCollectionFromResource(string url, ObservableCollection<SourceModel> Collection)
        {
            var response = (SynchronizationContext.Current is null ? GetRequest(url) : Task.Run(() => GetRequest(url))).Result;
            var News = HTMLParser.SwitchParser(url, response);
            var source = new SourceModel(url, News);
            Collection.Add(source);
        }
    }
}
