using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpVue
{
    [Dependency(ReplaceServices = true)]
    public class AbpVueBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "AbpVue";
    }
}
