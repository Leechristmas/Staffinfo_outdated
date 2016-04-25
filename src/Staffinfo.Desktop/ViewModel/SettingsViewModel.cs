using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view-model для окна настроек
    /// </summary>
    public class SettingsViewModel: WindowViewModelBase
    {
        public SettingsViewModel()
        {
            //инициализируем журнал
            //RefreshLogRecords();
        }

        #region Log implementation

        /// <summary>
        /// Период по умолчанию
        /// </summary>
        //private const string DefaultPeriod = "за 1 день";

        /// <summary>
        /// Записи из лог-таблицы
        /// </summary>
        private List<DbLogRecord> _logRecords;

        /// <summary>
        /// Выбранный период для отображения
        /// </summary>
        private string _selectedPeriod; // = DefaultPeriod;

        /// <summary>
        /// Активный таб
        /// </summary>
        private int _selectedTab;

        /// <summary>
        /// Активный таб
        /// </summary>
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                //if (_selectedTab == 1) RefreshLogRecords(); //определяем журнал по номеру таба: если открываем журнал - обновляем его
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Записи из лог-таблицы для журнала
        /// </summary>
        public List<DbLogRecord> LogRecords
        {
            get { return _logRecords; }
            set
            {
                _logRecords = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный период для отображения
        /// </summary>
        public string SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                RefreshLogRecords();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Обновить журнал
        /// </summary>
        /// <returns></returns>
        public void RefreshLogRecords()
        {
            ViewIsEnable = false;
            try
            {
                using (LogTableProvider lPrvdr = new LogTableProvider())
                {
                    switch (SelectedPeriod)
                    {
                        case "за 1 день":
                            LogRecords = lPrvdr.Select(DateTime.Today).ToList();
                            break;
                        case "за 7 дней":
                            LogRecords = lPrvdr.Select(DateTime.Today.AddDays(-6)).ToList();
                            break;
                        case "за 30 дней":
                            LogRecords = lPrvdr.Select(DateTime.Today.AddDays(-29)).ToList();
                            break;
                        case "за все время":
                            LogRecords = lPrvdr.Select().ToList();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось загрузить журнал!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.OK);
            }
            finally
            {
                ViewIsEnable = true;
            }
        }

        /// <summary>
        /// При закрытии окна
        /// </summary>
        private RelayCommand _windowClosing;
        public RelayCommand WindowClosing => _windowClosing ?? (_windowClosing = new RelayCommand(WindowClosingExecute))
            ;

        private void WindowClosingExecute()
        {
            //указываем стартовый таб
            SelectedTab = 0;
            //сбрасываем период для журнала
            SelectedPeriod = null;
            //зануляем журнал лога
            LogRecords = null;
        }

        #endregion

        /// <summary>
        /// индекс выбранного справочника
        /// </summary>
        private int _selectedCatalogIndex;

        /// <summary>
        /// Индекс выбранного справочника
        /// </summary>
        private int _selectedCatalogRecordIndex;

        /// <summary>
        /// Текст ошибки
        /// </summary>
        private string _catalogTextError;

        /// <summary>
        /// Справочники
        /// </summary>
        public List<string> Catalogs => new List<string> //...рука лицо: костыль на костыле...
        {
            "Военная часть",
            "Учебное заведение",
            "Специальность"
        };

        /// <summary>
        /// Индекс выбранного справочника
        /// </summary>
        public int SelectedCatalogIndex
        {
            get { return _selectedCatalogIndex; }
            set
            {
                _selectedCatalogIndex = value;
                RaisePropertyChanged(nameof(SelectedCatalog));
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Индекс выбранной записи
        /// </summary>
        public int SelectedCatalogRecordIndex
        {
            get { return _selectedCatalogRecordIndex; }
            set
            {
                _selectedCatalogRecordIndex = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Военные части
        /// </summary>
        public ObservableCollection<MilitaryUnitViewModel> MilitaryUnits { get; set; } 

        /// <summary>
        /// Учебные заведения
        /// </summary>
        public ObservableCollection<EducationalIntitutionViewModel> Institutions { get; set; } 

        /// <summary>
        /// Специальности
        /// </summary>
        public ObservableCollection<SpecialityViewModel> Specialities { get; set; } 
 

        /// <summary>
        /// Активный справочник
        /// </summary>
        public object SelectedCatalog  //Очередной гребанный костыль...стоит запилить что-то получше.
        {
            get
            {
                if (SelectedCatalogIndex < 0) return null;
                switch (SelectedCatalogIndex)
                {
                    case 0:
                        return (MilitaryUnits = new ObservableCollection<MilitaryUnitViewModel>(
                                       DataSingleton.Instance.MilitaryUnitList.Select(m => new MilitaryUnitViewModel(m))));
                    case 1:
                        return (Institutions =
                                   new ObservableCollection<EducationalIntitutionViewModel>(
                                       DataSingleton.Instance.EducationalInstitutionList.Select(
                                           e => new EducationalIntitutionViewModel(e))));
                    case 2:
                        return (Specialities =
                                   new ObservableCollection<SpecialityViewModel>(
                                       DataSingleton.Instance.SpecialityList.Select(s => new SpecialityViewModel(s))));
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string CatalogTextError
        {
            get { return _catalogTextError; }
            set
            {
                _catalogTextError = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        private RelayCommand _removeItem;
        public RelayCommand RemoveItem => _removeItem ?? (_removeItem = new RelayCommand(RemoveItemExecute));

        private void RemoveItemExecute()
        {
            CatalogTextError = String.Empty;
            if (SelectedCatalogRecordIndex < 0)
            {
                CatalogTextError = "Запись не выбрана";
                return;
            }
            var answer = MessageBox.Show("Удаление записи повлечет за собой удаление других, зависящих от неё. Продолжить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No);

            if (answer == MessageBoxResult.No) return;

            try
            {
                switch (SelectedCatalogIndex)
                {
                    case 0:     //военная часть
                        using (MilitaryUnitTableProvider prvdr = new MilitaryUnitTableProvider())
                        {
                            var unitId = MilitaryUnits.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!prvdr.DeleteById(unitId)) throw new Exception(prvdr.ErrorInfo); //если удалить не удалось, бросаем exception
                        }
                        MilitaryUnits.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 1:     //учреждения образования
                        using (EducationalInstitutonTableProvider prvdr = new EducationalInstitutonTableProvider())
                        {
                            var instId = Institutions.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!prvdr.DeleteById(instId)) throw new Exception(prvdr.ErrorInfo);
                        }
                        Institutions.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                    case 2:     //специальности
                        using (SpecialityTableProvider prvdr = new SpecialityTableProvider())
                        {
                            var specId = Specialities.ElementAt(SelectedCatalogRecordIndex).GetModel().Id;
                            if (!prvdr.DeleteById(specId)) throw new Exception(prvdr.ErrorInfo);
                        }
                        Specialities.RemoveAt(SelectedCatalogRecordIndex);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось удалить запись: " + e.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error, MessageBoxResult.OK);
            }

        }

    }
}