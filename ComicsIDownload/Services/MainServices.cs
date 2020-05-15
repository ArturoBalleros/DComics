using CG.Web.MegaApiClient;
using ComicsIDownload.Services;
using ComicsIDownload.Utils;
using DComics.Models;
using HtmlAgilityPack;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DComics
{
    internal sealed class MainServices
    {
        private const string Url = "http://www.comicsid.com/inicio";
        private readonly MegaApiClient ApiMega;
        private readonly ILog logger;

        public MainServices(MegaApiClient mega)
        {
            ApiMega = mega;
            logger = initLogger();
        }

        #region Main Services
        public void DownloadNews()
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> news = new List<Comic>();
                ReportService reportService = new ReportService();
                string nameLastDonwload = reportService.ReadLastDownload(), name, link, newNameLastDonwload = null;
                int countNovedades = 1;

                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(Url);

                foreach (var Nodo in doc.DocumentNode.CssSelect(".listadoComics").CssSelect(".mdl-list__item"))
                {
                    //Get Data
                    name = Nodo.CssSelect(".mdl-list__item-primary-content").CssSelect("span").FirstOrDefault().InnerText;
                    link = Nodo.CssSelect(".btnMaterialGlobal").CssSelect("a").First().Attributes[0].Value;
                    if (countNovedades == 1) newNameLastDonwload = name;
                    if (!string.IsNullOrEmpty(nameLastDonwload) && nameLastDonwload.Equals(name)) break; else news.Add(new Comic(countNovedades, name, link));
                    if (string.IsNullOrEmpty(nameLastDonwload) && countNovedades == 5) break;
                    countNovedades++;
                }

                string path = string.Format("{0:dd-MM-yyyy}.json", DateTime.Now);
                if (reportService.CreateFileReport(news, path))
                {
                    reportService.CreateFileReport(newNameLastDonwload, "LastDownload.txt");
                    ReadFile(path);
                }

            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void ReadFile(string path)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                using (StreamReader jsonStream = File.OpenText(path))
                {
                    string line;
                    while ((line = jsonStream.ReadLine()) != null)
                        ProcessJSON((JArray)JsonConvert.DeserializeObject(line));
                }
                logger.Info(string.Format("Fin del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void RenameFiles(string path)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> listComic = new List<Comic>();
                List<Comic> listNoRename = new List<Comic>();
                ReportService reportService = new ReportService();
                FileService fileService = new FileService();
                using (StreamReader jsonStream = File.OpenText(path))
                {
                    string line;
                    while ((line = jsonStream.ReadLine()) != null)
                        foreach (var x in (JArray)JsonConvert.DeserializeObject(line))
                            listComic.Add(JsonConvert.DeserializeObject<Comic>(x.ToString()));
                }

                foreach (Comic c in listComic)
                    if (!fileService.RenameFile(c))
                        listNoRename.Add(c);

                reportService.CreateFileReport(listNoRename, "NoRename.json");
                logger.Info(string.Format("Fin del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void ListCollections(string option = Constantes.Name)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                ReportService reportService = new ReportService();
                List<string> listCollections = new List<string>();

                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(@"http://www.comicsid.com/categorias/dc");

                if (option.Equals(Constantes.Name))
                {
                    foreach (var Nodo in doc.DocumentNode.CssSelect(".serieComic"))
                        listCollections.Add(Nodo.ChildNodes.Where(x => x.Name.Equals("br")).Select(y => y.NextSibling).Select(z => z.InnerText.Trim()).FirstOrDefault());
                    reportService.CreateFileReport(listCollections, "ListCollections.txt");
                }

                if (option.Equals(Constantes.File))
                {
                    var listNodos = doc.DocumentNode.CssSelect(".serieComic").CssSelect(".urlSerie").Select(n => n.ChildNodes.Where(cn => cn.Name.Equals("div"))).Select(n => n.Select(la => la.Attributes.Where(a => a.Name.Equals("data-valor")))).ToList();
                    foreach (var Nodo in listNodos)
                        ReadCollection(@"http://www.comicsid.com/serie/" + Nodo.FirstOrDefault().FirstOrDefault().Value + "-serie");
                }

                logger.Info(string.Format("Fin del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void TreeDirectory(DirectoryInfo root, List<string> listFiles)
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
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    listFiles.Add(fi.Name.Replace(fi.Extension, string.Empty));

                // Now find all the subdirectories under this directory.
                DirectoryInfo[] subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                    // Resursive call for each subdirectory.
                    TreeDirectory(dirInfo, listFiles);
            }
        }
        public void ReadCollection(string url)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                ReportService reportService = new ReportService();
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
                reportService.CreateFileReport(collection.OrderBy(x => x.Id).ToList(), nameCollection.Replace("/", "&").Replace(":", " -") + Constantes.TXT, Constantes.ReportCollections, true);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        public void ReviewNoDownload(string fileName)
        {
            try
            {
                logger.Info(string.Format("Inicio del servicio: '{0}'", MethodBase.GetCurrentMethod().Name));
                List<Comic> listComicsDia = new List<Comic>();
                List<Comic> listComicsDiaNo = new List<Comic>();
                ReportService reportService = new ReportService();
                using (StreamReader jsonStream = File.OpenText(fileName))
                {
                    string line;
                    while ((line = jsonStream.ReadLine()) != null)
                        foreach (var json in (JArray)JsonConvert.DeserializeObject(line))
                            listComicsDia.Add(JsonConvert.DeserializeObject<Comic>(json.ToString()));
                }

                foreach (Comic c in listComicsDia)
                {
                    bool Cbr = new FileInfo(string.Format(@"I:\DComics\ComicsIDownload\bin\Debug\netcoreapp3.0\Download\{0}.cbr", c.Name.Replace(Constantes.BarraLateral, Constantes.Ampersand))).Exists;
                    bool Cbz = new FileInfo(string.Format(@"I:\DComics\ComicsIDownload\bin\Debug\netcoreapp3.0\Download\{0}.cbz", c.Name.Replace(Constantes.BarraLateral, Constantes.Ampersand))).Exists;
                    if (!Cbr && !Cbz)
                        listComicsDiaNo.Add(c);
                }
                reportService.CreateFileReport(listComicsDiaNo, "NoDownload.json");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion

        #region Hook Method
        private void ProcessJSON(JArray array)
        {
            try
            {
                List<Comic> collectionNoDownload = new List<Comic>();
                List<Comic> collectionNoRename = new List<Comic>();
                DownloadService downloadService = new DownloadService(ApiMega);
                ReportService reportService = new ReportService();
                FileService fileService = new FileService();
                int cont = 1;
                foreach (JObject c in array.OfType<JObject>())
                {
                    //Convert Object
                    Comic comic = new Comic(cont++, c.GetValue("name").ToString().Replace(Constantes.BarraLateral, Constantes.Ampersand), c.GetValue("link").ToString());
                    Console.WriteLine(comic.Name);
                    logger.Warn(string.Format("Inicio: {0}", comic.ToString()));
                    logger.Info(string.Format("JSON: {0}", Comic.Serializer(new List<Comic> { comic }, false)));

                    //Rename File
                    if (downloadService.DownloadFile(comic))
                    {
                        if (!fileService.RenameFile(comic)) 
                            collectionNoRename.Add(comic);
                    }
                    else
                        collectionNoDownload.Add(comic);

                    logger.Warn(string.Format("Fin: {0}", comic.ToString()));
                }

                if (collectionNoDownload.Any()) reportService.CreateFileReport(collectionNoDownload, "NoDownload.json");
                if (collectionNoRename.Any()) reportService.CreateFileReport(collectionNoRename, "NoRename.json");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        #endregion

        private ILog initLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(MainServices));
        }
    }
}