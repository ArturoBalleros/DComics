﻿using CG.Web.MegaApiClient;
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
        private const string Url = "http://www.comicsid.com/inicio";
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
                    logger.Debug(string.Format("JSON: {0}", Comic.Serializer(new List<Comic> { comic })));
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
        private bool ExecuteBatch(string link, ILog logger)
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
                if (!new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\").Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Scripts\");
                if (!new FileInfo(Environment.CurrentDirectory + @"\Scripts\Download.bat").Exists)
                    return false;
                if (!new FileInfo(Environment.CurrentDirectory + @"\Scripts\DownloadMega.py").Exists)
                    return false;
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
                FileInfo fileDownloadInfo = new FileInfo(Environment.CurrentDirectory + @"\Scripts\" + comic.NameWeb);
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
                if (fileDownloadInfo.Extension.Equals(".rar")) //rar
                {
                    using ArchiveFile archiveFile = new ArchiveFile(fileDownloadInfo.FullName);
                    archiveFile.Extract(Environment.CurrentDirectory + @"\Scripts\Comic\");
                }
                else //zip      
                    ZipFile.ExtractToDirectory(fileDownloadInfo.FullName, Environment.CurrentDirectory + @"\Scripts\Comic\");

                int countFiles = new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\Comic\").GetFiles().Count();
                if (countFiles == 0) //folder
                {
                    DirectoryInfo directoryDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\Comic\").GetDirectories().OrderByDescending(d => d.CreationTime).First();
                    long diffTimeDir = DateAndTime.DateDiff(DateInterval.Second, directoryDescom.CreationTime, DateTime.Now);
                    if (diffTimeDir <= 60) //If extract file is a file
                    {
                        ZipFile.CreateFromDirectory(directoryDescom.FullName, destinyPath + ".cbr");
                        Directory.Delete(directoryDescom.FullName, true);
                    }
                }

                if (countFiles == 1) //file 
                {
                    FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\Comic\").GetFiles().OrderByDescending(f => f.CreationTime).First();
                    long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                    if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file
                    {
                        File.Move(fileDescom.FullName, destinyPath + fileDescom.Extension);
                        File.Delete(fileDescom.FullName);
                    }
                }

                if (countFiles > 1) //collection photo
                {
                    FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + @"\Scripts\Comic\").GetFiles().OrderByDescending(f => f.CreationTime).First();
                    long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                    if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file                    
                        ZipFile.CreateFromDirectory(Environment.CurrentDirectory + @"\Scripts\Comic\", destinyPath + ".cbr");

                }

                Directory.Delete(Environment.CurrentDirectory + @"\Scripts\Comic\", true);
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