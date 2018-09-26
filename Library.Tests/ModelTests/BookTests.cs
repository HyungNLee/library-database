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

    [TestMethod]
    public void Save_SaveBookToList_True()
    {
      Book newBook = new Book("Meria Potter");
      newBook.Save();

      List<Book> result = Book.GetAll();

      Assert.AreEqual(newBook, result[0]);
    }

    [TestMethod]
    public void Find_FoundBookIsSameAsSavedBook_True()
    {
      //Arrange
      Book newBook = new Book("Meria Potter");
      newBook.Save();
      int searchId = newBook.Id;

      //Act
      Book foundBook = Book.Find(searchId);

      //Assert
      Assert.AreEqual(newBook, foundBook);
    }

    [TestMethod]
    public void Edit_EditedBookHasNewName_True()
    {
      //Arrange
      Book newBook = new Book("Meria Potter");
      newBook.Save();

      //Act
      string updateName = "Ryan Potter";
      newBook.Edit(updateName);

      //Assert
      Assert.AreEqual(newBook.Title, updateName);
    }
  }
}