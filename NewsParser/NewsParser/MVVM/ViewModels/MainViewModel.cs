using NewsParser.Core;
using NewsParser.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.MVVM.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private List<string> _UrlList = new List<string>()
        {
            "https://mail.ru",
        };
        public ObservableCollection<SourceModel> SourceCollection { get; }

        #region Main window Title

        private string _title = "Corean news";

        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        public MainViewModel () 
        {
            SourceCollection = new ObservableCollection<SourceModel> ();

            foreach (var url in _UrlList) 
            {
                
            }
        }
    }
}
