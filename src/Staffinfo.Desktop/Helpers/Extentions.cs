using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Staffinfo.Desktop.Helpers
{
    /// <summary>
    /// Расширения
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Возвращает отсортированный observable collection
        /// </summary>
        /// <typeparam name="T">объект коллекции</typeparam>
        /// <param name="collection">объект сортировки</param>
        public static ObservableCollection<T> GetSorted<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                collection.Move(collection.IndexOf(sorted[i]), i);

            return collection;
        }

        /// <summary>
        /// Сортировка observable collection
        /// </summary>
        /// <typeparam name="T">объект коллекции</typeparam>
        /// <param name="collection">объект сортировки</param>
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                collection.Move(collection.IndexOf(sorted[i]), i);
        }
    }
}