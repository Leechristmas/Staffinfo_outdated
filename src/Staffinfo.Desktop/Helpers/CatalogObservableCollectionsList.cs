﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Staffinfo.Desktop.Data.DataTableProviders;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogLists">Кортеж со списками справочников и соответствующими для них "заголовками"</param>
        public CatalogObservableCollectionsList(params Tuple<List<BaseModel>, string>[] catalogLists )
        {
            foreach (var catalog in catalogLists)
            {
                _catalogsCollection.Add(catalog.Item1);
                _catalogsNames.Add(catalog.Item2);
            }
        }

        #endregion

        

        /// <summary>
        /// Коллекция справочников
        /// </summary>
        private ObservableCollection<List<BaseModel>> _catalogsCollection = new ObservableCollection<List<BaseModel>>();

        /// <summary>
        /// Список соответствующих "названий" справочников
        /// (нужны для отображения в контроле выбора - combobox)
        /// </summary>
        private List<string> _catalogsNames = new List<string>(); 

        /// <summary>
        /// Индекс выделенного справочника
        /// </summary>
        private int _selectedIndex = -1;

        /// <summary>
        /// Коллекция справочников
        /// </summary>
        public ObservableCollection<List<BaseModel>> CatalogsCollection => _catalogsCollection;

        /// <summary>
        /// Список соответствующих "названий" справочников
        /// (нужны для отображения в контроле выбора - combobox)
        /// </summary>
        public List<string> CatalogsNames => _catalogsNames;

        /// <summary>
        /// Выделенный справочник
        /// </summary>
        public object SelectedItem//гребанный костыль
        {
            get
            {
                if (SelectedIndex < 0) return null;
                else if (CatalogsCollection[SelectedIndex].Count <= 0) return 0;
                else
                {
                    return null;
                    switch (SelectedIndex)
                    {
                        
                    }

                    //using (var prvdr = new ClasinessTableProvider())
                    //{
                    //    return prvdr.Select();
                    //}
                }
                    //return
                    //    actualSourceList = _catalogsCollection[SelectedIndex]
                    //    .Select(el =>
                    //    {
                    //        switch (el.GetType().Name)
                    //        {
                    //            case "ClasinessModel":
                    //                return el as ClasinessModel;
                    //            default:
                    //                return el;
                    //        }
                    //    })
                    //    .ToList();
            }
        }

        /// <summary>
        /// Добавить справочник в коллекцию
        /// </summary>
        /// <param name="catalog">справочник</param>
        /// <returns></returns>
        public ObservableCollection<List<BaseModel>> Add(Tuple<List<BaseModel>, string> catalog)
        {
            //var t = catalog.Item1.ElementAt(0).GetType().Name;

            CatalogsCollection.Add(catalog.Item1);
            CatalogsNames.Add(catalog.Item2);

            return CatalogsCollection;
        }

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
                RaisePropertyChanged("SelectedItem");
            }
        }
        
        

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
    }
}