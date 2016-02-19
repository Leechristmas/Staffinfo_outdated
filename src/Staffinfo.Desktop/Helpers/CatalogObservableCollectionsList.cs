using System;
using System.Collections.Generic;
using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.Helpers
{
    /// <summary>
    /// Коллекция справочников(костыль: стоит заменить на биндинг combobox->tabcontrol) 
    /// </summary>
    public class CatalogObservableCollectionsList: INotifyPropertyChanged
    {
        #region Constructors

        public CatalogObservableCollectionsList()
        {

        }

        public CatalogObservableCollectionsList(params List<BaseModel>[] catalogLists )
        {
            foreach (var catalog in catalogLists)
            {
                _catalogsCollection.Add(catalog);
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        /// <summary>
        /// Коллекция справочников
        /// </summary>
        private List<List<BaseModel>> _catalogsCollection;

        /// <summary>
        /// Индекс выделенного справочника
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Выделенный справочник
        /// </summary>
        public List<BaseModel> SelectedItem => _catalogsCollection[_selectedIndex];

        /// <summary>
        /// Индекс выделенного справочника
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        /// <summary>
        /// Коллекция справочников
        /// </summary>
        public List<List<BaseModel>> CatalogsList => _catalogsCollection;
    }
}