using NewsParser.Core;
using NewsParser.Data;
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

        public SourceModel SelectedSource 
        { 
            get => _SelectedSource;
            set 
            { 
                Set(ref _SelectedSource, value);
                OnPropertyChanged(nameof(ListIndex));
            } 
        }

        #endregion

        #region ListIndex

        private int _listIndex = 0;

        public int ListIndex { get => _listIndex; set => Set(ref _listIndex, value); }

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
            /*NewsListVisibility = "Visible";
            SelectedSource = TestingData.GetData();*/
        }
    }
}
