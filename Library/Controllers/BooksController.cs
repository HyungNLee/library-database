using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
  public class BooksController : Controller
  {
    [HttpGet]
    public ActionResult Index()
    {
      List<Book> allBooks = Book.GetAll();
      return View(allBooks);
    }

    [HttpPost("/books/new")]
    public ActionResult Create()
    {
      Book newBook = new Book(Request.Form["newTitle"]);
      newBook.Save();
      return RedirectToAction("Index");
    }
    [HttpGet("/books/{id}")]
    public ActionResult Details(int id)
    {
      Book newBook = Book.Find(id);
      return View(newBook);
    }
  }
}