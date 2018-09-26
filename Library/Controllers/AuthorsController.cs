using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {
    [HttpGet("/authors")]
    public ActionResult Index()
    {
      List<Author> allAuthors = Author.GetAll();
      return View(allAuthors);
    }

    [HttpPost("/authors/new")]
    public ActionResult Create()
    {
      Author newAuthor = new Author(Request.Form["newName"]);
      newAuthor.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/authors/{id}")]
    public ActionResult Details(int id)
    {
      Author foundAuthor = Author.Find(id);
      return View(foundAuthor);
    }
  }
}