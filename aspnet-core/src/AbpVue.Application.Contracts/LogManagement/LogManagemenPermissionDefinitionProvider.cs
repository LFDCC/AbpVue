
using AbpVue.Localization;

using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpVue.LogManagement
{
    public class LogManagemenPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(LogManagemenPermissions.GroupName,L("Permission:LogManagement"));

            var auditLogPermission = myGroup.AddPermission(LogManagemenPermissions.AuditLog.Default, L("Permission:AuditLogManagement"));
            auditLogPermission.AddChild(LogManagemenPermissions.AuditLog.Delete, L("Permission:Delete"));

            var securityLogPermission = myGroup.AddPermission(LogManagemenPermissions.SecurityLog.Default, L("Permission:SecurityLogManagement"));
            securityLogPermission.AddChild(LogManagemenPermissions.SecurityLog.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LoggingResource>(name);
        }
    }
}
