using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpVue.Data
{
    /* This is used if database provider does't define
     * IAbpVueDbSchemaMigrator implementation.
     */
    public class NullAbpVueDbSchemaMigrator : IAbpVueDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}