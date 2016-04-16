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
    /// View-model для добавления нового учебного заведения
    /// </summary>
    public class AddNewEducationalInstitutionViewModel: WindowViewModelBase
    {
        /// <summary>
        /// Название учебного заведения
        /// </summary>
        private string _institutionName = String.Empty;

        /// <summary>
        /// Тип учебного заведения
        /// </summary>
        private string _institutionType = String.Empty;

        /// <summary>
        /// Описание учебного заведения
        /// </summary>
        private string _description = String.Empty;

        /// <summary>
        /// Название учебного заведения
        /// </summary>
        public string InstitutionName
        {
            get { return _institutionName; }
            set
            {
                _institutionName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Тип учебного заведения
        /// </summary>
        public string InstitutionType
        {
            get { return _institutionType; }
            set
            {
                _institutionType = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Описание учебного заведения
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

        private RelayCommand _addInstitution;
        private string _error;

        public RelayCommand AddInstitution
            => _addInstitution ?? (_addInstitution = new RelayCommand(AddInstitutionExecute));

        private void AddInstitutionExecute()
        {
            Error = String.Empty;
            if (InstitutionName == String.Empty)
            {
                Error = "Не указано название учебного заведения";
                return;
            }
            if (InstitutionType == String.Empty)
            {
                Error = "Не указано тип учебного заведения";
                return;
            }
            if (Description.Length > 200)
            {
                Error = "Слишком длинное описание";
                return;
            }

            ViewIsEnable = false;
            using (EducationalInstitutonTableProvider ePrvdr = new EducationalInstitutonTableProvider())
            {
                var institution = ePrvdr.Save(new EducationalInstitutionModel()
                {
                    InstituitionTitle = InstitutionName,
                    InstituitionType = InstitutionType,
                    Description = Description
                });
                if (institution == null)
                {
                    MessageBox.Show("Не удалось сохранить учебное заведение: " + ePrvdr.ErrorInfo, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataSingleton.Instance.EducationalInstitutionList.Add(institution); //добавляем элемент
                DataSingleton.Instance.EducationalInstitutionList.Sort();           //сортируем новый список
            }
            ViewIsEnable = true;

            CloseWindow();
        }

    }
}