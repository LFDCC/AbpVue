using AbpVue.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpVue.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class AbpVueController : AbpController
    {
        protected AbpVueController()
        {
            LocalizationResource = typeof(AbpVueResource);
        }
    }
}