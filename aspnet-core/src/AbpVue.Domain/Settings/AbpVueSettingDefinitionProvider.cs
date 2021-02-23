using AbpVue.Localization;

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

            context.Add(new SettingDefinition(
                name:AbpVueSettings.AllowedUserAvatarFormats,
                defaultValue: ".jpg,.jpeg,.png,.gif", 
                displayName: L("DisplayName:AllowedUserAvatar.Format"),
                description: L("Description:AllowedUserAvatar.Format")));

            context.Add(new SettingDefinition(
                name: AbpVueSettings.AllowedUserAvatarSize,
                defaultValue: "1024",
                displayName: L("DisplayName:AllowedUserAvatar.Size"),
                description: L("Description:AllowedUserAvatar.Size")));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SettingsResource>(name);
        }
    }
}
