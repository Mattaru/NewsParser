using NewsParser.Core;
using NewsParser.Infrastructure.Commands;
using NewsParser.MVVM.Models;
using NewsParser.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // for parsing
        public string medipeel = "https://medipeel.co.kr/product/list.html?cate_no=502";
        public string avajar = "https://m.avajar.co.kr/product/list_thumb.html?cate_no=117";
        public string iope = "https://www.iope.com/kr/ko/products/new/index.html";
        public string labonita = "https://labonita-nc1.co.kr/29";
        public string snpmall = "https://snpmall.net/product/list_new.html?cate_no=293";

        // for links
        public string ohui = "https://www.ohui.co.kr/news/brandnews.jsp";
        public string sum37 = "http://www.sum37.co.kr/online/magazine/magazine.jsp";
        public string whoo = "https://www.whoo.co.kr";

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

        public MainViewModel() 
        {
            GetResourceDataCommand = new LambdaCommand(OnGetResourceDataCommandExecuted, CanGetResourceDataCommandExecute);
        }
    }
}
