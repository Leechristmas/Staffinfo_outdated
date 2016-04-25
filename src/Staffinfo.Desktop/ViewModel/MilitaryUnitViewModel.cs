using System.ComponentModel;
using System.Runtime.CompilerServices;
using Staffinfo.Desktop.Annotations;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Военная часть view model
    /// </summary>
    public class MilitaryUnitViewModel : INotifyPropertyChanged
    {
        private MilitaryUnitModel _militaryUnitModel;

        public MilitaryUnitViewModel(MilitaryUnitModel militaryUnitModel)
        {
            _militaryUnitModel = militaryUnitModel;
        }

        /// <summary>
        /// Название военной части
        /// </summary>
        [DisplayName(@"Название/номер части")]
        public string MilitaryUnitName => _militaryUnitModel.MilitaryName;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _militaryUnitModel.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public MilitaryUnitModel GetModel()
        {
            return _militaryUnitModel;
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