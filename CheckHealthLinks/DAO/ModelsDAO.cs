using CheckHealthLinks.Models;
using CheckHealthLinks.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CheckHealthLinks.DAO
{
    internal sealed class ModelsDao
    {
        public List<Comic> GetComics()
        {
            List<Comic> comics = new List<Comic>();
            DataTable dt = new DataTable();
            using MySqlConnection conn = new MySqlConnection(Config.ConnectionString);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("select Id, Link from comics", conn);
            conn.Open();
            mySqlDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
                foreach (DataRow row in dt.Rows) 
                    comics.Add(new Comic(row[0].ToString(), row[1].ToString()));
            return comics;
        }

        public void UpdateComic(Comic c)
        {
            using MySqlConnection conn = new MySqlConnection(Config.ConnectionString);
            string query = string.Format( "Update Comics set Status = '{0}' where Id = '{1}'", c.Status, c.Id);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
