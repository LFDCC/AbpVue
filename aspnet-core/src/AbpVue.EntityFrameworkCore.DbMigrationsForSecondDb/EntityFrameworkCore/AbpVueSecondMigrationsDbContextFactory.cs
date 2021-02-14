using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpVue.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class AbpVueSecondMigrationsDbContextFactory : IDesignTimeDbContextFactory<AbpVueSecondMigrationsDbContext>
    {
        public AbpVueSecondMigrationsDbContext CreateDbContext(string[] args)
        {
            AbpVueEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<AbpVueSecondMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("AbpAuditLogging"));

            return new AbpVueSecondMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
