using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Controllers
{
    public class TodoController : Controller
    {

       public static String ConnectionString = "Data Source=E:\\rider\\projects\\Todo\\identifier.sqlite;";
       
        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }

        internal TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();

            using (SqliteConnection con = new SqliteConnection(ConnectionString))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = "select * from todo";
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todoList.Add( new TodoItem
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                });
                            }
                        }
                        
                    }
                }
            }

            return new TodoViewModel
            {
                TodoList = todoList
            };;
        }
        
        public RedirectResult Insert(TodoViewModel item)
        {

            Console.WriteLine(item.Todo.Name);
            using (SqliteConnection con = 
                   new SqliteConnection(ConnectionString))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"Insert into todo(name) VALUES ('{item.Todo.Name}')";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return Redirect("https://localhost:5001/");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (SqliteConnection con = new SqliteConnection(ConnectionString))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();
                    
                }
            }

            return Json(new {  });
        }

        [HttpPost]
        public JsonResult PopulateForm(int id)
        {
            Console.WriteLine(id);
            var todo = GetById(id);
            return new JsonResult(todo);
        }

        internal TodoItem GetById(int id)
        {
            TodoItem todoItem = new();
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todoItem.Id = reader.GetInt32(0);
                            todoItem.Name = reader.GetString(1);
                        }
                    }
                }
            }
            return todoItem;
        }

        public RedirectResult Update(TodoItem todo)
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                using (var cmdTable = connection.CreateCommand())
                {
                    connection.Open();
                    cmdTable.CommandText = $"Update todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
                    try
                    {
                        cmdTable.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return Redirect("https://localhost:5001");
        }
        
    }
}