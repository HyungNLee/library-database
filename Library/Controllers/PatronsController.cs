using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
  public class PatronsController : Controller
  {
    [HttpGet("/patrons")]
    public ActionResult Index()
    {
      List<Patron> allPatrons = Patron.GetAll();
      return View(allPatrons);
    }
    [HttpPost("/patrons/new")]
    public ActionResult Create(string newPatron)
    {
      Patron newPerson = new Patron(newPatron);
      newPerson.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/patrons/{id}")]
    public ActionResult Details(int id)
    {
      Patron foundPatron = Patron.Find(id);
      List<Book> allAvailableBooks = Book.GetAllAvailableBooks();
      Dictionary<string, object> model = new Dictionary<string, object>{};
      model.Add("patron", foundPatron);
      model.Add("bookList", allAvailableBooks);
      return View(model);
    }

    [HttpPost("/patrons/newcheckout")]
    public ActionResult NewCheckout(int bookId, int patronId)
    {
      Book foundBook = Book.Find(bookId);
      Copy newCopy = foundBook.GetAvailableCopies()[0];

      DateTime checkoutDate = DateTime.Now;
      DateTime dueDate = checkoutDate.AddDays(14);

      Checkout newCheckout = new Checkout(patronId, newCopy.Id, checkoutDate, dueDate);
      newCheckout.Save();
    
      return RedirectToAction("Details", new {id = patronId});
    }

    [HttpGet("/patrons/{patronId}/checkouts/{checkoutId}/return")]
    public ActionResult ReturnBook(int patronId, int checkoutId)
    {
      Checkout newCheckout = Checkout.Find(checkoutId);
      newCheckout.ReturnBook();
      return RedirectToAction("Details", new { id = patronId});
    }

  }
}