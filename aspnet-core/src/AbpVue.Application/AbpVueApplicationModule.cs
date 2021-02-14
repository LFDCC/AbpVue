using AbpVue.Files;

using Microsoft.Extensions.DependencyInjection;

using System;

using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace AbpVue
{
    [DependsOn(
        typeof(AbpVueDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpVueApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule)
        )]
    public class AbpVueApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AbpVueApplicationModule>();
            });
            //context.Services.Add(ServiceDescriptor.Singleton(provider =>
            //{
            //    IFileAppService func(FileType type)
            //    {
            //        return type switch
            //        {
            //            FileType.Default => provider.GetService<DefaultFileAppService>(),
            //            FileType.Profile => provider.GetService<ProfileFileAppService>(),
            //            _ => throw new NotSupportedException(),
            //        };
            //    }
            //    return (Func<FileType, IFileAppService>)func;
            //}));
        }
    }
}
