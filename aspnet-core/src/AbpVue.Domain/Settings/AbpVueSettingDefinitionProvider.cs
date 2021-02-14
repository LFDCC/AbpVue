using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace AbpVue.Settings
{
    public class AbpVueSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(AbpVueSettings.MySetting1));

            context.Add(new SettingDefinition(AbpVueSettings.AllowedUserAvatarFormats, ".jpg,.jpeg,.png,.gif", isVisibleToClients: true));
            context.Add(new SettingDefinition(AbpVueSettings.AllowedUserAvatarSize, "1024", isVisibleToClients: true));

        }
    }
}
