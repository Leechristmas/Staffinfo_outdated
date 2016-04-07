using System;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// View-model для добавления воинской части
    /// </summary>
    public class AddNewMilitaryUnitViewModel: WindowViewModelBase
    {
        /// <summary>
        /// Название воинской части
        /// </summary>
        private string _militaryUnitName = String.Empty;

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
        /// Название воинской части
        /// </summary>
        public string MilitaryUnitName
        {
            get { return _militaryUnitName; }
            set
            {
                _militaryUnitName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить воинскую часть
        /// </summary>
        private RelayCommand _addMilitaryUnit;
        public RelayCommand AddMilitaryUnit
            => _addMilitaryUnit ?? (_addMilitaryUnit = new RelayCommand(AddMilitaryUnitExecute));

        private void AddMilitaryUnitExecute()
        {
            Error = String.Empty;
            if (MilitaryUnitName == String.Empty)
            {
                Error = "Не указано название воинской части";
                return;
            }
            if (MilitaryUnitName.Length > 100)
            {
                Error = "Название слишком длинное";
                return;
            }
            if (Description.Length > 240)
            {
                Error = "Слишком длинное описание";
                return;
            }

            using (MilitaryUnitTableProvider mPrvdr = new MilitaryUnitTableProvider())
            {
                var militaryUnit = mPrvdr.Save(new MilitaryUnitModel
                {
                    MilitaryName = MilitaryUnitName,
                    Description = Description
                });
                if (militaryUnit == null)
                {
                    MessageBox.Show("Не удалось сохранить воинскую часть: " + mPrvdr.ErrorInfo, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataSingleton.Instance.MilitaryUnitList.Add(militaryUnit);
                WindowsClosed = true;
            }
        }

    }
}