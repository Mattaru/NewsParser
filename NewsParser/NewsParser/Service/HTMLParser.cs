﻿using HtmlAgilityPack;
using NewsParser.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NewsParser.Service
{
    internal static class HTMLParser
    {
        public static ObservableCollection<NewsModel> SwitchParser(string resourceName, string response)
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

        public static ObservableCollection<NewsModel> LabonitaParser(string response)
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
                    Text = item.Descendants("h2").First().InnerText,
                    Url = "https://labonita-nc1.co.kr" + item.Descendants("a").First().Attributes["href"].Value,
                    ImageUrl = item.Descendants("img").First().Attributes["src"].Value
                };

                NewsCollection.Add(newsModel);
            }

            return NewsCollection;
        }

        public static ObservableCollection<NewsModel> IopeParser(string response)
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

        public static ObservableCollection<NewsModel> AvajarParser(string response)
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

        public static ObservableCollection<NewsModel> MedipeelParser(string response)
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
    }
}