using AbpVue.Files.Dtos;
using AbpVue.Localization;

using System.Threading.Tasks;

using Volo.Abp.Application.Services;

namespace AbpVue
{
    /* Inherit your application services from this class.
     */
    public abstract class AbpVueAppService : ApplicationService
    {
        protected AbpVueAppService()
        {
            LocalizationResource = typeof(AbpVueResource);
        }
    }
}
