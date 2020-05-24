using ComicsIDownload.Utils;
using DComics.Models;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ComicsIDownload.Services
{
    internal sealed class ReportService
    {
        private readonly ILog logger;

        public ReportService()
        {
            logger = initLogger();
        }

        public bool CreateFileReport(object obj, string title, string path = Constantes.Report, bool indented = false, bool append = true)
        {
            try
            {
                bool result = false;
                string docPath = Environment.CurrentDirectory + path + title;
                using StreamWriter outputFile = new StreamWriter(docPath, append);

                if (obj.GetType().Name.Equals("String"))
                {
                    outputFile.WriteLine((string)obj);
                    result = true;
                }

                if (!result && obj.GetType().FullName.Contains("DComics.Models.Comic"))
                {
                    outputFile.WriteLine(Comic.Serializer((List<Comic>)obj, indented) + "\n");
                    result = true;
                }

                if (!result && obj.GetType().FullName.Contains("System.String,"))
                {
                    foreach (string name in (List<string>)obj)
                        outputFile.WriteLine(name);
                    result = true;
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
        public string ReadLastDownload()
        {
            try
            {
                string docPath = Environment.CurrentDirectory + Constantes.Report + "LastDownload.txt";
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

        private ILog initLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(ReportService));
        }
    }
}
