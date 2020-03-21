using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace ReadDCInfo.Models 
{
    class ComicsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=127.0.0.1;port=3306;user=root;password=56151621;database=DCComics",
                    b => b.ServerVersion(new Version(5, 7, 16), ServerType.MySql))
                //b => b.ServerVersion(new ServerVersion("5.7.16-mysql")))
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(MyLoggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole(); //PM> Install-Package Microsoft.Extensions.Logging.Console
        });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Borrado suave (que solo me retorno los estudiantes que "no esten borrados") /* 0 = false; 1 = true */
            modelBuilder.Entity<Collection>().HasQueryFilter(x => !x.IsDelete);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Collection> Collections { get; set; } //Indica que en este contexto vamos a tener una tabla con la que nos vamos a comunicar y tomara forma por parte de estudiante
        public DbSet<Issue> Issues { get; set; } //Indica que en este contexto vamos a tener una tabla con la que nos vamos a comunicar y tomara forma por parte de estudiante

    }
}
