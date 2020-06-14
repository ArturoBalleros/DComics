using Microsoft.Extensions.Configuration;

namespace CheckHealthLinks.Utils
{
    static class Config
    {
        public static string ConnectionString { get; set; }
        public static void LoadConfig()
        {
            IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            ConnectionString = Configuration["ConnectionStrings:ComicsRepository"];
        }
    }
}
