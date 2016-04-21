using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using Staffinfo.Desktop.Annotations;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class SertificationViewModel : INotifyPropertyChanged
    {
        private SertificationModel _sertificationModel;

        public SertificationViewModel(SertificationModel sertificationModel)
        {
            _sertificationModel = sertificationModel;
        }

        /// <summary>
        /// Дата аттестации
        /// </summary>
        private const string SertificationDatePropertyName = "SertificationDate";
        [DisplayName(@"Дата аттестации")]
        public string SertificationDate
        {
            get { return _sertificationModel.SertificationDate.ToString("d"); }
            /*  set
              {
                  if (_sertificationModel.SertificationDate == value)
                      return;

                  _sertificationModel.SertificationDate = value;
                  OnPropertyChanged(SertificationDatePropertyName);
              }*/
        }

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _sertificationModel.Description;

        /// <summary>
        /// Возвращае модель
        /// </summary>
        /// <returns></returns>
        public SertificationModel GetModel()
        {
            return _sertificationModel;
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