using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для родственника
    /// </summary>
    public class RelativeViewModel
    {
        /// <summary>
        /// модель
        /// </summary>
        private RelativeModel _relativeModel;

        public RelativeViewModel(RelativeModel relativeModel)
        {
            _relativeModel = relativeModel;
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DisplayName(@"Фамилия")]
        public string Lastname => _relativeModel.LastName;

        /// <summary>
        /// Имя
        /// </summary>
        [DisplayName(@"Имя")]
        public string Firtname => _relativeModel.FirstName;

        /// <summary>
        /// Отчество
        /// </summary>
        [DisplayName(@"Отчество")]
        public string Middlename => _relativeModel.MiddleName;

        /// <summary>
        /// Тип родства
        /// </summary>
        [DisplayName(@"Родство")]
        public string RelationType => _relativeModel.RelationType;

        /// <summary>
        /// Дата рождения
        /// </summary>
        [DisplayName(@"Дата рождения")]
        public string BornDate => _relativeModel.BornDate.ToString("d");

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public RelativeModel GetModel()
        {
            return _relativeModel;
        }
    }
}