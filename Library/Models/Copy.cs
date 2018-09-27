using System;
using System.Collections.Generic;
using Library;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Copy
  {
    public int Id {get; set;}
    public int BookId {get; set;}
    public bool IsAvailable {get; set;}

    public Copy (int newBookId, bool newIsAvailable = true, int id = 0)
    {
      BookId = newBookId;
      IsAvailable = newIsAvailable;
      Id = id;
    }

    public void Available()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE copies SET is_avaiable = 1 WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();
      this.IsAvailable = true;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }  
    }

    public void Unavailable()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE copies SET is_avaiable = 0 WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", this.Id);

      cmd.ExecuteNonQuery();
      this.IsAvailable = false;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }  
    }

    public static Copy Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      int newBookId= 0;
      bool IsAvailable = false;


      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        newBookId= rdr.GetInt32(1);
        IsAvailable = rdr.GetBoolean(2);
      }

      Copy newCopy = new Copy(newBookId, IsAvailable, newId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return newCopy;
    }

  }
}
