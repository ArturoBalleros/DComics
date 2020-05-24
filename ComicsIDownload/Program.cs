using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System;
using CG.Web.MegaApiClient;
using System.Collections.Generic;
using ComicsIDownload.Services;
using ComicsIDownload.Utils;

namespace DComics
{
    static class Program
    {
        static ILog logger;

        public static void Main(string[] args)
        {
            //args = new string[] { "1", @"json.txt" };
            // args = new string[] { "0", @"23-05-2020.json" };
            try
            {
                FileService fileService = new FileService();

                int option = !string.IsNullOrEmpty(args[0]) ? int.Parse(args[0]) : 0;
                string infoAdditional = !string.IsNullOrEmpty(args[1]) ? args[1] : string.Empty;
                string infoAdditional2 = !string.IsNullOrEmpty(args[2]) ? args[2] : string.Empty;

                logger = initLogger();
                logger.Info("Inicio del proceso");

                if (!fileService.CheckDirectories())
                {
                    logger.Info("Fin del proceso por falta de carpetas");
                    return;
                }

                ExecuteService(option, infoAdditional, infoAdditional2);

                logger.Info("Fin del proceso");
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }
        private static void ExecuteService(int option, string infoAdditional, string infoAdditional2)
        {
            try
            {
                MegaApiClient mega = new MegaApiClient();
                mega.LoginAnonymous();
                MainServices services = new MainServices(mega);
                switch (option)
                {
                    case 0: //Bajar últimos
                        services.DownloadNews();
                        break;

                    case 1: //Descarga Fichero
                        if (!string.IsNullOrEmpty(infoAdditional)) //Path
                            services.DownloadFile(infoAdditional);
                        break;

                    case 2: //Renombra comics en la carpeta Download
                        if (!string.IsNullOrEmpty(infoAdditional)) //Path
                            services.RenameFiles(infoAdditional);
                        break;

                    case 3: //Lista Colecciones, nombres de colleciones
                        services.ListCollections();
                        break;

                    case 4: //Arbol de directorios
                        if (!string.IsNullOrEmpty(infoAdditional)) //Path
                        {
                            DirectoryInfo rootDir = new DirectoryInfo(infoAdditional);
                            services.TreeDirectory(rootDir, new List<string>());
                        }
                        break;

                    case 5: //Fichero por collecion, con todos los nombres y links (TODAS)
                        services.ListCollections(Constantes.File);
                        break;

                    case 6: //Fichero por lista, con todos los nombres y links (UNA)
                        if (!string.IsNullOrEmpty(infoAdditional)) //Url
                            services.ReadCollection(infoAdditional); 
                        break;

                    case 7: //Segunda revision de los descargados 
                        if (!string.IsNullOrEmpty(infoAdditional) && !string.IsNullOrEmpty(infoAdditional2)) //File and Path
                            services.ReviewNoDownload(infoAdditional, infoAdditional2);
                        break;

                    default:
                        return;
                }
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
