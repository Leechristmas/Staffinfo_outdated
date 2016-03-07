using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Desktop.Data
{
    /// <summary>
    /// Хелпер для управления БД
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        private static string ErrorMessage { get; set; }

        /// <summary>
        /// Ищет и возвращает список всех экземпляров sql-серверов
        /// </summary>
        /// <returns></returns>
        public static List<string> GetServerInstances()
        {
            // Retrieve the enumerator instance and then the data.
            SqlDataSourceEnumerator instance =
              SqlDataSourceEnumerator.Instance;
            var servers = instance.GetDataSources();
            return (from DataRow row in servers.Rows where row["ServerName"].ToString() != "" && row["InstanceName"].ToString() != "" select row["ServerName"].ToString() + "\\" + row["InstanceName"].ToString()).ToList();
        }

        /// <summary>
        /// Сохраняет список имен экземпляров сервера в файл
        /// </summary>
        public static void SaveServerInstancesIntoFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                   $"...\\src\\Staffinfo.Desktop\\Data\\{DataSingleton.Instance.ServersFile}");
            List<string> serverInstances = GetServerInstances();
            File.WriteAllLines(filePath, serverInstances);
        }

        /// <summary>
        /// Возвращает список всех экземпляров sql-серверов из .ini файла
        /// </summary>
        /// <returns>список имен экземпляров локального сервера</returns>
        public static List<string> LoadServerInstances()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"...\\src\\Staffinfo.Desktop\\Data\\{DataSingleton.Instance.ServersFile}");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл инициализации серверов не найден");
            return File.ReadAllLines(filePath).ToList();
        } 

        /// <summary>
        /// Выполняет скрипт по соданию БД
        /// TODO: продумать реализацию удаления БД, в случае неудачи ее создания
        /// </summary>
        public static void CreateDatabase(string connectionString, int сommandTimeOut = 60)
        {
            using (var sqlConn = new SqlConnection(connectionString))
            {
                var createDbCmd = new SqlCommand($"CREATE DATABASE {DataSingleton.Instance.DatabaseName}", sqlConn);
                sqlConn.Open();
                //парсим скрипт
                string[] scriptParts =
                    ParseSqlSqript(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "...\\src\\Staffinfo.Desktop\\Data\\Scripts\\create_and_init_database_staffinfo.sql"));
                //количество операторов скрипта
                int scriptPartsCount = scriptParts.Length;
                //выполняемая в настоящий момент часть скрипта
                string actualScriptPart = null;

                SqlCommand command = sqlConn.CreateCommand();

                //создаем непосредственно database
                createDbCmd.ExecuteNonQuery();

                //инициализируем транзакцию
                var transaction = sqlConn.BeginTransaction("LoadSQLTransaction");
                command.Connection = sqlConn;
                command.Transaction = transaction;
                command.CommandTimeout = сommandTimeOut;
                try
                {
                    //выполняем сами операторы по созданию компонентов бд
                    for (int i = 0; i < scriptPartsCount; i++)
                    {
                        actualScriptPart = scriptParts[i];
                        if (!string.IsNullOrEmpty(actualScriptPart))
                        {
                            command.CommandText = actualScriptPart;
                            command.ExecuteNonQuery();
                        }
                    }
                    // если все прошло успешно, комитим
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ErrorMessage +=
                        $"Ошибка выполнения скрипта 'create database'.\r\nНе выполнен фрагмент:\r\n***{actualScriptPart}***\r\nИсключение: {ex.Message}\r\n";


                    // Попытка откатить транзакцию
                    try
                    {
                        transaction.Rollback();
                        throw new Exception("Ошибка создания базы данных.");
                    }
                    catch (Exception ex2)
                    {
                        // Если возникла ошибка при откате транзакции
                        ErrorMessage += $"Rollback Exception Type: {ex2.GetType()}\r\n";
                        ErrorMessage += $"Message: {ex2.Message}\r\n";
                        throw new Exception(ErrorMessage, ex2);
                    }
                }
            }
        }

        /// <summary>
        /// Вовзвращает массив операторов sql-скрипта
        /// </summary>
        /// <param name="scriptFileName">файл sql-скрипта</param>
        /// <returns></returns>
        private static string[] ParseSqlSqript(string scriptFileName)
        {
            if (!File.Exists(scriptFileName))
            {
                ErrorMessage += "Файл " + scriptFileName + " не найден.\r\n Продолжение невозможно.\r\n";
                throw new FileNotFoundException(ErrorMessage);
            }
            // загружаем скрипт
            string script = File.ReadAllText(scriptFileName, Encoding.UTF8);


            // делим скрипт на части
            string[] separators = { "GO\r\n" };
            return script.Split(separators, StringSplitOptions.None);
        }
    }
}
