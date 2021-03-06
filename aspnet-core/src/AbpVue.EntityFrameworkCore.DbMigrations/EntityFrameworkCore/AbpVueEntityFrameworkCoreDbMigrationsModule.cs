﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpVue.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpVueEntityFrameworkCoreModule)
        )]
    public class AbpVueEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpVueMigrationsDbContext>();
        }
    }
}
