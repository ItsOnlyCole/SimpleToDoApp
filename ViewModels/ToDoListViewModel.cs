using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using SimpleToDoApp.Models;
using SimpleToDoApp.Helpers;

namespace SimpleToDoApp.ViewModels
{
    public class ToDoListViewModel
    {
        public List<ToDoItem> ToDoItems { get; set; }
        public ToDoItem EditableItem { get; set; }

        public ToDoListViewModel()
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.CheckIfDbExist();

            using (var db = new SqliteConnection(dbHelper.GetConnection().ConnectionString)) {
                db.Open();
                this.EditableItem = new ToDoItem();
                this.ToDoItems = new List<ToDoItem>();

                var listItemsCmd = db.CreateCommand();
                listItemsCmd.CommandText = "SELECT * FROM todo_items ORDER BY add_date DESC";
                
                using (SqliteDataReader reader = listItemsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ToDoItem toDoItem = new Models.ToDoItem()
                        {
                            Id = (int)(long)reader["id"],
                            DateAdded = Convert.ToDateTime(reader["add_date"]),
                            Title = reader["title"].ToString(),
                            IsDone = Convert.ToBoolean(reader["is_done"])
                        };
                        this.ToDoItems.Add(toDoItem);
                    }
                }
            }
        }
    }
}