using System.Configuration;
using ESRINederland.Framework.Logging;
using MySql.Data.MySqlClient;

namespace MannusBackup.FtpIndexer
{
    internal class MySqlDatabase
    {
        MySqlConnection connection;

        internal MySqlDatabase()
        {
            connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysql"].ConnectionString);
        }

        internal void OpenDatabase()
        {
            try
            {
                connection.Open();
            }
            catch (MySqlException e)
            {
            }
        }

        internal void DeleteExistingData()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string sql = "DELETE FROM mb_ftpindex";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        internal MySqlDataReader ReadData(string ftpSite)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string sql = "SELECT filenaam FROM mb_ftpindex WHERE ftpsite = ?ftpsite";
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlParameter param1 = new MySqlParameter("?ftpsite", MySqlDbType.VarChar);
                param1.Value = ftpSite;
                command.Parameters.Add(param1);
                return command.ExecuteReader();
            }
            else
            {
                return null;
            }
        }

        internal void CloseDatabase()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        internal void AddFileToTable(string ftpSite, string fileName)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string sql = "INSERT INTO mb_ftpindex (ftpsite, filenaam) VALUES (?ftpsite, ?filenaam)";
                Logger.GetLogger().LogDebug(sql);
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlParameter param1 = new MySqlParameter("?ftpsite", MySqlDbType.VarChar);
                MySqlParameter param2 = new MySqlParameter("?filenaam", MySqlDbType.VarChar);
                param1.Value = ftpSite;
                param2.Value = fileName;
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
            }
        }
    }
}