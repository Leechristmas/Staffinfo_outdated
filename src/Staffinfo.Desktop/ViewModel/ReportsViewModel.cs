using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class ReportsViewModel: WindowViewModelBase
    {
        public ReportsViewModel()
        {
            Reports = new List<Report>
            {
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                },
                new Report
                {
                    Name = "Штатная расстановка",
                    Description =
                        "Штатная расстановка пожарного аварийно спасательного отряда учреждения \"Гомельское областное управление МЧС\""
                }
            };
        }

        /// <summary>
        /// Блокировка интерфейса
        /// </summary>
        private bool _isEnable;


        public List<Report> Reports { get; }

        /// <summary>
        /// Выбранный отчет
        /// </summary>
        public Report SelectedReport { get; set; }

        #region Commands
        /// <summary>
        /// Построить отчет
        /// </summary>
        private RelayCommand _getReport;
        public RelayCommand GetReport => _getReport ?? (_getReport = new RelayCommand(GetReportExecute));

        private void GetReportExecute()
        {
            ViewIsEnable = false;
            //построение отчета
            ViewIsEnable = true;
        }

        #endregion

        
    }
}
