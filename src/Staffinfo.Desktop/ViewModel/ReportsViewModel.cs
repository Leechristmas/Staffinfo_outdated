using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Model;
using Staffinfo.Desktop.Reporting;

namespace Staffinfo.Desktop.ViewModel
{
    public class ReportsViewModel: WindowViewModelBase
    {
        public ReportsViewModel()
        {
            //Определяем уровень доступа пользователя, вошедвшего в систему
            AccessLevel = DataSingleton.Instance.User.AccessLevel;

            Reports = new List<Report>
            {
                new Report
                {
                    Id = 1,
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                }
            };
        }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        private string _error;

        /// <summary>
        /// Выбранный отчет
        /// </summary>
        private Report _selectedReport;

        /// <summary>
        /// Список отчетов
        /// </summary>
        public List<Report> Reports { get; }

        /// <summary>
        /// Выбранный отчет
        /// </summary>
        public Report SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// visibility для progress bar'a
        /// </summary>
        public Visibility ProgressVisibility => ViewIsEnable ? Visibility.Collapsed : Visibility.Visible;

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

        #region Commands
        /// <summary>
        /// Построить отчет
        /// </summary>
        private RelayCommand _getReport;
        public RelayCommand GetReport => _getReport ?? (_getReport = new RelayCommand(GetReportExecute));

        private async void GetReportExecute()
        {
            ViewIsEnable = false;
            RaisePropertyChanged(nameof(ProgressVisibility));

            await Task.Run(() =>
            {
                if (SelectedReport == null)
                {
                    Error = "Отчет не выбран";
                    ViewIsEnable = true;
                    return;
                }

                Error = null;

                //построение отчета
                switch (SelectedReport.Id.Value)
                {
                    case 1:
                        using (var stfReports = new StaffInfoReports())
                        {
                            stfReports.Make();
                        }
                        break;
                    default:
                        break;
                }
            });
            
            ViewIsEnable = true;
            RaisePropertyChanged(nameof(ProgressVisibility));
        }

        #endregion

        
    }
}
