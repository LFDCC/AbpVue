using AbpVue.Localization;

using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace AbpVue
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpBackgroundJobsDomainSharedModule),
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpIdentityDomainSharedModule),
        typeof(AbpIdentityServerDomainSharedModule),
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpTenantManagementDomainSharedModule)
        )]
    public class AbpVueDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AbpVueGlobalFeatureConfigurator.Configure();
            AbpVueModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpVueDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpVueResource>("zh-Hans")
                    //.AddBaseTypes(typeof(AbpValidationResource))  //包含默认资源
                    .AddVirtualJson("/Localization/AbpVue");

                options.Resources.Remove(typeof(AuditLoggingResource));
                options.Resources
                    .Add<LoggingResource>("zh-Hans")
                    //.AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/LogManagement");

                options.Resources
                   .Add<FileResource>("zh-Hans")
                   //.AddBaseTypes(typeof(AbpValidationResource))
                   .AddVirtualJson("/Localization/Files");

                options.DefaultResourceType = typeof(AbpVueResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("AbpVue", typeof(AbpVueResource));
            });
        }
    }
}
