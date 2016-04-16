using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Класс-коллекция для списков моделей
    /// </summary>
    /// <typeparam name="T">тип модели</typeparam>
    public class ListViewModel<T>: ViewModelBase where T : new()
    {
        public ListViewModel(List<T> modelList)
        {
            ModelList = modelList;
        }

        public List<T> ModelList { get; private set; }

        #region SelectedItem
        
        //private const string SelectedItemPropertyName = "SelectedItem";

        private T _selectedItem = default(T);

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        #endregion
    }
}
