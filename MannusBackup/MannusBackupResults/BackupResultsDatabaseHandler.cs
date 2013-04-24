using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace MannusBackup.BackupResults
{
    internal class BackupResultsDatabaseHandler
    {
        private ILogger _logger;
        private const string SELECTSQL = "SELECT datum, host, tijd, status, naam, password, size,sizeingb FROM backup_results ORDER BY id DESC";
        private const string INSERTSQL = "INSERT INTO backup_results (datum, host, tijd, status, naam, password, size,sizeingb) VALUES (?datum, ?host, ?tijd, ?status, ?naam, ?password, ?size, ?sizeingb)";
        private readonly string _connectionstring;

        public BackupResultsDatabaseHandler()
        {
            _logger = Logger.GetLogger();
            string connectionStringName = MannusBackupConfiguration.GetConfig().ConnectionStringName;
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connectionstring = connectionString;
        }

        public IEnumerable<BackupResult> GetResults()
        {
            List<BackupResult> results = new List<BackupResult>();
            using (var connection = new MySqlConnection(_connectionstring))
            {
                using (var command = new MySqlCommand(SELECTSQL, connection))
                {
                    MySqlDataReader reader = null;
                    try
                    {
                        connection.Open();
                        reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch (Exception e)
                    {
                        _logger.LogException(e);
                    }
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var result = new BackupResult();
                            result.Datum = reader.GetDateTime(0);
                            result.Host = reader.GetString(1);
                            result.Tijd = DateTime.Parse(reader.GetString(2), CultureInfo.InvariantCulture);
                            result.Status = reader.GetString(3);
                            result.Naam = reader.GetString(4);
                            result.Password = reader.GetString(5);
                            result.Size = reader.GetString(6);
                            result.SizeInGb = reader.GetString(7);
                            results.Add(result);
                        }
                    }
                }
            }
            return results;
        }

        internal void SaveNewResults(IEnumerable<BackupResult> newItems)
        {
            using (var connection = new MySqlConnection(_connectionstring))
            {
                using (var command = new MySqlCommand(INSERTSQL, connection))
                {
                    foreach (var record in newItems)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?datum", Value = record.Datum });
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?host", Value = record.Host});
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?tijd", Value = record.Tijd });
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?status", Value = record.Status });
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?naam", Value = record.Naam });
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?password", Value = record.Password});
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?size", Value = record.Size});
                        command.Parameters.Add(new MySqlParameter() { ParameterName = "?sizeingb", Value = record.SizeInGb});
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            _logger.LogException(e);
                        }
                    }
                }
            }
        }
    }
}