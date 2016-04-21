using System.ComponentModel;
using System.Linq;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для прохождения службы
    /// </summary>
    public class MilitaryProcessViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private MilitaryProcessModel _militaryProcessModel;

        public MilitaryProcessViewModel(MilitaryProcessModel militaryProcessModel)
        {
            _militaryProcessModel = militaryProcessModel;   
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName(@"Начало прохождения службы")]
        public string StartDate => _militaryProcessModel.StartDate.ToString("d");

        /// <summary>
        /// Дата окончания
        /// </summary>
        [DisplayName(@"Окончание службы")]
        public string FinishDate => _militaryProcessModel.FinishDate.ToString("d");

        /// <summary>
        /// Военая часть
        /// </summary>
        [DisplayName(@"Воинская часть")]
        public string MilitaryUnit
            =>
                DataSingleton.Instance.MilitaryUnitList.FirstOrDefault(p => p.Id == _militaryProcessModel.Id)
                    .MilitaryName;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public MilitaryProcessModel GetModel()
        {
            return _militaryProcessModel;
        }
    }
}