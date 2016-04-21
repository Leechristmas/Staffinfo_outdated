using System.ComponentModel;
using System.Linq;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model обучения
    /// </summary>
    public class EducationalTimeViewModel
    {
        /// <summary>
        /// Модель
        /// </summary>
        private EducationTimeModel _educationTimeModel;

        public EducationalTimeViewModel(EducationTimeModel educationTimeModel)
        {
            _educationTimeModel = educationTimeModel;
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName(@"Дата начала обучения")]
        public string StartDate => _educationTimeModel.StartDate.ToString("d");

        /// <summary>
        /// Дата окончания
        /// </summary>
        [DisplayName(@"Дата завершения")]
        public string FinishDate => _educationTimeModel.FinishDate.ToString("d");

        /// <summary>
        /// Название учебного заведения
        /// </summary>
        [DisplayName(@"Название учебного заведения")]
        public string InstitutionName
            =>
                DataSingleton.Instance.EducationalInstitutionList.FirstOrDefault(p => p.Id == _educationTimeModel.Id)
                    .InstituitionTitle;

        /// <summary>
        /// Тип учебного заведения
        /// </summary>
        [DisplayName(@"Тип учебного заведения")]
        public string InstitutionType
            =>
                DataSingleton.Instance.EducationalInstitutionList.FirstOrDefault(p => p.Id == _educationTimeModel.Id)
                    .InstituitionType;

        /// <summary>
        /// Специальность
        /// </summary>
        [DisplayName(@"Специальность")]
        public string SpecialityName
            =>
                DataSingleton.Instance.SpecialityList.FirstOrDefault(p => p.Id == _educationTimeModel.SpecialityId)
                    .Speciality;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _educationTimeModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public EducationTimeModel GetModel()
        {
            return _educationTimeModel;
        }
    }
}