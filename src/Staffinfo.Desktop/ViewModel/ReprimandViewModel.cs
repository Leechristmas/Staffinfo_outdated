using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для выговора
    /// </summary>
    public class ReprimandViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private ReprimandModel _reprimandModel;

        public ReprimandViewModel(ReprimandModel reprimandModel)
        {
            _reprimandModel = reprimandModel;
        }

        /// <summary>
        /// Дата выговора
        /// </summary>
        [DisplayName(@"Дата вынесения")]
        public string ReprimandDate => _reprimandModel.ReprimandDate.ToString("d");

        /// <summary>
        /// Сумма выговора
        /// </summary>
        [DisplayName(@"Сумма выговора")]
        public decimal ReprimandSum => _reprimandModel.ReprimandSum;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _reprimandModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public ReprimandModel GetModel()
        {
            return _reprimandModel;
        }
    }
}