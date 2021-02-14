using AbpVue.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace AbpVue.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpVueEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpVueEntityFrameworkCoreSecondDbMigrationsModule),
        typeof(AbpVueApplicationContractsModule)
        )]
    public class AbpVueDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
