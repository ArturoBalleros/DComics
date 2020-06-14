using CG.Web.MegaApiClient;
using CheckHealthLinks.Services;
using CheckHealthLinks.Utils;

namespace CheckHealthLinks
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.LoadConfig();
            MegaApiClient mega = new MegaApiClient();
            mega.LoginAnonymous();
            ScrapService scrapService = new ScrapService(mega);
                 scrapService.CheckHealth();
            
        }
    }
}
