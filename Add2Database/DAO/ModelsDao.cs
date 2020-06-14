using Add2Database.Models;
using Add2Database.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Add2Database.DAO
{
    internal sealed class ModelsDao
    {
        public string GetNewCode()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(Config.ConnectionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("GetNewCode", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("@result", MySqlDbType.VarChar, 8));
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                string return_value = cmd.Parameters["@result"].Value.ToString();

                return !string.IsNullOrEmpty(return_value) ? return_value : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void AddEditorial(Editorial e)
        {
            e.Name = e.Name.Replace("'", "\\\'");

            using MySqlConnection conn = new MySqlConnection(Config.ConnectionString);
            string query = string.Format("select Id from editorials where Name like '%{0}%'", e.Name);

            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            if (string.IsNullOrWhiteSpace((string)cmd.ExecuteScalar()))
            {               
                e.Id = GetNewCode();
                query = string.Format("Insert Into Editorials(Id, Name) Values ('{0}', '{1}')", e.Id, e.Name);
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        public void AddCollection(Collection c, string editorialId)
        {
            c.Name = c.Name.Replace("'", "\\\'");

            using MySqlConnection conn = new MySqlConnection(Config.ConnectionString);
            string query = string.Format("select Id from collections where Name like '%{0}%'", c.Name);

            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            if (string.IsNullOrWhiteSpace((string)cmd.ExecuteScalar()))
            {
                c.Id = GetNewCode();
                query = string.Format("Insert Into collections(Id, Name, EditorialId) Values ('{0}', '{1}', '{2}')", c.Id, c.Name, editorialId);
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        public void AddComic(Comic c, string collectionId)
        {
            c.Name = c.Name.Replace("'", "\\\'");
            c.Link = c.Link.Replace("'", "\\\'");

            using MySqlConnection conn = new MySqlConnection(Config.ConnectionString);
            string query = string.Format("select Id from comics where Name like '%{0}%'", c.Name);

            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            if (string.IsNullOrWhiteSpace((string)cmd.ExecuteScalar()))
            {
                c.Id = GetNewCode();
                query = string.Format("Insert Into comics(Id, Name, CollectionId, Link) Values ('{0}', '{1}', '{2}', '{3}')", c.Id, c.Name, collectionId, c.Link);
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
