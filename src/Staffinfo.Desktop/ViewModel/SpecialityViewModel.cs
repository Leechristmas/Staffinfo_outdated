using System.ComponentModel;
using System.Runtime.CompilerServices;
using Staffinfo.Desktop.Annotations;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class SpecialityViewModel: INotifyPropertyChanged
    {
        private SpecialityModel _specialityModel;

        public SpecialityViewModel(SpecialityModel specialityModel)
        {
            _specialityModel = specialityModel;
        }

        /// <summary>
        /// Название военной части
        /// </summary>
        [DisplayName(@"Специальность")]
        public string MilitaryUnitName => _specialityModel.Speciality;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _specialityModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public SpecialityModel GetModel()
        {
            return _specialityModel;
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