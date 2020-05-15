using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System;
using CG.Web.MegaApiClient;
using System.Collections.Generic;
using ComicsIDownload.Services;
using DComics.Models;

namespace DComics
{
    static class Program
    {
        static ILog logger;
        public static void Main(string[] args)
        {                
            try
            {
                MegaApiClient mega = new MegaApiClient();
                FileService fileService = new FileService();
                MainServices services = new MainServices(mega);
                
                int option = 0;
                string infoAdditional = string.Empty;

                mega.LoginAnonymous();
                logger = initLogger();
                logger.Info("Inicio del proceso");

                if (!fileService.CheckDirectories())
                {
                    logger.Info("Fin del proceso por falta de carpetas");
                    return;
                }

                //args = new string[] { "1", @"json.txt" };
                //args = new string[] { "0", @"json.txt" };
                if (args != null)
                {
                    option = !string.IsNullOrEmpty(args[0]) ? int.Parse(args[0]) : 0;
                    infoAdditional = !string.IsNullOrEmpty(args[1]) ? args[1] : string.Empty;
                }
                switch (option)
                {
                    case 0: //Bajar últimos
                        services.DownloadNews();
                        break;

                    case 1: //Fichero
                        services.ReadFile(infoAdditional);
                        break;

                    case 2: //Renombrar
                        services.RenameFiles(infoAdditional);
                        break;

                    case 3: //Lista Colecciones
                        services.ListCollections();
                        break;

                    case 4: //Arbol de directorios
                        if (!string.IsNullOrEmpty(infoAdditional))
                        {
                            DirectoryInfo rootDir = new DirectoryInfo(infoAdditional);
                            services.TreeDirectory(rootDir, new List<string>());
                        }
                        break;

                    case 5: //Ficheros pos lista con todos los nombres y links
                        if (!string.IsNullOrEmpty(infoAdditional))
                            services.ListCollections(infoAdditional);
                        break;

                    case 6: //Ficheros por lista con todos los nombres y links
                        if (!string.IsNullOrEmpty(infoAdditional))
                            services.ReadCollection(infoAdditional);
                        break;

                    case 7: //Segunda revision de los descargados
                        services.ReviewNoDownload(infoAdditional);
                        break;

                    default:
                        return;
                }
                logger.Info("Fin del proceso");
                mega.Logout();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        private static ILog initLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(Program));
        }
    }
}
