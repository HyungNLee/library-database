using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.TestTools
{
  [TestClass]
  public class BookTests : IDisposable
  {
    public BookTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Book.DeleteAll();
    }
    [TestMethod]
    public void GetAll_DBStartsEmpty_0()
    {
      //Arrange
      List<Book> allBooks = Book.GetAll();

      //Act
      int count = allBooks.Count;

      //Assert
      Assert.AreEqual(0,count);
    }
  }

}