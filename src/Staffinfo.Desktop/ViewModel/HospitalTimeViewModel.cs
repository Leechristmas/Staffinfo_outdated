using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для больничного
    /// </summary>
    public class HospitalTimeViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private HospitalTimeModel _hospitalTimeModel;

        public HospitalTimeViewModel(HospitalTimeModel hospitalTimeModel)
        {
            _hospitalTimeModel = hospitalTimeModel;
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName(@"Открытие больничного")]
        public string StartDate => _hospitalTimeModel.StartDate.ToString("d");

        /// <summary>
        /// Дата окончания
        /// </summary>
        [DisplayName(@"Закрытие больничного")]
        public string FinishDate => _hospitalTimeModel.FinishDate.ToString("d");

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _hospitalTimeModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public HospitalTimeModel GetModel()
        {
            return _hospitalTimeModel;
        }
    }
}