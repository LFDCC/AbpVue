using AbpVue.Localization;

using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpVue.Permissions
{
    public class AbpVuePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(AbpVuePermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(AbpVuePermissions.MyPermission1, L("Permission:MyPermission1"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpVueResource>(name);
        }
    }
}
