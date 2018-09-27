using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
  public class CheckoutsController : Controller
  {
    [HttpGet("/checkouts")]
    public ActionResult Index()
    {
      List<Checkout> allCheckouts = Checkout.GetAll();
      return View(allCheckouts);
    }
  }
}