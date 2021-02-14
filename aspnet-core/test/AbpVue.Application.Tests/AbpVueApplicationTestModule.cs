using Volo.Abp.Modularity;

namespace AbpVue
{
    [DependsOn(
        typeof(AbpVueApplicationModule),
        typeof(AbpVueDomainTestModule)
        )]
    public class AbpVueApplicationTestModule : AbpModule
    {

    }
}