using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для нарушения
    /// </summary>
    public class ViolationViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private ViolationModel _violationModel;

        public ViolationViewModel(ViolationModel reprimandModel)
        {
            _violationModel = reprimandModel;
        }

        /// <summary>
        /// Дата нарушения
        /// </summary>
        [DisplayName(@"Дата нарушения")]
        public string ReprimandDate => _violationModel.ViolationDate.ToString("d");
        
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _violationModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public ViolationModel GetModel()
        {
            return _violationModel;
        }
    }
}