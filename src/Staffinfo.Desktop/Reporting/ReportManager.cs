using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Staffinfo.Desktop.Reporting
{
    public static class ReportManager
    {
        /// <summary>
        /// Формирование имени файла с подстановкой индекса "(i)" если файл с таким именем существует
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <returns></returns>
        static private string GetFileName(string fileName)
        {
            var path = Path.GetTempPath();
            var newFileName = fileName;
            var i = 1;
            while (File.Exists(path + newFileName))
            {
                newFileName = Path.GetFileNameWithoutExtension(fileName) + " (" + i + ")" + Path.GetExtension(fileName);
                i++;
            }
            return path + newFileName;
        }

        /// <summary>
        /// Сохраняет отчет в файл (во временном каталоге) и открывает его в Excel
        /// </summary>
        /// <param name="workbook">Книга</param>
        /// <param name="fileName">Имя файла</param>
        static public string OpenReport(Workbook workbook, string fileName, Application excel)
        {
            var outputFileName = GetFileName(fileName);
            try
            {
                workbook.SaveAs(outputFileName);
                workbook.Close();
                excel.Quit();
                Process.Start(outputFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно сохранить отчет в файл.\n" + ex.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error, MessageBoxResult.OK);
            }
            return Path.GetFileName(outputFileName);
        }

        ///// <summary>
        ///// Сохраняет отчет в файл (во временном каталоге) и открывает его в Excel
        ///// </summary>
        ///// <param name="workbook">Книга</param>
        ///// <param name="fileName">Имя файла</param>
        //static public string OpenReport(Aspose.Cells.Workbook workbook, string fileName)
        //{
        //    var outputFileName = GetFileName(fileName);
        //    try
        //    {
        //        //после сохранения добавляет лишнюю страницу
        //        //var t = workbook.Worksheets;
        //        workbook.Save(outputFileName);
        //        //var tt = workbook.Worksheets;

        //        //workbook.Sa

        //        RemoveAsposeLicenseSheet(outputFileName);
        //        Process.Start(outputFileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Невозможно сохранить отчет в файл.\n" + ex.Message, "Ошибка", MessageBoxButton.OK,
        //            MessageBoxImage.Error, MessageBoxResult.OK);
        //    }
        //    return Path.GetFileName(outputFileName);
        //}

        //static public void RemoveAsposeLicenseSheet(string filename)
        //{
        //    var excel = new Microsoft.Office.Interop.Excel.Application();
        //    var workbook = excel.Workbooks.Open(filename);

        //    var sheets = ((Microsoft.Office.Interop.Excel.Worksheet) workbook.Sheets[3]);
        //    ((Microsoft.Office.Interop.Excel.Worksheet) workbook.Sheets[3]).Delete();
        //    workbook.SaveAs(filename);
        //    workbook.Close();
        //    excel.Quit();
        //}

        /// <summary>
        /// получить шаблон
        /// </summary>
        /// <param name="templateName">Имя шаблона</param>
        /// <returns>Последовательность байтов</returns>
        static public Stream GetTemplate(string templateName)
        {
            // Текущая сборка, для получения доступа к ресурсам (sql файлы и шаблоны отчета)
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentException("name Template is null or empty");

            return Assembly.GetExecutingAssembly().GetManifestResourceStream
                 (string.Concat(executingAssembly.GetName().Name, ".Templates.", templateName));
        }

        /// <summary>
        /// получить путь к шаблону
        /// </summary>
        /// <param name="templateName">Имя шаблона</param>
        /// <returns>Последовательность байтов</returns>
        static public string GetTemplatePath(string templateName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                 $"Reporting/Templates/{templateName}");
        }

        /// <summary>
        /// Возвращает путь временной папки пользователя для сохранения отчета
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <returns></returns>
        static public string GetTempPathForSave(string fileName)
        {
            var tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, fileName);
        }
    }
}