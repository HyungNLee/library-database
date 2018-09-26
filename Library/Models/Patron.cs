using System;
using System.Collections.Generic;
using Library;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Patron
  {
    public int Id {get; set;}
    public string Name {get; set;}

    public Patron(string newName, int newId = 0)
    {
      Id = newId;
      Name = newName;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if(!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool nameEquality = (this.Name == newPatron.Name);
        return (nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Patron newPatron = new Patron(name, id);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }  

      return allPatrons;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE patrons;";

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
      cmd.CommandText = @"INSERT INTO patrons (name) VALUES (@name);";

      cmd.Parameters.AddWithValue("@name", this.Name);

      cmd.ExecuteNonQuery();

      this.Id = (int) cmd.LastInsertedId;

      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      string newName= "";

      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        newName = rdr.GetString(1);
      }

      Patron newPatron = new Patron(newName, newId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return newPatron;
    }

    public void Edit(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE patrons SET name = @newName WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@newName", newName);
      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();

      this.Name = newName;

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
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @searchId; DELETE FROM checkouts WHERE patron_id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Checkout> GetCheckouts()
    {
      List<Checkout> allCheckouts = new List<Checkout>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT checkouts.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patron_id) WHERE patrons.id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int patronId = rdr.GetInt32(1);
        int copyId = rdr.GetInt32(2);
        DateTime checkoutDate = rdr.GetDateTime(3);
        DateTime dueDate = rdr.GetDateTime(4);
        bool isReturned = rdr.GetBoolean(5);
        Checkout newCheckout = new Checkout(patronId, copyId, checkoutDate, dueDate, isReturned, id);
        allCheckouts.Add(newCheckout);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return allCheckouts;
    }
  }
}
