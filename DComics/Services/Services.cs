using CG.Web.MegaApiClient;
using DComics.Models;
using HtmlAgilityPack;
using log4net;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScrapySharp.Extensions;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        #endregion

        #region Hook Method
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
                        RenameFile(comic, logger);
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
                comic.SizeWeb = FormatSize(infoMega.Size);
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

                //GetSize()
                string size = doc.DocumentNode.CssSelect(".dl-info").CssSelect(".details").ToList()[0].InnerText;
                size = size.Replace("File size: ", "º").Replace("MB", "º");
                int dif = size.LastIndexOf("º") - size.IndexOf("º") - 1;
                comic.SizeWeb = size.Substring(size.IndexOf("º") + 1, dif);
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.DownloadFile(linkDownload, Environment.CurrentDirectory + @"\Scripts\" + comic.NameWeb);
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
                return output.Equals("true\r\n");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        #endregion

        private void RenameFile(Comic comic, ILog logger)
        {
            try
            {        
                string destinyPath = Environment.CurrentDirectory + @"\Download\" + comic.Name + ".cbr";
                FileInfo fileDownloadInfo = new FileInfo(Environment.CurrentDirectory + @"\Scripts\" + comic.NameWeb);
                if (fileDownloadInfo.Exists)
                {
                    string sizeWeb = string.Format("{0:n1}", float.Parse(comic.SizeWeb.Replace(".", ",")));
                    string sizeDisk = string.Format("{0:n1}", float.Parse(FormatSize(fileDownloadInfo.Length)));
                    if (sizeWeb.Equals(sizeDisk))
                    {
                        //zip y cbz
                        if (fileDownloadInfo.Extension.Equals(".cbr")) //Download file cbr
                        {
                            File.Move(fileDownloadInfo.FullName, destinyPath);
                        }
                        else if (fileDownloadInfo.Extension.Equals(".cbz")) //Download file cbz
                        {
                            File.Move(fileDownloadInfo.FullName, destinyPath);
                        }
                        else if (fileDownloadInfo.Extension.Equals(".rar")) //Download file rar
                        {
                            extractFile(fileDownloadInfo.FullName);//Extract file or directory
                            //Get InfoFile
                            FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\").GetFiles().OrderByDescending(f => f.CreationTime).First();
                            long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                            if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file
                            {
                                File.Move(fileDescom.FullName, destinyPath);
                                File.Delete(fileDescom.FullName);
                            }
                            else //If extract file is a directory
                            {
                                DirectoryInfo directoryDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\").GetDirectories().OrderByDescending(d => d.CreationTime).First();
                                long diffTimeDir = DateAndTime.DateDiff(DateInterval.Second, directoryDescom.CreationTime, DateTime.Now);
                                if (diffTimeDir <= 60) //If extract file is a file
                                {
                                    ZipFile.CreateFromDirectory(directoryDescom.FullName, destinyPath);
                                    Directory.Delete(directoryDescom.FullName, true);
                                }
                            }
                        }
                        else //Download file unknown
                        {
                            File.Move(fileDownloadInfo.FullName, destinyPath);
                        }
                        File.Delete(fileDownloadInfo.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        private static void extractFile(string Path)
        {
            using (ArchiveFile archiveFile = new ArchiveFile(Path))
            {
                archiveFile.Extract(Environment.CurrentDirectory + @"\Scripts\");
            }
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







        private static string FormatSize(long bytes)
        {
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
                number = number / 1024;
            return string.Format("{0:n2}", number);
        }

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
