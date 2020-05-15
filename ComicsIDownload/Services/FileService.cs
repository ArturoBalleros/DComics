using ComicsIDownload.Utils;
using DComics.Models;
using log4net;
using log4net.Config;
using Microsoft.VisualBasic;
using SevenZipExtractor;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace ComicsIDownload.Services
{
    internal sealed class FileService
    {
        private readonly ILog logger;

        public FileService()
        {
            logger = initLogger();
        }

        public bool CheckDirectories()
        {
            try
            {
                if (!new DirectoryInfo(Environment.CurrentDirectory + Constantes.Log).Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + Constantes.Log);
                if (!new DirectoryInfo(Environment.CurrentDirectory + Constantes.Download).Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + Constantes.Download);
                if (!new DirectoryInfo(Environment.CurrentDirectory + Constantes.Report).Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + Constantes.Report);
                if (!new DirectoryInfo(Environment.CurrentDirectory + Constantes.ReportCollections).Exists)
                    Directory.CreateDirectory(Environment.CurrentDirectory + Constantes.ReportCollections);
                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        public bool RenameFile(Comic comic)
        {
            try
            {
                bool result = true, sameName = IsSameName(comic);
                string destinyPath = Environment.CurrentDirectory + Constantes.Download + comic.Name;
                FileInfo fileDownloadInfo = new FileInfo(Environment.CurrentDirectory + Constantes.Download + comic.NameWeb);

                if (IsSameFile(fileDownloadInfo, destinyPath)) return true;

                if (fileDownloadInfo.Exists && HasSameSize(fileDownloadInfo, comic))
                {
                    switch (fileDownloadInfo.Extension)
                    {
                        case Constantes.CBR: //Download file cbr
                            File.Move(fileDownloadInfo.FullName, destinyPath + Constantes.CBR);
                            break;
                        case Constantes.CBZ: //Download file cbz
                            File.Move(fileDownloadInfo.FullName, destinyPath + Constantes.CBZ);
                            break;
                        case Constantes.RAR://Extract file or directory
                        case Constantes.ZIP:
                            result = ExtractFile(fileDownloadInfo, destinyPath);
                            break;
                        default: //Download file unknown
                            File.Move(fileDownloadInfo.FullName, destinyPath + Constantes.CBR);
                            break;
                    }

                    if (result && !sameName)
                        File.Delete(fileDownloadInfo.FullName);
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

        private bool ExtractFile(FileInfo fileDownloadInfo, string destinyPath)
        {
            try
            {
                //Extract compress file
                if (fileDownloadInfo.Extension.Equals(Constantes.RAR)) //rar
                {
                    using ArchiveFile archiveFile = new ArchiveFile(fileDownloadInfo.FullName);
                    archiveFile.Extract(Environment.CurrentDirectory + Constantes.DownloadComics);
                }
                else //zip      
                    ZipFile.ExtractToDirectory(fileDownloadInfo.FullName, Environment.CurrentDirectory + Constantes.DownloadComics);

                //Create/Rename extract file(s)
                int countFiles = new DirectoryInfo(Environment.CurrentDirectory + Constantes.DownloadComics).GetFiles().Count();
                if (countFiles == 0) //folder                
                    RenameFolderExtract(destinyPath);

                if (countFiles == 1) //file                 
                    RenameFileExtract(fileDownloadInfo, destinyPath);

                if (countFiles > 1) //collection photo                
                    RenameCollectionExtract(fileDownloadInfo, destinyPath);

                Directory.Delete(Environment.CurrentDirectory + Constantes.DownloadComics, true);
                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private void RenameFolderExtract(string destinyPath)
        {
            try
            {
                DirectoryInfo directoryDescom = new DirectoryInfo(Environment.CurrentDirectory + Constantes.DownloadComics).GetDirectories().OrderByDescending(d => d.CreationTime).First();
                long diffTimeDir = DateAndTime.DateDiff(DateInterval.Second, directoryDescom.CreationTime, DateTime.Now);
                if (diffTimeDir <= 60) //If extract file is a file
                {
                    ZipFile.CreateFromDirectory(directoryDescom.FullName, destinyPath + Constantes.CBR);
                    Directory.Delete(directoryDescom.FullName, true);
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private void RenameFileExtract(FileInfo fileDownloadInfo, string destinyPath)
        {
            try
            {
                FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + Constantes.DownloadComics).GetFiles().OrderByDescending(f => f.CreationTime).First();
                long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file
                {
                    File.Move(fileDescom.FullName, destinyPath + fileDescom.Extension);
                    File.Delete(fileDescom.FullName);
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private void RenameCollectionExtract(FileInfo fileDownloadInfo, string destinyPath)
        {
            try
            {
                FileInfo fileDescom = new DirectoryInfo(Environment.CurrentDirectory + Constantes.DownloadComics).GetFiles().OrderByDescending(f => f.CreationTime).First();
                long diffTimeFile = DateAndTime.DateDiff(DateInterval.Second, fileDescom.CreationTime, DateTime.Now);
                if (diffTimeFile <= 60 && !fileDescom.FullName.Equals(fileDownloadInfo.FullName)) //If extract file is a file                    
                    ZipFile.CreateFromDirectory(Environment.CurrentDirectory + Constantes.DownloadComics, destinyPath + Constantes.CBR);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private bool IsSameName(Comic comic)
        {
            string nameWithCbr = comic.Name.ToLower() + Constantes.CBR, nameWithCbz = comic.Name.ToLower() + Constantes.CBZ;
            if (nameWithCbr.Equals(comic.NameWeb.ToLower()) || nameWithCbz.Equals(comic.NameWeb.ToLower()))
                return true;
            else
                return false;
        }
        private bool IsSameFile(FileInfo fileDownloadInfo, string destinyPath)
        {
            if (fileDownloadInfo.FullName.Equals(destinyPath + Constantes.CBR) || fileDownloadInfo.FullName.Equals(destinyPath + Constantes.CBZ))
                return true;
            else
                return false;
        }
        private bool HasSameSize(FileInfo fileDownloadInfo, Comic comic)
        {
            string sizeWeb = string.Format("{0:n1}", float.Parse(comic.SizeWeb.Replace(".", ",")));
            string sizeDisk = string.Format("{0:n1}", float.Parse(Functions.FormatSize(fileDownloadInfo.Length)));
            if (sizeWeb.Equals(sizeDisk))
                return true;
            else
                return false;
        }

        private ILog initLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(FileService));
        }
    }
}
