using HtmlAgilityPack;
using NewsParser.Core;
using NewsParser.Infrastructure.Commands;
using NewsParser.MVVM.Models;
using NewsParser.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsParser.MVVM.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        #region Main window Title

        private string _title = "Corean news";

        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        private List<string> _UrlList = new List<string>()
        {
            "https://www.ohui.co.kr",
            "https://medipeel.co.kr/product/list.html?cate_no=502",
            "https://m.avajar.co.kr/product/list_thumb.html?cate_no=117",
            "http://www.sum37.co.kr/online/magazine/magazine.jsp",
            "https://www.whoo.co.kr",
            "https://www.iope.com/kr/ko/products/new/index.html",
            "https://labonita-nc1.co.kr/29",
        };

        public ObservableCollection<SourceModel> SourceCollection { get; }

        #region SelectedSource

        private SourceModel _SelectedSource;

        public SourceModel SelectedSource { get => _SelectedSource; set => Set(ref _SelectedSource, value); }

        #endregion

        // Commands

        public MainViewModel () 
        {
            SourceCollection = new ObservableCollection<SourceModel> ();

            var response = (SynchronizationContext.Current is null ? HTTPRequest.GetRequest("https://mail.ru") : Task.Run(() => HTTPRequest.GetRequest("https://mail.ru"))).Result;
            var News = ParseHtml(response);

            var source = new SourceModel("https://mail.ru", News);

            SourceCollection.Add(source);
        }

        public ObservableCollection<NewsModel> ParseHtml(string response)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

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
