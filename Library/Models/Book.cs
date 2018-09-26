using System;
using System.Collections.Generic;
using Library;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Book
  {
    public int Id {get; set;}
    public string Title {get; set;}

    public Book (string newTitle, int id = 0)
    {
      Title = newTitle;
      Id = id;
    }

    public override bool Equals(System.Object otherBook)
    {
      if(!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool titleEquality = (this.Title == newBook.Title);
        return (titleEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Title.GetHashCode();
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        Book newBook = new Book(title, id);
        allBooks.Add(newBook);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }  

      return allBooks;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE books;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books (title) VALUES (@title);";

      cmd.Parameters.AddWithValue("@title", this.Title);

      cmd.ExecuteNonQuery();

      this.Id = (int) cmd.LastInsertedId;

      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Book Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      string newTitle= "";

      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        newTitle = rdr.GetString(1);
      }

      Book newBook = new Book(newTitle, newId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return newBook;
    }

    public void Edit(string newTitle)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE books SET title = @newTitle WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@newTitle", newTitle);
      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();

      this.Title = newTitle;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books WHERE id = @searchId; DELETE FROM books_authors WHERE book_id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddAuthor(Author newAuthor)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books_authors (book_id, author_id) VALUES (@bookId, @authorId);";

      cmd.Parameters.AddWithValue("@bookId", this.Id);
      cmd.Parameters.AddWithValue("@authorId", newAuthor.Id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Author> GetAuthors()
    {
      List<Author> allAuthors = new List<Author>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT authors.* FROM books JOIN books_authors ON (books.id = books_authors.book_id) JOIN authors ON (books_authors.author_id = authors.id) WHERE books.id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Author newAuthor =  new Author(name,id);
        allAuthors.Add(newAuthor);
      }
  
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allAuthors;
    }

    public void AddCopy()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO copies (book_id, is_available) VALUES (@bookId, 1);";

      cmd.Parameters.AddWithValue("@bookId", this.Id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<int> GetCopyIds()
    {
      List<int> allCopyIds = new List<int>() {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        allCopyIds.Add(newId);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return allCopyIds;
    }
  }
}
