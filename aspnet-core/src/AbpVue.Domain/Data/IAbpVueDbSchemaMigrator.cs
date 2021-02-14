using System.Threading.Tasks;

namespace AbpVue.Data
{
    public interface IAbpVueDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
