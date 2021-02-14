using AbpVue.LogManagement;
using AbpVue.LogManagement.AuditLogging;
using AbpVue.LogManagement.AuditLogging.Dtos;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpVue.Controllers.LogManagement
{
    /// <summary>
    /// 审计日志
    /// </summary>
    [RemoteService(Name = LogManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("logManagement")]
    [ControllerName("Auditlog")]
    [Route("api/log-management/audit-logs")]    
    public class AuditLogController : AbpController, IAuditLogAppService
    {
        readonly IAuditLogAppService _auditLogAppService;

        public AuditLogController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        /// <summary>
        /// 删除审计日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _auditLogAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public virtual async Task<AuditLogDto> GetAsync(Guid id)
        {
            return await _auditLogAppService.GetAsync(id);
        }

        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogDto input)
        {
            return await _auditLogAppService.GetListAsync(input);
        }
    }
}
