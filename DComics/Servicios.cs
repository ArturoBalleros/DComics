using CG.Web.MegaApiClient;
using DComics.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DComics
{
    class Servicios
    {
        private void ReadFile(string path)
        {
            using (StreamReader jsonStream = File.OpenText(path))
            {
                string line;
                while ((line = jsonStream.ReadLine()) != null)
                {
                    ProcessJSON((JArray)JsonConvert.DeserializeObject(line));
                    Console.WriteLine(line);
                }
            }
        }

        private void ProcessJSON(JArray array)
        {
            List<Comic> collection = new List<Comic>();
            int cont = 1;
            foreach (JObject c in array.OfType<JObject>())
            {
                Comic comic = new Comic(cont++, c.GetValue("name").ToString(), c.GetValue("link").ToString());
                if (c.GetValue("link").ToString().Contains("mega.nz"))
                    ProcessDownloadMega(comic, collection);
                else
                    collection.Add(comic);

            }
            CreateFileReportNoDownload(collection);
        }

        private static void CreateFileReportNoDownload(List<Comic> comics)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ComicsId";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath,  "a.txt")))
            {
                foreach (Comic comic in comics)
                    outputFile.WriteLine(comic.ToString());
            }
        }

        private static void ProcessDownloadMega(Comic comic, List<Comic> collection ) {
            MegaApiClient mega = new MegaApiClient();
            mega.LoginAnonymous();
            Uri uri = new Uri(comic.Link);
            INodeInfo infoMega = mega.GetNodeFromLink(uri);
            comic.NameWeb = infoMega.Name;
            ExecuteBatch(comic.Link);
        }

        private static  bool ExecuteBatch(string link) {
           Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "YOURBATCHFILE.bat";
            p.StartInfo.Arguments = link;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output.Equals("true");
        }








        private static void CreateFileLastDownload(string titleComics)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ComicsId";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath,  "a.txt")))
            {
                    outputFile.WriteLine(titleComics);
            }
        }
    }
}
