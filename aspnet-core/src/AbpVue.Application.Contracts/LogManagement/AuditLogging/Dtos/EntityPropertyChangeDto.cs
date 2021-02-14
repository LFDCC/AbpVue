using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace AbpVue.LogManagement.AuditLogging.Dtos
{
    public class EntityPropertyChangeDto : EntityDto<Guid>, IMultiTenant
    {
        public Guid? TenantId
        {
            get;
            set;
        }

        public Guid EntityChangeId
        {
            get;
            set;
        }

        public string NewValue
        {
            get;
            set;
        }

        public string OriginalValue
        {
            get;
            set;
        }

        public string PropertyName
        {
            get;
            set;
        }

        public string PropertyTypeFullName
        {
            get;
            set;
        }
    }
}
