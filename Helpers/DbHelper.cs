using System.IO;
using Microsoft.Data.Sqlite;

namespace SimpleToDoApp.Helpers
{
    public class DbHelper
    {
        public SqliteConnectionStringBuilder GetConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./Databases/ToDoList.db";

            return connectionStringBuilder;
        }
        
        public bool CheckIfDbExist(SqliteConnectionStringBuilder databaseConnectionString)
        {
            if(File.Exists(databaseConnectionString.DataSource))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CreateDb()
        {
            using (var connection = new SqliteConnection(GetConnection().ConnectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
        public void CreateTable()
        {
            using (var connection = new SqliteConnection(GetConnection().ConnectionString))
            {
                connection.Open();

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = 
                @"CREATE TABLE todo_items(
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    add_date DATETIME NOT NULL,
                    title NVARCHAR(200) NOT NULL,
                    is_done BIT NOT NULL DEFAULT 0
                    )";
                createTableCmd.ExecuteNonQuery();
            }
        }
    }
}