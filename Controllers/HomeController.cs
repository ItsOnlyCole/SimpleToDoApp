using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleToDoApp.Models;
using SimpleToDoApp.ViewModels;
using SimpleToDoApp.Helpers;

namespace SimpleToDoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ToDoListViewModel viewModel = new ToDoListViewModel();
            return View("Index", viewModel);
        }

        public IActionResult Edit(int id)
        {
            ToDoListViewModel viewModel = new ToDoListViewModel();
            viewModel.EditableItem = viewModel.ToDoItems.FirstOrDefault(x => x.Id == id);
            return View("Index", viewModel);
        }

        public IActionResult Delete(int id)
        {
            DbHelper dbHelper = new DbHelper();
            using (var db = new SqliteConnection(dbHelper.GetConnection().ConnectionString))
            {
                db.Open();
                var deleteCmd = db.CreateCommand();
                deleteCmd.CommandText = $"DELETE FROM todo_items WHERE id = {id}";
                deleteCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult CreateUpdate(ToDoListViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                DbHelper dbHelper = new DbHelper();
                using (var db = new SqliteConnection(dbHelper.GetConnection().ConnectionString))
                {
                    db.Open();
                    if (viewModel.EditableItem.Id <= 0)
                    {
                        viewModel.EditableItem.DateAdded = DateTime.Now;
                        var insertCmd = db.CreateCommand();
                        insertCmd.CommandText = "INSERT INTO todo_items (add_date,title,is_done) VALUES (@date, @title, 0)";
                        insertCmd.Parameters.AddWithValue("date", viewModel.EditableItem.DateAdded);
                        insertCmd.Parameters.AddWithValue("title", viewModel.EditableItem.Title);
                        insertCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        var updateCmd = db.CreateCommand();
                        updateCmd.CommandText = $"UPDATE todo_items SET title = @title WHERE id = @id";
                        updateCmd.Parameters.AddWithValue("title", viewModel.EditableItem.Title);
                        updateCmd.Parameters.AddWithValue("id", viewModel.EditableItem.Id);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new ToDoListViewModel());
            }
        }

        public IActionResult ToggleIsDone(int id)
        {
            if(ModelState.IsValid)
            {
                DbHelper dbHelper = new DbHelper();
                using (var db = new SqliteConnection(dbHelper.GetConnection().ConnectionString))
                {
                    bool completed = false;
                    db.Open();
                    //Pulls Info for specific item based on ID
                    var itemPullCmd = db.CreateCommand();
                    itemPullCmd.CommandText = "SELECT is_done FROM todo_items WHERE id = @id";
                    itemPullCmd.Parameters.AddWithValue("id", id);
                    using (SqliteDataReader reader = itemPullCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            completed = Convert.ToBoolean(reader["is_done"]);
                        }
                    }

                    //Checks if task is done or not then updates database based on that.
                    var updateIsDoneCmd = db.CreateCommand();
                    updateIsDoneCmd.CommandText = "UPDATE todo_items SET is_done = @newValue WHERE id = @id";
                    updateIsDoneCmd.Parameters.AddWithValue("id", id);
                    if(completed == false)
                    {
                        updateIsDoneCmd.Parameters.AddWithValue("newValue", 1);
                    }
                    else if (completed == true)
                    {
                        updateIsDoneCmd.Parameters.AddWithValue("newValue", 0);
                    }
                    else
                    {
                        Console.WriteLine("!!!Error with updating completed boolean!!!");
                    }
                    updateIsDoneCmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new ToDoListViewModel());
            }
        }
    }
}