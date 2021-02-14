using AbpVue.LogManagement;
using AbpVue.LogManagement.SecurityLogging;
using AbpVue.LogManagement.SecurityLogging.Dtos;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpVue.Controllers.LogManagement
{
    /// <summary>
    /// 安全日志
    /// </summary>
    [RemoteService(Name = LogManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("logManagement")]
    [ControllerName("SecurityLog")]
    [Route("api/log-management/security-logs")]
    public class SecurityLogController : AbpController, ISecurityLogAppService
    {
        readonly ISecurityLogAppService _securityLogAppService;

        public SecurityLogController(ISecurityLogAppService securityLogAppService)
        {
            _securityLogAppService = securityLogAppService;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _securityLogAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public virtual async Task<SecurityLogDto> GetAsync(Guid id)
        {
            return await _securityLogAppService.GetAsync(id);
        }

        /// <summary>
        /// 获取安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(GetSecurityLogDto input)
        {
            return await _securityLogAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取我的安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("my")]
        public virtual async Task<PagedResultDto<SecurityLogDto>> My(GetSecurityLogDto input)
        {
            input.UserId = CurrentUser.Id;
            return await _securityLogAppService.GetListAsync(input);
        }
    }
}
