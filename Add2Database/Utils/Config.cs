using Microsoft.Extensions.Configuration;

namespace Add2Database.Utils
{
    static class Config
    {
        public static string ConnectionString { get; set; }
        public static string EditorialPaths { get; set; }
        public static string CollectionsPaths { get; set; }
        public static void LoadConfig()
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
            IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            ConnectionString = Configuration["ConnectionStrings:ComicsRepository"];
            EditorialPaths = Configuration["AppSettings:EditorialPaths"];
            CollectionsPaths = Configuration["AppSettings:CollectionsPaths"];
        }
    }
}
