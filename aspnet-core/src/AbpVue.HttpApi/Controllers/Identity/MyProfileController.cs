using AbpVue.Files;
using AbpVue.Files.Dtos;
using AbpVue.Identity;
using AbpVue.Localization;
using AbpVue.LogManagement.SecurityLogging.Dtos;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http;
using Volo.Abp.Identity;

namespace AbpVue.Controllers.Identity
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    [ControllerName("Profile")]
    [Route("/api/identity/my-profile")]
    public class MyProfileController : AbpVueController//, IMyProfileAppService
    {
        private readonly IStringLocalizer<FileResource> _localizer;
        private readonly IMyProfileAppService _myProfileAppService;
        public MyProfileController(IMyProfileAppService myProfileAppService, IStringLocalizer<FileResource> localizer)
        {
            _localizer = localizer;
            _myProfileAppService = myProfileAppService;
        }
        /// <summary>
        /// 获取我的安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("security-logs")]
        public virtual async Task<PagedResultDto<SecurityLogDto>> GetMySecurityLogs(GetSecurityLogDto input)
        {
            return await _myProfileAppService.GetMySecurityLogs(input);
        }
        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("picture/{name}")]
        [ResponseCache(Duration = 60 * 60 * 24 * 365, Location = ResponseCacheLocation.Client)]
        public virtual async Task<IActionResult> GetProfilePicture(string name)
        {
            var input = new GetFileInput
            {
                BlobName = name
            };
            var output = await _myProfileAppService.GetFile(input);
            if (output.Bytes == null || output.Bytes.Length == 0)
            {
                return NotFound();
            }
            return File(output.Bytes, MimeTypes.GetByExtension(Path.GetExtension(output.BlobName)));
        }
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("picture")]
        public virtual async Task<SaveFileOutput> SaveProfilePicture(IFormFile file)
        {
            var exts = Path.GetExtension(file.FileName);
            var bytes = await file.GetAllBytesAsync();
            var blobName = Guid.NewGuid().ToString("N") + exts;
            var input = new SaveFileInput
            {
                BolbName = blobName,
                Bytes = bytes
            };
            return await _myProfileAppService.SaveFile(input);
        }
    }
}
