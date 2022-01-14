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
            "https://www.iope.com/kr/ko/products/new/index.html",
            "https://labonita-nc1.co.kr/29",
        };

        private List<string> _ToDoList = new List<string>()
        {
            "https://www.ohui.co.kr/news/brandnews.jsp",
            "http://www.sum37.co.kr/online/magazine/magazine.jsp",
            "https://www.whoo.co.kr"
        };

        public ObservableCollection<SourceModel> SourceCollection { get; }

        #region SelectedSource

        private SourceModel _SelectedSource;

        public SourceModel SelectedSource { get => _SelectedSource; set => Set(ref _SelectedSource, value); }

        #endregion

        // Commands

        #region GetResourceDataCommand

        public ICommand GetResourceDataCommand { get; }

        private bool CanGetResourceDataCommandExecute(object p) => true;

        private void OnGetResourceDataCommandExecuted(object p)
        {
            var url = _SelectedSource.Url;
            var response = (SynchronizationContext.Current is null ? HTTPRequest.GetRequest(url) : Task.Run(() => HTTPRequest.GetRequest(url))).Result;
            var News = SwitchParser(url, response);
            var source = new SourceModel(url, News);
            SourceCollection.Add(source);
        }

        #endregion

        public MainViewModel () 
        {
            SourceCollection = new ObservableCollection<SourceModel> ();

            #region reqests

            /*foreach (var url in _UrlList)
            {
                var response = (SynchronizationContext.Current is null ? HTTPRequest.GetRequest(url) : Task.Run(() => HTTPRequest.GetRequest(url))).Result;
                var News = SwitchParser(url, response);
                var source = new SourceModel(url, News);
                SourceCollection.Add(source);
            }*/

            #endregion

            var news = Enumerable.Range(0, 20).Select(i => new NewsModel()
            {
                Text = $"Just text for trying {i}",
                Url = "https://mail.ru",
                ImageUrl = ""
            });

            var NewsCollection = new ObservableCollection<NewsModel>(news);

            SourceCollection.Add(new SourceModel("https://mail.ru", NewsCollection));

        }

        private static ObservableCollection<NewsModel> LabonitaParser(string response) // With text
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var divList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => (node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("shop-item _shop_item")))
                    .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();

            foreach (var item in divList) 
            {
                var newsModel = new NewsModel()
                {
                    Text = item.Descendants("img").First().Attributes["alt"].Value,
                    Url = "https://labonita-nc1.co.kr" + item.Descendants("a").First().Attributes["href"].Value,
                    ImageUrl = item.Descendants("img").First().Attributes["src"].Value
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;
        }

        private static ObservableCollection<NewsModel> IopeParser(string response)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var linkList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => (node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("new-list list")))
                    .First()
                    .Descendants("a")
                    .ToList();

            var imgList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => (node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("new-list list")))
                    .First()
                    .Descendants("img")
                    .ToList();

            ObservableCollection<NewsModel> NewsCollection = new ObservableCollection<NewsModel>();

            for (int i = 0; i < imgList.Count; i++)
            {
                var newsModel = new NewsModel()
                {
                    Text = imgList[i].Attributes["alt"].Value,
                    Url = "https://www.iope.com" + linkList[i].Attributes["href"].Value,
                    ImageUrl = "https://www.iope.com" + imgList[i].Attributes["src"].Value
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;
        }

        private static ObservableCollection<NewsModel> AvajarParser(string response)
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

                var newsModel = new NewsModel()
                {
                    Text = image.Attributes["alt"].Value,
                    Url = "https://medipeel.co.kr" + link.Attributes["href"].Value,
                    ImageUrl = "https:" + image.Attributes["src"].Value
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;
        }

        private static ObservableCollection<NewsModel> MedipeelParser(string response)
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

                string imageUrl = string.Empty;

                if (image.Attributes.Contains("ec-data-src"))
                    imageUrl = image.Attributes["ec-data-src"].Value;
                else
                    imageUrl = image.Attributes["src"].Value;

                var newsModel = new NewsModel()
                {
                    Text = image.Attributes["alt"].Value,
                    Url = "https://medipeel.co.kr" + link.Attributes["href"].Value,
                    ImageUrl = "https:" + imageUrl
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;

        } 


        private static ObservableCollection<NewsModel> SwitchParser(string resourceName, string response)
        {
            switch (resourceName)
            {
                case "https://medipeel.co.kr/product/list.html?cate_no=502":
                    return MedipeelParser(response);

                case "https://m.avajar.co.kr/product/list_thumb.html?cate_no=117":
                    return AvajarParser(response);

                case "https://www.iope.com/kr/ko/products/new/index.html":
                    return IopeParser(response);

                case "https://labonita-nc1.co.kr/29":
                    return LabonitaParser(response);
            }

            throw new NotImplementedException("Parsing error. Have not resourcese for parsing.");
        }
    }
}
