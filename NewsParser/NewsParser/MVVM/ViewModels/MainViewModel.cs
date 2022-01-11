using NewsParser.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.MVVM.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        #region Main window Title

        private string _title = "Corean news";

        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        public MainViewModel () { }
    }
}
