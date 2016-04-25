using System.ComponentModel;
using System.Runtime.CompilerServices;
using Staffinfo.Desktop.Annotations;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Учебное заведение view model
    /// </summary>
    public class EducationalIntitutionViewModel: INotifyPropertyChanged
    {
        private EducationalInstitutionModel _educationalInstitutionModel;

        public EducationalIntitutionViewModel(EducationalInstitutionModel educationalInstitutionModel)
        {
            _educationalInstitutionModel = educationalInstitutionModel;
        }

        /// <summary>
        /// Название учебного заведения
        /// </summary>
        [DisplayName(@"Название")]
        public string InstitutionTitle => _educationalInstitutionModel.InstituitionTitle;

        /// <summary>
        /// Тип учебного заведения
        /// </summary>
        [DisplayName(@"Тип")]
        public string InstitutionType => _educationalInstitutionModel.InstituitionType;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _educationalInstitutionModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public EducationalInstitutionModel GetModel()
        {
            return _educationalInstitutionModel;
        }
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}