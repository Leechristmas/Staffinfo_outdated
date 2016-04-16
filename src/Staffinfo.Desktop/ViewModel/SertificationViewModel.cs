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
        private readonly SertificationModel _sertificationModel;

        public SertificationViewModel(SertificationModel sertificationModel)
        {
            _sertificationModel = sertificationModel;
        }

        /// <summary>
        /// Дата аттестации
        /// </summary>
        private const string SertificationDatePropertyName = "SertificationDate";
        public string SertificationDate
        {
            get { return _sertificationModel.SertificationDate.ToString("yy-mm-dd"); }
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
        public string Description { get { return _sertificationModel.Description; } }


        public SertificationModel GetModel()
        {
            return _sertificationModel;
        }

        public SertificationModel Get()
        {
            return _sertificationModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}