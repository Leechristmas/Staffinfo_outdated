using System.ComponentModel;
using System.Linq;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model для присвоения должности
    /// </summary>
    public class PostAssignmentViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private PostAssignmentModel _postAssignmentModel;

        public PostAssignmentViewModel(PostAssignmentModel postAssignmentModel)
        {
            _postAssignmentModel = postAssignmentModel;
        }

        /// <summary>
        /// Дата присвоения
        /// </summary>
        [DisplayName(@"Дата присвоения")]
        public string AssignmentDate => _postAssignmentModel.AssignmentDate.ToString("d");

        /// <summary>
        /// Старая должность
        /// </summary>
        [DisplayName(@"Предыдущая должность")]
        public string OldPost
            => DataSingleton.Instance.PostList.FirstOrDefault(p => p.Id == _postAssignmentModel.PreviousPostId).PostTitle;

        /// <summary>
        /// Новая должность
        /// </summary>
        [DisplayName(@"Новая должность")]
        public string NewPost
            => DataSingleton.Instance.PostList.FirstOrDefault(p => p.Id == _postAssignmentModel.NewPostId).PostTitle;

        /// <summary>
        /// Номер приказа
        /// </summary>
        [DisplayName(@"Номер приказа")]
        public int OrderNumber => _postAssignmentModel.OrderNumber;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _postAssignmentModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public PostAssignmentModel GetModel()
        {
            return _postAssignmentModel;
        }
    }
}