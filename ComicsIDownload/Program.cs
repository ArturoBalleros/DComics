using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System;
using CG.Web.MegaApiClient;

namespace DComics
{
    class Program
    {
        static ILog logger;
        public static void Main(string[] args)
        {
            logger = initLogger();
            int option = 0;
            string infoAdditional = "";       
            try
            {
                MegaApiClient mega = new MegaApiClient();
                mega.LoginAnonymous();
                Services services = new Services(mega);

                logger.Info("Inicio del proceso");
                if (!services.CheckDirectoriesAndFiles(logger))
                {
                    logger.Info("Fin del proceso por falta de archivos");
                    return;
                }
                //args = new string[] { "1", @"json.txt.json" };
                args = new string[] { "1", @"json.txt" };
                if (args != null)
                {
                    option = !string.IsNullOrEmpty(args[0]) ? int.Parse(args[0]) : 0;
                    infoAdditional = !string.IsNullOrEmpty(args[1]) ? args[1] : "";
                }
                switch (option)
                {
                    case 0: //Bajar últimos
                        services.DownloadNews(logger);
                        break;

                    case 1: //Fichero
                        services.ReadFile(infoAdditional, logger);
                        break;

                    case 2: //Renombrar
                        services.RenameFiles(infoAdditional, logger);
                        break;

                    case 3: //Lista Colecciones
                        services.ListCollections(logger);
                        break;

                    case 4: //Arbol de directorios
                        DirectoryInfo rootDir = new DirectoryInfo(@"I:\ComicsId\Colecciones");
                        services.TreeDirectory(rootDir, logger);
                        break;

                    case 5: //Ficheros pos lista con todos los nombres y links
                        services.ListCollections(logger, "File");
                        break;
                        
                    case 6: //Ficheros pos lista con todos los nombres y links
                        services.ReadCollection(@"http://www.comicsid.com/serie/142-90-serie", logger);
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


            /*   XmlDocument log4netConfig = new XmlDocument();
               log4netConfig.Load(File.OpenRead("log4net.config"));
               var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
               XmlConfigurator.Configure(repo, log4netConfig["log4net"]);*/



            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(Program));
        }
    }
}
