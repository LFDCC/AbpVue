using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace AbpVue.LogManagement.AuditLogging.Dtos
{
    public class AuditLogActionDto : EntityDto<Guid>, IMultiTenant, IHasExtraProperties
    {
        public Guid? TenantId
        {
            get;
            set;
        }

        public Guid AuditLogId
        {
            get;
            set;
        }

        public string ServiceName
        {
            get;
            set;
        }

        public string MethodName
        {
            get;
            set;
        }

        public string Parameters
        {
            get;
            set;
        }

        public DateTime ExecutionTime
        {
            get;
            set;
        }

        public int ExecutionDuration
        {
            get;
            set;
        }

        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}