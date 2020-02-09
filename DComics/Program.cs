using log4net;
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
            logger.Info("Inicio del proceso");
            try
            {
                args = new string[] { "2", "" };
                if (args != null)
                {

                    int option = !string.IsNullOrEmpty(args[0]) ? int.Parse(args[0]) : 0;
                    string infoAdditional = !string.IsNullOrEmpty(args[1]) ? args[1] : "";
                    /*
                        0 => Bajar últimos
                        1 => Fichero
                        2 => JSON
                     */
                    Services services = new Services();
                    switch (option)
                    {
                        case 0:

                            break;

                        case 1:
                            services.ReadFile(infoAdditional, logger);
                            break;

                        case 2:
                            services.ProcessJSON((JArray)infoAdditional, logger);
                            break;

                        default:
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                if(logger != null)
                    logger.Error(string.Format( "Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        private static ILog initLogger(){
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(Program));
        }
    }
}
