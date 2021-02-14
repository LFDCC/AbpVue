using AbpVue.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpVue
{
    [DependsOn(
        typeof(AbpVueEntityFrameworkCoreTestModule)
        )]
    public class AbpVueDomainTestModule : AbpModule
    {

    }
}