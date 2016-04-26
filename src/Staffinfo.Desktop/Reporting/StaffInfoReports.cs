using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace Staffinfo.Desktop.Reporting
{
    public class StaffInfoReports: IDisposable
    {
        public StaffInfoReports()
        {
            reportComponent = new StaffInfoComponent();
        }

        /// <summary>
        /// Книга
        /// </summary>
        private Workbook Workbook { get; set; }

        /// <summary>
        /// Excel-app
        /// </summary>
        private Application Excel { get; set; }

        /// <summary>
        /// Имя файла книги
        /// </summary>
        public string FileName { get; private set; }

        public StaffInfoComponent reportComponent;
        
        /// <summary>
        /// Формирует отчет "Штатная расстановка"
        /// </summary>
        public void Make()
        {
            Excel = new Application()
            {
                Visible = false,
                DisplayAlerts = false
            };
            try
            {
                Workbook = Excel.Workbooks.Open(ReportManager.GetTemplatePath("StaffInfo.xls"));
                ((Worksheet) Workbook.Sheets[2]).Visible = XlSheetVisibility.xlSheetHidden;

                FileName = String.Format($"Штатная расстановка на {DateTime.Today.ToString("d")}.xls");
                var reportSheet = (Worksheet) Workbook.Worksheets["Отчет"];

                int currentRow = 5, emplCount = 0;
                var data = reportComponent.GetStaffPlacementTable();

                foreach (var row in data.Select())
                {
                    emplCount = 0;
                    if (row.Field<string>("Rank") == "")
                    {
                        reportSheet.Range[reportSheet.Cells[currentRow, 1], reportSheet.Cells[currentRow, 7]].Merge();
                        reportSheet.Cells[currentRow, 1] = row.Field<string>("Post");
                        SetServiceHeaderStyle(
                            reportSheet.Range[reportSheet.Cells[currentRow, 1], reportSheet.Cells[currentRow, 7]]);
                    }
                    else
                    {
                        reportSheet.Cells[currentRow, 1] = row.Field<string>("Post");
                        reportSheet.Cells[currentRow, 2] = row.Field<string>("Rank");
                        reportSheet.Cells[currentRow, 3] = row.Field<string>("Name");
                        reportSheet.Cells[currentRow, 4] = row.Field<string>("PersonalNumber");
                        reportSheet.Cells[currentRow, 5] = row.Field<string>("BirthDate");
                        reportSheet.Cells[currentRow, 6] = row.Field<string>("Clasiness");
                        reportSheet.Cells[currentRow, 7] = row.Field<string>("Phone");

                        //устанавливаем стили
                        SetLeftStyle(reportSheet.Range[reportSheet.Cells[currentRow, 1], reportSheet.Cells[currentRow, 1]]);    //крайний левый столбец
                        SetRightStyle(reportSheet.Range[reportSheet.Cells[currentRow, 7], reportSheet.Cells[currentRow, 7]]);   //крайний правый столбец
                        SetMediumStyle(reportSheet.Range[reportSheet.Cells[currentRow, 4], reportSheet.Cells[currentRow, 6]]);  //центральные столбцы
                        SetNameStyle(reportSheet.Range[reportSheet.Cells[currentRow, 3], reportSheet.Cells[currentRow, 3]]);    //стобец с выравниванием по левому краю
                        SetMediumStyle(reportSheet.Range[reportSheet.Cells[currentRow, 2], reportSheet.Cells[currentRow, 2]]);  //центральный столбец
                        emplCount++;
                    }
                    currentRow++;
                }

                if (emplCount > 0)
                {
                    SetBottomStyle(reportSheet.Range[reportSheet.Cells[--currentRow, 2], reportSheet.Cells[currentRow, 6]]);  //нижняя строка
                    SetLeftBottomStyle(reportSheet.Range[reportSheet.Cells[currentRow, 1], reportSheet.Cells[currentRow, 1]]);  //нижняя строка левый столбец 
                    SetRightBottomStyle(reportSheet.Range[reportSheet.Cells[currentRow, 7], reportSheet.Cells[currentRow, 7]]);  //нижняя строка правый столбец
                }
                


                ReportManager.OpenReport(Workbook, FileName, Excel);
            }
            catch (Exception e)
            {
                Excel.Quit();
                var error = e;
            }
        }

        /// <summary>
        /// Установить стиль для названия службы
        /// </summary>
        /// <param name="range"></param>
        private void SetServiceHeaderStyle(Range range)
        {
            //жирность
            range.Font.Bold = true;
            //размер шрифта
            range.Font.Size = 12;
            //название шрифта
            range.Font.Name = "Times New Roman";
            //стиль границы
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            //толщина границы
            range.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlMedium;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
            //выравнивание по горизонтали
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //выравнивание по вертикали
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// Устанавливает стиль для левого столбца отчета
        /// </summary>
        /// <param name="range"></param>
        private void SetLeftStyle(Range range)
        {
            range.Font.Bold = true;
            range.Font.Size = 10;
            range.Font.Name = "Times New Roman";

            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            //range.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
            //выравнивание по горизонтали
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //выравнивание по вертикали
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// Устанавливает стиль для правого столбца отчета
        /// </summary>
        /// <param name="range"></param>
        private void SetRightStyle(Range range)
        {
            range.Font.Bold = false;
            range.Font.Size = 10;
            range.Font.Name = "Times New Roman";

            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            //range.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
            //выравнивание по горизонтали
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //выравнивание по вертикали
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// Устанавливает стиль для центральных столбцов отчета
        /// </summary>
        /// <param name="range"></param>
        private void SetMediumStyle(Range range)
        {
            range.Font.Bold = false;
            range.Font.Size = 10;
            range.Font.Name = "Times New Roman";
            //стиль внешних границ
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            //толщина внешних границ
            //range.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
            //толщина внутренних границ
            range.Borders[XlBordersIndex.xlInsideHorizontal].Weight = XlBorderWeight.xlThin;
            range.Borders[XlBordersIndex.xlInsideVertical].Weight = XlBorderWeight.xlThin;

            //выравнивание по горизонтали
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //выравнивание по вертикали
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// Устанавливает стиль для последней строчки отчета
        /// </summary>
        /// <param name="range"></param>
        private void SetBottomStyle(Range range)
        {
            SetMediumStyle(range);
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;
        }

        /// <summary>
        /// Устанавливает стиль для крайнего правого столбца последней строчки отчета
        /// </summary>
        /// <param name="range"></param>
        private void SetRightBottomStyle(Range range)
        {
            SetMediumStyle(range);
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
        }

        /// <summary>
        /// Устанавливает стиль для крайнего левого столбца последней строчки отчета
        /// </summary>
        /// <param name="range"></param>
        private void SetLeftBottomStyle(Range range)
        {
            SetMediumStyle(range);
            range.Font.Bold = true;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
        }

        /// <summary>
        /// Устанавливает стиль для стобца имени (выравнивание по левому краю)
        /// </summary>
        /// <param name="range"></param>
        private void SetNameStyle(Range range)
        {
            SetMediumStyle(range);
            range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
        }
        #region IDisposable implementation

        private bool _disposed;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || disposing)
                return;

            _disposed = true;
        }

        #endregion
    }
}