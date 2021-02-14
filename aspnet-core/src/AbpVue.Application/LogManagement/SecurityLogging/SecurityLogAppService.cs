
using AbpVue.LogManagement.SecurityLogging.Dtos;

using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace AbpVue.LogManagement.SecurityLogging
{
    /// <summary>
    /// 安全日志
    /// </summary>
    [Authorize(LogManagemenPermissions.SecurityLog.Default)]
    public class SecurityLogAppService : AbpVueAppService, ISecurityLogAppService
    {
        private readonly IIdentitySecurityLogRepository _identitySecurityLogRepository;
        public SecurityLogAppService(IIdentitySecurityLogRepository identitySecurityLogRepository)
        {
            _identitySecurityLogRepository = identitySecurityLogRepository;
        }
        /// <summary>
        /// 获取安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<SecurityLogDto> GetAsync(Guid id)
        {
            var securityLog = await _identitySecurityLogRepository.GetAsync(id);

            return ObjectMapper.Map<IdentitySecurityLog, SecurityLogDto>(securityLog);
        }

        /// <summary>
        /// 获取安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(GetSecurityLogDto input)
        {
            var securityLogCount = await _identitySecurityLogRepository
                .GetCountAsync(input.StartTime, input.EndTime,
                    input.ApplicationName, input.Identity, input.ActionName,
                    input.UserId, input.UserName, input.ClientId, input.CorrelationId
                );

            var securityLogs = await _identitySecurityLogRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                    input.StartTime, input.EndTime,
                    input.ApplicationName, input.Identity, input.ActionName,
                    input.UserId, input.UserName, input.ClientId, input.CorrelationId,
                    includeDetails: false
                );

            return new PagedResultDto<SecurityLogDto>(securityLogCount,
                ObjectMapper.Map<List<IdentitySecurityLog>, List<SecurityLogDto>>(securityLogs));
        }

        /// <summary>
        /// 删除安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(LogManagemenPermissions.SecurityLog.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var securityLog = await _identitySecurityLogRepository.GetAsync(id);
            await _identitySecurityLogRepository.DeleteAsync(securityLog);
        }
    }
}
