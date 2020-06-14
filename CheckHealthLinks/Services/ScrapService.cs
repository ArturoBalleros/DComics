using CG.Web.MegaApiClient;
using CheckHealthLinks.DAO;
using CheckHealthLinks.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CheckHealthLinks.Services
{
    internal sealed class ScrapService
    {
        private readonly MegaApiClient Mega;
        public ScrapService(MegaApiClient mega)
        {
            Mega = mega;
        }

        public void CheckHealth()
        {
            ModelsDao modelsDAO = new ModelsDao();
            List<Comic> comics = modelsDAO.GetComics();
            if (comics.Any())
            {
                foreach (Comic c in comics)
                {
                    Console.WriteLine(c.Link);
                    c.Status = CheckLink(c);
                    modelsDAO.UpdateComic(c);
                }
            }
        }
        private string CheckLink(Comic c)
        {
            HtmlWeb oWeb = new HtmlWeb();
            oWeb.Load(c.Link);

            if (oWeb.StatusCode != HttpStatusCode.OK)
                return oWeb.StatusCode.ToString();
            else if (c.Link.Contains("mega.nz"))
                try { Mega.GetNodeFromLink(new Uri(c.Link)); }
                catch { return HttpStatusCode.InternalServerError.ToString(); }
            return oWeb.StatusCode.ToString();
        }
    }
}
