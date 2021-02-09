using System.IO;
using Microsoft.Data.Sqlite;

namespace SimpleToDoApp.Helpers
{
    public class DbHelper
    {
        public DbHelper() {}
        
        public SqliteConnectionStringBuilder GetConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "Databases/ToDoList.db";

            return connectionStringBuilder;
        }
        public void CheckIfDbExist()
        {
            var connectionString = this.GetConnection();
            if(File.Exists(connectionString.DataSource))
            {
                //Do Nothing
            }
            else
            {
                CreateDb();
                CreateTable();
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