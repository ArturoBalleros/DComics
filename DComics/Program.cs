﻿using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System;

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
            Services services = new Services();
            try
            {
                logger.Info("Inicio del proceso");
                if (!services.CheckDirectoriesAndFiles(logger)) {
                    logger.Info("Fin del proceso por falta de archivos");
                    return;
                }
                args = new string[] { "1", @"json.txt" };
                if (args != null)
                {
                    option = !string.IsNullOrEmpty(args[0]) ? int.Parse(args[0]) : 0;
                    infoAdditional = !string.IsNullOrEmpty(args[1]) ? args[1] : "";
                }
                switch (option)
                {
                    case 0: //Bajar últimos

                        break;

                    case 1://Fichero
                        services.ReadFile(infoAdditional, logger);
                        break;

                    default:
                        return;
                }
                logger.Info("Fin del proceso");
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
