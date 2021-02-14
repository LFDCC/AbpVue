using AbpVue.LogManagement.AuditLogging.Dtos;

using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Features;

namespace AbpVue.LogManagement.AuditLogging
{
    /// <summary>
    /// 审计日志
    /// </summary>
    [Authorize(LogManagemenPermissions.AuditLog.Default)]
    public class AuditLogAppService : AbpVueAppService, IAuditLogAppService
    {

        private readonly IAuditLogRepository _auditingLogRepository;
        public AuditLogAppService(IAuditLogRepository auditingLogRepository)
        {
            _auditingLogRepository = auditingLogRepository;
        }
        /// <summary>
        /// 删除审计日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(LogManagemenPermissions.AuditLog.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var auditLog = await _auditingLogRepository.GetAsync(id);
            auditLog.EntityChanges.Clear();
            auditLog.Actions.Clear();
            await _auditingLogRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AuditLogDto> GetAsync(Guid id)
        {
            var item = await _auditingLogRepository.GetAsync(id);
            return ObjectMapper.Map<AuditLog, AuditLogDto>(item);
        }

        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogDto input)
        {
            var count = await _auditingLogRepository.GetCountAsync(
               startTime: input.StartTime,
               endTime: input.EndTime,
               httpMethod: input.HttpMethod,
               url: input.Url,
               userName: input.UserName,
               applicationName: input.ApplicationName,
               correlationId: input.CorrelationId,
               maxExecutionDuration: input.MaxExecutionDuration,
               minExecutionDuration: input.MinExecutionDuration,
               hasException: input.HasException,
               httpStatusCode: input.HttpStatusCode
           );
            var list = await _auditingLogRepository.GetListAsync(
                sorting: input.Sorting,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount,
                startTime: input.StartTime,
                endTime: input.EndTime,
                httpMethod: input.HttpMethod,
                url: input.Url,
                userName: input.UserName,
                applicationName: input.ApplicationName,
                correlationId: input.CorrelationId,
                maxExecutionDuration: input.MaxExecutionDuration,
                minExecutionDuration: input.MinExecutionDuration,
                hasException: input.HasException,
                httpStatusCode: input.HttpStatusCode,
                includeDetails: input.IncludeDetails
            );
            return new PagedResultDto<AuditLogDto>(
                count,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(list)
            );
        }
    }
}
