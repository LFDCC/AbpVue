using AbpVue.EntityFrameworkCore;
using AbpVue.MultiTenancy;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SecurityLog;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace AbpVue
{
    [DependsOn(
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpVueHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpVueApplicationModule),
        typeof(AbpVueEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpVueEntityFrameworkCoreSecondDbMigrationsModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
    )]
    public class AbpVueHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();


            ConfigureFileSystem(hostingEnvironment);
            ConfigureJsonConvert(context);
            ConfigureAuditLog();
            ConfigureSecurityLog();
            ConfigureBundles();
            ConfigureUrls(configuration);
            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);
        }

        private void ConfigureFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = Path.Combine(hostingEnvironment.ContentRootPath, "upload");
                    });
                });
            });
        }

        private void ConfigureJsonConvert(ServiceConfigurationContext context)
        {
            Configure<AbpJsonOptions>(options =>
            {
                options.DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:sss";
            });
        }
        private void ConfigureSecurityLog()
        {
            Configure<AbpSecurityLogOptions>(options =>
            {
                options.ApplicationName = "AbpVue";
            });
        }

        private void ConfigureAuditLog()
        {
            Configure<AbpAuditingOptions>(options =>
            {
                options.ApplicationName = "AbpVue";
                //一般审计 只会记录当前请求了哪些方法 携带哪些参数
                //实体审计 哪些实体的哪些字段被更改 更改成什么值 产生的数据会非常多
                //options.EntityHistorySelectors.AddAllEntities();
            });
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle => { bundle.AddFiles("/global-styles.css"); }
                );
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpVueDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}AbpVue.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpVueDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}AbpVue.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpVueApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}AbpVue.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpVueApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}AbpVue.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            //自动weibapi 当请求401时，会重定向到登录页
            //不会生成各种状态码的swagger信息
            //Configure<AbpAspNetCoreMvcOptions>(options =>
            //{
            //    options.ConventionalControllers.Create(typeof(AbpVueApplicationModule).Assembly);
            //});
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "AbpVue";
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
               configuration["AuthServer:Authority"],
               new Dictionary<string, string>
               {
                    {"AbpVue", "AbpVue API"}
               },
               options =>
               {
                   options.SwaggerDoc("v1", new OpenApiInfo { Title = "AbpVue API", Version = "v1" });
                   options.DocInclusionPredicate((docName, description) => true);
               });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();
            //app.UseOwlRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AbpVue API");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
