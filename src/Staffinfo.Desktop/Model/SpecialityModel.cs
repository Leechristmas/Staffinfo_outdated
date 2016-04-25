using System;
using System.ComponentModel;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Специальность
    /// </summary>
    public class SpecialityModel: BaseModel, IComparable
    {

        #region Properties

        /// <summary>
        /// Название специальности
        /// </summary>
        public string Speciality { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion

        public int CompareTo(object obj)
        {
            var speciality = obj as SpecialityModel;

            if (speciality == null)
            {
                throw new ArgumentException("Передан объект неподходящего типа");
            }

            return String.CompareOrdinal(Speciality, speciality.Speciality);
        }
    }
}
