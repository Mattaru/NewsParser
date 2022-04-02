using NewsParser.Core;
using NewsParser.Infrastructure.Commands;
using NewsParser.MVVM.Models;
using NewsParser.Service;
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

        #region For parsing

        public string sum37 = "http://www.sum37.co.kr/online/magazine/magazine.jsp";
        public string ohui = "https://www.ohui.co.kr/news/brandnews.jsp";
        public string medipeel = "https://medipeel.co.kr/product/list.html?cate_no=502";
        public string avajar = "https://m.avajar.co.kr/product/list_thumb.html?cate_no=117";
        public string iope = "https://www.iope.com/kr/ko/products/new/index.html";
        public string labonita = "https://labonita-nc1.co.kr/29";

        #endregion

        #region For links

        public string whoo = "https://www.whoo.co.kr";
        public string snpmall = "https://snpmall.net/product/list_new.html?cate_no=293";

        #endregion

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

        private void OnGetResourceDataCommandExecuted(object p)
        {
            GetSource((string)p);
        }

        #endregion

        public MainViewModel() 
        {
            CloseAppCommand = new LambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);
            GetSourceDataCommand = new LambdaCommand(OnGetResourceDataCommandExecuted, CanGetResourceDataCommandExecute);

            // For tests
            /*NewsListVisibility = "Visible";
            SelectedSource = TestingData.GetData();*/
        }

        private async void GetSource(string url)
        {
            await Task.Run(() =>
            {
                if (NewsListVisibility != "Collapsed") NewsListVisibility = "Collapsed";
                if (SpinnerVisibility != "Visible") SpinnerVisibility = "Visible";

                SelectedSource = HTTPRequest.GetSourceData(url);
                OnPropertyChanged(nameof(SelectedSource));

                NewsListVisibility = "Visible";
                SpinnerVisibility = "Collapsed";
            });
        }
    }
}
