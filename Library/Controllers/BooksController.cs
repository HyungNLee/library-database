using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
  public class BooksController : Controller
  {
    [HttpGet("/books")]
    public ActionResult Index()
    {
      List<Book> allBooks = Book.GetAll();
      List<Author> allAuthors = Author.GetAll();
      Dictionary<string, object> model = new Dictionary<string, object>{};
      model.Add("bookList", allBooks);
      model.Add("authorList", allAuthors);
      return View(model);
    }

    [HttpPost("/books/new")]
    public ActionResult Create(string newAuthor, string newTitle, string addAuthor)
    {
      Author addThisAuthor;
      int addAuthorInt = int.Parse(addAuthor);

      if (newAuthor != null)
      {
        addThisAuthor = new Author(newAuthor);
        addThisAuthor.Save();
      }
      else
      {
        addThisAuthor = Author.Find(addAuthorInt);
      }

      Book newBook = new Book(newTitle);
      newBook.Save();
      newBook.AddAuthor(addThisAuthor);
      return RedirectToAction("Index");
    }

    [HttpGet("/books/{id}")]
    public ActionResult Details(int id)
    {
      Book newBook = Book.Find(id);
      List<Copy> allCopies = newBook.GetCopies();
      List<Author> allAuthors = Author.GetAll();
      Dictionary<string, object> model = new Dictionary<string, object>{};
      model.Add("book", newBook);
      model.Add("authorList", allAuthors);
      model.Add("copiesList", allCopies);
      return View(model);
    }

    [HttpPost("books/addauthor")]
    public ActionResult AddAuthor(string bookId, string newAuthor, string addAuthor)
    {
      Author addThisAuthor;

      int bookIdInt = int.Parse(bookId);
      int addAuthorInt = int.Parse(addAuthor);

      if (newAuthor != null)
      {
        addThisAuthor = new Author(newAuthor);
        addThisAuthor.Save();
      }
      else
      {
        addThisAuthor = Author.Find(addAuthorInt);
      }
      Book foundBook = Book.Find(bookIdInt);
      foundBook.AddAuthor(addThisAuthor);

      return RedirectToAction("Details", new { id = bookId});
    }

    [HttpGet("/books/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Book foundBook = Book.Find(id);
      foundBook.Delete();
      return RedirectToAction("Index");
    }

    [HttpGet("/books/{id}/addcopy")]
    public ActionResult AddCopy(int id)
    {
      Book foundBook = Book.Find(id);
      foundBook.AddCopy();
      return RedirectToAction("Details", new { id = id });
    }
  }
}