using HtmlAgilityPack;
using NewsParser.MVVM.Models;
using NewsParser.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NewsParser.Service
{
    internal static class HTMLParser
    {
        public static IEnumerable<NewsModel> SwitchParser(string resourceName, HtmlDocument htmlDoc)
        {
            switch (resourceName)
            {
                case Sources.OHUI:
                    return OhuiParser(htmlDoc);

                case Sources.SUM37:
                    return Sum37Parser(htmlDoc);

                case Sources.MEDIPEEL:
                    return MedipeelParser(htmlDoc);

                case Sources.IOPE:
                    return IopeParser(htmlDoc);

                case Sources.LABONITA:
                    return LabonitaParser(htmlDoc);
            }

            throw new NotImplementedException("Parsing error. Have not resourcese for parsing.");
        }

        public static IEnumerable<NewsModel> OhuiParser(HtmlDocument htmlDoc)
        {
            var div = htmlDoc.DocumentNode.Descendants()
                    .Where(node => node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("brandNewsList"))
                    .First();

            var liList = div.Descendants("li").ToArray();

            foreach (var item in liList)
            {
                var newsModel = new NewsModel()
                {
                    Text = item.Descendants("img").First().Attributes["alt"].Value,
                    Url = "https://www.ohui.co.kr" + item.Descendants("a").First().Attributes["href"].Value,
                    ImageUrl = "https://www.ohui.co.kr" + item.Descendants("img").First().Attributes["src"].Value
                };

                yield return newsModel;
            }
        }

        public static IEnumerable<NewsModel> Sum37Parser(HtmlDocument htmlDoc)
        {
            var ul = htmlDoc.DocumentNode.Descendants()
                    .Where(node => node.Name == "ul"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("prdList prdList01"))
                    .First();

            var liList = ul.Descendants()
                    .Where(node => node.Name == "li"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("swiper-slide xans-record-"))
                    .ToArray();

            foreach (var item in liList)
            { 
                var spanDiv = item.Descendants()
                    .Where(node => node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("desc-wrap"))
                    .First();

                var newsModel = new NewsModel()
                {
                    Text = spanDiv.Descendants("span").First().InnerText,
                    Url = "https://www.sum37.com" + item.Descendants("a").First().Attributes["href"].Value,
                    ImageUrl = "https:" + item.Descendants("img").First().Attributes["src"].Value
                };

                yield return newsModel;
            }
        }

        public static IEnumerable<NewsModel> MedipeelParser(HtmlDocument htmlDoc)
        {
            var liList = htmlDoc.DocumentNode.Descendants()
                .Where(node => node.Name == "li"
                && node.Attributes["class"] != null
                && node.Attributes["class"].Value.Contains("item xans-record-"))
                .ToList();

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
                    ImageUrl = imageUrl
                };

                yield return newsModel;
            }
        }

        public static IEnumerable<NewsModel> IopeParser(HtmlDocument htmlDoc)
        {
            var linkList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("new-list list"))
                    .First()
                    .Descendants("a")
                    .ToList();

            var imgList = htmlDoc.DocumentNode.Descendants()
                    .Where(node => node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("new-list list"))
                    .First()
                    .Descendants("img")
                    .ToList();

            for (int i = 0; i < imgList.Count; i++)
            {
                var newsModel = new NewsModel()
                {
                    Text = imgList[i].Attributes["alt"].Value,
                    Url = "https://www.iope.com" + linkList[i].Attributes["href"].Value,
                    ImageUrl = "https://www.iope.com" + imgList[i].Attributes["src"].Value
                };

                yield return newsModel;
            }
        }

        public static IEnumerable<NewsModel> LabonitaParser(HtmlDocument htmlDoc)
        {
            var div = htmlDoc.DocumentNode.Descendants()
                    .Where(node => node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("thumb-row  m-list-type2 m-thumb-item-1 list-type-a thumb-item-5  _item_wrap "))
                    .First();

            var divList = div.Descendants()
                    .Where(node => node.Name == "div"
                    && node.Attributes["class"] != null
                    && node.Attributes["class"].Value.Contains("shop-item _shop_item"))
                    .ToArray();

            foreach (var item in divList)
            {
                var newsModel = new NewsModel()
                {
                    Text = item.Descendants("h2").First().InnerText,
                    Url = "https://labonita-nc1.co.kr" + item.Descendants("a").First().Attributes["href"].Value,
                    ImageUrl = item.Descendants("img").First().Attributes["data-src"].Value
                };

                yield return newsModel;
            }
        }
    }
}
