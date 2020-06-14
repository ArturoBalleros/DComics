using CG.Web.MegaApiClient;
using DComics.Models;
using ComicsIDownload.Utils;
using HtmlAgilityPack;
using log4net;
using log4net.Config;
using ScrapySharp.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ComicsIDownload.Services
{
    internal sealed class DownloadService
    {
        private readonly MegaApiClient ApiMega;
        private readonly ILog logger;

        public DownloadService(MegaApiClient mega)
        {
            ApiMega = mega;
            logger = InitLogger();
        }

        public bool DownloadFile(Comic comic)
        {
            try
            {
                if (comic.Link.Contains(Constantes.Mega))
                    return ProcessDownloadMega(comic);
                if (comic.Link.Contains(Constantes.MediaFile))
                    return ProcessDownloadMediaFire(comic);
                return false;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }

        private bool ProcessDownloadMega(Comic comic)
        {
            try
            {
                Uri uri = new Uri(comic.Link);
                INodeInfo infoMega = ApiMega.GetNodeFromLink(uri);
                ApiMega.DownloadFile(uri, Environment.CurrentDirectory + Constantes.Download + infoMega.Name);
                comic.NameWeb = infoMega.Name;
                comic.SizeWeb = Functions.FormatSize(infoMega.Size);

                return true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(string.Format("Error en el método: '{0}', Mensaje de error: '{1}'", MethodBase.GetCurrentMethod().Name, ex.Message));
                return false;
            }
        }
        private bool ProcessDownloadMediaFire(Comic comic)
        {
            try
            {
                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(comic.Link);
                comic.NameWeb = doc.DocumentNode.CssSelect(".dl-info").CssSelect(".filename").ToList()[0].InnerText;
                string linkDownload = doc.DocumentNode.CssSelect(".download_link").CssSelect(".input").First().Attributes[2].Value;

                string size = doc.DocumentNode.CssSelect(".dl-info").CssSelect(".details").ToList()[0].InnerText;
                size = size.Replace("File size: ", "º").Replace("MB", "º");
                int dif = size.LastIndexOf("º") - size.IndexOf("º") - 1;
                comic.SizeWeb = size.Substring(size.IndexOf("º") + 1, dif);
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.DownloadFile(linkDownload, Environment.CurrentDirectory + Constantes.Download + comic.NameWeb);
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

        private ILog InitLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return LogManager.GetLogger(typeof(DownloadService));
        }
    }
}
