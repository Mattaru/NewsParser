using NewsParser.Core;
using NewsParser.Infrastructure.Commands;
using NewsParser.MVVM.Models;
using NewsParser.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewsParser.MVVM.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        #region Main window Title

        private string _title = "Corean news";

        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        // for parsing
        public string sum37 = "http://www.sum37.co.kr/online/magazine/magazine.jsp";
        public string ohui = "https://www.ohui.co.kr/news/brandnews.jsp";
        public string medipeel = "https://medipeel.co.kr/product/list.html?cate_no=502";
        public string avajar = "https://m.avajar.co.kr/product/list_thumb.html?cate_no=117";
        public string iope = "https://www.iope.com/kr/ko/products/new/index.html";
        public string labonita = "https://labonita-nc1.co.kr/29";
        public string snpmall = "https://snpmall.net/product/list_new.html?cate_no=293";

        // for links
        public string whoo = "https://www.whoo.co.kr";

        public ObservableCollection<SourceModel> SourceCollection { get; set; }

        #region SelectedSource

        private SourceModel _SelectedSource;

        public SourceModel SelectedSource { get => _SelectedSource; set => Set(ref _SelectedSource, value); }

        #endregion

        #region LoadingSpinner visibility

        private string _spinnerVisibility = "Collapsed";

        public string SpinnerVisibility { get => _spinnerVisibility; set => Set(ref _spinnerVisibility, value); }

        #endregion

        #region NewsList visibility

        private string _newsListVisibility = "Collapsed";

        public string NewsListVisibility { get => _newsListVisibility; set => Set(ref _newsListVisibility, value); }

        #endregion

        #region NewsLoaded

        private bool _newsLoaded = false;

        public bool NewsLoaded { get => _newsLoaded; set => Set(ref _newsLoaded, value); }

        #endregion

        // Commands

        #region CloseAppCommand

        public ICommand CloseAppCommand { get; }

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p) => Application.Current.Shutdown();

        #endregion

        #region GetSourceDataCommand

        public ICommand GetSourceDataCommand { get; } 

        private bool CanGetResourceDataCommandExecute(object p) => true;

        private async void OnGetResourceDataCommandExecuted(object p)
        {
            await Task.Run(() =>
            {
                if (NewsListVisibility != "Collapsed") NewsListVisibility = "Collapsed";
                if (SpinnerVisibility != "Visible") SpinnerVisibility = "Visible";

                SelectedSource = HTTPRequest.GetSourceData((string)p);
                OnPropertyChanged(nameof(SelectedSource));

                NewsListVisibility = "Visible";
                SpinnerVisibility = "Collapsed";
            });
        }

        #endregion

        public MainViewModel() 
        {
            CloseAppCommand = new LambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);
            GetSourceDataCommand = new LambdaCommand(OnGetResourceDataCommandExecuted, CanGetResourceDataCommandExecute);


            // For tests

            /*var news = Enumerable.Range(0, 10).Select(i => new NewsModel()
            {
                Text = "아무나 가질 수 없는 특별함이 깃든 오휘 다이아데인 크림",
                Url = "https://mail.ru",
                ImageUrl = "https://www.ohui.co.kr/upload/_ctrl_ohui_/news/brandnews/210429_5%EC%9B%94_%EB%B8%8C%EB%9E%9C%EB%93%9C%EB%89%B4%EC%8A%A4_thumnail.jpg"
            });

            ObservableCollection<NewsModel> NewsColl = new ObservableCollection<NewsModel>(news);

            var source = new SourceModel("https://mail.ru", NewsColl);

            SelectedSource = source;*/
        }
    }
}
