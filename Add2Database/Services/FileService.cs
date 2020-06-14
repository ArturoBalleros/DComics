using Add2Database.DAO;
using Add2Database.Models;
using Add2Database.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Add2Database.Services
{
    internal sealed class FileService
    {
        public List<Editorial> ReadEditorials()
        {
            List<Editorial> editorials = new List<Editorial>();
            ModelsDao modelsDAO = new ModelsDao();
            FileInfo[] files = new DirectoryInfo(Config.EditorialPaths).GetFiles();

            foreach (FileInfo fileEditorial in files)
            {
                Editorial e = new Editorial(fileEditorial.Name.Replace(".txt", string.Empty));
                e.Collections = GetCollections(e.Id, fileEditorial, modelsDAO);
                editorials.Add(e);
            }

            return editorials;
        }
        private List<Collection> GetCollections(string editorialId, FileInfo file, ModelsDao modelsDao)
        {
            string line;
            List<Collection> collections = new List<Collection>();
            using StreamReader streamReader = new StreamReader(file.FullName);
            while ((line = streamReader.ReadLine()) != null)
                collections.Add(new Collection(line));

            return collections;
        }
        public void ReadCollections(List<Collection> collections)
        {
            ModelsDao modelsDAO = new ModelsDao();
            FileInfo[] files = new DirectoryInfo(Config.CollectionsPaths).GetFiles();

            foreach (FileInfo fileCollection in files)
            {
                Collection c = collections.FirstOrDefault(x => x.Name == fileCollection.Name.Replace(".txt", string.Empty));
                if (c != null)
                    GetComics(c, fileCollection, modelsDAO);
            }
        }
        private void GetComics(Collection collection, FileInfo file, ModelsDao modelsDao)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string line;
            using StreamReader streamReader = new StreamReader(file.FullName);

            while ((line = streamReader.ReadLine()) != null)
                stringBuilder.Append(line);

            try
            {
                foreach (JToken x1 in (JArray)JsonConvert.DeserializeObject(stringBuilder.ToString()))
                {
                    Comic c = JsonConvert.DeserializeObject<Comic>(x1.ToString());
                    collection.Comics.Add(c);
                }
            }
            catch { }
        }
    }
}
