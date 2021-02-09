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
                        updateCmd.CommandText = $"UPDATE todo_items SET title = {viewModel.EditableItem.Title} WHERE id = {viewModel.EditableItem.Id}";
                        updateCmd.ExecuteNonQuery();
                        Console.WriteLine("Executing Update Cmd");
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new ToDoListViewModel());
            }
        }

/*        public IActionResult ToggleIsDone(int id)
        {
            DbHelper dbHelper = new DbHelper();
            using (var db = new SqliteConnection(dbHelper.GetConnection().ConnectionString))
            {
                
                var toggleCmd = db.CreateCommand();
                toggleCmd.CommandText = $"UPDATE todo_items SET is_done = "
            }
        }*/
    }
}