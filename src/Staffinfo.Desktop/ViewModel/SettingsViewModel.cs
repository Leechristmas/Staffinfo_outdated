using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
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
        private string _selectedPeriod;// = DefaultPeriod;

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
        public RelayCommand WindowClosing => _windowClosing ?? (_windowClosing = new RelayCommand(WindowClosingExecute));

        private void WindowClosingExecute()
        {
            //указываем стартовый таб
            SelectedTab = 0;
            //сбрасываем период для журнала
            SelectedPeriod = null;
            //зануляем журнал лога
            LogRecords = null;
        }

        //нужные действия произведены в WindowClosingExecute
        //protected override void CloseWindow()
        //{
        //    //указываем стартовый таб
        //    SelectedTab = 0;
        //    base.CloseWindow();  
        //}

    }
}