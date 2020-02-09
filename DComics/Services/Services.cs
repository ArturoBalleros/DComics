using CG.Web.MegaApiClient;
using DComics.Models;
using HtmlAgilityPack;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DComics
{
    public class Services
    {
        #region Main Services
        public void ReadFile(string path, ILog logger)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                using (StreamReader jsonStream = File.OpenText(path))
                {
                    string line;
                    while ((line = jsonStream.ReadLine()) != null)
                    {
                        ProcessJSON((JArray)JsonConvert.DeserializeObject(line), logger);
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void ProcessJSON(JArray array, ILog logger)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> collection = new List<Comic>();
                int cont = 1; bool result = false;
                foreach (JObject c in array.OfType<JObject>())
                {
                    Comic comic = new Comic(cont++, c.GetValue("name").ToString(), c.GetValue("link").ToString());
                    if (comic.Link.Contains("mega.nz"))
                        result = ProcessDownloadMega(comic, logger);
                    else if (comic.Link.Contains("mediafire.com"))
                        result = ProcessDownloadMediaFire(comic, logger);
                    else
                        result = false;

                    if (result)
                        cont++; //Renombrar
                    else
                        collection.Add(comic);
                }
                CreateFileReportNoDownload(collection, logger);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion

        #region Download Comics
        private static bool ProcessDownloadMega(Comic comic, ILog logger)
        {
            try
            {
                MegaApiClient mega = new MegaApiClient();
                mega.LoginAnonymous();
                Uri uri = new Uri(comic.Link);
                INodeInfo infoMega = mega.GetNodeFromLink(uri);
                comic.NameWeb = infoMega.Name;
                return ExecuteBatch(comic.Link, logger);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private static bool ProcessDownloadMediaFire(Comic comic, ILog logger)
        {
            try
            {
                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(comic.Link);
                comic.NameWeb = doc.DocumentNode.CssSelect(".dl-info").CssSelect(".filename").ToList()[0].InnerText;
                string linkDownload = doc.DocumentNode.CssSelect(".download_link").CssSelect(".input").First().Attributes[2].Value;
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.DownloadFile(linkDownload, comic.NameWeb);
                wc.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private static bool ExecuteBatch(string link, ILog logger)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = Directory.GetCurrentDirectory() + @"\Scripts\Download.bat";
                p.StartInfo.Arguments = link;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                return output.Equals("true");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        #endregion

        private void RenameFile(Comic comic, ILog logger) {

            string fileName = @"C:\Temp\MaheshTXFI.txt";
            FileInfo fi = new FileInfo(fileName);
          //  fi.Length;//Tamaño

        http://decodigo.com/c-sharp-renombrar-archivo
            //Mirar nombres extensiones etc......
            File.Move(comic.NameWeb, comic.Name);//Asi se renombra
        }

        #region Create Report
        //Mirar donde se crean los archivos
        private static void CreateFileReportNoDownload(List<Comic> comics, ILog logger)
        {
            try
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ComicsId";
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "a.txt")))
                {
                    foreach (Comic comic in comics)
                        outputFile.WriteLine(comic.ToString());
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private static void CreateFileLastDownload(string titleComics, ILog logger)
        {
            try
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ComicsId";
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "a.txt")))
                {
                    outputFile.WriteLine(titleComics);
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion
    }
}


/*

https://social.msdn.microsoft.com/Forums/es-ES/94cb6af5-9016-4248-a7b3-55eedbc63af8/descomprimir-archivos-rar-automaticamente?forum=vcses

quizas podrias evaluar

https://www.nuget.org/packages/dotnet-rar

https://www.nuget.org/packages/dotnet-unrar

o quizas este otro

https://www.nuget.org/packages/SevenZipExtractor/


*/