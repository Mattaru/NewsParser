using NewsParser.MVVM.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace NewsParser.Data
{
    internal static class TestingData
    {
        public static SourceModel GetData() 
        {
            var news = Enumerable.Range(0, 10).Select(i => new NewsModel()
            {
                Text = "아무나 가질 수 없는 특별함이 깃든 오휘 다이아데인 크림",
                Url = "https://www.ohui.co.kr",
                ImageUrl = "https://www.ohui.co.kr/upload/_ctrl_ohui_/news/brandnews/210429_5%EC%9B%94_%EB%B8%8C%EB%9E%9C%EB%93%9C%EB%89%B4%EC%8A%A4_thumnail.jpg"
            });

            return new SourceModel("https://www.ohui.co.kr", new ObservableCollection<NewsModel>(news));
        }
    }
}
