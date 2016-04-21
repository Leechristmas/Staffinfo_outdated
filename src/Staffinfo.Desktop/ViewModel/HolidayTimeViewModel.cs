using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model отпуска
    /// </summary>
    public class HolidayTimeViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private HolidayTimeModel _holidayTimeModel;

        public HolidayTimeViewModel(HolidayTimeModel holidayTimeModel)
        {
            _holidayTimeModel = holidayTimeModel;
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName(@"Начало отпуска")]
        public string StartDate => _holidayTimeModel.StartDate.ToString("d");
        
        /// <summary>
        /// Дата окончания
        /// </summary>
        [DisplayName(@"Окончание отпуска")]
        public string FinishDate => _holidayTimeModel.FinishDate.ToString("d");

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _holidayTimeModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public HolidayTimeModel GetModel()
        {
            return _holidayTimeModel;
        }
    }
}