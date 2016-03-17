using System;

namespace Staffinfo.Desktop.Model
{

    /// <summary>
    /// Родственник
    /// </summary>
    public class RelativeModel: BaseModel
    {
        #region Properties
        
        /// <summary>
        /// Код служащего
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Тип родства
        /// </summary>
        public long RelationTypeId { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Дата родждения
        /// </summary>
        public DateTime BornDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        #endregion

    }
}
