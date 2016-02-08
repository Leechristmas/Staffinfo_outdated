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
        
        //private const string SelectedIndexPropertyName = "SelectedIndex";
        //private int? _selectedIndex = -1;
        ///// <summary>
        ///// Нужен был для selected item
        ///// </summary>
        //public int? SelectedIndex
        //{
        //    get { return _selectedIndex; }

        //    set
        //    {
        //        _selectedIndex = value;
        //        RaisePropertyChanged(SelectedIndexPropertyName);
        //    }
        //}

        private const string SelectedItemPropertyName = "SelectedItem";

        private T _selectedItem = new T();

        public T SelectedItem
        {
            get { return _selectedItem; }
            //get { return ModelList[_selectedIndex.Value]; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(SelectedItemPropertyName);
            }
        }
        #endregion
    }
}
