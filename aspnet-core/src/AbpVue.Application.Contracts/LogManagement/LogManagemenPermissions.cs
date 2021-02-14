using System;
using System.Collections.Generic;
using System.Text;

namespace AbpVue.LogManagement
{
    public class LogManagemenPermissions
    {
        public const string GroupName = "LogManagement";

        public class AuditLog
        {
            public const string Default = GroupName + ".AuditLogs";
            public const string Delete = Default + ".Delete";
        }
        public class SecurityLog
        {
            public const string Default = GroupName + ".SecurityLogs";
            public const string Delete = Default + ".Delete";
        }
    }
}
