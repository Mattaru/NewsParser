using NewsParser.Core;
using NewsParser.Service;
using System.Collections.ObjectModel;

namespace NewsParser.MVVM.Models
{
    internal class SourceModel : ObservableObject
    {
        #region Resource Url

        private string _url;

        public string Url { get => _url; set => Set(ref _url, value); }

        #endregion

        #region Name

        private string _name;

        public string Name { get => _name; set => Set(ref _name, value); }

        #endregion

        #region News collection

        public ObservableCollection<NewsModel> News { get; set; }

        #endregion

        public SourceModel(string url, ObservableCollection<NewsModel> News)
        {
            _url = url;
            _name = GetResourceName(url);
            this.News = News;
        }

        public static string GetResourceName(string url)
        {
            var resource = url.Split(":")[1].Split('/')[2];

            return resource;
        }
    }
}
