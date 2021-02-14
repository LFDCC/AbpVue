using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AbpVue.Data;
using Volo.Abp.DependencyInjection;

namespace AbpVue.EntityFrameworkCore
{
    public class EntityFrameworkCoreSecondAbpVueDbSchemaMigrator
        : IAbpVueDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreSecondAbpVueDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the AbpVueMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<AbpVueSecondMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}