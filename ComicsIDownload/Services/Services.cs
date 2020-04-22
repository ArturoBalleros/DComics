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
        int count = 0;
        private const string Url = "http://www.comicsid.com/inicio";
        private readonly MegaApiClient ApiMega;

        public Services(MegaApiClient mega) {
            ApiMega = mega;
        }
        #region Main Services
        public void DownloadNews(ILog logger)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> news = new List<Comic>();
                string nameLastDonwload = ReadLastDownload(logger), name, link, newNameLastDonwload = null;
                int countNovedades = 1;

                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(Url);

                foreach (var Nodo in doc.DocumentNode.CssSelect(".listadoComics").CssSelect(".mdl-list__item"))
                {
                    //Get Data
                    name = Nodo.CssSelect(".mdl-list__item-primary-content").CssSelect("span").FirstOrDefault().InnerText;
                    link = Nodo.CssSelect(".btnMaterialGlobal").CssSelect("a").First().Attributes[0].Value;
                    if (countNovedades == 1) newNameLastDonwload = name;
                    if (!string.IsNullOrEmpty(nameLastDonwload) && nameLastDonwload.Equals(name))
                        break;
                    else
                        news.Add(new Comic(countNovedades, name, link));
                    if (string.IsNullOrEmpty(nameLastDonwload) && countNovedades == 5)
                        break;
                    countNovedades++;
                }

                string path = CreateFileReportNews(news, logger);
                if (!string.IsNullOrEmpty(path))
                {
                    CreateFileLastDownload(newNameLastDonwload, logger);
                    ReadFile(path, logger);
                }

            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void ReadFile(string path, ILog logger)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                using (StreamReader jsonStream = File.OpenText(path))
                {
                    string line;
                    while ((line = jsonStream.ReadLine()) != null)
                        ProcessJSON((JArray)JsonConvert.DeserializeObject(line), logger);
                }
                logger.Info(string.Format("Fin del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void RenameFiles(string path, ILog logger)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> listComic = new List<Comic>();
                List<Comic> listNoRename = new List<Comic>();
                using (StreamReader jsonStream = File.OpenText(path))
                {
                    string line;
                    while ((line = jsonStream.ReadLine()) != null)
                        foreach (var x in (JArray)JsonConvert.DeserializeObject(line))
                            listComic.Add(JsonConvert.DeserializeObject<Comic>(x.ToString()));
                }
                foreach (Comic c in listComic)
                    if (!RenameFile(c, logger))
                        listNoRename.Add(c);
                CreateFileReportNoRename(listNoRename, logger);
                logger.Info(string.Format("Fin del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void ListCollections(ILog logger, string option = "Name")
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<string> listCollections = new List<string>();

                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(@"http://www.comicsid.com/categorias/dc");

                if (option.Equals("Name"))
                {
                    foreach (var Nodo in doc.DocumentNode.CssSelect(".serieComic"))
                        listCollections.Add(Nodo.ChildNodes.Where(x => x.Name.Equals("br")).Select(y => y.NextSibling).Select(z => z.InnerText.Trim()).FirstOrDefault());
                    CreateFileReportCollections(listCollections, logger);
                }

                if (option.Equals("File"))
                {
                    var listNodos = doc.DocumentNode.CssSelect(".serieComic").CssSelect(".urlSerie").Select(n => n.ChildNodes.Where(cn => cn.Name.Equals("div"))).Select(n => n.Select(la => la.Attributes.Where(a => a.Name.Equals("data-valor")))).ToList();
                    foreach (var Nodo in listNodos)
                        ReadCollection(@"http://www.comicsid.com/serie/" + Nodo.FirstOrDefault().FirstOrDefault().Value + "-serie", logger);
                }

                logger.Info(string.Format("Fin del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void TreeDirectory(DirectoryInfo root, ILog logger)
        {
            FileInfo[] files = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                logger.Warn(e.Message);
            }

            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    count++;
                    Console.WriteLine(fi.FullName + " " + count);
                }


                // Now find all the subdirectories under this directory.
                DirectoryInfo[] subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                    // Resursive call for each subdirectory.
                    TreeDirectory(dirInfo, logger);
            }
        }
        public void ReadCollection(string url, ILog logger)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> collection = new List<Comic>();
                string name, link;
                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(url);
                string nameCollection = doc.DocumentNode.CssSelect(".titul").Select(cn => cn.ChildNodes.Where(n => n.Name == "h3")).FirstOrDefault().Select(n => n.InnerText).FirstOrDefault().Replace(@"\n", string.Empty).TrimStart().Trim();
                int count = doc.DocumentNode.CssSelect(".listadoComics").CssSelect(".mdl-list__item").Count();
                foreach (var Nodo in doc.DocumentNode.CssSelect(".listadoComics").CssSelect(".mdl-list__item"))
                {
                    //Get Data     
                    name = Nodo.CssSelect(".mdl-list__item-primary-content").CssSelect("span").FirstOrDefault().InnerText;
                    link = Nodo.CssSelect(".btnMaterialGlobal").CssSelect("a").First().Attributes[0].Value;
                    collection.Add(new Comic(count--, name, link));
                }
                CreateFileReportCollection(collection.OrderBy(x => x.Id).ToList(), nameCollection, logger);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion

        #region Hook Method
        private void ProcessJSON(JArray array, ILog logger)
        {
            try
            {
                List<Comic> collectionNoDownload = new List<Comic>();
                List<Comic> collectionNoRename = new List<Comic>();
                int cont = 1; bool result = false;
                foreach (JObject c in array.OfType<JObject>())
                {
                    //Convert Object
                    Comic comic = new Comic(cont++, c.GetValue("name").ToString().Replace("/", "&"), c.GetValue("link").ToString());
                    Console.WriteLine(comic.Name); logger.Warn(string.Format("Inicio: {0}", comic.ToString()));

                    //Download File
                    if (comic.Link.Contains("mega.nz"))
                        result = ProcessDownloadMega(comic, logger);
                    else if (comic.Link.Contains("mediafire.com"))
                        result = ProcessDownloadMediaFire(comic, logger);
                    else
                        result = false;

                    //Rename File
                    if (result)
                    {
                        if (!RenameFile(comic, logger))
                            collectionNoRename.Add(comic);
                    }
                    else
                        collectionNoDownload.Add(comic);
                    logger.Debug(string.Format("JSON: {0}", Comic.Serializer(new List<Comic> { comic },false)));
                    logger.Warn(string.Format("Fin: {0}", comic.ToString()));
                }
                if (collectionNoDownload.Count > 0)
                    CreateFileReportNoDownload(collectionNoDownload, logger);
                if (collectionNoRename.Count > 0)
                    CreateFileReportNoRename(collectionNoDownload, logger);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion

        #region Download Comics
        private bool ProcessDownloadMega(Comic comic, ILog logger)
        {
            try
            {
                
            
                Uri uri = new Uri(comic.Link);
                INodeInfo infoMega = ApiMega.GetNodeFromLink(uri);
                ApiMega.DownloadFile(uri, Environment.CurrentDirectory + @"\Download\" + infoMega.Name);               
                comic.NameWeb = infoMega.Name;
                comic.SizeWeb = FormatSize(infoMega.Size);
       
                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private bool ProcessDownloadMediaFire(Comic comic, ILog logger)
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
                wc.DownloadFile(linkDownload, Environment.CurrentDirectory + @"\Download\" + comic.NameWeb);
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
        #endregion

        #region Files and Directories
        public bool CheckDirectoriesAndFiles(ILog logger)
        {
            try
            {
                if (!new DirectoryInfo(Environment.CurrentDirectory + @"\Log\").Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Log\");
                if (!new DirectoryInfo(Environment.CurrentDirectory + @"\Download\").Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Download\");
                if (!new DirectoryInfo(Environment.CurrentDirectory + @"\Report\").Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Report\");
                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private bool RenameFile(Comic comic, ILog logger)
        {
            try
            {
                bool result = true;
                string destinyPath = Environment.CurrentDirectory + @"\Download\" + comic.Name;
                FileInfo fileDownloadInfo = new FileInfo(Environment.CurrentDirectory + @"\Download\" + comic.NameWeb);
                if (destinyPath.Equals(fileDownloadInfo.FullName))
                    return true;
                if (fileDownloadInfo.Exists)
                {
                    string sizeWeb = string.Format("{0:n1}", float.Parse(comic.SizeWeb.Replace(".", ",")));
                    string sizeDisk = string.Format("{0:n1}", float.Parse(FormatSize(fileDownloadInfo.Length)));
                    if (sizeWeb.Equals(sizeDisk))
                    {
                        if (fileDownloadInfo.Extension.Equals(".cbr")) //Download file cbr
                            File.Move(fileDownloadInfo.FullName, destinyPath + ".cbr");
                        else if (fileDownloadInfo.Extension.Equals(".cbz")) //Download file cbz
                            File.Move(fileDownloadInfo.FullName, destinyPath + ".cbz");
                        else if (fileDownloadInfo.Extension.Equals(".rar") || fileDownloadInfo.Extension.Equals(".zip")) //Download file rar
                            result = ExtractFile(fileDownloadInfo, destinyPath, logger);//Extract file or directory
                        else //Download file unknown
                            File.Move(fileDownloadInfo.FullName, destinyPath + ".cbr");
                        if (result)
                            File.Delete(fileDownloadInfo.FullName);
                    }
                }
                else
                    return false;
                return result;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private bool ExtractFile(FileInfo fileDownloadInfo, string destinyPath, ILog logger)
        {
            try
            {
                //Extract compress file
                if (fileDownloadInfo.Extension.Equals(".rar")) //rar
                {
                    using ArchiveFile archiveFile = new ArchiveFile(fileDownloadInfo.FullName);
                    archiveFile.Extract(Environment.CurrentDirectory + @"\Download\Comic\");
                }
                else //zip      
                    ZipFile.ExtractToDirectory(fileDownloadInfo.FullName, Environment.CurrentDirectory + @"\Download\Comic\");

                //Create/Rename extract file(s)
                int countFiles = new DirectoryInfo(Environment.CurrentDirectory + @"\Download\Comic\").GetFiles().Count();
                if (countFiles == 0) //folder
                {
                    DirectoryInfo directoryDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Download\Comic\").GetDirectories().OrderByDescending(d => d.CreationTime).First();
                    long diffTimeDir = DateAndTime.DateDiff(DateInterval.Second, directoryDescom.CreationTime, DateTime.Now);
                    if (diffTimeDir <= 60) //If extract file is a file
                    {
                        ZipFile.CreateFromDirectory(directoryDescom.FullName, destinyPath + ".cbr");
                        Directory.Delete(directoryDescom.FullName, true);
                    }
                }

                if (countFiles == 1) //file 
                {
                    FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Download\Comic\").GetFiles().OrderByDescending(f => f.CreationTime).First();
                    long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                    if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file
                    {
                        File.Move(fileDescom.FullName, destinyPath + fileDescom.Extension);
                        File.Delete(fileDescom.FullName);
                    }
                }

                if (countFiles > 1) //collection photo
                {
                    FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Download\Comic\").GetFiles().OrderByDescending(f => f.CreationTime).First();
                    long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                    if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file                    
                        ZipFile.CreateFromDirectory(Environment.CurrentDirectory + @"\Download\Comic\", destinyPath + ".cbr");

                }

                Directory.Delete(Environment.CurrentDirectory + @"\Download\Comic\", true);
                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        #endregion

        #region Create Report
        private void CreateFileReportNoDownload(List<Comic> comics, ILog logger)
        {
            try
            {
                string docPath = Environment.CurrentDirectory + @"\Report\NoDownload.json";
                using StreamWriter outputFile = File.AppendText(docPath);
                outputFile.WriteLine(Comic.Serializer(comics) + "\n");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private void CreateFileReportNoRename(List<Comic> comics, ILog logger)
        {
            try
            {
                string docPath = Environment.CurrentDirectory + @"\Report\NoRename.json";
                using StreamWriter outputFile = File.AppendText(docPath);
                outputFile.WriteLine(Comic.Serializer(comics) + "\n");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private string CreateFileReportNews(List<Comic> comics, ILog logger)
        {
            try
            {
                string docPath = Environment.CurrentDirectory + string.Format(@"\Report\{0:dd-MM-yyyy}.json", DateTime.Now);
                using StreamWriter outputFile = File.AppendText(docPath);
                outputFile.WriteLine(Comic.Serializer(comics, false));
                return docPath;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return null;
            }
        }
        private void CreateFileLastDownload(string titleComics, ILog logger)
        {
            try
            {
                string docPath = Environment.CurrentDirectory + @"\Report\LastDownload.txt";
                using StreamWriter outputFile = new StreamWriter(docPath);
                outputFile.WriteLine(titleComics);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private void CreateFileReportCollections(List<string> list, ILog logger)
        {
            try
            {
                string docPath = Environment.CurrentDirectory + @"\Report\ListCollections.txt";
                using StreamWriter outputFile = new StreamWriter(docPath);
                foreach (string name in list)
                    outputFile.WriteLine(name);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private string ReadLastDownload(ILog logger)
        {
            try
            {
                string docPath = Environment.CurrentDirectory + @"\Report\LastDownload.txt";
                if (!new FileInfo(docPath).Exists) return null;
                using StreamReader file = new StreamReader(docPath);
                return file.ReadLine();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return null;
            }
        }
        private void CreateFileReportCollection(List<Comic> comics, string title, ILog logger)
        {
            try
            {
                if (!new DirectoryInfo(Environment.CurrentDirectory + @"\Report\").Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Report\");
                if (!new DirectoryInfo(Environment.CurrentDirectory + @"\Report\Collections").Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Report\Collections");
                string docPath = Environment.CurrentDirectory + @"\Report\Collections\" + title.Replace("/", "&").Replace(":", " -") + ".txt";
                using StreamWriter outputFile = File.AppendText(docPath);
                outputFile.WriteLine(Comic.Serializer(comics));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion

        #region Utils
        private string FormatSize(long bytes)
        {
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
                number /= 1024;
            return string.Format("{0:n2}", number);
        }
        #endregion
    }
}