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
        /// Записи из лог-таблицы
        /// </summary>
        private List<DbLogRecord> _logRecords;

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
                if (_selectedTab == 1) RefreshLogRecords(); //определяем журнал по номеру таба: если открываем журнал - обновляем его
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
                    LogRecords = lPrvdr.Select().ToList();
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
        }
        

        protected override void CloseWindow()
        {
            //указываем стартовый таб
            SelectedTab = 0;
            base.CloseWindow();  
        }

    }
}