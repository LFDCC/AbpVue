using Microsoft.EntityFrameworkCore;

using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpVue.EntityFrameworkCore
{
    /* This DbContext is only used for database migrations.
     * It is not used on runtime. See AbpVueDbContext for the runtime DbContext.
     * It is a unified model that includes configuration for
     * all used modules and your application.
     */
    [ConnectionStringName("AbpAuditLogging")]
    public class AbpVueSecondMigrationsDbContext : AbpDbContext<AbpVueSecondMigrationsDbContext>
    {
        public AbpVueSecondMigrationsDbContext(DbContextOptions<AbpVueSecondMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigureAuditLogging();
        }
    }
}