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

            //_Client.DefaultRequestHeaders.Add("Host", "ohui.co.kr");
            //Client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            //Client.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "max-age=0");
            //Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36 OPR/67.0.3575.137");
            //Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "euc-kr,ko;q=0.8,en-US;q=0.6,en;q=0.4");
            Client.DefaultRequestHeaders.Accept.Clear();
            
            var response = await Client.GetStringAsync(url);

            return response;
        }


        public static async Task<string> GetHtmlData(string url)
        {
            HttpClient httpClient = new HttpClient();
            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
            {
                request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml, charset=UTF-8, text/javascript, */*; q=0.01");
                request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36 OPR/67.0.3575.137");
                request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");

                using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    using (var streamReader = new StreamReader(decompressedStream))
                    {
                        var result = await streamReader.ReadToEndAsync().ConfigureAwait(false);

                        return result;
                    }
                }
            }
        }
    }
}
