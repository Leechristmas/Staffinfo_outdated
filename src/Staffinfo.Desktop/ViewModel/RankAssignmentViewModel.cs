using System.ComponentModel;
using System.Linq;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class RankAssignmentViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private RankAssignmentModel _rankAssignmentModel;

        public RankAssignmentViewModel(RankAssignmentModel rankAssignmentModel)
        {
            _rankAssignmentModel = rankAssignmentModel;
        }

        /// <summary>
        /// Дата присвоения
        /// </summary>
        [DisplayName(@"Дата присвоения")]
        public string AssignmentDate => _rankAssignmentModel.AssignmentDate.ToString("d");

        /// <summary>
        /// Старое звание
        /// </summary>
        [DisplayName(@"Предыдущее звание")]
        public string OldRank
            => DataSingleton.Instance.RankList.FirstOrDefault(p => p.Id == _rankAssignmentModel.PreviousRankId).RankTitle;

        /// <summary>
        /// Новое звание
        /// </summary>
        [DisplayName(@"Новое звание")]
        public string NewRank
            => DataSingleton.Instance.RankList.FirstOrDefault(p => p.Id == _rankAssignmentModel.NewRankId).RankTitle;

        /// <summary>
        /// Номер приказа
        /// </summary>
        [DisplayName(@"Номер приказа")]
        public int OrderNumber => _rankAssignmentModel.OrderNumber;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _rankAssignmentModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public RankAssignmentModel GetModel()
        {
            return _rankAssignmentModel;
        }
    }
}