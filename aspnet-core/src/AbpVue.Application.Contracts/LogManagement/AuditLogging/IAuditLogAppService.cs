using AbpVue.LogManagement.AuditLogging.Dtos;

using System;

using Volo.Abp.Application.Services;

namespace AbpVue.LogManagement.AuditLogging
{
    public interface IAuditLogAppService : IReadOnlyAppService<AuditLogDto, Guid, GetAuditLogDto>, IDeleteAppService<Guid>
    {
      
    }
}
