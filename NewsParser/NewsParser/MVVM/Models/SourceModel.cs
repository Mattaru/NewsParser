using NewsParser.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.MVVM.Models
{
    internal class SourceModel : ObservableObject
    {


        #region Name

        private string _name;

        public string Name { get => _name; set => Set(ref _name, value); }

        #endregion

        #region News collection

        public ObservableCollection<NewsModel> News { get; set; }

        #endregion

        public SourceModel(string url)
        {
            _name = url;
        }
    }
}
