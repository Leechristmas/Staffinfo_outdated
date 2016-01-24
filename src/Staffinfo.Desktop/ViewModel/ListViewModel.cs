using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Staffinfo.Desktop.ViewModel
{
    public class ListViewModel<T>: ViewModelBase where T : new()
    {
        public ListViewModel(List<T> modelList)
        {
            ModelList = modelList;
        }

        public List<T> ModelList { get; private set; }

        #region SelectedItem

        private const string SelectedItemPropertyName = "SelectedItem";

        private T _selectedItem = new T();

        public T SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = SelectedItem;
                RaisePropertyChanged(SelectedItemPropertyName);
            }
        }
        #endregion

    }
}
