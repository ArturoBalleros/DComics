using Add2Database.Models;
using Add2Database.Services;
using Add2Database.Utils;
using System.Collections.Generic;

namespace Add2Database
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.LoadConfig();

            FileService fileService = new FileService();
            List<Editorial> editorials = fileService.ReadEditorials();
            foreach (Editorial e in editorials)
                fileService.ReadCollections(e.Collections);

            AppendService appendService = new AppendService();
            appendService.Append2Database(editorials);
        }
    }
}
