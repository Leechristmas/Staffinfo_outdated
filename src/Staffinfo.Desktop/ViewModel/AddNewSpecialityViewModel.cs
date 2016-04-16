using System;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Helpers;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// View-model для добавления специальности
    /// </summary>
    public class AddNewSpecialityViewModel: WindowViewModelBase
    {
        /// <summary>
        /// Название специальности
        /// </summary>
        private string _specialityName = String.Empty;

        /// <summary>
        /// Описание
        /// </summary>
        private string _description = String.Empty;

        /// <summary>
        /// Текст ошибки
        /// </summary>
        private string _error = String.Empty;

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Название специальности
        /// </summary>
        public string SpecialityName
        {
            get { return _specialityName; }
            set
            {
                _specialityName = value;
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить специальность
        /// </summary>
        private RelayCommand _addSpeciality;
        public RelayCommand AddSpeciality => _addSpeciality ?? (_addSpeciality = new RelayCommand(AddSpecialityExecute));

        private void AddSpecialityExecute()
        {
            Error = String.Empty;
            if (SpecialityName == String.Empty)
            {
                Error = "Не указано название специальности";
                return;
            }
            if (Description.Length > 200)
            {
                Error = "Слишком длинное описание";
                return;
            }

            ViewIsEnable = false;
            using (SpecialityTableProvider sPrvdr = new SpecialityTableProvider())
            {
                var speciality = sPrvdr.Save(new SpecialityModel()
                {
                    Speciality = SpecialityName,
                    Description = Description
                });
                if (speciality == null)
                {
                    MessageBox.Show("Не удалось сохранить специальность: " + sPrvdr.ErrorInfo, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataSingleton.Instance.SpecialityList.Add(speciality);  //добавляем элемент в список
                DataSingleton.Instance.SpecialityList.Sort();           //сортируем список
            }
            ViewIsEnable = true;

            CloseWindow();
        }
    }
}