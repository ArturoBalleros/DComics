using Add2Database.DAO;
using Add2Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Add2Database.Services
{
    internal sealed class AppendService
    {
        public void Append2Database(List<Editorial> editorials)
        {
            List<string> col = new List<string>();
            ModelsDao modelsDAO = new ModelsDao();
            foreach (Editorial e in editorials)
            {
                Console.WriteLine(e.Name);
                modelsDAO.AddEditorial(e);
                foreach (Collection c in e.Collections)
                {
                    Console.WriteLine("\t" + c.Name);
                    modelsDAO.AddCollection(c, e.Id);

                    if (!c.Comics.Any()) col.Add(e.Id + " - " + c.Id + " - " +c.Name);

                    foreach (Comic co in c.Comics)
                    {
                        Console.WriteLine("\t\t" + co.Name);
                        modelsDAO.AddComic(co, c.Id);
                    }
                }
            }
            foreach (string s in col) Console.WriteLine(s);
        }
    }
}
