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
        public static async Task<string> GetRequest(string url)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            HttpClient Client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            Client.DefaultRequestHeaders.Accept.Clear();
            var response = await Client.GetStringAsync(url);
            
            return response;
        }

        public static void GetResourceCollection(string url, ObservableCollection<SourceModel> Collection)
        {
            var response = (SynchronizationContext.Current is null ? GetRequest(url) : Task.Run(() => GetRequest(url))).Result;
            var News = HTMLParser.SwitchParser(url, response);
            var source = new SourceModel(url, News);
            Collection.Add(source);
        }

        public static SourceModel GetSourceData(string url)
        {
            var response = (SynchronizationContext.Current is null ? GetRequest(url) : Task.Run(() => GetRequest(url))).Result;
            var News = HTMLParser.SwitchParser(url, response);
            var source = new SourceModel(url, News);

            return source;
        }
    }
}
