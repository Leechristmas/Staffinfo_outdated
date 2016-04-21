using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для контракта
    /// </summary>
    public class ContractViewModel
    {
        private ContractModel _contract;

        public ContractViewModel(ContractModel contract)
        {
            _contract = contract;
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName(@"Дата подписания")]
        public string StartDate => _contract.StartDate.ToString("d");

        /// <summary>
        /// Дата завершения
        /// </summary>
        [DisplayName(@"Дата завершения")]
        public string FinishDate => _contract.FinishDate.ToString("d");

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _contract.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public ContractModel GetModel()
        {
            return _contract;
        }
    }
}