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
            "https://medipeel.co.kr/product/list.html?cate_no=502",
            "https://m.avajar.co.kr/product/list_thumb.html?cate_no=117",
        };

        private List<string> _ToDoList = new List<string>()
        {
            "https://www.ohui.co.kr/news/brandnews.jsp",

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

        public MainViewModel () 
        {
            SourceCollection = new ObservableCollection<SourceModel> ();

            #region reqests

            var url = "http://www.sum37.co.kr/online/magazine/magazine.jsp";
            var response = (SynchronizationContext.Current is null ? HTTPRequest.GetRequest(url) : Task.Run(() => HTTPRequest.GetRequest(url))).Result;
            //var News = Sum37Parser(response);
            //var source = new SourceModel(url, News);
            //SourceCollection.Add(source);

            #endregion
        }

        static ObservableCollection<NewsModel> Sum37Parser(string response)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var liList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => (node.Name == "li"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("item_list xans-record-")))
                    .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();

            foreach (var item in liList)
            {
                var link = item.Descendants("a").First();
                var image = link.Descendants("img").First();

                var linkUrl = link.Attributes["href"].Value;
                var imageUrl = image.Attributes["src"].Value;

                var newsModel = new NewsModel()
                {
                    Url = "https://medipeel.co.kr" + linkUrl,
                    ImageUrl = "https:" + imageUrl
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;
        }

        static ObservableCollection<NewsModel> AvajarParser(string response)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var liList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => (node.Name == "li"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("item_list xans-record-")))
                    .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();

            foreach (var item in liList)
            {
                var link = item.Descendants("a").First();
                var image = link.Descendants("img").First();

                var linkUrl = link.Attributes["href"].Value;
                var imageUrl = image.Attributes["src"].Value;

                var newsModel = new NewsModel()
                {
                    Url = "https://medipeel.co.kr" + linkUrl,
                    ImageUrl = "https:" + imageUrl
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;
        }

        static ObservableCollection<NewsModel> MedipeelParser(string response)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(response);

            var liList = htmlDoc.DocumentNode.Descendants()
                .Where(node => (node.Name == "li"
                && node.Attributes["class"] != null 
                && node.Attributes["class"].Value.Contains("item xans-record-")))
                .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();
            
            foreach (var item in liList)
            {
                var link = item.Descendants().
                    Where(node => (node.Name == "a"
                    && node.Attributes["class"] != null 
                    && node.Attributes["class"].Value
                    .Contains("prd_thumb_img")))
                    .First();
                var image = link.Descendants("img").First();

                var linkUrl = link.Attributes["href"].Value;
                string imageUrl = string.Empty;

                try
                {
                    imageUrl = image.Attributes["src"].Value;
                }
                catch (NullReferenceException ex)
                {
                    imageUrl = image.Attributes["ec-data-src"].Value;
                    continue;
                }

                var newsModel = new NewsModel()
                {
                    Url = "https://medipeel.co.kr" + linkUrl,
                    ImageUrl = "https:" + imageUrl
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;

        }



        static ObservableCollection<NewsModel> OhuiParser(string response)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(response);

            var li = htmlDoc.DocumentNode.Descendants("li")
                    .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();

            foreach (var item in li)
            {
                var text = item.ChildNodes[0].ChildNodes[0].InnerText;
                var lincUrl = item.ChildNodes[0].Attributes["href"].Value;
                var imgUrl = item.ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes["src"].Value;

                var newsModel = new NewsModel()
                {
                    Text = text,
                    Url = lincUrl,
                    ImageUrl = imgUrl,
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;

        }

        static ObservableCollection<NewsModel> ParseHtml(string response)
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
