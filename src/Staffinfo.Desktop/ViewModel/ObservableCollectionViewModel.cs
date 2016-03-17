using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Обновляемая коллекция для моделей
    /// </summary>
    /// <typeparam name="T">Тип модели</typeparam>
    public class ObservableCollectionViewModel<T>: ViewModelBase where T : new()
    {
        public ObservableCollectionViewModel(ObservableCollection<T> modelCollection)
        {
            ModelCollection = modelCollection;
        }

        public ObservableCollection<T> ModelCollection { get; private set; }

        #region SelectedItem

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        private const string SelectedItemPropertyName = "SelectedItem";

        private T _selectedItem = new T();
        private int _selectedIndex;

        public T SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;
                RaisePropertyChanged(SelectedItemPropertyName);
            }
        }
        #endregion 
    }
}