using NewsParser.Core;
using NewsParser.Infrastructure.Commands;
using NewsParser.MVVM.Models;
using NewsParser.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<SourceModel> SourceCollection { get; set; }

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
            Task.Run(() => 
            {
                SelectedSource = HTTPRequest.GetResourceData((string)p);
                OnPropertyChanged(nameof(SelectedSource));
            });

        }

        #endregion

        public MainViewModel () 
        {
            GetResourceDataCommand = new LambdaCommand(OnGetResourceDataCommandExecuted, CanGetResourceDataCommandExecute);
        }

        private void MakeTestCollection()
        {
            var news = Enumerable.Range(0, 20).Select(i => new NewsModel()
            {
                Text = $"Just text for trying {i}",
                Url = "https://mail.ru",
                ImageUrl = "/Images/fatbear.jpg"
            });

            var NewsCollection = new ObservableCollection<NewsModel>(news);

            var sources = Enumerable.Range(1, 10).Select(i => new SourceModel($"https://mail.ru{i}", NewsCollection));

            SourceCollection = new ObservableCollection<SourceModel>(sources);
        }
    }
}
