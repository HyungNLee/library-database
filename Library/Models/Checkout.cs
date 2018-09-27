using System;
using System.Collections.Generic;
using Library;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Checkout
  {
    public int Id {get; set;}
    public int PatronId {get; set;}
    public int CopyId {get; set;}
    public DateTime CheckoutDate {get; set;}
    public DateTime DueDate {get; set;}
    public bool IsReturned {get; set;}

    public Checkout(int newPatronId, int newCopyId, DateTime newCheckoutDate, DateTime newDueDate, bool newIsReturned = false, int newId = 0)
    {
      Id = newId;
      PatronId = newPatronId;
      CopyId = newCopyId;
      CheckoutDate = newCheckoutDate;
      DueDate = newDueDate;
      IsReturned = newIsReturned;
    }

    public override bool Equals(System.Object otherCheckout)
    {
      if(!(otherCheckout is Checkout))
      {
        return false;
      }
      else
      {
        Checkout newCheckout = (Checkout) otherCheckout;
        bool patronIdEquality = (this.PatronId == newCheckout.PatronId);
        bool copyIdEquality = (this.CopyId == newCheckout.CopyId);
        bool checkoutDateEquality = (this.CheckoutDate == newCheckout.CheckoutDate);
        bool dueDateEquality = (this.DueDate == newCheckout.DueDate);
        bool isReturnedEquality = (this.IsReturned == newCheckout.IsReturned);
  
        return (patronIdEquality && copyIdEquality && checkoutDateEquality && dueDateEquality && isReturnedEquality);
      }
    }

    public override int GetHashCode()
    {
      string allHash = this.PatronId.ToString() + this.CopyId.ToString() + this.CheckoutDate.ToString() + this.DueDate.ToString() + this.IsReturned.ToString();
      return allHash.GetHashCode();
    }

    public static List<Checkout> GetAll()
    {
      List<Checkout> allCheckouts = new List<Checkout>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM Checkouts;";

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
      if(conn != null)
      {
        conn.Dispose();
      }  

      return allCheckouts;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE checkouts;";

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
      cmd.CommandText = @"INSERT INTO checkouts (patron_id, copy_id, checkout_date, due_date, is_returned) VALUES (@patronId, @copyId, @checkoutDate, @dueDate, 0); UPDATE copies SET (is_available = 0) WHERE id = @copyId;";

      cmd.Parameters.AddWithValue("@patronId", this.PatronId);
      cmd.Parameters.AddWithValue("@copyId", this.CopyId);
      cmd.Parameters.AddWithValue("@checkoutDate", this.CheckoutDate);
      cmd.Parameters.AddWithValue("@dueDate", this.DueDate);

      cmd.ExecuteNonQuery();

      this.Id = (int) cmd.LastInsertedId;

      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Checkout Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM checkouts WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      int patronId = 0;
      int copyId = 0;
      DateTime checkoutDate = new DateTime(1111, 11, 11);
      DateTime dueDate = new DateTime(1111, 11, 11);
      bool isReturned = false;
      

      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        patronId = rdr.GetInt32(1);
        copyId = rdr.GetInt32(2);
        checkoutDate = rdr.GetDateTime(3);
        dueDate = rdr.GetDateTime(4);
        isReturned = rdr.GetBoolean(5);
      }

      Checkout newCheckout = new Checkout(patronId, copyId, checkoutDate, dueDate, isReturned, id);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return newCheckout;
    }

    public void ReturnBook(bool returned)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE checkouts SET is_returned = 1 WHERE id = @searchId; UPDATE copies SET (is_available = 1) WHERE id = @copyId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);
      cmd.Parameters.AddWithValue("@copyId", this.CopyId);

      cmd.ExecuteNonQuery();

      this.IsReturned = returned;

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
      cmd.CommandText = @"DELETE FROM checkouts WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }


  }
}
