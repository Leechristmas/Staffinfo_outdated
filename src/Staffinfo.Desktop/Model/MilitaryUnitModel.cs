using System;
using System.ComponentModel;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Воинская часть
    /// </summary>
    public class MilitaryUnitModel: BaseModel, IComparable
    {

        #region Properties
        
        /// <summary>
        /// Название(номер) части
        /// </summary>
        public string MilitaryName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion

        public int CompareTo(object obj)
        {
            var militaryUnit = obj as MilitaryUnitModel;

            if(militaryUnit == null)
                throw new ArgumentException("Передан объект неподходящего типа");

            return String.CompareOrdinal(MilitaryName, militaryUnit.MilitaryName);
        }
    }
}
