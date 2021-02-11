# Simple TODO List App
This is just a simple todo list app I made using ASP.Net Core and SQLite

It's based off of the [tutorial here](https://asp.mvc-tutorial.com/working-with-databases/todo-list-models-viewmodels-helpers/), but I've changed aspects of the project to fit the system I was developing on (Linux).

### Some Changes I've made between the original project and my own
- I use SQLite instead of MS SQL local DB
- I don't use dapper for database management. All the commands and connections are handwritten.
- HomeController.cs, DbHelper.cs, and ToDoListViewModel.cs follow the same core structure from the tutorial, but the code is written differently to work with the technologies I used.